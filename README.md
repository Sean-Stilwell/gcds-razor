# GCDS Blazor wrappers

Thin Blazor components backed by the official `@gcds-core/components` web components. The library does not reproduce GCDS markup or CSS.

The library exposes a Blazor component for every custom element shipped by the pinned GCDS package, including supporting elements such as `GcdsBreadcrumbsItem`, `GcdsGridCol`, `GcdsNavGroup`, and `GcdsNavLink`. Component parameters use PascalCase and are emitted as their kebab-case GCDS attributes; for example, `CardTitleTag` maps to `card-title-tag`.

## Setup

Render `<GcdsAssets />` once near the root of the consuming app (for example in `App.razor`). It loads the official stylesheet and ES module into the document head. Loading is idempotent, so accidental duplicate instances still create each asset exactly once.

The pinned GCDS version is configured in `GcdsVersion.Components`. Upgrade that single constant after reviewing the [GCDS changelog](https://github.com/cds-snc/gcds-components/blob/main/CHANGELOG.md).

Use `<RawGcdsDemo />` in a development route to verify that unwrapped GCDS elements load before diagnosing wrapper behavior.

## Examples

```razor
<GcdsButton Type="GcdsButtonType.Submit" OnClick="Save">Save</GcdsButton>

<EditForm Model="model" OnValidSubmit="Save">
    <DataAnnotationsValidator />
    <GcdsInput Id="email" Name="email" Label="Email address"
               Type="GcdsInputType.Email" Required @bind-Value="model.Email" />
    <GcdsSelect Id="province" Name="province" Label="Province"
                DefaultValue="Select a province" @bind-Value="model.Province">
        <option value="on">Ontario</option>
        <option value="qc">Quebec</option>
    </GcdsSelect>
    <GcdsTextarea Id="summary" Name="summary" Label="Summary"
                  Required @bind-Value="model.Summary" />
    <GcdsRadios Name="contact" Legend="Preferred contact method"
                Options="contactOptions" @bind-Value="model.ContactMethod" />
    <GcdsCheckboxes Name="topics" Legend="Topics"
                    Options="topicOptions" @bind-Value="model.Topics" />
    <GcdsDateInput Name="start-date" Legend="Start date"
                   @bind-Value="model.StartDate" />
</EditForm>
```

`GcdsInput`, `GcdsSelect`, `GcdsTextarea`, `GcdsRadios`, `GcdsDateInput`, and `GcdsSearch` bind as `string?`. `GcdsCheckboxes` and `GcdsFileUploader` bind as `string[]?`. All derive from Blazor's `InputBase<TValue>`, so `@bind-Value`, `EditContext` field changes, data-annotation messages, and GCDS validation events work through one component API. Unmatched attributes pass through to the underlying custom element.

```razor
<GcdsAlert Heading="Service update" AlertRole="info">
    Scheduled maintenance begins at 8 p.m.
</GcdsAlert>

<GcdsCard CardTitle="Application guide" Href="/guide"
          Description="Learn how to submit an application." />

<GcdsBreadcrumbs>
    <GcdsBreadcrumbsItem Href="/">Home</GcdsBreadcrumbsItem>
    <GcdsBreadcrumbsItem Href="/services">Services</GcdsBreadcrumbsItem>
</GcdsBreadcrumbs>
```

Structured JavaScript properties such as checkbox `Options`, table `Columns`, and table `Data` accept either the JSON string supported by GCDS or a serializable .NET object. Use `AdditionalAttributes` for native or newly introduced upstream attributes that are not yet represented by a typed parameter.

Wrappers expose GCDS custom events as Blazor callbacks such as `OnClick`, `OnChange`, `OnDismiss`, `OnSubmit`, and `OnTableStateChange`. Event detail is available as JSON and can be converted to a .NET type:

```razor
<GcdsAlert Heading="Saved" OnDismiss="HandleDismiss">Changes saved.</GcdsAlert>

@code {
    private void HandleDismiss(GcdsEventArgs args)
    {
        // args.Name is "gcdsDismiss"; use args.GetDetail<T>() when detail is present.
    }
}
```
