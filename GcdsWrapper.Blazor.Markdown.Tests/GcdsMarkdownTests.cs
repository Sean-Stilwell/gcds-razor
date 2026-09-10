using Bunit;
using GcdsWrapper.Blazor;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace GcdsWrapper.Blazor.Markdown.Tests;

public sealed class GcdsMarkdownTests
{
    [Fact]
    public void CoreMarkdown_UsesGcdsComponentsAndSemanticFallbacks()
    {
        using var context = CreateContext();
        const string markdown = """
            # Service **guide**

            Read the *instructions*, ~~ignore this~~, and use `code`.

            > Important information.

            - First
              - Nested
            - Second

            1. One
            2. Two

            ---
            """;

        var component = context.Render<GcdsMarkdown>(parameters => parameters
            .Add(item => item.Value, markdown));

        Assert.Equal("h1", component.Find("gcds-heading").GetAttribute("tag"));
        Assert.Equal("Service guide", component.Find("gcds-heading").TextContent.Trim());
        Assert.Equal("guide", component.Find("gcds-heading strong").TextContent);
        Assert.Contains("Read the instructions", component.Find("gcds-text").TextContent);
        Assert.Equal("instructions", component.Find("gcds-text em").TextContent);
        Assert.Equal("ignore this", component.Find("gcds-text del").TextContent);
        Assert.Equal("code", component.Find("gcds-text code").TextContent);
        Assert.Equal("Important information.", component.Find("blockquote p").TextContent.Trim());
        Assert.Equal(2, component.FindAll("article > ul > li").Count);
        Assert.Single(component.FindAll("article > ul > li > ul"));
        Assert.Equal("1", component.Find("article > ol").GetAttribute("start") ?? "1");
        Assert.Single(component.FindAll("hr"));
    }

    [Fact]
    public void ExtendedMarkdown_RendersTablesTasksImagesAndFencedCode()
    {
        using var context = CreateContext();
        const string markdown = """
            | Name | Status |
            | --- | --- |
            | **Alpha** | [Active](/status) |

            - [x] Complete
            - [ ] Pending

            ![GC wordmark](/images/gc.svg "Government of Canada")

            ```csharp
            Console.WriteLine("Bonjour");
            ```
            """;

        var component = context.Render<GcdsMarkdown>(parameters => parameters
            .Add(item => item.Value, markdown));

        Assert.Equal(["Name", "Status"], component.FindAll("thead th").Select(cell => cell.TextContent.Trim()));
        Assert.Equal("Alpha", component.Find("tbody strong").TextContent);
        Assert.Equal("/status", component.Find("tbody gcds-link").GetAttribute("href"));
        var tasks = component.FindAll("input[type='checkbox']");
        Assert.Equal(2, tasks.Count);
        Assert.All(tasks, task => Assert.True(task.HasAttribute("disabled")));
        Assert.True(tasks[0].HasAttribute("checked"));
        Assert.False(tasks[1].HasAttribute("checked"));
        Assert.Equal("Completed task", tasks[0].GetAttribute("aria-label"));
        var image = component.Find("img");
        Assert.Equal("GC wordmark", image.GetAttribute("alt"));
        Assert.Equal("Government of Canada", image.GetAttribute("title"));
        var code = component.Find("pre > code");
        Assert.Equal("language-csharp", code.GetAttribute("class"));
        Assert.Contains("Console.WriteLine", code.TextContent);
    }

    [Fact]
    public void AutolinksAndHardBreaks_RenderWithExpectedSemantics()
    {
        using var context = CreateContext();
        var component = context.Render<GcdsMarkdown>(parameters => parameters
            .Add(item => item.Value, "Visit <https://example.com>  \nNext line"));

        var link = component.Find("gcds-link");
        Assert.Equal("https://example.com", link.GetAttribute("href"));
        Assert.Equal("true", link.GetAttribute("external"));
        Assert.Single(component.FindAll("gcds-text br"));
        Assert.Contains("Next line", component.Find("gcds-text").TextContent);
    }

    [Theory]
    [InlineData("[unfinished")]
    [InlineData("```csharp\nunterminated")]
    [InlineData("<div>unterminated")]
    public void MalformedOrIncompleteMarkdown_DoesNotThrow(string markdown)
    {
        using var context = CreateContext();

        var exception = Record.Exception(() => context.Render<GcdsMarkdown>(parameters => parameters
            .Add(item => item.Value, markdown)));

        Assert.Null(exception);
    }

    [Fact]
    public void Headings_HaveUniqueIdentifiersAndApplyOffset()
    {
        using var context = CreateContext();
        var component = context.Render<GcdsMarkdown>(parameters => parameters
            .Add(item => item.Value, "# Overview\n\n# Overview\n\n##### Deep")
            .Add(item => item.HeadingLevelOffset, 2));

        var headings = component.FindAll("gcds-heading");
        Assert.Equal(["h3", "h3", "h6"], headings.Select(heading => heading.GetAttribute("tag")));
        Assert.Equal(3, headings.Select(heading => heading.Id).Distinct().Count());
        Assert.All(headings, heading => Assert.False(string.IsNullOrWhiteSpace(heading.Id)));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(6)]
    public void HeadingOffset_RejectsValuesOutsideSupportedRange(int offset)
    {
        using var context = CreateContext();

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            context.Render<GcdsMarkdown>(parameters => parameters
                .Add(item => item.Value, "# Heading")
                .Add(item => item.HeadingLevelOffset, offset)));

        Assert.Equal("HeadingLevelOffset", exception.ParamName);
    }

    [Fact]
    public void Links_DistinguishNavigationKindsAndProtectExternalTargets()
    {
        using var context = CreateContext();
        const string markdown = """
            [Relative](/guide)
            [Anchor](#section)
            [Email](mailto:help@example.ca)
            [Same origin](http://localhost/same)
            [External](https://example.com/path "Example")
            """;

        var component = context.Render<GcdsMarkdown>(parameters => parameters.Add(item => item.Value, markdown));
        var links = component.FindAll("gcds-link");

        Assert.Equal(5, links.Count);
        Assert.All(links.Take(4), link =>
        {
            Assert.Equal("false", link.GetAttribute("external"));
            Assert.Null(link.GetAttribute("target"));
            Assert.Null(link.GetAttribute("rel"));
        });
        Assert.Equal("true", links[4].GetAttribute("external"));
        Assert.Equal("_blank", links[4].GetAttribute("target"));
        Assert.Equal("noopener noreferrer", links[4].GetAttribute("rel"));
        Assert.Equal("Example", links[4].GetAttribute("title"));
    }

    [Fact]
    public async Task LinkCallback_ReportsDestinationWithoutChangingNavigationAttributes()
    {
        using var context = CreateContext();
        GcdsMarkdownLinkEventArgs? received = null;
        var component = context.Render<GcdsMarkdown>(parameters => parameters
            .Add(item => item.Value, "[External](https://example.com \"Example\")")
            .Add(item => item.OnLinkClick, EventCallback.Factory.Create<GcdsMarkdownLinkEventArgs>(
                this, args => received = args)));

        var linkComponent = component.FindComponent<GcdsLink>();
        await component.InvokeAsync(() => linkComponent.Instance.HandleGcdsEvent("gcdsClick", null));

        Assert.Equal(new GcdsMarkdownLinkEventArgs("https://example.com", "Example", true), received);
        Assert.Equal("_blank", linkComponent.Find("gcds-link").GetAttribute("target"));
    }

    [Fact]
    public void RawHtml_IsDroppedUnlessExplicitlyAllowed()
    {
        using var context = CreateContext();
        const string markdown = "Before\n\n<script>alert('unsafe')</script>\n\n<span data-test=\"trusted\">After</span>";

        var safe = context.Render<GcdsMarkdown>(parameters => parameters.Add(item => item.Value, markdown));
        Assert.Empty(safe.FindAll("script"));
        Assert.Empty(safe.FindAll("span[data-test='trusted']"));
        Assert.DoesNotContain("unsafe", safe.Markup);

        var trusted = context.Render<GcdsMarkdown>(parameters => parameters
            .Add(item => item.Value, markdown)
            .Add(item => item.AllowHtml, true));
        Assert.Single(trusted.FindAll("script"));
        Assert.Single(trusted.FindAll("span[data-test='trusted']"));
    }

    [Fact]
    public void RootAttributes_MergeClassesAndPreserveOtherAttributes()
    {
        using var context = CreateContext();
        var component = context.Render<GcdsMarkdown>(parameters => parameters
            .Add(item => item.Value, "Bonjour")
            .AddUnmatched("class", "article-content")
            .AddUnmatched("id", "guide")
            .AddUnmatched("lang", "fr"));

        var article = component.Find("article");
        Assert.Equal("gcds-markdown article-content", article.ClassName);
        Assert.Equal("guide", article.Id);
        Assert.Equal("fr", article.GetAttribute("lang"));
    }

    [Fact]
    public void ValueChanges_RerenderAndNullOrEmptyValuesRemainValid()
    {
        using var context = CreateContext();
        var component = context.Render<GcdsMarkdown>(parameters => parameters.Add(item => item.Value, "First"));
        Assert.Equal("First", component.Find("gcds-text").TextContent);

        component.Render(parameters => parameters.Add(item => item.Value, "Second — deuxième"));
        Assert.Equal("Second — deuxième", component.Find("gcds-text").TextContent);

        component.Render(parameters => parameters.Add(item => item.Value, null));
        Assert.Empty(component.Find("article").Children);

        component.Render(parameters => parameters.Add(item => item.Value, string.Empty));
        Assert.Empty(component.Find("article").Children);
    }

    private static BunitContext CreateContext()
    {
        var context = new BunitContext();
        context.JSInterop.Mode = JSRuntimeMode.Loose;
        return context;
    }
}
