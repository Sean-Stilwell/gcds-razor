using Bunit;
using GcdsWrapper.Demo.Components.Docs;
using GcdsWrapper.Demo.Components.Layout;
using GcdsWrapper.Demo.Components.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
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
    public void Disclaimer_AppliesIsolatedCssScopeToNativeRoot()
    {
        using var context = CreateContext();

        var component = context.Render<Disclaimer>();
        var root = component.Find("div.disclaimer");

        Assert.Contains(root.Attributes, attribute => attribute.Name.StartsWith("b-", StringComparison.Ordinal));
        Assert.NotNull(root.QuerySelector("gcds-container"));
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
    public void NoticeExample_FormatsContentWithGcdsText()
    {
        using var context = CreateContext();
        var navigation = context.Services.GetRequiredService<NavigationManager>();
        navigation.NavigateTo("en/components/notice");

        var component = context.Render<ComponentReference>(parameters => parameters.Add(page => page.Slug, "notice"));
        var text = component.Find(".docs-preview gcds-notice gcds-text");

        Assert.Equal("Important information.", text.TextContent.Trim());
        Assert.Contains("<GcdsText>Important information.</GcdsText>",
            DocsCatalog.FindByKey("notice")!.SnippetEnglish);
    }

    [Theory]
    [InlineData("container")]
    [InlineData("details")]
    [InlineData("grid")]
    [InlineData("grid-col")]
    [InlineData("label")]
    public void ContentExamples_FormatTextWithGcdsText(string key)
    {
        using var context = CreateContext();
        var descriptor = DocsCatalog.FindByKey(key)!;
        var navigation = context.Services.GetRequiredService<NavigationManager>();
        navigation.NavigateTo(descriptor.RouteFor(DocsLanguage.English));

        var component = context.Render<ComponentReference>(parameters =>
            parameters.Add(page => page.Slug, descriptor.Slug.English));

        Assert.NotEmpty(component.FindAll($".docs-preview gcds-{key} gcds-text"));
        Assert.Contains("<GcdsText>", descriptor.SnippetEnglish);
    }

    [Fact]
    public void TableExample_ProvidesRenderableColumnsRowsAndSampleCode()
    {
        using var context = CreateContext();
        var descriptor = DocsCatalog.FindByKey("table")!;
        var navigation = context.Services.GetRequiredService<NavigationManager>();
        navigation.NavigateTo(descriptor.RouteFor(DocsLanguage.English));

        var component = context.Render<ComponentReference>(parameters =>
            parameters.Add(page => page.Slug, descriptor.Slug.English));
        var table = component.Find(".docs-preview gcds-table");
        using var columns = JsonDocument.Parse(table.GetAttribute("columns")!);
        using var rows = JsonDocument.Parse(table.GetAttribute("data")!);

        Assert.Equal("name", columns.RootElement[0].GetProperty("field").GetString());
        Assert.Equal("Name", columns.RootElement[0].GetProperty("header").GetString());
        Assert.Equal(3, rows.RootElement.GetArrayLength());
        Assert.Equal("Alice Martin", rows.RootElement[0].GetProperty("name").GetString());
        Assert.Contains("private readonly object[] rows", descriptor.SnippetEnglish);
        Assert.Contains("private readonly object[] lignes", descriptor.SnippetFrench);
    }

    [Fact]
    public void StepperExample_ProvidesCurrentStepHeadingText()
    {
        using var context = CreateContext();
        var descriptor = DocsCatalog.FindByKey("stepper")!;
        var navigation = context.Services.GetRequiredService<NavigationManager>();
        navigation.NavigateTo(descriptor.RouteFor(DocsLanguage.English));

        var component = context.Render<ComponentReference>(parameters =>
            parameters.Add(page => page.Slug, descriptor.Slug.English));
        var stepper = component.Find(".docs-preview gcds-stepper");

        Assert.Equal("Review your application", stepper.TextContent.Trim());
        Assert.Contains(">Review your application</GcdsStepper>", descriptor.SnippetEnglish);
        Assert.Contains(">Vérifiez votre demande</GcdsStepper>", descriptor.SnippetFrench);
    }

    [Theory]
    [InlineData(DocsLanguage.English, "en")]
    [InlineData(DocsLanguage.French, "fr")]
    public void TopicMenuExample_UsesPageLanguage(DocsLanguage language, string expected)
    {
        using var context = CreateContext();
        var descriptor = DocsCatalog.FindByKey("topic-menu")!;
        var navigation = context.Services.GetRequiredService<NavigationManager>();
        navigation.NavigateTo(descriptor.RouteFor(language));

        var component = context.Render<ComponentReference>(parameters =>
            parameters.Add(page => page.Slug, descriptor.SlugFor(language)));

        Assert.Equal(expected,
            component.Find(".docs-preview gcds-topic-menu").GetAttribute("lang"));
        Assert.Contains($"Lang=\"{expected}\"", descriptor.SnippetFor(language));
    }

    [Theory]
    [InlineData(DocsLanguage.English, "en")]
    [InlineData(DocsLanguage.French, "fr")]
    public void BrandingExamples_UsePageLanguage(DocsLanguage language, string expected)
    {
        using var context = CreateContext();
        var navigation = context.Services.GetRequiredService<NavigationManager>();

        foreach (var key in new[] { "header", "footer", "signature" })
        {
            var descriptor = DocsCatalog.FindByKey(key)!;
            navigation.NavigateTo(descriptor.RouteFor(language));

            var component = context.Render<ComponentReference>(parameters =>
                parameters.Add(page => page.Slug, descriptor.SlugFor(language)));

            Assert.Equal(expected, component.Find($".docs-preview gcds-{key}").GetAttribute("lang"));
            Assert.Contains($"Lang=\"{expected}\"", descriptor.SnippetFor(language));
        }
    }

    [Theory]
    [InlineData("side-nav")]
    [InlineData("top-nav")]
    public void NavigationUsageExamples_IncludeHomeAndRegularLinks(string key)
    {
        var descriptor = DocsCatalog.FindByKey(key)!;

        foreach (var snippet in new[] { descriptor.SnippetEnglish, descriptor.SnippetFrench })
        {
            Assert.Equal(2, snippet.Split("<GcdsNavLink", StringSplitOptions.None).Length - 1);
            Assert.Contains("slot=\"home\"", snippet);
        }
    }

    [Theory]
    [InlineData(DocsLanguage.English, "British Columbia", "Quebec")]
    [InlineData(DocsLanguage.French, "Colombie-Britannique", "Québec")]
    public void SelectExample_ListsAllProvincesAndTerritories(
        DocsLanguage language,
        string expectedBritishColumbia,
        string expectedQuebec)
    {
        using var context = CreateContext();
        var descriptor = DocsCatalog.FindByKey("select")!;
        var navigation = context.Services.GetRequiredService<NavigationManager>();
        navigation.NavigateTo(descriptor.RouteFor(language));

        var component = context.Render<ComponentReference>(parameters =>
            parameters.Add(page => page.Slug, descriptor.SlugFor(language)));
        var options = component.FindAll(".docs-preview gcds-select option");

        Assert.Equal(13, options.Count);
        Assert.Equal(
            ["ab", "bc", "mb", "nb", "nl", "nt", "ns", "nu", "on", "pe", "qc", "sk", "yt"],
            options.Select(option => option.GetAttribute("value")));
        Assert.Equal(expectedBritishColumbia, options.Single(option => option.GetAttribute("value") == "bc").TextContent);
        Assert.Equal(expectedQuebec, options.Single(option => option.GetAttribute("value") == "qc").TextContent);
    }

    [Fact]
    public void ChoiceAndFileExamples_UseValidInitialAttributes()
    {
        using var context = CreateContext();
        var navigation = context.Services.GetRequiredService<NavigationManager>();

        var checkboxesDescriptor = DocsCatalog.FindByKey("checkboxes")!;
        navigation.NavigateTo(checkboxesDescriptor.RouteFor(DocsLanguage.English));
        var checkboxesPage = context.Render<ComponentReference>(parameters =>
            parameters.Add(page => page.Slug, checkboxesDescriptor.Slug.English));
        var checkboxes = checkboxesPage.Find(".docs-preview gcds-checkboxes");
        using var checkboxOptions = JsonDocument.Parse(checkboxes.GetAttribute("options")!);

        Assert.Equal(["topics-email", "topics-phone"],
            checkboxOptions.RootElement.EnumerateArray().Select(option => option.GetProperty("id").GetString()));
        Assert.All(checkboxOptions.RootElement.EnumerateArray(), option =>
        {
            Assert.False(option.TryGetProperty("hint", out _));
            Assert.False(option.TryGetProperty("checked", out _));
        });
        Assert.Equal("[\"email\"]", checkboxes.GetAttribute("value"));

        var radiosDescriptor = DocsCatalog.FindByKey("radios")!;
        navigation.NavigateTo(radiosDescriptor.RouteFor(DocsLanguage.English));
        var radiosPage = context.Render<ComponentReference>(parameters =>
            parameters.Add(page => page.Slug, radiosDescriptor.Slug.English));
        var radios = radiosPage.Find(".docs-preview gcds-radios");
        using var radioOptions = JsonDocument.Parse(radios.GetAttribute("options")!);

        Assert.Equal(["contact-email", "contact-phone"],
            radioOptions.RootElement.EnumerateArray().Select(option => option.GetProperty("id").GetString()));
        Assert.All(radioOptions.RootElement.EnumerateArray(), option =>
        {
            Assert.False(option.TryGetProperty("hint", out _));
            Assert.False(option.TryGetProperty("checked", out _));
        });
        Assert.Equal("email", radios.GetAttribute("value"));

        var uploaderDescriptor = DocsCatalog.FindByKey("file-uploader")!;
        navigation.NavigateTo(uploaderDescriptor.RouteFor(DocsLanguage.English));
        var uploaderPage = context.Render<ComponentReference>(parameters =>
            parameters.Add(page => page.Slug, uploaderDescriptor.Slug.English));
        var uploader = uploaderPage.Find(".docs-preview gcds-file-uploader");

        Assert.Equal("documents", uploader.GetAttribute("uploader-id"));
        Assert.Null(uploader.GetAttribute("value"));
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
