namespace GcdsWrapper.Blazor;

public enum GcdsButtonRole { Primary, Secondary, Danger, Start }
public enum GcdsButtonSize { Regular, Small }
public enum GcdsButtonType { Button, Link, Reset, Submit }
public enum GcdsInputType { Text, Email, Number, Password, Search, Telephone, Url }
public enum GcdsInputMode { None, Text, Decimal, Numeric, Telephone, Search, Email, Url }
public enum GcdsValidateOn { Blur, Submit, Other }

internal static class GcdsEnumExtensions
{
    public static string ToAttribute(this GcdsButtonRole value) => value.ToString().ToLowerInvariant();
    public static string ToAttribute(this GcdsButtonSize value) => value.ToString().ToLowerInvariant();
    public static string ToAttribute(this GcdsButtonType value) => value.ToString().ToLowerInvariant();
    public static string ToAttribute(this GcdsValidateOn value) => value.ToString().ToLowerInvariant();
    public static string ToAttribute(this GcdsInputType value) => value == GcdsInputType.Telephone ? "tel" : value.ToString().ToLowerInvariant();
    public static string ToAttribute(this GcdsInputMode value) => value == GcdsInputMode.Telephone ? "tel" : value.ToString().ToLowerInvariant();
}
