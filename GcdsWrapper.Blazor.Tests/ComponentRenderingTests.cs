using Bunit;
using Xunit;

namespace GcdsWrapper.Blazor.Tests;

public sealed class ComponentRenderingTests
{
    [Theory]
    [InlineData("en")]
    [InlineData("fr")]
    public void TopicMenu_RendersSelectedLanguage(string language)
    {
        using var context = new BunitContext();
        context.JSInterop.Mode = JSRuntimeMode.Loose;

        var component = context.Render<GcdsTopicMenu>(parameters => parameters
            .Add(item => item.Lang, language));

        Assert.Equal(language, component.Find("gcds-topic-menu").GetAttribute("lang"));
    }
}
