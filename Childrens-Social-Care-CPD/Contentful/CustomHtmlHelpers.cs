using Contentful.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Childrens_Social_Care_CPD.Contentful;

public static class CustomHtmlHelpers
{
    public static async Task RenderContentfulPartialAsync(this IHtmlHelper helper, IContent item)
    {
        if (item == null) return;

        var partialName = PartialsFactory.GetPartialFor(item);

        if (string.IsNullOrEmpty(partialName)) return;

        await helper.RenderPartialAsync(partialName, item);
    }
}