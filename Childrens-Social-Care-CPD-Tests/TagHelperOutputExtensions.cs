using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.WebEncoders.Testing;
using System.IO;

namespace Childrens_Social_Care_CPD_Tests;

public static class TagHelperOutputExtensions
{
    public static string AsString(this TagHelperOutput tagHelperOutput)
    {
        var stringWriter = new StringWriter();
        tagHelperOutput.WriteTo(stringWriter, new HtmlTestEncoder());
        return stringWriter.ToString();
    }
}
