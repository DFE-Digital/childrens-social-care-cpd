using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Childrens_Social_Care_CPD.Models;

public enum SortOrder
{
    UpdatedLatest,
    UpdatedOldest,
    Relevance,
}

public static class SearchRequestPropertyNames
{
    public const string Term = "q";
    public const string Tags = "t";
    public const string Page = "p";
    public const string SortOrder = "so";
}

[ExcludeFromCodeCoverage]
public record SearchRequestModel
{
    [BindProperty(Name = SearchRequestPropertyNames.Term, SupportsGet = true)]
    public required string Term { get; init; }

    [BindProperty(Name = SearchRequestPropertyNames.Tags)]
    public required string[] Tags { get; init; }

    [BindProperty(Name = SearchRequestPropertyNames.Page)]
    public int Page { get; init; } = 1;

    [BindProperty(Name = SearchRequestPropertyNames.SortOrder)]
    public SortOrder SortOrder { get; init; } = SortOrder.UpdatedLatest;
};