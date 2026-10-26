using Bunit;
using GcdsWrapper.Demo.Components.Layout;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using Xunit;
using MarkdownPage = GcdsWrapper.Demo.Components.Pages.Markdown;

namespace GcdsWrapper.Blazor.Markdown.Tests;

public sealed class MarkdownDocsTests
{
    [Theory]
    [InlineData("en/markdown", "GCDS Markdown", "Raw HTML is ignored by default")]
    [InlineData("fr/markdown", "Markdown avec GCDS", "Le HTML brut est ignoré par défaut")]
    public void MarkdownPage_RendersLocalizedExampleAndGuidance(
        string route,
        string expectedHeading,
        string expectedWarning)
    {
        using var context = CreateContext();
        context.Services.GetRequiredService<NavigationManager>().NavigateTo(route);

        var page = context.Render<MarkdownPage>();

        Assert.Equal(expectedHeading, page.Find("h1, gcds-heading[tag='h1']").TextContent.Trim());
        Assert.Single(page.FindAll(".docs-preview article.gcds-markdown"));
        Assert.Equal("h2", page.Find(".docs-preview gcds-heading").GetAttribute("tag"));
        Assert.Contains("GcdsWrapper.Blazor.Markdown", page.Markup);
        Assert.Contains(expectedWarning, page.Markup);
        Assert.Contains("HeadingLevelOffset", page.Markup);
        Assert.Contains("OnLinkClick", page.Markup);
        Assert.Contains(page.FindAll(".docs-code code"), code =>
            code.TextContent.Contains("private const string markdown", StringComparison.Ordinal));
        Assert.Contains(page.FindAll(".docs-code code"), code =>
            code.TextContent.Contains(IsFrenchRoute(route) ? "| Étape | État |" : "| Step | Status |", StringComparison.Ordinal) &&
            code.TextContent.Contains(IsFrenchRoute(route) ? "- [x] Lire le guide" : "- [x] Read the guide", StringComparison.Ordinal) &&
            code.TextContent.Contains(IsFrenchRoute(route) ? "- [ ] Envoyer la demande" : "- [ ] Submit the application", StringComparison.Ordinal));

        var table = page.Find("gcds-table");
        using var columns = JsonDocument.Parse(table.GetAttribute("columns")!);
        using var rows = JsonDocument.Parse(table.GetAttribute("data")!);
        Assert.Equal(IsFrenchRoute(route) ? "Nom" : "Name",
            columns.RootElement[0].GetProperty("header").GetString());
        Assert.True(columns.RootElement[0].GetProperty("rowHeader").GetBoolean());
        Assert.Equal(IsFrenchRoute(route) ? "Valeur par défaut" : "Default",
            columns.RootElement[2].GetProperty("header").GetString());
        Assert.Equal(4, rows.RootElement.GetArrayLength());
        Assert.Equal("Value", rows.RootElement[0].GetProperty("name").GetString());
        Assert.Equal(IsFrenchRoute(route) ? "Texte Markdown à rendre." : "Markdown text to render.",
            rows.RootElement[0].GetProperty("description").GetString());
        Assert.Empty(page.FindAll("table.docs-api"));
    }

    [Theory]
    [InlineData("en/markdown", "http://localhost/fr/markdown")]
    [InlineData("fr/markdown", "http://localhost/en/markdown")]
    public void Layout_PreservesMarkdownDestinationWhenSwitchingLanguage(string route, string expectedHref)
    {
        using var context = CreateContext();
        var navigation = context.Services.GetRequiredService<NavigationManager>();
        navigation.NavigateTo(route);

        var layout = context.Render<DocsLayout>(parameters => parameters
            .Add(item => item.Body, builder => builder.AddContent(0, "Markdown")));

        Assert.Equal(expectedHref, layout.Find("gcds-lang-toggle").GetAttribute("href"));
        var markdownLink = layout.FindAll("gcds-nav-link")
            .Single(link => link.TextContent.Trim() == "Markdown");
        Assert.Equal("true", markdownLink.GetAttribute("current"));
        Assert.Equal("Markdown", layout.FindAll("gcds-side-nav gcds-nav-link").Last().TextContent.Trim());
    }

    private static BunitContext CreateContext()
    {
        var context = new BunitContext();
        context.JSInterop.Mode = JSRuntimeMode.Loose;
        return context;
    }

    private static bool IsFrenchRoute(string route) => route.StartsWith("fr/", StringComparison.Ordinal);
}
