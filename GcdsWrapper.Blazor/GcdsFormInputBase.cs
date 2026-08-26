using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace GcdsWrapper.Blazor;

/// <summary>Shared Blazor forms integration for value-bearing GCDS components.</summary>
public abstract class GcdsFormInputBase<TValue> : InputBase<TValue>, IAsyncDisposable
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private IJSObjectReference? module;
    private IJSObjectReference? subscription;
    private DotNetObjectReference<GcdsFormInputBase<TValue>>? reference;

    [Inject] private IJSRuntime JS { get; set; } = default!;

    [Parameter] public string? ErrorMessage { get; set; }
    [Parameter] public EventCallback OnBlur { get; set; }
    [Parameter] public EventCallback<TValue> OnChange { get; set; }
    [Parameter] public EventCallback OnFocus { get; set; }
    [Parameter] public EventCallback<TValue> OnInput { get; set; }
    [Parameter] public EventCallback OnValid { get; set; }
    [Parameter] public EventCallback OnInvalid { get; set; }
    [Parameter] public EventCallback<GcdsEventArgs> OnRemoveFile { get; set; }
    [Parameter] public EventCallback<TValue> OnSubmit { get; set; }
    [Parameter] public EventCallback<TValue> OnSuggestionSelected { get; set; }

    protected ElementReference Element;
    protected virtual string[] EventNames =>
        ["gcdsBlur", "gcdsChange", "gcdsError", "gcdsFocus", "gcdsInput", "gcdsValid"];
    protected string? EffectiveErrorMessage =>
        ErrorMessage ?? EditContext?.GetValidationMessages(FieldIdentifier).FirstOrDefault();

    protected override bool TryParseValueFromString(
        string? value,
        out TValue result,
        out string validationErrorMessage)
    {
        try
        {
            result = JsonSerializer.Deserialize<TValue>(JsonSerializer.Serialize(value), JsonOptions)!;
            validationErrorMessage = null!;
            return true;
        }
        catch (JsonException)
        {
            result = default!;
            validationErrorMessage = $"The {DisplayName ?? FieldIdentifier.FieldName} field has an invalid value.";
            return false;
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        module = await JS.InvokeAsync<IJSObjectReference>(
            "import",
            "./_content/GcdsWrapper.Blazor/gcdsInterop.js");
        reference = DotNetObjectReference.Create(this);
        subscription = await module.InvokeAsync<IJSObjectReference>(
            "listenFormEvents",
            Element,
            EventNames,
            reference);
    }

    [JSInvokable]
    public async Task HandleGcdsFormEvent(string eventName, string? detailJson)
    {
        if (eventName is "gcdsInput" or "gcdsChange")
        {
            var value = detailJson is null
                ? default!
                : JsonSerializer.Deserialize<TValue>(detailJson, JsonOptions)!;

            if (eventName == "gcdsInput") await OnInput.InvokeAsync(value);
            else
            {
                CurrentValue = value;
                await OnChange.InvokeAsync(value);
            }
        }

        if (eventName == "gcdsBlur") await OnBlur.InvokeAsync();
        if (eventName == "gcdsFocus") await OnFocus.InvokeAsync();
        if (eventName == "gcdsValid") await OnValid.InvokeAsync();
        if (eventName == "gcdsError") await OnInvalid.InvokeAsync();
        if (eventName == "gcdsRemoveFile")
            await OnRemoveFile.InvokeAsync(new GcdsEventArgs(eventName, detailJson));
        if (eventName == "gcdsSubmit")
            await OnSubmit.InvokeAsync(Deserialize(detailJson));
        if (eventName == "gcdsSuggestionSelected")
            await OnSuggestionSelected.InvokeAsync(Deserialize(detailJson));
    }

    public async ValueTask DisposeAsync()
    {
        if (subscription is not null) await subscription.InvokeVoidAsync("dispose");
        reference?.Dispose();
        if (module is not null) await module.DisposeAsync();
    }

    private static TValue Deserialize(string? detailJson) => detailJson is null
        ? default!
        : JsonSerializer.Deserialize<TValue>(detailJson, JsonOptions)!;
}

internal static class GcdsJson
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

    public static string? Serialize(object? value) => value switch
    {
        null => null,
        string text => text,
        _ => JsonSerializer.Serialize(value, Options)
    };
}
