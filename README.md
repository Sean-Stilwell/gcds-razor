# GCDS Blazor wrappers

Thin Blazor components backed by the official `@gcds-core/components` web components. The library does not reproduce GCDS markup or CSS.

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
</EditForm>
```

`GcdsInput` and `GcdsSelect` derive from Blazor's `InputBase<string?>`, so `@bind-Value`, `EditContext` field changes, data-annotation messages, and GCDS validation events work through one component API. Unmatched attributes pass through to the underlying custom element.
