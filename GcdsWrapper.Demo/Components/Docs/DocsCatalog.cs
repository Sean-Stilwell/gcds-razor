using GcdsWrapper.Demo.Components.Examples;
using GcdsWrapper.Blazor;
using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Reflection;

namespace GcdsWrapper.Demo.Components.Docs;

internal static class DocsCatalog
{
    private static readonly string[] FormEvents = ["OnInput", "OnChange", "OnValid", "OnInvalid", "OnFocus", "OnBlur"];
    private const string TableSnippetEnglish = """
        <GcdsTable Columns="@columns" Data="@rows" Sort />

        @code {
            private readonly object[] columns =
            [
                new { field = "name", header = "Name", rowHeader = true },
                new { field = "status", header = "Status", rowHeader = false }
            ];

            private readonly object[] rows =
            [
                new { name = "Alice Martin", status = "Active" },
                new { name = "Benoît Roy", status = "Pending" },
                new { name = "Chen Li", status = "Complete" }
            ];
        }
        """;
    private const string TableSnippetFrench = """
        <GcdsTable Columns="@colonnes" Data="@lignes" Sort />

        @code {
            private readonly object[] colonnes =
            [
                new { field = "name", header = "Nom", rowHeader = true },
                new { field = "status", header = "Statut", rowHeader = false }
            ];

            private readonly object[] lignes =
            [
                new { name = "Alice Martin", status = "Actif" },
                new { name = "Benoît Roy", status = "En attente" },
                new { name = "Chen Li", status = "Terminé" }
            ];
        }
        """;

    public static readonly IReadOnlyList<DocsComponentDescriptor> All =
    [
        D<GcdsAssets, GcdsAssetsExample>("assets", "Assets", "Ressources", "assets", "ressources", DocsCategory.Supporting,
            "assets", "ressources", "<GcdsAssets />", "<GcdsAssets />", [], asset: true),
        D<GcdsAlert, GcdsAlertExample>("alert", "Alert", "Alerte", "alert", "alerte", DocsCategory.Feedback,
            "alert", "alerte", "<GcdsAlert Heading=\"Service update\">Message</GcdsAlert>", "<GcdsAlert Heading=\"Mise à jour du service\">Message</GcdsAlert>", ["OnDismiss"]),
        D<GcdsBreadcrumbs, GcdsBreadcrumbsExample>("breadcrumbs", "Breadcrumbs", "Fil d’Ariane", "breadcrumbs", "fil-ariane", DocsCategory.Navigation,
            "breadcrumbs", "fil-ariane", "<GcdsBreadcrumbs>...</GcdsBreadcrumbs>", "<GcdsBreadcrumbs>...</GcdsBreadcrumbs>", []),
        D<GcdsBreadcrumbsItem, GcdsBreadcrumbsItemExample>("breadcrumbs-item", "Breadcrumbs item", "Élément du fil d’Ariane", "breadcrumbs-item", "element-fil-ariane", DocsCategory.Supporting,
            "breadcrumbs", "fil-ariane", "<GcdsBreadcrumbsItem Href=\"/\">Home</GcdsBreadcrumbsItem>", "<GcdsBreadcrumbsItem Href=\"/\">Accueil</GcdsBreadcrumbsItem>", ["OnClick", "OnFocus", "OnBlur"]),
        D<GcdsButton, GcdsButtonExample>("button", "Button", "Bouton", "button", "bouton", DocsCategory.Forms,
            "button", "bouton", "<GcdsButton Role=\"GcdsButtonRole.Primary\">Save</GcdsButton>", "<GcdsButton Role=\"GcdsButtonRole.Primary\">Enregistrer</GcdsButton>", ["OnClick"]),
        D<GcdsCard, GcdsCardExample>("card", "Card", "Carte", "card", "carte", DocsCategory.Layout,
            "card", "carte", "<GcdsCard CardTitle=\"Application guide\" Href=\"/guide\" />", "<GcdsCard CardTitle=\"Guide de demande\" Href=\"/guide\" />", ["OnClick", "OnFocus", "OnBlur"]),
        D<GcdsCheckboxes, GcdsCheckboxesExample>("checkboxes", "Checkboxes", "Cases à cocher", "checkboxes", "cases-cocher", DocsCategory.Forms,
            "checkboxes", "cases-cocher", "<GcdsCheckboxes Name=\"topics\" Legend=\"Topics\" Options=\"options\" @bind-Value=\"model.Topics\" />", "<GcdsCheckboxes Name=\"sujets\" Legend=\"Sujets\" Options=\"options\" @bind-Value=\"model.Sujets\" />", FormEvents, bind: true),
        D<GcdsContainer, GcdsContainerExample>("container", "Container", "Conteneur", "container", "conteneur", DocsCategory.Layout,
            "container", "conteneur", "<GcdsContainer Size=\"md\"><GcdsText>Container content</GcdsText></GcdsContainer>", "<GcdsContainer Size=\"md\"><GcdsText>Contenu du conteneur</GcdsText></GcdsContainer>", []),
        D<GcdsDateInput, GcdsDateInputExample>("date-input", "Date input", "Champ de date", "date-input", "champ-date", DocsCategory.Forms,
            "date-input", "champ-date", "<GcdsDateInput Name=\"date\" Legend=\"Start date\" @bind-Value=\"model.Date\" />", "<GcdsDateInput Name=\"date\" Legend=\"Date de début\" @bind-Value=\"model.Date\" />", FormEvents, bind: true),
        D<GcdsDateModified, GcdsDateModifiedExample>("date-modified", "Date modified", "Date de modification", "date-modified", "date-modification", DocsCategory.Data,
            "date-modified", "date-modification", "<GcdsDateModified>2026-08-26</GcdsDateModified>", "<GcdsDateModified>2026-08-26</GcdsDateModified>", []),
        D<GcdsDetails, GcdsDetailsExample>("details", "Details", "Détails", "details", "details", DocsCategory.Layout,
            "details", "details", "<GcdsDetails DetailsTitle=\"More information\"><GcdsText>Helpful details.</GcdsText></GcdsDetails>", "<GcdsDetails DetailsTitle=\"Renseignements supplémentaires\"><GcdsText>Détails utiles.</GcdsText></GcdsDetails>", ["OnClick", "OnFocus", "OnBlur"]),
        D<GcdsErrorMessage, GcdsErrorMessageExample>("error-message", "Error message", "Message d’erreur", "error-message", "message-erreur", DocsCategory.Feedback,
            "error-message", "message-erreur", "<GcdsErrorMessage>Enter a value.</GcdsErrorMessage>", "<GcdsErrorMessage>Saisissez une valeur.</GcdsErrorMessage>", []),
        D<GcdsErrorSummary, GcdsErrorSummaryExample>("error-summary", "Error summary", "Sommaire des erreurs", "error-summary", "sommaire-erreurs", DocsCategory.Feedback,
            "error-summary", "sommaire-erreurs", "<GcdsErrorSummary ErrorLinks=\"errors\" />", "<GcdsErrorSummary ErrorLinks=\"erreurs\" />", ["OnClick", "OnFocus", "OnBlur"]),
        D<GcdsFieldset, GcdsFieldsetExample>("fieldset", "Fieldset", "Groupe de champs", "fieldset", "groupe-champs", DocsCategory.Forms,
            "fieldset", "groupe-champs", "<GcdsFieldset Legend=\"Contact\" LegendSize=\"h2\">...</GcdsFieldset>", "<GcdsFieldset Legend=\"Coordonnées\" LegendSize=\"h2\">...</GcdsFieldset>", []),
        D<GcdsFileUploader, GcdsFileUploaderExample>("file-uploader", "File uploader", "Téléverseur de fichiers", "file-uploader", "televerseur-fichiers", DocsCategory.Forms,
            "file-uploader", "televerseur-fichiers", "<GcdsFileUploader Id=\"files\" Name=\"files\" Label=\"Documents\" @bind-Value=\"model.Files\" />", "<GcdsFileUploader Id=\"fichiers\" Name=\"fichiers\" Label=\"Documents\" @bind-Value=\"model.Fichiers\" />", [.. FormEvents, "OnRemoveFile"], bind: true),
        D<GcdsFooter, GcdsFooterExample>("footer", "Footer", "Pied de page", "footer", "pied-page", DocsCategory.Branding,
            "footer", "pied-page", "<GcdsFooter Display=\"compact\" />", "<GcdsFooter Display=\"compact\" />", ["OnClick", "OnFocus", "OnBlur"]),
        D<GcdsGrid, GcdsGridExample>("grid", "Grid", "Grille", "grid", "grille", DocsCategory.Layout,
            "grid", "grille", "<GcdsGrid Columns=\"1fr 1fr\"><GcdsText>First column</GcdsText><GcdsText>Second column</GcdsText></GcdsGrid>", "<GcdsGrid Columns=\"1fr 1fr\"><GcdsText>Première colonne</GcdsText><GcdsText>Deuxième colonne</GcdsText></GcdsGrid>", []),
        D<GcdsGridCol, GcdsGridColExample>("grid-col", "Grid column", "Colonne de grille", "grid-col", "colonne-grille", DocsCategory.Supporting,
            "grid", "grille", "<GcdsGridCol Desktop=\"6\"><GcdsText>Grid column</GcdsText></GcdsGridCol>", "<GcdsGridCol Desktop=\"6\"><GcdsText>Colonne de grille</GcdsText></GcdsGridCol>", []),
        D<GcdsHeader, GcdsHeaderExample>("header", "Header", "En-tête", "header", "en-tete", DocsCategory.Branding,
            "header", "en-tete", "<GcdsHeader LangHref=\"/fr\" SkipToHref=\"#main\" />", "<GcdsHeader LangHref=\"/en\" SkipToHref=\"#main\" />", ["OnClick", "OnFocus", "OnBlur"]),
        D<GcdsHeading, GcdsHeadingExample>("heading", "Heading", "Titre", "heading", "titre", DocsCategory.Layout,
            "heading", "titre", "<GcdsHeading Tag=\"h2\">Section heading</GcdsHeading>", "<GcdsHeading Tag=\"h2\">Titre de section</GcdsHeading>", []),
        D<GcdsHint, GcdsHintExample>("hint", "Hint", "Indice", "hint", "indice", DocsCategory.Supporting,
            "input", "champ-saisie", "<GcdsHint>Helpful context.</GcdsHint>", "<GcdsHint>Contexte utile.</GcdsHint>", []),
        D<GcdsIcon, GcdsIconExample>("icon", "Icon", "Icône", "icon", "icone", DocsCategory.Layout,
            "icon", "icone", "<GcdsIcon Name=\"info-circle\" Label=\"Information\" />", "<GcdsIcon Name=\"info-circle\" Label=\"Information\" />", []),
        D<GcdsInput, GcdsInputExample>("input", "Input", "Champ de saisie", "input", "champ-saisie", DocsCategory.Forms,
            "input", "champ-saisie", "<GcdsInput Id=\"email\" Name=\"email\" Label=\"Email address\" @bind-Value=\"model.Email\" />", "<GcdsInput Id=\"courriel\" Name=\"courriel\" Label=\"Adresse courriel\" @bind-Value=\"model.Courriel\" />", [.. FormEvents, "OnSuggestionSelected"], bind: true),
        D<GcdsLabel, GcdsLabelExample>("label", "Label", "Étiquette", "label", "etiquette", DocsCategory.Supporting,
            "input", "champ-saisie", "<GcdsLabel Label=\"Email address\" LabelFor=\"email\"><GcdsText>Email address</GcdsText></GcdsLabel>", "<GcdsLabel Label=\"Adresse courriel\" LabelFor=\"courriel\"><GcdsText>Adresse courriel</GcdsText></GcdsLabel>", []),
        D<GcdsLangToggle, GcdsLangToggleExample>("lang-toggle", "Language toggle", "Bascule de langue", "language-toggle", "bascule-langue", DocsCategory.Navigation,
            "language-toggle", "bascule-langue", "<GcdsLangToggle Href=\"/fr\" Lang=\"/fr\" />", "<GcdsLangToggle Href=\"/en\" Lang=\"/en\" />", ["OnClick", "OnFocus", "OnBlur"]),
        D<GcdsLink, GcdsLinkExample>("link", "Link", "Lien", "link", "lien", DocsCategory.Navigation,
            "link", "lien", "<GcdsLink Href=\"/guide\">Read the guide</GcdsLink>", "<GcdsLink Href=\"/guide\">Lire le guide</GcdsLink>", ["OnClick", "OnFocus", "OnBlur"]),
        D<GcdsNavGroup, GcdsNavGroupExample>("nav-group", "Navigation group", "Groupe de navigation", "nav-group", "groupe-navigation", DocsCategory.Supporting,
            "side-navigation", "navigation-laterale", "<GcdsNavGroup MenuLabel=\"Guides\" OpenTrigger=\"Guides\">...</GcdsNavGroup>", "<GcdsNavGroup MenuLabel=\"Guides\" OpenTrigger=\"Guides\">...</GcdsNavGroup>", ["OnClick", "OnFocus", "OnBlur"]),
        D<GcdsNavLink, GcdsNavLinkExample>("nav-link", "Navigation link", "Lien de navigation", "nav-link", "lien-navigation", DocsCategory.Supporting,
            "side-navigation", "navigation-laterale", "<GcdsNavLink Href=\"/components\">Components</GcdsNavLink>", "<GcdsNavLink Href=\"/composants\">Composants</GcdsNavLink>", ["OnClick", "OnFocus", "OnBlur"]),
        D<GcdsNotice, GcdsNoticeExample>("notice", "Notice", "Avis", "notice", "avis", DocsCategory.Feedback,
            "notice", "avis", "<GcdsNotice NoticeRole=\"info\" NoticeTitle=\"Note\" NoticeTitleTag=\"h2\"><GcdsText>Important information.</GcdsText></GcdsNotice>", "<GcdsNotice NoticeRole=\"info\" NoticeTitle=\"Remarque\" NoticeTitleTag=\"h2\"><GcdsText>Renseignements importants.</GcdsText></GcdsNotice>", []),
        D<GcdsPagination, GcdsPaginationExample>("pagination", "Pagination", "Pagination", "pagination", "pagination", DocsCategory.Navigation,
            "pagination", "pagination", "<GcdsPagination Label=\"Results\" CurrentPage=\"2\" TotalPages=\"5\" Url=\"/results?page={}\" />", "<GcdsPagination Label=\"Résultats\" CurrentPage=\"2\" TotalPages=\"5\" Url=\"/resultats?page={}\" />", ["OnClick", "OnFocus", "OnBlur"]),
        D<GcdsRadios, GcdsRadiosExample>("radios", "Radios", "Boutons radio", "radios", "boutons-radio", DocsCategory.Forms,
            "radios", "boutons-radio", "<GcdsRadios Name=\"contact\" Legend=\"Contact method\" Options=\"options\" @bind-Value=\"model.Contact\" />", "<GcdsRadios Name=\"contact\" Legend=\"Mode de communication\" Options=\"options\" @bind-Value=\"model.Contact\" />", FormEvents, bind: true),
        D<GcdsSearch, GcdsSearchExample>("search", "Search", "Recherche", "search", "recherche", DocsCategory.Forms,
            "search", "recherche", "<GcdsSearch Id=\"site-search\" Name=\"q\" @bind-Value=\"query\" />", "<GcdsSearch Id=\"recherche-site\" Name=\"q\" @bind-Value=\"requete\" />", [.. FormEvents, "OnSubmit"], bind: true),
        D<GcdsSelect, GcdsSelectExample>("select", "Select", "Liste de sélection", "select", "liste-selection", DocsCategory.Forms,
            "select", "liste-selection", "<GcdsSelect Id=\"province\" Name=\"province\" Label=\"Province\" @bind-Value=\"model.Province\">...</GcdsSelect>", "<GcdsSelect Id=\"province\" Name=\"province\" Label=\"Province\" @bind-Value=\"model.Province\">...</GcdsSelect>", FormEvents, bind: true),
        D<GcdsSideNav, GcdsSideNavExample>("side-nav", "Side navigation", "Navigation latérale", "side-navigation", "navigation-laterale", DocsCategory.Navigation,
            "side-navigation", "navigation-laterale", "<GcdsSideNav Label=\"Documentation\">...</GcdsSideNav>", "<GcdsSideNav Label=\"Documentation\">...</GcdsSideNav>", []),
        D<GcdsSignature, GcdsSignatureExample>("signature", "Signature", "Signature", "signature", "signature", DocsCategory.Branding,
            "signature", "signature", "<GcdsSignature Type=\"signature\" Variant=\"colour\" />", "<GcdsSignature Type=\"signature\" Variant=\"colour\" />", []),
        D<GcdsSrOnly, GcdsSrOnlyExample>("sr-only", "Screenreader-only", "Lecteur d’écran seulement", "screenreader-only", "lecteur-ecran-seulement", DocsCategory.Supporting,
            "screenreader-only", "lecteur-ecran-seulement", "<GcdsSrOnly>Additional context</GcdsSrOnly>", "<GcdsSrOnly>Contexte supplémentaire</GcdsSrOnly>", []),
        D<GcdsStepper, GcdsStepperExample>("stepper", "Stepper", "Indicateur d’étapes", "stepper", "indicateur-etapes", DocsCategory.Data,
            "stepper", "indicateur-etapes", "<GcdsStepper CurrentStep=\"2\" TotalSteps=\"4\">Review your application</GcdsStepper>", "<GcdsStepper CurrentStep=\"2\" TotalSteps=\"4\">Vérifiez votre demande</GcdsStepper>", []),
        D<GcdsTable, GcdsTableExample>("table", "Table", "Tableau", "table", "tableau", DocsCategory.Data,
            "table", "tableau", TableSnippetEnglish, TableSnippetFrench, ["OnTableStateChange"]),
        D<GcdsText, GcdsTextExample>("text", "Text", "Texte", "text", "texte", DocsCategory.Layout,
            "text", "texte", "<GcdsText>Body text.</GcdsText>", "<GcdsText>Corps du texte.</GcdsText>", []),
        D<GcdsTextarea, GcdsTextareaExample>("textarea", "Textarea", "Zone de texte", "textarea", "zone-texte", DocsCategory.Forms,
            "textarea", "zone-texte", "<GcdsTextarea Id=\"summary\" Name=\"summary\" Label=\"Summary\" @bind-Value=\"model.Summary\" />", "<GcdsTextarea Id=\"resume\" Name=\"resume\" Label=\"Résumé\" @bind-Value=\"model.Resume\" />", FormEvents, bind: true),
        D<GcdsTopNav, GcdsTopNavExample>("top-nav", "Top navigation", "Navigation supérieure", "top-navigation", "navigation-superieure", DocsCategory.Navigation,
            "top-navigation", "navigation-superieure", "<GcdsTopNav Label=\"Main navigation\">...</GcdsTopNav>", "<GcdsTopNav Label=\"Navigation principale\">...</GcdsTopNav>", []),
        D<GcdsTopicMenu, GcdsTopicMenuExample>("topic-menu", "Theme and topic menu", "Menu des thèmes et sujets", "theme-topic-menu", "menu-themes-sujets", DocsCategory.Navigation,
            "theme-topic-menu", "menu-themes-sujets", "<GcdsTopicMenu Lang=\"en\" Home />", "<GcdsTopicMenu Lang=\"fr\" Home />", [])
    ];

    public static DocsComponentDescriptor? Find(DocsLanguage language, string slug) =>
        All.FirstOrDefault(item => string.Equals(item.SlugFor(language), slug, StringComparison.OrdinalIgnoreCase));

    public static DocsComponentDescriptor? FindByKey(string key) =>
        All.FirstOrDefault(item => item.Key == key);

    public static DocsComponentDescriptor? FindByPath(string relativePath)
    {
        var language = DocsLanguageExtensions.FromPath(relativePath);
        var slug = relativePath.Trim('/').Split('/').LastOrDefault();
        return slug is null ? null : Find(language, slug);
    }

    public static LocalizedText CategoryName(DocsCategory category) => category switch
    {
        DocsCategory.Forms => new("Forms", "Formulaires"),
        DocsCategory.Navigation => new("Navigation", "Navigation"),
        DocsCategory.Layout => new("Layout and content", "Mise en page et contenu"),
        DocsCategory.Feedback => new("Feedback", "Rétroaction"),
        DocsCategory.Branding => new("Page and branding", "Page et image de marque"),
        DocsCategory.Data => new("Data and progress", "Données et progression"),
        _ => new("Supporting elements", "Éléments de soutien")
    };

    private static DocsComponentDescriptor D<TWrapper, TExample>(
        string key, string enTitle, string frTitle, string enSlug, string frSlug,
        DocsCategory category, string officialEn, string officialFr,
        string snippetEn, string snippetFr, IReadOnlyList<string> events,
        bool bind = false, bool asset = false)
        where TWrapper : IComponent
        where TExample : IComponent
    {
        var notes = asset
            ? new LocalizedText(
                "This utility renders no UI. Place it once in App.razor to load the pinned GCDS stylesheet and module.",
                "Cet utilitaire ne produit aucune interface. Placez-le une fois dans App.razor pour charger la feuille de style et le module GCDS épinglés.")
            : bind
                ? new LocalizedText(
                    "This wrapper derives from InputBase and participates in EditForm validation.",
                    "Cet adaptateur dérive d’InputBase et participe à la validation EditForm.")
                : new LocalizedText(
                    "Use AdditionalAttributes for upstream attributes that do not yet have a typed parameter.",
                    "Utilisez AdditionalAttributes pour les attributs en amont qui n’ont pas encore de paramètre typé.");

        return new(
            key, typeof(TWrapper), typeof(TExample), new(enTitle, frTitle), new(enSlug, frSlug),
            category, officialEn, officialFr, snippetEn, snippetFr, notes,
            ReadDefaults(typeof(TWrapper)), events, bind, asset);
    }

    private static IReadOnlyDictionary<string, string> ReadDefaults(Type wrapperType)
    {
        var instance = Activator.CreateInstance(wrapperType);
        if (instance is null) return new Dictionary<string, string>();

        return wrapperType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(property => property.GetCustomAttribute<ParameterAttribute>() is not null)
            .Where(property => property.PropertyType != typeof(RenderFragment))
            .Where(property => !property.PropertyType.IsGenericType ||
                property.PropertyType.GetGenericTypeDefinition() != typeof(EventCallback<>))
            .Select(property => (property.Name, Value: property.GetValue(instance)))
            .Where(item => item.Value is not null)
            .ToDictionary(
                item => item.Name,
                item => item.Value is IFormattable formattable
                    ? formattable.ToString(null, CultureInfo.InvariantCulture) ?? string.Empty
                    : item.Value!.ToString() ?? string.Empty);
    }
}
