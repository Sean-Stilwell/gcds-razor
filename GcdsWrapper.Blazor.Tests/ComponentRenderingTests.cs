using Bunit;
using Xunit;

namespace GcdsWrapper.Blazor.Tests;

public sealed class ComponentRenderingTests
{
    [Fact]
    public void Assets_UsePinnedGcdsVersion()
    {
        Assert.Equal("1.6.0", GcdsVersion.Components);
        Assert.Equal(
            "https://cdn.design-system.canada.ca/@gcds-core/components@1.6.0/dist/gcds",
            GcdsVersion.CdnBase);
    }

    [Fact]
    public void Card_RendersRichTitleSlotWithTextFallback()
    {
        using var context = new BunitContext();
        context.JSInterop.Mode = JSRuntimeMode.Loose;

        var component = context.Render<GcdsCard>(parameters => parameters
            .Add(card => card.CardTitle, "GC application guide")
            .Add(card => card.Href, "/guide")
            .AddChildContent(builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddAttribute(1, "slot", "title");
                builder.OpenElement(2, "abbr");
                builder.AddAttribute(3, "title", "Government of Canada");
                builder.AddContent(4, "GC");
                builder.CloseElement();
                builder.AddContent(5, " application guide");
                builder.CloseElement();
            }));

        var card = component.Find("gcds-card");
        var title = component.Find("gcds-card > span[slot='title']");

        Assert.Equal("GC application guide", card.GetAttribute("card-title"));
        Assert.Equal("Government of Canada", title.QuerySelector("abbr")?.GetAttribute("title"));
        Assert.Equal("GC application guide", title.TextContent);
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

    [Theory]
    [InlineData("en")]
    [InlineData("fr")]
    public void PaginationAndStepper_RenderSelectedLanguage(string language)
    {
        using var context = new BunitContext();
        context.JSInterop.Mode = JSRuntimeMode.Loose;

        var pagination = context.Render<GcdsPagination>(parameters => parameters
            .Add(item => item.Label, "Results")
            .Add(item => item.Lang, language));
        var stepper = context.Render<GcdsStepper>(parameters => parameters
            .Add(item => item.CurrentStep, 2)
            .Add(item => item.TotalSteps, 4)
            .Add(item => item.Lang, language));

        Assert.Equal(language, pagination.Find("gcds-pagination").GetAttribute("lang"));
        Assert.Equal(language, stepper.Find("gcds-stepper").GetAttribute("lang"));
    }
}
