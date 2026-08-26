namespace GcdsWrapper.Blazor;

/// <summary>An option rendered by <see cref="GcdsCheckboxes"/>.</summary>
public sealed record GcdsCheckboxOption(
    string Id,
    string Label,
    string? Value = null,
    string? Hint = null,
    bool? Checked = null);

/// <summary>An option rendered by <see cref="GcdsRadios"/>.</summary>
public sealed record GcdsRadioOption(
    string Id,
    string Label,
    string Value,
    string? Hint = null,
    bool? Checked = null);

/// <summary>An autocomplete suggestion rendered by <see cref="GcdsInput"/>.</summary>
public sealed record GcdsSuggestionOption(string Label, string? Value = null);
