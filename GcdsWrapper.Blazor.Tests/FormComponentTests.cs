using System.ComponentModel.DataAnnotations;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Xunit;

namespace GcdsWrapper.Blazor.Tests;

public sealed class FormComponentTests
{
    [Fact]
    public void AllValueBearingComponents_UseBlazorInputBase()
    {
        Type[] scalarComponents =
        [
            typeof(GcdsInput), typeof(GcdsSelect), typeof(GcdsTextarea),
            typeof(GcdsRadios), typeof(GcdsDateInput), typeof(GcdsSearch)
        ];
        Type[] arrayComponents = [typeof(GcdsCheckboxes), typeof(GcdsFileUploader)];

        Assert.All(scalarComponents, component =>
            Assert.Equal(typeof(string), GetInputValueType(component)));
        Assert.All(arrayComponents, component =>
            Assert.Equal(typeof(string[]), GetInputValueType(component)));
    }

    [Fact]
    public async Task Textarea_UpdatesBoundValue_AndNotifiesEditContext()
    {
        using var context = CreateContext();
        var model = new TestModel();
        var editContext = new EditContext(model);
        FieldIdentifier? changedField = null;
        editContext.OnFieldChanged += (_, args) => changedField = args.FieldIdentifier;

        var host = context.Render<CascadingValue<EditContext>>(parameters => parameters
            .Add(component => component.Value, editContext)
            .AddChildContent<GcdsTextarea>(component => component
                .Add(input => input.Id, "summary")
                .Add(input => input.Name, "summary")
                .Add(input => input.Label, "Summary")
                .Add(input => input.Value, model.Text)
                .Add(input => input.ValueChanged, value => { model.Text = value; })
                .Add(input => input.ValueExpression, () => model.Text)));

        var textarea = host.FindComponent<GcdsTextarea>();
        await textarea.Instance.HandleGcdsFormEvent("gcdsChange", "\"Updated text\"");

        Assert.Equal("Updated text", model.Text);
        Assert.Equal(FieldIdentifier.Create(() => model.Text), changedField);
        Assert.True(editContext.IsModified(FieldIdentifier.Create(() => model.Text)));
    }

    [Fact]
    public async Task Checkboxes_UpdatesArrayBinding()
    {
        using var context = CreateContext();
        var model = new TestModel();

        var component = context.Render<GcdsCheckboxes>(parameters => parameters
            .Add(input => input.Name, "services")
            .Add(input => input.Legend, "Services")
            .Add(input => input.Options, "[]")
            .Add(input => input.Value, model.Services)
            .Add(input => input.ValueChanged, value => { model.Services = value; })
            .Add(input => input.ValueExpression, () => model.Services));

        await component.Instance.HandleGcdsFormEvent(
            "gcdsChange",
            "[\"benefits\",\"taxes\"]");

        Assert.NotNull(model.Services);
        Assert.Equal(["benefits", "taxes"], model.Services);
    }

    [Fact]
    public async Task DateInput_BindsCompleteComposedDate()
    {
        using var context = CreateContext();
        var model = new TestModel();

        var component = context.Render<GcdsDateInput>(parameters => parameters
            .Add(input => input.Name, "start-date")
            .Add(input => input.Legend, "Start date")
            .Add(input => input.Value, model.StartDate)
            .Add(input => input.ValueChanged, value => { model.StartDate = value; })
            .Add(input => input.ValueExpression, () => model.StartDate));

        await component.Instance.HandleGcdsFormEvent(
            "gcdsChange",
            "\"2026-10-05\"");

        Assert.Equal("2026-10-05", model.StartDate);
    }

    [Fact]
    public void DateInput_RendersAutocompleteOnlyWhenProvided()
    {
        using var context = CreateContext();
        var model = new TestModel();

        var withAutocomplete = context.Render<GcdsDateInput>(parameters => parameters
            .Add(input => input.Name, "birth-date")
            .Add(input => input.Legend, "Birth date")
            .Add(input => input.Autocomplete, "bday")
            .Add(input => input.ValueExpression, () => model.StartDate));
        var withoutAutocomplete = context.Render<GcdsDateInput>(parameters => parameters
            .Add(input => input.Name, "start-date")
            .Add(input => input.Legend, "Start date")
            .Add(input => input.ValueExpression, () => model.StartDate));

        Assert.Equal("bday", withAutocomplete.Find("gcds-date-input").GetAttribute("autocomplete"));
        Assert.Null(withoutAutocomplete.Find("gcds-date-input").GetAttribute("autocomplete"));
    }

    [Fact]
    public void Input_DisplaysEditFormValidationMessage()
    {
        using var context = CreateContext();
        var model = new TestModel();
        var editContext = new EditContext(model);
        var messages = new ValidationMessageStore(editContext);
        messages.Add(FieldIdentifier.Create(() => model.Text), "Enter a summary.");

        var host = context.Render<CascadingValue<EditContext>>(parameters => parameters
            .Add(component => component.Value, editContext)
            .AddChildContent<GcdsInput>(component => component
                .Add(input => input.Id, "summary")
                .Add(input => input.Name, "summary")
                .Add(input => input.Label, "Summary")
                .Add(input => input.Value, model.Text)
                .Add(input => input.ValueChanged, value => { model.Text = value; })
                .Add(input => input.ValueExpression, () => model.Text)));

        Assert.Equal(
            "Enter a summary.",
            host.Find("gcds-input").GetAttribute("error-message"));
    }

    private static BunitContext CreateContext()
    {
        var context = new BunitContext();
        context.JSInterop.Mode = JSRuntimeMode.Loose;
        return context;
    }

    private static Type? GetInputValueType(Type component)
    {
        for (var type = component; type is not null; type = type.BaseType)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(InputBase<>))
                return type.GetGenericArguments()[0];
        }

        return null;
    }

    private sealed class TestModel
    {
        [Required]
        public string? Text { get; set; }

        public string[]? Services { get; set; }
        public string? StartDate { get; set; }
    }
}
