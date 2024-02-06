using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Mime;
using System.Text.Json.Serialization;

namespace Childrens_Social_Care_CPD_Tests;

public sealed class AntiforgeryTokens
{
    [JsonProperty("cookieName")]
    [JsonPropertyName("cookieName")]
    public string CookieName { get; set; } = string.Empty;

    [JsonProperty("cookieValue")]
    [JsonPropertyName("cookieValue")]
    public string CookieValue { get; set; } = string.Empty;

    [JsonProperty("formFieldName")]
    [JsonPropertyName("formFieldName")]
    public string FormFieldName { get; set; } = string.Empty;

    [JsonProperty("headerName")]
    [JsonPropertyName("headerName")]
    public string HeaderName { get; set; } = string.Empty;

    [JsonProperty("requestToken")]
    [JsonPropertyName("requestToken")]
    public string RequestToken { get; set; } = string.Empty;
}

public sealed class AntiforgeryTokenController : Controller
{
    private const string GetUrl = "_testing/get-xsrf-token";

    public static Uri GetTokensUri { get; } = new Uri(GetUrl, UriKind.Relative);

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json, Type = typeof(AntiforgeryTokens))]
    [Route(GetUrl)]
    public IActionResult GetAntiforgeryTokens([FromServices] IAntiforgery antiforgery, [FromServices] IOptions<AntiforgeryOptions> options)
    {
        if (antiforgery == null)
        {
            throw new ArgumentNullException(nameof(antiforgery));
        }

        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        AntiforgeryTokenSet tokens = antiforgery.GetTokens(HttpContext);

        var model = new AntiforgeryTokens()
        {
            CookieName = options.Value.Cookie.Name,
            CookieValue = tokens.CookieToken,
            FormFieldName = options.Value.FormFieldName,
            HeaderName = tokens.HeaderName,
            RequestToken = tokens.RequestToken,
        };

        return Json(model);
    }
}
