# GCDS Wrapper for Blazor

Thin, idiomatic Blazor wrappers for the official [GC Design System (GCDS) components](https://design-system.canada.ca/en/). The library maps Razor parameters and events to the upstream web components; it does not reimplement their markup or styles.

This version targets **.NET 10** and pins `@gcds-core/components` **1.4.0**.

## What is included

- A Blazor component for every custom element in the pinned GCDS release
- Typed parameters for commonly used GCDS attributes, with unmatched attributes passed through
- `EditForm` integration, two-way binding, and validation for GCDS form controls
- Blazor callbacks for GCDS custom events
- Automatic, idempotent loading of the pinned GCDS stylesheet and JavaScript module
- A bilingual interactive documentation site in `GcdsWrapper.Demo`

Available wrappers include:

`GcdsAlert`, `GcdsBreadcrumbs`, `GcdsBreadcrumbsItem`, `GcdsButton`, `GcdsCard`, `GcdsCheckboxes`, `GcdsContainer`, `GcdsDateInput`, `GcdsDateModified`, `GcdsDetails`, `GcdsErrorMessage`, `GcdsErrorSummary`, `GcdsFieldset`, `GcdsFileUploader`, `GcdsFooter`, `GcdsGrid`, `GcdsGridCol`, `GcdsHeader`, `GcdsHeading`, `GcdsHint`, `GcdsIcon`, `GcdsInput`, `GcdsLabel`, `GcdsLangToggle`, `GcdsLink`, `GcdsNavGroup`, `GcdsNavLink`, `GcdsNotice`, `GcdsPagination`, `GcdsRadios`, `GcdsSearch`, `GcdsSelect`, `GcdsSideNav`, `GcdsSignature`, `GcdsSrOnly`, `GcdsStepper`, `GcdsTable`, `GcdsText`, `GcdsTextarea`, `GcdsTopNav`, and `GcdsTopicMenu`.

## Installation

Install the package from NuGet:

```shell
dotnet add YourApp/YourApp.csproj package GcdsWrapper.Blazor
```

When developing against this repository, reference the library project directly instead:

```shell
dotnet add YourApp/YourApp.csproj reference GcdsWrapper.Blazor/GcdsWrapper.Blazor.csproj
```

Add the namespace to your app's `_Imports.razor`:

```razor
@using GcdsWrapper.Blazor
```

Render `GcdsAssets` once near the root of the app, such as in `App.razor`:

```razor
<GcdsAssets />
```

`GcdsAssets` loads the official stylesheet and ES module from the GCDS CDN. Loading is idempotent, so duplicate instances do not add duplicate assets.

## Basic usage

Component parameters use PascalCase and are emitted as the corresponding kebab-case GCDS attributes. For example, `CardTitleTag` becomes `card-title-tag`.

```razor
<GcdsAlert Heading="Service update" AlertRole="info">
    Scheduled maintenance begins at 8 p.m.
</GcdsAlert>

<GcdsCard CardTitle="Application guide"
          CardTitleTag="h2"
          Href="/guide"
          Description="Learn how to submit an application." />

<GcdsBreadcrumbs>
    <GcdsBreadcrumbsItem Href="/">Home</GcdsBreadcrumbsItem>
    <GcdsBreadcrumbsItem Href="/services">Services</GcdsBreadcrumbsItem>
</GcdsBreadcrumbs>
```

Use `AdditionalAttributes` for native attributes or upstream GCDS attributes that do not yet have a typed parameter.

## Forms and validation

The form wrappers derive from Blazor's `InputBase<TValue>`, so they support `@bind-Value`, `EditContext` field notifications, data-annotation validation, and GCDS validation events.

- `GcdsInput`, `GcdsSelect`, `GcdsTextarea`, `GcdsRadios`, `GcdsDateInput`, and `GcdsSearch` bind to `string?`.
- `GcdsCheckboxes` and `GcdsFileUploader` bind to `string[]?`.

```razor
<EditForm Model="model" OnValidSubmit="Save">
    <DataAnnotationsValidator />

    <GcdsInput Id="email"
               Name="email"
               Label="Email address"
               Type="GcdsInputType.Email"
               Required
               @bind-Value="model.Email" />

    <GcdsSelect Id="province"
                Name="province"
                Label="Province"
                DefaultValue="Select a province"
                @bind-Value="model.Province">
        <option value="on">Ontario</option>
        <option value="qc">Quebec</option>
    </GcdsSelect>

    <GcdsTextarea Id="summary"
                  Name="summary"
                  Label="Summary"
                  Required
                  @bind-Value="model.Summary" />

    <GcdsButton Type="GcdsButtonType.Submit">Save</GcdsButton>
</EditForm>
```

Checkbox, radio, and autocomplete data can be supplied with `GcdsCheckboxOption`, `GcdsRadioOption`, and `GcdsSuggestionOption`. Structured properties such as options, table columns, and table data also accept the JSON string supported by GCDS or a serializable .NET object.

## Events

Wrappers expose supported custom events as Blazor callbacks, including `OnBlur`, `OnChange`, `OnClick`, `OnDismiss`, `OnFocus`, `OnInput`, `OnRemoveFile`, `OnSubmit`, `OnTableStateChange`, and validation callbacks. General wrapper events use `GcdsEventArgs`, whose detail can be deserialized to a .NET type.

```razor
<GcdsAlert Heading="Saved" OnDismiss="HandleDismiss">
    Changes saved.
</GcdsAlert>

@code {
    private void HandleDismiss(GcdsEventArgs args)
    {
        // args.Name is "gcdsDismiss".
        // Use args.GetDetail<T>() when the event includes detail.
    }
}
```

Form wrappers provide strongly typed value callbacks where applicable, such as `OnInput`, `OnChange`, `OnSubmit`, and `OnSuggestionSelected`.

## Interactive documentation

Run the bilingual WebAssembly documentation site:

```shell
dotnet run --project GcdsWrapper.Demo/GcdsWrapper.Demo.csproj
```

Open `/en/components` or `/fr/composants`. Each component page provides a live example, Razor usage, reflected parameter metadata, supported events, and a link to the matching official GCDS guidance.

## Development

```shell
dotnet restore GcdsWrapper.Blazor/GcdsWrapper.Blazor.csproj
dotnet build GcdsWrapper.Blazor/GcdsWrapper.Blazor.csproj
dotnet test --project GcdsWrapper.Blazor.Tests/GcdsWrapper.Blazor.Tests.csproj
dotnet format GcdsWrapper.Blazor/GcdsWrapper.Blazor.csproj --verify-no-changes
```

To create and verify the release packages:

```shell
dotnet pack GcdsWrapper.Blazor/GcdsWrapper.Blazor.csproj -c Release -o artifacts
```

The GitHub Pages workflow publishes the static demo output on pushes to `main`.

## Updating GCDS

The upstream version is centralized in `GcdsVersion.Components`. Change that constant only after reviewing the [GCDS changelog](https://github.com/cds-snc/gcds-components/blob/main/CHANGELOG.md), then build the library, run the tests, and exercise the demo against the new assets.
