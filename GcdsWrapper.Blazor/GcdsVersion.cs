namespace GcdsWrapper.Blazor;

/// <summary>Central configuration for the official GC Design System browser assets.</summary>
public static class GcdsVersion
{
    // Update this value when upgrading GCDS, then verify the wrappers against its changelog.
    public const string Components = "1.6.0";
    public const string CdnBase = "https://cdn.design-system.canada.ca/@gcds-core/components@" + Components + "/dist/gcds";
}
