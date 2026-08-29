using Bunit;
using Xunit;

namespace GcdsWrapper.Blazor.Tests;

public sealed class ComponentRenderingTests
{
    [Fact]
    public void Assets_UsePinnedGcdsVersion()
    {
        Assert.Equal("1.5.0", GcdsVersion.Components);
        Assert.Equal(
            "https://cdn.design-system.canada.ca/@gcds-core/components@1.5.0/dist/gcds",
            GcdsVersion.CdnBase);
    }

    [Fact]
    public void BrandingComponents_RenderEnglishLanguageByDefault()
    {
        using var context = new BunitContext();
        context.JSInterop.Mode = JSRuntimeMode.Loose;

        Assert.Equal("en", context.Render<GcdsHeader>().Find("gcds-header").GetAttribute("lang"));
        Assert.Equal("en", context.Render<GcdsFooter>().Find("gcds-footer").GetAttribute("lang"));
        Assert.Equal("en", context.Render<GcdsSignature>().Find("gcds-signature").GetAttribute("lang"));
    }

    [Fact]
    public void BrandingComponents_RenderSelectedFrenchLanguage()
    {
        using var context = new BunitContext();
        context.JSInterop.Mode = JSRuntimeMode.Loose;

        var header = context.Render<GcdsHeader>(parameters => parameters.Add(item => item.Lang, "fr"));
        var footer = context.Render<GcdsFooter>(parameters => parameters.Add(item => item.Lang, "fr"));
        var signature = context.Render<GcdsSignature>(parameters => parameters.Add(item => item.Lang, "fr"));

        Assert.Equal("fr", header.Find("gcds-header").GetAttribute("lang"));
        Assert.Equal("fr", footer.Find("gcds-footer").GetAttribute("lang"));
        Assert.Equal("fr", signature.Find("gcds-signature").GetAttribute("lang"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("EN")]
    [InlineData("de")]
    public void BrandingComponents_RejectUnsupportedLanguage(string language)
    {
        using var context = new BunitContext();
        context.JSInterop.Mode = JSRuntimeMode.Loose;

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            context.Render<GcdsHeader>(parameters => parameters.Add(item => item.Lang, language)));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            context.Render<GcdsFooter>(parameters => parameters.Add(item => item.Lang, language)));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            context.Render<GcdsSignature>(parameters => parameters.Add(item => item.Lang, language)));
    }

    [Theory]
    [InlineData("en")]
    [InlineData("fr")]
    public void TopicMenu_RendersSelectedLanguage(string language)
    {
        using var context = new BunitContext();
        context.JSInterop.Mode = JSRuntimeMode.Loose;

        var component = context.Render<GcdsTopicMenu>(parameters => parameters
            .Add(item => item.Lang, language));

        Assert.Equal(language, component.Find("gcds-topic-menu").GetAttribute("lang"));
    }
}
