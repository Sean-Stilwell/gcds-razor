namespace GcdsWrapper.Blazor.Markdown;

/// <summary>Information about a link selected in rendered Markdown.</summary>
/// <param name="Href">The link destination from the Markdown source.</param>
/// <param name="Title">The optional link title.</param>
/// <param name="IsExternal">Whether the link targets a different HTTP or HTTPS origin.</param>
public sealed record GcdsMarkdownLinkEventArgs(string Href, string? Title, bool IsExternal);
