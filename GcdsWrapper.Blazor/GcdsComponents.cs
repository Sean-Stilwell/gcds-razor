using Microsoft.AspNetCore.Components;

namespace GcdsWrapper.Blazor;

public sealed class GcdsAlert : GcdsComponentBase
{
    protected override string TagName => "gcds-alert";
    [Parameter, EditorRequired] public string Heading { get; set; } = default!;
    [Parameter] public string? AlertRole { get; set; }
    [Parameter] public string? Container { get; set; }
    [Parameter] public bool? HideCloseBtn { get; set; }
    [Parameter] public bool? HideRoleIcon { get; set; }
    [Parameter] public bool? IsFixed { get; set; }
}

public sealed class GcdsBreadcrumbs : GcdsComponentBase
{
    protected override string TagName => "gcds-breadcrumbs";
    [Parameter] public bool? HideCanadaLink { get; set; }
}

public sealed class GcdsBreadcrumbsItem : GcdsComponentBase
{
    protected override string TagName => "gcds-breadcrumbs-item";
    [Parameter, EditorRequired] public string Href { get; set; } = default!;
}

public sealed class GcdsCard : GcdsComponentBase
{
    protected override string TagName => "gcds-card";
    [Parameter, EditorRequired] public string CardTitle { get; set; } = default!;
    [Parameter, EditorRequired] public string Href { get; set; } = default!;
    [Parameter] public string? Badge { get; set; }
    [Parameter] public string? CardTitleTag { get; set; }
    [Parameter] public string? Description { get; set; }
    [Parameter] public string? ImgAlt { get; set; }
    [Parameter] public string? ImgSrc { get; set; }
    [Parameter] public string? Rel { get; set; }
    [Parameter] public string? Target { get; set; }
}

public sealed class GcdsCheckboxes : GcdsComponentBase
{
    protected override string TagName => "gcds-checkboxes";
    [Parameter, EditorRequired] public string Name { get; set; } = default!;
    [Parameter, EditorRequired] public string Legend { get; set; } = default!;
    [Parameter, EditorRequired] public object Options { get; set; } = default!;
    [Parameter] public bool? Autofocus { get; set; }
    [Parameter] public bool? Disabled { get; set; }
    [Parameter] public string? ErrorMessage { get; set; }
    [Parameter] public string? Form { get; set; }
    [Parameter] public bool? HideLabel { get; set; }
    [Parameter] public bool? HideLegend { get; set; }
    [Parameter] public string? Hint { get; set; }
    [Parameter] public bool? Required { get; set; }
    [Parameter] public string? ValidateOn { get; set; }
    [Parameter] public object? Value { get; set; }
}

public sealed class GcdsContainer : GcdsComponentBase
{
    protected override string TagName => "gcds-container";
    [Parameter] public string? Alignment { get; set; }
    [Parameter] public bool? Border { get; set; }
    [Parameter] public string? Layout { get; set; }
    [Parameter] public string? Margin { get; set; }
    [Parameter] public string? Padding { get; set; }
    [Parameter] public string? Size { get; set; }
    [Parameter] public string? Tag { get; set; }
}

public sealed class GcdsDateInput : GcdsComponentBase
{
    protected override string TagName => "gcds-date-input";
    [Parameter, EditorRequired] public string Name { get; set; } = default!;
    [Parameter, EditorRequired] public string Legend { get; set; } = default!;
    [Parameter, EditorRequired] public string Format { get; set; } = default!;
    [Parameter] public bool? Autofocus { get; set; }
    [Parameter] public bool? Disabled { get; set; }
    [Parameter] public string? ErrorMessage { get; set; }
    [Parameter] public string? Form { get; set; }
    [Parameter] public string? Hint { get; set; }
    [Parameter] public string? Max { get; set; }
    [Parameter] public string? Min { get; set; }
    [Parameter] public bool? Required { get; set; }
    [Parameter] public string? ValidateOn { get; set; }
    [Parameter] public string? Value { get; set; }
}

public sealed class GcdsDateModified : GcdsComponentBase
{
    protected override string TagName => "gcds-date-modified";
    [Parameter] public string? Type { get; set; }
}

public sealed class GcdsDetails : GcdsComponentBase
{
    protected override string TagName => "gcds-details";
    [Parameter, EditorRequired] public string DetailsTitle { get; set; } = default!;
    [Parameter] public bool? Open { get; set; }
}

public sealed class GcdsErrorMessage : GcdsComponentBase
{
    protected override string TagName => "gcds-error-message";
    [Parameter] public string? MessageId { get; set; }
}

public sealed class GcdsErrorSummary : GcdsComponentBase
{
    protected override string TagName => "gcds-error-summary";
    [Parameter, EditorRequired] public object ErrorLinks { get; set; } = default!;
    [Parameter] public string? Heading { get; set; }
    [Parameter] public bool? Listen { get; set; }
}

public sealed class GcdsFieldset : GcdsComponentBase
{
    protected override string TagName => "gcds-fieldset";
    [Parameter, EditorRequired] public string Legend { get; set; } = default!;
    [Parameter, EditorRequired] public string LegendSize { get; set; } = default!;
    [Parameter] public string? Hint { get; set; }
}

public sealed class GcdsFileUploader : GcdsComponentBase
{
    protected override string TagName => "gcds-file-uploader";
    [Parameter, EditorRequired] public string UploaderId { get; set; } = default!;
    [Parameter, EditorRequired] public string Name { get; set; } = default!;
    [Parameter, EditorRequired] public string Label { get; set; } = default!;
    [Parameter] public string? Accept { get; set; }
    [Parameter] public bool? Autofocus { get; set; }
    [Parameter] public bool? Disabled { get; set; }
    [Parameter] public string? ErrorMessage { get; set; }
    [Parameter] public string? Form { get; set; }
    [Parameter] public bool? HideLabel { get; set; }
    [Parameter] public string? Hint { get; set; }
    [Parameter] public bool? Multiple { get; set; }
    [Parameter] public bool? Required { get; set; }
    [Parameter] public string? ValidateOn { get; set; }
    [Parameter] public string[]? Value { get; set; }
}

public sealed class GcdsFooter : GcdsComponentBase
{
    protected override string TagName => "gcds-footer";
    [Parameter] public string? ContextualHeading { get; set; }
    [Parameter] public object? ContextualLinks { get; set; }
    [Parameter] public string? Display { get; set; }
    [Parameter] public object? SubLinks { get; set; }
}

public sealed class GcdsGrid : GcdsComponentBase
{
    protected override string TagName => "gcds-grid";
    [Parameter] public string? AlignContent { get; set; }
    [Parameter] public string? AlignItems { get; set; }
    [Parameter] public string? Alignment { get; set; }
    [Parameter] public string? Columns { get; set; }
    [Parameter] public string? ColumnsDesktop { get; set; }
    [Parameter] public string? ColumnsTablet { get; set; }
    [Parameter] public string? Container { get; set; }
    [Parameter] public string? Display { get; set; }
    [Parameter] public bool? EqualRowHeight { get; set; }
    [Parameter] public string? Gap { get; set; }
    [Parameter] public string? GapDesktop { get; set; }
    [Parameter] public string? GapTablet { get; set; }
    [Parameter] public string? JustifyContent { get; set; }
    [Parameter] public string? JustifyItems { get; set; }
    [Parameter] public string? PlaceContent { get; set; }
    [Parameter] public string? PlaceItems { get; set; }
    [Parameter] public string? Tag { get; set; }
}

public sealed class GcdsGridCol : GcdsComponentBase
{
    protected override string TagName => "gcds-grid-col";
    [Parameter] public int? Desktop { get; set; }
    [Parameter] public int? Tablet { get; set; }
    [Parameter] public string? Tag { get; set; }
}

public sealed class GcdsHeader : GcdsComponentBase
{
    protected override string TagName => "gcds-header";
    [Parameter, EditorRequired] public string LangHref { get; set; } = default!;
    [Parameter, EditorRequired] public string SkipToHref { get; set; } = default!;
    [Parameter] public bool? SignatureHasLink { get; set; }
}

public sealed class GcdsHeading : GcdsComponentBase
{
    protected override string TagName => "gcds-heading";
    [Parameter, EditorRequired] public string Tag { get; set; } = default!;
    [Parameter] public bool? CharacterLimit { get; set; }
    [Parameter] public string? HeadingRole { get; set; }
    [Parameter] public string? MarginBottom { get; set; }
    [Parameter] public string? MarginTop { get; set; }
}

public sealed class GcdsHint : GcdsComponentBase
{
    protected override string TagName => "gcds-hint";
    [Parameter] public string? HintId { get; set; }
}

public sealed class GcdsIcon : GcdsComponentBase
{
    protected override string TagName => "gcds-icon";
    [Parameter, EditorRequired] public string Name { get; set; } = default!;
    [Parameter] public string? Label { get; set; }
    [Parameter] public string? MarginLeft { get; set; }
    [Parameter] public string? MarginRight { get; set; }
    [Parameter] public string? Size { get; set; }
}

public sealed class GcdsLabel : GcdsComponentBase
{
    protected override string TagName => "gcds-label";
    [Parameter] public bool? HideLabel { get; set; }
    [Parameter] public string? Label { get; set; }
    [Parameter] public string? LabelFor { get; set; }
    [Parameter] public bool? Required { get; set; }
}

public sealed class GcdsLangToggle : GcdsComponentBase
{
    protected override string TagName => "gcds-lang-toggle";
    [Parameter, EditorRequired] public string Href { get; set; } = default!;
}

public sealed class GcdsLink : GcdsComponentBase
{
    protected override string TagName => "gcds-link";
    [Parameter, EditorRequired] public string Href { get; set; } = default!;
    [Parameter] public string? Display { get; set; }
    [Parameter] public string? Download { get; set; }
    [Parameter] public bool? External { get; set; }
    [Parameter] public string? LinkRole { get; set; }
    [Parameter] public string? Rel { get; set; }
    [Parameter] public string? Size { get; set; }
    [Parameter] public string? Target { get; set; }
    [Parameter] public string? Type { get; set; }
}

public sealed class GcdsNavGroup : GcdsComponentBase
{
    protected override string TagName => "gcds-nav-group";
    [Parameter, EditorRequired] public string MenuLabel { get; set; } = default!;
    [Parameter, EditorRequired] public string OpenTrigger { get; set; } = default!;
    [Parameter] public string? CloseTrigger { get; set; }
    [Parameter] public bool? Open { get; set; }
}

public sealed class GcdsNavLink : GcdsComponentBase
{
    protected override string TagName => "gcds-nav-link";
    [Parameter, EditorRequired] public string Href { get; set; } = default!;
    [Parameter] public bool? Current { get; set; }
}

public sealed class GcdsNotice : GcdsComponentBase
{
    protected override string TagName => "gcds-notice";
    [Parameter, EditorRequired] public string NoticeRole { get; set; } = default!;
    [Parameter, EditorRequired] public string NoticeTitle { get; set; } = default!;
    [Parameter, EditorRequired] public string NoticeTitleTag { get; set; } = default!;
}

public sealed class GcdsPagination : GcdsComponentBase
{
    protected override string TagName => "gcds-pagination";
    [Parameter, EditorRequired] public string Label { get; set; } = default!;
    [Parameter] public int? CurrentPage { get; set; }
    [Parameter] public string? Display { get; set; }
    [Parameter] public string? NextHref { get; set; }
    [Parameter] public string? NextLabel { get; set; }
    [Parameter] public string? PreviousHref { get; set; }
    [Parameter] public string? PreviousLabel { get; set; }
    [Parameter] public int? TotalPages { get; set; }
    [Parameter] public object? Url { get; set; }
}

public sealed class GcdsRadios : GcdsComponentBase
{
    protected override string TagName => "gcds-radios";
    [Parameter, EditorRequired] public string Name { get; set; } = default!;
    [Parameter, EditorRequired] public string Legend { get; set; } = default!;
    [Parameter, EditorRequired] public object Options { get; set; } = default!;
    [Parameter] public bool? Autofocus { get; set; }
    [Parameter] public bool? Disabled { get; set; }
    [Parameter] public string? ErrorMessage { get; set; }
    [Parameter] public string? Form { get; set; }
    [Parameter] public bool? HideLegend { get; set; }
    [Parameter] public string? Hint { get; set; }
    [Parameter] public bool? Required { get; set; }
    [Parameter] public string? ValidateOn { get; set; }
    [Parameter] public string? Value { get; set; }
}

public sealed class GcdsSearch : GcdsComponentBase
{
    protected override string TagName => "gcds-search";
    [Parameter] public string? Action { get; set; }
    [Parameter] public string? Method { get; set; }
    [Parameter] public string? Name { get; set; }
    [Parameter] public string? Placeholder { get; set; }
    [Parameter] public string? SearchId { get; set; }
    [Parameter] public object? Suggested { get; set; }
    [Parameter] public string? Value { get; set; }
}

public sealed class GcdsSideNav : GcdsComponentBase
{
    protected override string TagName => "gcds-side-nav";
    [Parameter, EditorRequired] public string Label { get; set; } = default!;
}

public sealed class GcdsSignature : GcdsComponentBase
{
    protected override string TagName => "gcds-signature";
    [Parameter] public bool? HasLink { get; set; }
    [Parameter] public string? Type { get; set; }
    [Parameter] public string? Variant { get; set; }
}

public sealed class GcdsSrOnly : GcdsComponentBase
{
    protected override string TagName => "gcds-sr-only";
    [Parameter] public string? Tag { get; set; }
}

public sealed class GcdsStepper : GcdsComponentBase
{
    protected override string TagName => "gcds-stepper";
    [Parameter, EditorRequired] public int CurrentStep { get; set; }
    [Parameter, EditorRequired] public int TotalSteps { get; set; }
    [Parameter] public string? Tag { get; set; }
}

public sealed class GcdsTable : GcdsComponentBase
{
    protected override string TagName => "gcds-table";
    [Parameter] public object? Columns { get; set; }
    [Parameter] public object? Data { get; set; }
    [Parameter] public bool? Filter { get; set; }
    [Parameter] public string? FilterValue { get; set; }
    [Parameter] public bool? Pagination { get; set; }
    [Parameter] public int? PaginationCurrentPage { get; set; }
    [Parameter] public int? PaginationSize { get; set; }
    [Parameter] public object? PaginationSizeOptions { get; set; }
    [Parameter] public bool? Sort { get; set; }
}

public sealed class GcdsText : GcdsComponentBase
{
    protected override string TagName => "gcds-text";
    [Parameter] public bool? CharacterLimit { get; set; }
    [Parameter] public string? Display { get; set; }
    [Parameter] public string? MarginBottom { get; set; }
    [Parameter] public string? MarginTop { get; set; }
    [Parameter] public string? Size { get; set; }
    [Parameter] public string? TextRole { get; set; }
}

public sealed class GcdsTextarea : GcdsComponentBase
{
    protected override string TagName => "gcds-textarea";
    [Parameter, EditorRequired] public string TextareaId { get; set; } = default!;
    [Parameter, EditorRequired] public string Name { get; set; } = default!;
    [Parameter, EditorRequired] public string Label { get; set; } = default!;
    [Parameter] public bool? Autofocus { get; set; }
    [Parameter] public int? Cols { get; set; }
    [Parameter] public bool? Disabled { get; set; }
    [Parameter] public string? ErrorMessage { get; set; }
    [Parameter] public string? Form { get; set; }
    [Parameter] public bool? HideLabel { get; set; }
    [Parameter] public bool? HideLimit { get; set; }
    [Parameter] public string? Hint { get; set; }
    [Parameter] public int? Maxlength { get; set; }
    [Parameter] public int? Minlength { get; set; }
    [Parameter] public bool? Required { get; set; }
    [Parameter] public int? Rows { get; set; }
    [Parameter] public string? ValidateOn { get; set; }
    [Parameter] public string? Value { get; set; }
}

public sealed class GcdsTopNav : GcdsComponentBase
{
    protected override string TagName => "gcds-top-nav";
    [Parameter, EditorRequired] public string Label { get; set; } = default!;
    [Parameter] public string? Alignment { get; set; }
}

public sealed class GcdsTopicMenu : GcdsComponentBase
{
    protected override string TagName => "gcds-topic-menu";
    [Parameter] public bool? Home { get; set; }
}
