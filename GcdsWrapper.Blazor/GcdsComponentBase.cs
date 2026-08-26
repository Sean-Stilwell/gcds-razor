using System.Globalization;
using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace GcdsWrapper.Blazor;

/// <summary>Base class for thin wrappers around GC Design System custom elements.</summary>
public abstract class GcdsComponentBase : ComponentBase, IAsyncDisposable
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
    [Parameter] public EventCallback<GcdsEventArgs> OnBlur { get; set; }
    [Parameter] public EventCallback<GcdsEventArgs> OnChange { get; set; }
    [Parameter] public EventCallback<GcdsEventArgs> OnClick { get; set; }
    [Parameter] public EventCallback<GcdsEventArgs> OnDismiss { get; set; }
    [Parameter] public EventCallback<GcdsEventArgs> OnError { get; set; }
    [Parameter] public EventCallback<GcdsEventArgs> OnFocus { get; set; }
    [Parameter] public EventCallback<GcdsEventArgs> OnInput { get; set; }
    [Parameter] public EventCallback<GcdsEventArgs> OnRemoveFile { get; set; }
    [Parameter] public EventCallback<GcdsEventArgs> OnSubmit { get; set; }
    [Parameter] public EventCallback<GcdsEventArgs> OnTableStateChange { get; set; }
    [Parameter] public EventCallback<GcdsEventArgs> OnValid { get; set; }

    [Inject] private IJSRuntime JS { get; set; } = default!;

    private ElementReference element;
    private IJSObjectReference? module;
    private IJSObjectReference? subscription;
    private DotNetObjectReference<GcdsComponentBase>? reference;

    protected abstract string TagName { get; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, TagName);
        var sequence = 1;

        foreach (var property in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (property.Name is nameof(ChildContent) or nameof(AdditionalAttributes)) continue;
            if (property.GetCustomAttribute<ParameterAttribute>() is null) continue;
            if (property.PropertyType.IsGenericType &&
                property.PropertyType.GetGenericTypeDefinition() == typeof(EventCallback<>)) continue;

            var value = property.GetValue(this);
            if (value is null) continue;
            builder.AddAttribute(sequence++, ToKebabCase(property.Name), ToAttributeValue(value));
        }

        if (AdditionalAttributes is not null)
            builder.AddMultipleAttributes(sequence++, AdditionalAttributes);

        builder.AddContent(sequence, ChildContent);
        builder.AddElementReferenceCapture(++sequence, value => element = value);
        builder.CloseElement();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var events = EventCallbacks()
            .Where(item => item.Callback.HasDelegate)
            .Select(item => item.EventName)
            .ToArray();
        if (events.Length == 0) return;

        module = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/GcdsWrapper.Blazor/gcdsInterop.js");
        reference = DotNetObjectReference.Create(this);
        subscription = await module.InvokeAsync<IJSObjectReference>("listenEvents", element, events, reference);
    }

    [JSInvokable]
    public Task HandleGcdsEvent(string eventName, string? detailJson)
    {
        var callback = EventCallbacks().FirstOrDefault(item => item.EventName == eventName).Callback;
        return callback.HasDelegate
            ? callback.InvokeAsync(new GcdsEventArgs(eventName, detailJson))
            : Task.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        if (subscription is not null) await subscription.InvokeVoidAsync("dispose");
        reference?.Dispose();
        if (module is not null) await module.DisposeAsync();
    }

    private IEnumerable<(string EventName, EventCallback<GcdsEventArgs> Callback)> EventCallbacks()
    {
        yield return ("gcdsBlur", OnBlur);
        yield return ("gcdsChange", OnChange);
        yield return ("gcdsClick", OnClick);
        yield return ("gcdsDismiss", OnDismiss);
        yield return ("gcdsError", OnError);
        yield return ("gcdsFocus", OnFocus);
        yield return ("gcdsInput", OnInput);
        yield return ("gcdsRemoveFile", OnRemoveFile);
        yield return ("gcdsSubmit", OnSubmit);
        yield return ("gcdsTableStateChange", OnTableStateChange);
        yield return ("gcdsValid", OnValid);
    }

    private static object ToAttributeValue(object value) => value switch
    {
        bool boolean => boolean.ToString().ToLowerInvariant(),
        Enum enumeration => enumeration.ToString().ToLowerInvariant(),
        string text => text,
        IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture),
        _ => JsonSerializer.Serialize(value, JsonOptions)
    };

    private static string ToKebabCase(string value) => string.Concat(value.Select((character, index) =>
        char.IsUpper(character) && index > 0
            ? $"-{char.ToLowerInvariant(character)}"
            : char.ToLowerInvariant(character).ToString()));
}

/// <summary>Data raised by a GC Design System custom event.</summary>
/// <param name="Name">The browser event name, such as <c>gcdsChange</c>.</param>
/// <param name="DetailJson">The event detail serialized as JSON, or <see langword="null"/>.</param>
public sealed record GcdsEventArgs(string Name, string? DetailJson)
{
    public T? GetDetail<T>() => DetailJson is null
        ? default
        : JsonSerializer.Deserialize<T>(DetailJson, new JsonSerializerOptions(JsonSerializerDefaults.Web));
}
