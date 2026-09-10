using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace GcdsWrapper.Demo.Components.Docs;

public enum DocsLanguage { English, French }
public enum DocsCategory { Forms, Navigation, Layout, Feedback, Branding, Data, Supporting }

public sealed record LocalizedText(string English, string French)
{
    public string Get(DocsLanguage language) => language == DocsLanguage.French ? French : English;
}

public sealed record LocalizedSlug(string English, string French)
{
    public string Get(DocsLanguage language) => language == DocsLanguage.French ? French : English;
}

public sealed record DocsComponentDescriptor(
    string Key,
    [property: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type WrapperType,
    [property: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type ExampleType,
    LocalizedText Title,
    LocalizedSlug Slug,
    DocsCategory Category,
    string OfficialEnglishSlug,
    string OfficialFrenchSlug,
    string SnippetEnglish,
    string SnippetFrench,
    LocalizedText Notes,
    IReadOnlyDictionary<string, string> Defaults,
    IReadOnlyList<string> Events,
    bool SupportsBinding = false,
    bool IsAssetLoader = false)
{
    public string TitleFor(DocsLanguage language) => Title.Get(language);
    public string SlugFor(DocsLanguage language) => Slug.Get(language);

    public string SummaryFor(DocsLanguage language) => language == DocsLanguage.French
        ? $"{WrapperType.Name} adapte le composant {Title.French.ToLowerInvariant()} du Système de design GC à Blazor."
        : $"{WrapperType.Name} adapts the GC Design System {Title.English.ToLowerInvariant()} component for Blazor.";

    public string SnippetFor(DocsLanguage language) =>
        language == DocsLanguage.French ? SnippetFrench : SnippetEnglish;

    public string RouteFor(DocsLanguage language) => language == DocsLanguage.French
        ? $"/fr/composants/{Slug.French}"
        : $"/en/components/{Slug.English}";

    public string OfficialUrlFor(DocsLanguage language)
    {
        if (IsAssetLoader)
            return language == DocsLanguage.French
                ? "https://systeme-design.canada.ca/fr/demarrer/developper/html/"
                : "https://design-system.canada.ca/en/start-to-use/develop/html/";

        return language == DocsLanguage.French
            ? $"https://systeme-design.canada.ca/fr/composants/{OfficialFrenchSlug}/"
            : $"https://design-system.canada.ca/en/components/{OfficialEnglishSlug}/";
    }
}

public sealed record DocsApiParameter(string Name, string Type, bool Required, string? Default, string DeclaredBy);

internal static class DocsApiReader
{
    private static readonly HashSet<string> Hidden =
    [
        "ValueChanged", "ValueExpression", "DisplayName", "ChildContent"
    ];

    public static IReadOnlyList<DocsApiParameter> Read(DocsComponentDescriptor descriptor)
    {
        var properties = descriptor.WrapperType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(property => property.GetCustomAttribute<ParameterAttribute>() is not null)
            .Where(property => !Hidden.Contains(property.Name))
            .Where(property => !IsEventCallback(property.PropertyType))
            .Select(property => new DocsApiParameter(
                property.Name == "Value" && descriptor.SupportsBinding ? "@bind-Value" : property.Name,
                FriendlyType(property.PropertyType),
                property.GetCustomAttribute<EditorRequiredAttribute>() is not null,
                descriptor.Defaults.GetValueOrDefault(property.Name),
                property.DeclaringType?.Name ?? descriptor.WrapperType.Name))
            .OrderBy(parameter => parameter.Name == "@bind-Value" ? 0 : 1)
            .ThenBy(parameter => parameter.Name)
            .ToArray();

        return properties;
    }

    private static bool IsEventCallback(Type type) =>
        type == typeof(EventCallback) ||
        type.IsGenericType && type.GetGenericTypeDefinition() == typeof(EventCallback<>);

    private static string FriendlyType(Type type)
    {
        var nullable = Nullable.GetUnderlyingType(type);
        if (nullable is not null) return $"{FriendlyType(nullable)}?";
        if (type == typeof(string)) return "string?";
        if (type == typeof(bool)) return "bool";
        if (type == typeof(int)) return "int";
        if (type == typeof(decimal)) return "decimal";
        if (type.IsArray) return $"{FriendlyType(type.GetElementType()!)}[]";
        if (type.IsGenericType)
        {
            var name = type.Name[..type.Name.IndexOf('`')];
            return $"{name}<{string.Join(", ", type.GetGenericArguments().Select(FriendlyType))}>";
        }
        return type.Name;
    }
}

internal static class DocsLanguageExtensions
{
    public static DocsLanguage FromPath(string relativePath) =>
        relativePath.StartsWith("fr/", StringComparison.OrdinalIgnoreCase)
            ? DocsLanguage.French
            : DocsLanguage.English;

    public static string Code(this DocsLanguage language) =>
        language == DocsLanguage.French ? "fr" : "en";

    public static string ComponentsRoute(this DocsLanguage language) =>
        language == DocsLanguage.French ? "/fr/composants" : "/en/components";

    public static string MarkdownRoute(this DocsLanguage language) =>
        language == DocsLanguage.French ? "/fr/markdown" : "/en/markdown";
}
