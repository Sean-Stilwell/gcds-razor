# GCDS Markdown for Blazor

![Markdown package NuGet Version](https://img.shields.io/nuget/v/GcdsWrapper.Blazor.Markdown)
![Markdown package NuGet Downloads](https://img.shields.io/nuget/dt/GcdsWrapper.Blazor.Markdown)
![Markdown package GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/Sean-Stilwell/gcds-razor/.github%2Fworkflows%2Fmarkdown-ci.yml)

Render Markdown as GC Design System components in Blazor applications. Headings, paragraphs, and links use `GcdsWrapper.Blazor`; lists, tables, blockquotes, images, and code use accessible semantic HTML styled with GCDS design tokens.

## Installation

```shell
dotnet add package GcdsWrapper.Blazor.Markdown
```

Add both namespaces to `_Imports.razor` and render the core asset loader once near the root of the application:

```razor
@using GcdsWrapper.Blazor
@using GcdsWrapper.Blazor.Markdown

<GcdsAssets />
```

## Usage

```razor
<GcdsMarkdown Value="@markdown" />

@code {
    private const string markdown = """
        # Service guide

        Read the **application instructions** before [starting](/apply).
        """;
}
```

Use `HeadingLevelOffset` when embedding a document below an existing page heading. `OnLinkClick` observes link selections without cancelling normal navigation.

Raw HTML is not rendered by default. Only set `AllowHtml="true"` for Markdown from a source you completely trust; enabled HTML is emitted without sanitization.

The initial release does not include remote-file loading, a table of contents, syntax highlighting, copy buttons, math rendering, an editor, or a public custom Markdig pipeline.
