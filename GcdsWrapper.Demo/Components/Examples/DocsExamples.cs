using System.Linq.Expressions;
using GcdsWrapper.Blazor;
using GcdsWrapper.Demo.Components.Docs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace GcdsWrapper.Demo.Components.Examples;

internal abstract class DocsExampleBase : ComponentBase
{
    private string? textValue = "example";
    private string[]? checkboxValues = ["email"];
    private string[]? uploadedFiles;

    [Parameter] public DocsLanguage Language { get; set; }

    protected abstract string Key { get; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var descriptor = DocsCatalog.FindByKey(Key)
            ?? throw new InvalidOperationException($"Unknown documentation example '{Key}'.");

        builder.OpenComponent<DynamicComponent>(0);
        builder.AddAttribute(1, nameof(DynamicComponent.Type), descriptor.WrapperType);
        builder.AddAttribute(2, nameof(DynamicComponent.Parameters), Parameters());
        builder.CloseComponent();
    }

    private IDictionary<string, object> Parameters()
    {
        var french = Language == DocsLanguage.French;
        var parameters = Key switch
        {
            "assets" => New(),
            "alert" => New(("Heading", french ? "Mise à jour du service" : "Service update"),
                ("ChildContent", Text(french ? "Le service est disponible." : "The service is available."))),
            "breadcrumbs" => New(("ChildContent", Breadcrumbs(french))),
            "breadcrumbs-item" => New(("Href", "#preview"), ("ChildContent", Text(french ? "Accueil" : "Home"))),
            "button" => New(("Role", GcdsButtonRole.Primary), ("ChildContent", Text(french ? "Enregistrer" : "Save"))),
            "card" => New(("CardTitle", french ? "Guide de demande du GC" : "GC application guide"), ("Href", "#preview"),
                ("Description", french ? "Un exemple de carte Blazor." : "An example Blazor card."),
                ("ChildContent", CardContent(french))),
            "checkboxes" => New(("Name", "topics"), ("Legend", french ? "Sujets" : "Topics"),
                ("Options", CheckboxOptions(french))),
            "container" => New(("Size", "md"), ("Padding", "300"),
                ("ChildContent", FormattedText(french ? "Contenu du conteneur" : "Container content"))),
            "date-input" => New(("Name", "birth-date"), ("Legend", french ? "Date de naissance" : "Birth date"),
                ("Autocomplete", "bday")),
            "date-modified" => New(("ChildContent", Text("2026-08-26"))),
            "details" => New(("DetailsTitle", french ? "Renseignements supplémentaires" : "More information"),
                ("ChildContent", FormattedText(french ? "Détails utiles." : "Helpful details."))),
            "error-message" => New(("ChildContent", Text(french ? "Saisissez une valeur." : "Enter a value."))),
            "error-summary" => New(("ErrorLinks", french
                ? new Dictionary<string, string>
                {
                    ["#nom"] = "Saisissez votre nom",
                    ["#courriel"] = "Saisissez une adresse courriel valide"
                }
                : new Dictionary<string, string>
                {
                    ["#name"] = "Enter your name",
                    ["#email"] = "Enter a valid email address"
                })),
            "fieldset" => New(("Legend", french ? "Coordonnées" : "Contact information"), ("LegendSize", "h2"),
                ("ChildContent", Text(french ? "Champs connexes" : "Related fields"))),
            "file-uploader" => New(("Id", "documents"), ("Name", "documents"), ("Label", french ? "Documents" : "Documents"),
                ("Accept", ".pdf")),
            "footer" => New(("Lang", french ? "fr" : "en"), ("Display", "compact"),
                ("ContextualHeading", french ? "Système de design GC" : "GC Design System"),
                ("ContextualLinks", FooterLinks(french))),
            "grid" => New(("Columns", "1fr 1fr"), ("Gap", "300"), ("ChildContent", GridContent(french))),
            "grid-col" => New(("Desktop", 6), ("ChildContent", FormattedText(french ? "Colonne de grille" : "Grid column"))),
            "header" => New(("Lang", french ? "fr" : "en"),
                ("LangHref", french ? "en/components" : "fr/composants"), ("SkipToHref", "#main-content")),
            "heading" => New(("Tag", "h2"), ("ChildContent", Text(french ? "Titre de section" : "Section heading"))),
            "hint" => New(("ChildContent", Text(french ? "Contexte utile." : "Helpful context."))),
            "icon" => New(("Name", "info-circle"), ("Label", "Information"), ("Size", "h3")),
            "input" => New(("Id", "email"), ("Name", "email"), ("Label", french ? "Adresse courriel" : "Email address"),
                ("Type", GcdsInputType.Email)),
            "label" => New(("Label", french ? "Adresse courriel" : "Email address"), ("LabelFor", "email-preview"),
                ("ChildContent", FormattedText(french ? "Adresse courriel" : "Email address"))),
            "lang-toggle" => New(("Href", french ? "en/components" : "fr/composants")),
            "link" => New(("Href", "#preview"), ("ChildContent", Text(french ? "Lire le guide" : "Read the guide"))),
            "nav-group" => New(("MenuLabel", french ? "Guides" : "Guides"), ("OpenTrigger", french ? "Ouvrir les guides" : "Open guides"),
                ("CloseTrigger", french ? "Fermer les guides" : "Close guides"), ("Open", true), ("ChildContent", NavLinks(french))),
            "nav-link" => New(("Href", "#preview"), ("Current", true), ("ChildContent", Text(french ? "Composants" : "Components"))),
            "notice" => New(("NoticeRole", "info"), ("NoticeTitle", french ? "Remarque" : "Note"), ("NoticeTitleTag", "h2"),
                ("ChildContent", FormattedText(french ? "Renseignements importants." : "Important information."))),
            "pagination" => New(("Label", french ? "Résultats" : "Results"), ("Display", "simple"),
                ("PreviousHref", "#previous"), ("NextHref", "#next")),
            "radios" => New(("Name", "contact"), ("Legend", french ? "Mode de communication" : "Contact method"),
                ("Options", RadioOptions(french))),
            "search" => New(("Id", "site-search"), ("Name", "q"), ("Placeholder", french ? "Rechercher" : "Search")),
            "select" => New(("Id", "province"), ("Name", "province"), ("Label", "Province"),
                ("DefaultValue", french ? "Sélectionnez une province" : "Select a province"), ("ChildContent", SelectOptions(french))),
            "side-nav" => New(("Label", french ? "Documentation" : "Documentation"), ("ChildContent", NavLinks(french))),
            "signature" => New(("Lang", french ? "fr" : "en"),
                ("Type", "signature"), ("Variant", "colour"), ("HasLink", false)),
            "sr-only" => New(("ChildContent", Text(french ? "Contexte supplémentaire" : "Additional context"))),
            "stepper" => New(("CurrentStep", 2), ("TotalSteps", 4), ("Tag", "h2"),
                ("ChildContent", Text(french ? "Vérifiez votre demande" : "Review your application"))),
            "table" => New(("Columns", new[]
                {
                    new { field = "name", header = french ? "Nom" : "Name", rowHeader = true },
                    new { field = "status", header = french ? "Statut" : "Status", rowHeader = false }
                }),
                ("Data", new[]
                {
                    new { name = "Alice Martin", status = french ? "Actif" : "Active" },
                    new { name = "Benoît Roy", status = french ? "En attente" : "Pending" },
                    new { name = "Chen Li", status = french ? "Terminé" : "Complete" }
                }),
                ("Sort", true)),
            "text" => New(("ChildContent", Text(french ? "Corps du texte." : "Body text."))),
            "textarea" => New(("Id", "summary"), ("Name", "summary"), ("Label", french ? "Résumé" : "Summary"), ("Rows", 4)),
            "top-nav" => New(("Label", french ? "Navigation principale" : "Main navigation"), ("ChildContent", TopNavLinks(french))),
            "topic-menu" => New(("Lang", french ? "fr" : "en"), ("Home", true)),
            _ => throw new InvalidOperationException($"No parameters are defined for '{Key}'.")
        };

        if (Key == "checkboxes")
        {
            parameters["Value"] = checkboxValues!;
            parameters["ValueChanged"] = EventCallback.Factory.Create<string[]?>(this, value => checkboxValues = value);
            parameters["ValueExpression"] = (Expression<Func<string[]?>>)(() => checkboxValues);
        }
        else if (Key == "file-uploader")
        {
            parameters["Value"] = uploadedFiles!;
            parameters["ValueChanged"] = EventCallback.Factory.Create<string[]?>(this, value => uploadedFiles = value);
            parameters["ValueExpression"] = (Expression<Func<string[]?>>)(() => uploadedFiles);
        }
        else if (DocsCatalog.FindByKey(Key)?.SupportsBinding == true)
        {
            if (Key == "date-input" && textValue == "example") textValue = null;
            if (Key == "radios" && textValue == "example") textValue = "email";
            parameters["Value"] = textValue!;
            parameters["ValueChanged"] = EventCallback.Factory.Create<string?>(this, value => textValue = value);
            parameters["ValueExpression"] = (Expression<Func<string?>>)(() => textValue);
        }

        return parameters;
    }

    private static Dictionary<string, object> New(params (string Name, object Value)[] values) =>
        values.ToDictionary(item => item.Name, item => item.Value);

    private static GcdsCheckboxOption[] CheckboxOptions(bool french) =>
    [
        new("topics-email", french ? "Courriel" : "Email", "email"),
        new("topics-phone", french ? "Téléphone" : "Telephone", "phone")
    ];

    private static GcdsRadioOption[] RadioOptions(bool french) =>
    [
        new("contact-email", french ? "Courriel" : "Email", "email"),
        new("contact-phone", french ? "Téléphone" : "Telephone", "phone")
    ];

    private static Dictionary<string, string> FooterLinks(bool french) => french
        ? new()
        {
            ["Composants"] = "#composants",
            ["Styles"] = "#styles",
            ["Ressources"] = "#ressources"
        }
        : new()
        {
            ["Components"] = "#components",
            ["Styles"] = "#styles",
            ["Resources"] = "#resources"
        };

    private static RenderFragment Text(string value) => builder => builder.AddContent(0, value);

    private static RenderFragment FormattedText(string value) => builder =>
    {
        builder.OpenComponent<GcdsText>(0);
        builder.AddAttribute(1, nameof(GcdsComponentBase.ChildContent), Text(value));
        builder.CloseComponent();
    };

    private static RenderFragment CardContent(bool french) => builder =>
    {
        builder.OpenElement(0, "span");
        builder.AddAttribute(1, "slot", "title");

        if (french)
            builder.AddContent(2, "Guide de demande du ");

        builder.OpenElement(3, "abbr");
        builder.AddAttribute(4, "title", french ? "Gouvernement du Canada" : "Government of Canada");
        builder.AddContent(5, "GC");
        builder.CloseElement();

        if (!french)
            builder.AddContent(6, " application guide");

        builder.CloseElement();
    };

    private static RenderFragment Breadcrumbs(bool french) => builder =>
    {
        builder.OpenComponent<GcdsBreadcrumbsItem>(0);
        builder.AddAttribute(1, nameof(GcdsBreadcrumbsItem.Href), "#home");
        builder.AddAttribute(2, nameof(GcdsComponentBase.ChildContent), Text(french ? "Accueil" : "Home"));
        builder.CloseComponent();

        builder.OpenComponent<GcdsBreadcrumbsItem>(3);
        builder.AddAttribute(4, nameof(GcdsBreadcrumbsItem.Href), "#components");
        builder.AddAttribute(5, nameof(GcdsComponentBase.ChildContent), Text(french ? "Composants" : "Components"));
        builder.CloseComponent();
    };

    private static RenderFragment NavLinks(bool french) => builder =>
    {
        builder.OpenComponent<GcdsNavLink>(0);
        builder.AddAttribute(1, nameof(GcdsNavLink.Href), "#preview");
        builder.AddAttribute(2, nameof(GcdsComponentBase.ChildContent), Text(french ? "Composants" : "Components"));
        builder.CloseComponent();
    };

    private static RenderFragment TopNavLinks(bool french) => builder =>
    {
        builder.OpenComponent<GcdsNavLink>(0);
        builder.AddAttribute(1, nameof(GcdsNavLink.Href), french ? "/fr" : "/");
        builder.AddAttribute(2, nameof(GcdsComponentBase.AdditionalAttributes),
            new Dictionary<string, object> { ["slot"] = "home" });
        builder.AddAttribute(3, nameof(GcdsComponentBase.ChildContent), Text(french ? "Accueil" : "Home"));
        builder.CloseComponent();

        builder.OpenComponent<GcdsNavLink>(4);
        builder.AddAttribute(5, nameof(GcdsNavLink.Href), french ? "/fr/services" : "/services");
        builder.AddAttribute(6, nameof(GcdsComponentBase.ChildContent), Text(french ? "Services" : "Services"));
        builder.CloseComponent();
    };

    private static RenderFragment GridContent(bool french) => builder =>
    {
        builder.OpenComponent<GcdsText>(0);
        builder.AddAttribute(1, nameof(GcdsComponentBase.ChildContent), Text(french ? "Première colonne" : "First column"));
        builder.CloseComponent();
        builder.OpenComponent<GcdsText>(2);
        builder.AddAttribute(3, nameof(GcdsComponentBase.ChildContent), Text(french ? "Deuxième colonne" : "Second column"));
        builder.CloseComponent();
    };

    private static RenderFragment SelectOptions(bool french) => builder =>
    {
        (string Value, string English, string French)[] regions =
        [
            ("ab", "Alberta", "Alberta"),
            ("bc", "British Columbia", "Colombie-Britannique"),
            ("mb", "Manitoba", "Manitoba"),
            ("nb", "New Brunswick", "Nouveau-Brunswick"),
            ("nl", "Newfoundland and Labrador", "Terre-Neuve-et-Labrador"),
            ("nt", "Northwest Territories", "Territoires du Nord-Ouest"),
            ("ns", "Nova Scotia", "Nouvelle-Écosse"),
            ("nu", "Nunavut", "Nunavut"),
            ("on", "Ontario", "Ontario"),
            ("pe", "Prince Edward Island", "Île-du-Prince-Édouard"),
            ("qc", "Quebec", "Québec"),
            ("sk", "Saskatchewan", "Saskatchewan"),
            ("yt", "Yukon", "Yukon")
        ];
        var sequence = 0;

        foreach (var region in regions)
        {
            builder.OpenElement(sequence++, "option");
            builder.AddAttribute(sequence++, "value", region.Value);
            builder.AddContent(sequence++, french ? region.French : region.English);
            builder.CloseElement();
        }
    };
}

internal sealed class GcdsAssetsExample : DocsExampleBase { public GcdsAssetsExample() { } protected override string Key => "assets"; }
internal sealed class GcdsAlertExample : DocsExampleBase { public GcdsAlertExample() { } protected override string Key => "alert"; }
internal sealed class GcdsBreadcrumbsExample : DocsExampleBase { public GcdsBreadcrumbsExample() { } protected override string Key => "breadcrumbs"; }
internal sealed class GcdsBreadcrumbsItemExample : DocsExampleBase { public GcdsBreadcrumbsItemExample() { } protected override string Key => "breadcrumbs-item"; }
internal sealed class GcdsButtonExample : DocsExampleBase { public GcdsButtonExample() { } protected override string Key => "button"; }
internal sealed class GcdsCardExample : DocsExampleBase { public GcdsCardExample() { } protected override string Key => "card"; }
internal sealed class GcdsCheckboxesExample : DocsExampleBase { public GcdsCheckboxesExample() { } protected override string Key => "checkboxes"; }
internal sealed class GcdsContainerExample : DocsExampleBase { public GcdsContainerExample() { } protected override string Key => "container"; }
internal sealed class GcdsDateInputExample : DocsExampleBase { public GcdsDateInputExample() { } protected override string Key => "date-input"; }
internal sealed class GcdsDateModifiedExample : DocsExampleBase { public GcdsDateModifiedExample() { } protected override string Key => "date-modified"; }
internal sealed class GcdsDetailsExample : DocsExampleBase { public GcdsDetailsExample() { } protected override string Key => "details"; }
internal sealed class GcdsErrorMessageExample : DocsExampleBase { public GcdsErrorMessageExample() { } protected override string Key => "error-message"; }
internal sealed class GcdsErrorSummaryExample : DocsExampleBase { public GcdsErrorSummaryExample() { } protected override string Key => "error-summary"; }
internal sealed class GcdsFieldsetExample : DocsExampleBase { public GcdsFieldsetExample() { } protected override string Key => "fieldset"; }
internal sealed class GcdsFileUploaderExample : DocsExampleBase { public GcdsFileUploaderExample() { } protected override string Key => "file-uploader"; }
internal sealed class GcdsFooterExample : DocsExampleBase { public GcdsFooterExample() { } protected override string Key => "footer"; }
internal sealed class GcdsGridExample : DocsExampleBase { public GcdsGridExample() { } protected override string Key => "grid"; }
internal sealed class GcdsGridColExample : DocsExampleBase { public GcdsGridColExample() { } protected override string Key => "grid-col"; }
internal sealed class GcdsHeaderExample : DocsExampleBase { public GcdsHeaderExample() { } protected override string Key => "header"; }
internal sealed class GcdsHeadingExample : DocsExampleBase { public GcdsHeadingExample() { } protected override string Key => "heading"; }
internal sealed class GcdsHintExample : DocsExampleBase { public GcdsHintExample() { } protected override string Key => "hint"; }
internal sealed class GcdsIconExample : DocsExampleBase { public GcdsIconExample() { } protected override string Key => "icon"; }
internal sealed class GcdsInputExample : DocsExampleBase { public GcdsInputExample() { } protected override string Key => "input"; }
internal sealed class GcdsLabelExample : DocsExampleBase { public GcdsLabelExample() { } protected override string Key => "label"; }
internal sealed class GcdsLangToggleExample : DocsExampleBase { public GcdsLangToggleExample() { } protected override string Key => "lang-toggle"; }
internal sealed class GcdsLinkExample : DocsExampleBase { public GcdsLinkExample() { } protected override string Key => "link"; }
internal sealed class GcdsNavGroupExample : DocsExampleBase { public GcdsNavGroupExample() { } protected override string Key => "nav-group"; }
internal sealed class GcdsNavLinkExample : DocsExampleBase { public GcdsNavLinkExample() { } protected override string Key => "nav-link"; }
internal sealed class GcdsNoticeExample : DocsExampleBase { public GcdsNoticeExample() { } protected override string Key => "notice"; }
internal sealed class GcdsPaginationExample : DocsExampleBase { public GcdsPaginationExample() { } protected override string Key => "pagination"; }
internal sealed class GcdsRadiosExample : DocsExampleBase { public GcdsRadiosExample() { } protected override string Key => "radios"; }
internal sealed class GcdsSearchExample : DocsExampleBase { public GcdsSearchExample() { } protected override string Key => "search"; }
internal sealed class GcdsSelectExample : DocsExampleBase { public GcdsSelectExample() { } protected override string Key => "select"; }
internal sealed class GcdsSideNavExample : DocsExampleBase { public GcdsSideNavExample() { } protected override string Key => "side-nav"; }
internal sealed class GcdsSignatureExample : DocsExampleBase { public GcdsSignatureExample() { } protected override string Key => "signature"; }
internal sealed class GcdsSrOnlyExample : DocsExampleBase { public GcdsSrOnlyExample() { } protected override string Key => "sr-only"; }
internal sealed class GcdsStepperExample : DocsExampleBase { public GcdsStepperExample() { } protected override string Key => "stepper"; }
internal sealed class GcdsTableExample : DocsExampleBase { public GcdsTableExample() { } protected override string Key => "table"; }
internal sealed class GcdsTextExample : DocsExampleBase { public GcdsTextExample() { } protected override string Key => "text"; }
internal sealed class GcdsTextareaExample : DocsExampleBase { public GcdsTextareaExample() { } protected override string Key => "textarea"; }
internal sealed class GcdsTopNavExample : DocsExampleBase { public GcdsTopNavExample() { } protected override string Key => "top-nav"; }
internal sealed class GcdsTopicMenuExample : DocsExampleBase { public GcdsTopicMenuExample() { } protected override string Key => "topic-menu"; }
