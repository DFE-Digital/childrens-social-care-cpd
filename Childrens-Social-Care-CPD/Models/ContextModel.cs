﻿using Childrens_Social_Care_CPD.Contentful.Models;

namespace Childrens_Social_Care_CPD.Models;

public record PublishDates (
    DateTime ?FirstPublishedAt,
    DateTime ?LastPublishedAt
);

public record ContextModel(
    string Id,
    string Title,
    string PageName,
    string Category,
    bool UseContainers,
    bool PreferenceSet,
    bool HideConsent = false,
    ContentLink BackLink = null,
    bool FeedbackSubmitted = false,
    List<KeyValuePair<string, string>> BreadcrumbTrail = null,
    PublishDates PublishDates = null)
{
    public Stack<string> ContentStack { get; } = new Stack<string>();
}
