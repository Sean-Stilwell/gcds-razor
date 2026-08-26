using Bunit;
using GcdsWrapper.Demo.Components.Docs;
using GcdsWrapper.Demo.Components.Layout;
using GcdsWrapper.Demo.Components.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GcdsWrapper.Blazor.Tests;

public sealed class DocsCatalogTests
{
    [Fact]
    public void Catalogue_ContainsEveryPublicWrapperExactlyOnce()
    {
        var expected = typeof(GcdsAssets).Assembly.GetTypes()
            .Where(type => type.Namespace == typeof(GcdsAssets).Namespace)
            .Where(type => !type.IsAbstract && typeof(IComponent).IsAssignableFrom(type))
            .ToHashSet();
        var actual = DocsCatalog.All.Select(item => item.WrapperType).ToArray();

        Assert.Equal(42, DocsCatalog.All.Count);
        Assert.Equal(actual.Length, actual.Distinct().Count());
        Assert.True(expected.SetEquals(actual));
    }

    [Fact]
    public void Catalogue_HasCompleteUniqueBilingualMetadata()
    {
        Assert.Equal(DocsCatalog.All.Count, DocsCatalog.All.Select(item => item.Key).Distinct().Count());
        Assert.Equal(DocsCatalog.All.Count, DocsCatalog.All.Select(item => item.Slug.English).Distinct().Count());
        Assert.Equal(DocsCatalog.All.Count, DocsCatalog.All.Select(item => item.Slug.French).Distinct().Count());

        Assert.All(DocsCatalog.All, item =>
        {
            Assert.False(string.IsNullOrWhiteSpace(item.Title.English));
            Assert.False(string.IsNullOrWhiteSpace(item.Title.French));
            Assert.False(string.IsNullOrWhiteSpace(item.SnippetEnglish));
            Assert.False(string.IsNullOrWhiteSpace(item.SnippetFrench));
            Assert.False(string.IsNullOrWhiteSpace(item.Notes.English));
            Assert.False(string.IsNullOrWhiteSpace(item.Notes.French));
            Assert.StartsWith("https://", item.OfficialUrlFor(DocsLanguage.English));
            Assert.StartsWith("https://", item.OfficialUrlFor(DocsLanguage.French));
            Assert.True(typeof(IComponent).IsAssignableFrom(item.ExampleType));
            Assert.NotNull(item.ExampleType.GetConstructor(Type.EmptyTypes));
            if (!item.IsAssetLoader)
                Assert.NotEmpty(DocsApiReader.Read(item));
        });
    }

    [Fact]
    public void Routes_ResolveBothLanguagesAndPreserveEquivalentComponent()
    {
        var input = DocsCatalog.FindByKey("input")!;

        Assert.Same(input, DocsCatalog.Find(DocsLanguage.English, input.Slug.English));
        Assert.Same(input, DocsCatalog.Find(DocsLanguage.French, input.Slug.French));
        Assert.Equal("/en/components/input", input.RouteFor(DocsLanguage.English));
        Assert.Equal("/fr/composants/champ-saisie", input.RouteFor(DocsLanguage.French));
        Assert.Same(input, DocsCatalog.FindByPath("fr/composants/champ-saisie"));
    }

    [Fact]
    public void FormApi_UsesBindValueAndRetainsAdditionalAttributes()
    {
        var parameters = DocsApiReader.Read(DocsCatalog.FindByKey("input")!);

        Assert.Contains(parameters, parameter => parameter.Name == "@bind-Value");
        Assert.Contains(parameters, parameter => parameter.Name == "AdditionalAttributes");
        Assert.DoesNotContain(parameters, parameter => parameter.Name is "Value" or "ValueChanged" or "ValueExpression");
    }

    [Fact]
    public void SideNavigation_GroupsItemsAndMarksCurrentRoute()
    {
        using var context = CreateContext();
        var current = DocsCatalog.FindByKey("button")!;

        var component = context.Render<DocsSideNavigation>(parameters => parameters
            .Add(item => item.Language, DocsLanguage.English)
            .Add(item => item.Current, current));

        Assert.Equal(Enum.GetValues<DocsCategory>().Length, component.FindAll("gcds-nav-group").Count);
        Assert.Contains(component.FindAll("gcds-nav-link"), link =>
            link.GetAttribute("href")?.EndsWith(current.RouteFor(DocsLanguage.English), StringComparison.Ordinal) == true &&
            link.GetAttribute("current") == "true");
        Assert.Equal("true", component.FindAll("gcds-nav-group")
            .Single(group => group.GetAttribute("menu-label") == DocsCatalog.CategoryName(current.Category).English)
            .GetAttribute("open"));
        Assert.Equal("Component documentation", component.Find("nav").GetAttribute("aria-label"));
    }

    [Fact]
    public void ComponentPage_RendersPreviewUsageApiBindingAndUpstreamLink()
    {
        using var context = CreateContext();
        var navigation = context.Services.GetRequiredService<NavigationManager>();
        navigation.NavigateTo("en/components/input");

        var component = context.Render<ComponentReference>(parameters => parameters.Add(page => page.Slug, "input"));

        Assert.NotEmpty(component.FindAll(".docs-preview gcds-input"));
        Assert.Contains("@bind-Value", component.Markup);
        Assert.Contains("EditForm", component.Markup);
        Assert.Contains("AdditionalAttributes", component.Markup);
        Assert.Contains("https://design-system.canada.ca/en/components/input/", component.Markup);
    }

    [Fact]
    public void RootRoute_SelectsFrenchFromBrowserPreference()
    {
        using var context = CreateContext();
        context.JSInterop.Setup<string>("gcdsWrapperDocs.preferredLanguage").SetResult("fr");
        var navigation = context.Services.GetRequiredService<NavigationManager>();

        context.Render<RootRedirect>();

        Assert.EndsWith("/fr/composants", navigation.Uri);
    }

    [Fact]
    public void RootRoute_FallsBackToEnglishForUnsupportedPreference()
    {
        using var context = CreateContext();
        context.JSInterop.Setup<string>("gcdsWrapperDocs.preferredLanguage").SetResult("es");
        var navigation = context.Services.GetRequiredService<NavigationManager>();

        context.Render<RootRedirect>();

        Assert.EndsWith("/en/components", navigation.Uri);
    }

    private static BunitContext CreateContext()
    {
        var context = new BunitContext();
        context.JSInterop.Mode = JSRuntimeMode.Loose;
        return context;
    }
}
