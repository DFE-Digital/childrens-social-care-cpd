function migrationFunction(migration, context) {

    const detailedPathway = migration.createContentType("detailedPathway");
    detailedPathway
        .displayField("title")
        .name("Detailed Pathway")
        .description("Detailed information around career pathways")

    const detailedPathwayTitle = detailedPathway.createField("title");
    detailedPathwayTitle
        .name("Title")
        .type("Symbol")
        .localized(false)
        .required(true)
        .validations([{ "unique": true }])
        .disabled(false)
        .omitted(false)

    const detailedPathwayPathway = detailedPathway.createField("pathway");
    detailedPathwayPathway
        .name("Pathway")
        .type("Symbol")
        .localized(false)
        .required(true)
        .validations([])
        .disabled(false)
        .omitted(false)

    const detailedPathwayAudience = detailedPathway.createField("audience");
    detailedPathwayAudience
        .name("Audience")
        .type("Symbol")
        .localized(false)
        .required(true)
        .validations([])
        .disabled(false)
        .omitted(false)

    const detailedPathwayDuration = detailedPathway.createField("duration");
    detailedPathwayDuration
        .name("Duration")
        .type("Symbol")
        .localized(false)
        .required(true)
        .validations([])
        .disabled(false)
        .omitted(false)

    const detailedPathwayTimeCommitment = detailedPathway.createField("timeCommitment");
    detailedPathwayTimeCommitment
        .name("TimeCommitment")
        .type("Symbol")
        .localized(false)
        .required(true)
        .validations([])
        .disabled(false)
        .omitted(false)

    const detailedPathwayCost = detailedPathway.createField("cost");
    detailedPathwayCost
        .name("Cost")
        .type("Symbol")
        .localized(false)
        .required(true)
        .validations([])
        .disabled(false)
        .omitted(false)

    const detailedPathwayDeliveredBy = detailedPathway.createField("deliveredBy");
    detailedPathwayDeliveredBy
        .name("Delivered by")
        .type("RichText")
        .localized(false)
        .required(true)
        .validations([{ "enabledMarks": ["code"], "message": "Only code marks are allowed" }, { "enabledNodeTypes": ["hyperlink", "entry-hyperlink"], "message": "Only link to Url and link to entry nodes are allowed" }, { "nodes": {} }])
        .disabled(false)
        .omitted(false)

    const detailedPathwayWhoItsFor = detailedPathway.createField("whoItsFor");
    detailedPathwayWhoItsFor
        .name("Who it's for")
        .type("RichText")
        .localized(false)
        .required(false)
        .validations([{ "enabledMarks": ["bold", "italic", "underline", "code", "superscript", "subscript"], "message": "Only bold, italic, underline, code, superscript, and subscript marks are allowed" }, { "enabledNodeTypes": ["ordered-list", "unordered-list", "hr", "blockquote", "table", "hyperlink", "entry-hyperlink", "asset-hyperlink", "embedded-asset-block", "heading-3", "heading-4", "heading-5", "heading-6"], "message": "Only ordered list, unordered list, horizontal rule, quote, table, link to Url, link to entry, link to asset, asset, heading 3, heading 4, heading 5, and heading 6 nodes are allowed" }, { "nodes": {} }])
        .disabled(false)
        .omitted(false)

    const detailedPathwayWhatYoullLearn = detailedPathway.createField("whatYoullLearn");
    detailedPathwayWhatYoullLearn
        .name("What you'll learn")
        .type("RichText")
        .localized(false)
        .required(false)
        .validations([{ "enabledMarks": ["bold", "italic", "underline", "code", "superscript", "subscript"], "message": "Only bold, italic, underline, code, superscript, and subscript marks are allowed" }, { "enabledNodeTypes": ["ordered-list", "unordered-list", "hr", "blockquote", "embedded-asset-block", "table", "hyperlink", "entry-hyperlink", "asset-hyperlink", "heading-3", "heading-4", "heading-5", "heading-6"], "message": "Only ordered list, unordered list, horizontal rule, quote, asset, table, link to Url, link to entry, link to asset, heading 3, heading 4, heading 5, and heading 6 nodes are allowed" }, { "nodes": {} }])
        .disabled(false)
        .omitted(false)

    const detailedPathwayWhatYoullGet = detailedPathway.createField("whatYoullGet");
    detailedPathwayWhatYoullGet
        .name("What you'll get")
        .type("RichText")
        .localized(false)
        .required(false)
        .validations([{ "enabledMarks": ["bold", "italic", "underline", "code", "superscript", "subscript"], "message": "Only bold, italic, underline, code, superscript, and subscript marks are allowed" }, { "enabledNodeTypes": ["ordered-list", "unordered-list", "hr", "blockquote", "embedded-asset-block", "table", "hyperlink", "entry-hyperlink", "asset-hyperlink", "heading-3", "heading-4", "heading-5", "heading-6"], "message": "Only ordered list, unordered list, horizontal rule, quote, asset, table, link to Url, link to entry, link to asset, heading 3, heading 4, heading 5, and heading 6 nodes are allowed" }, { "nodes": {} }])
        .disabled(false)
        .omitted(false)

    const detailedPathwayHowItsDelivered = detailedPathway.createField("howItsDelivered");
    detailedPathwayHowItsDelivered
        .name("How it's delivered")
        .type("RichText")
        .localized(false)
        .required(false)
        .validations([{ "enabledMarks": ["bold", "italic", "underline", "code", "superscript", "subscript"], "message": "Only bold, italic, underline, code, superscript, and subscript marks are allowed" }, { "enabledNodeTypes": ["ordered-list", "unordered-list", "hr", "blockquote", "embedded-asset-block", "table", "hyperlink", "entry-hyperlink", "asset-hyperlink", "heading-3", "heading-4", "heading-5", "heading-6"], "message": "Only ordered list, unordered list, horizontal rule, quote, asset, table, link to Url, link to entry, link to asset, heading 3, heading 4, heading 5, and heading 6 nodes are allowed" }, { "nodes": {} }])
        .disabled(false)
        .omitted(false)

    const detailedPathwayFunding = detailedPathway.createField("funding");
    detailedPathwayFunding
        .name("Funding")
        .type("RichText")
        .localized(false)
        .required(false)
        .validations([{ "enabledMarks": ["bold", "italic", "underline", "code", "superscript", "subscript"], "message": "Only bold, italic, underline, code, superscript, and subscript marks are allowed" }, { "enabledNodeTypes": ["ordered-list", "unordered-list", "hr", "blockquote", "embedded-asset-block", "table", "hyperlink", "entry-hyperlink", "asset-hyperlink", "heading-3", "heading-4", "heading-5", "heading-6"], "message": "Only ordered list, unordered list, horizontal rule, quote, asset, table, link to Url, link to entry, link to asset, heading 3, heading 4, heading 5, and heading 6 nodes are allowed" }, { "nodes": {} }])
        .disabled(false)
        .omitted(false)

    const detailedPathwayNextSteps = detailedPathway.createField("nextSteps");
    detailedPathwayNextSteps
        .name("Next steps")
        .type("RichText")
        .localized(false)
        .required(false)
        .validations([{ "enabledMarks": ["bold", "italic", "underline", "code", "superscript", "subscript"], "message": "Only bold, italic, underline, code, superscript, and subscript marks are allowed" }, { "enabledNodeTypes": ["ordered-list", "unordered-list", "hr", "blockquote", "embedded-asset-block", "table", "hyperlink", "entry-hyperlink", "asset-hyperlink", "heading-3", "heading-4", "heading-5", "heading-6"], "message": "Only ordered list, unordered list, horizontal rule, quote, asset, table, link to Url, link to entry, link to asset, heading 3, heading 4, heading 5, and heading 6 nodes are allowed" }, { "nodes": {} }])
        .disabled(false)
        .omitted(false)
    detailedPathway.changeFieldControl("title", "builtin", "singleLine")
    detailedPathway.changeFieldControl("pathway", "builtin", "singleLine")
    detailedPathway.changeFieldControl("audience", "builtin", "singleLine")
    detailedPathway.changeFieldControl("duration", "builtin", "singleLine")
    detailedPathway.changeFieldControl("timeCommitment", "builtin", "singleLine")
    detailedPathway.changeFieldControl("cost", "builtin", "singleLine")
    detailedPathway.changeFieldControl("deliveredBy", "builtin", "richTextEditor")
    detailedPathway.changeFieldControl("whoItsFor", "builtin", "richTextEditor")
    detailedPathway.changeFieldControl("whatYoullLearn", "builtin", "richTextEditor")
    detailedPathway.changeFieldControl("whatYoullGet", "builtin", "richTextEditor")
    detailedPathway.changeFieldControl("howItsDelivered", "builtin", "richTextEditor")
    detailedPathway.changeFieldControl("funding", "builtin", "richTextEditor")
    detailedPathway.changeFieldControl("nextSteps", "builtin", "richTextEditor")

    const linkListCard = migration.createContentType("linkListCard");
    linkListCard
        .displayField("title")
        .name("Link List Card")
        .description("A card with a title and a list of Content Links")

    const linkListCardTitle = linkListCard.createField("title");
    linkListCardTitle
        .name("Title")
        .type("Symbol")
        .localized(false)
        .required(true)
        .validations([{ "unique": true }])
        .disabled(false)
        .omitted(false)

    const linkListCardLinks = linkListCard.createField("links");
    linkListCardLinks
        .name("Links")
        .type("Array")
        .localized(false)
        .required(true)
        .validations([])
        .disabled(false)
        .omitted(false)
        .items({ "type": "Link", "validations": [{ "linkContentType": ["contentLink"] }], "linkType": "Entry" })
    linkListCard.changeFieldControl("title", "builtin", "singleLine", { "helpText": "The title text of the card" })
    linkListCard.changeFieldControl("links", "builtin", "entryLinksEditor", { "helpText": "One or more Content Links to display in the card", "bulkEditing": false, "showLinkEntityAction": true, "showCreateEntityAction": true })

    const heroBanner = migration.createContentType("heroBanner");
    heroBanner
        .displayField("title")
        .name("Hero Banner")
        .description("A large banner to display on a content item, will only be shown when viewing the content as the root item")

    const heroBannerTitle = heroBanner.createField("title");
    heroBannerTitle
        .name("Title")
        .type("Symbol")
        .localized(false)
        .required(true)
        .validations([{ "unique": true }])
        .disabled(false)
        .omitted(false)

    const heroBannerText = heroBanner.createField("text");
    heroBannerText
        .name("Text")
        .type("Text")
        .localized(false)
        .required(false)
        .validations([{ "size": { "max": 500 } }])
        .disabled(false)
        .omitted(false)
    heroBanner.changeFieldControl("title", "builtin", "singleLine", { "helpText": "Displays as the large title in the banner" })
    heroBanner.changeFieldControl("text", "builtin", "multipleLine", { "helpText": "Displays as the text under the title" })

    const contentLink = migration.createContentType("contentLink");
    contentLink
        .displayField("name")
        .name("Content Link")
        .description("Acts as a link to other content, either internally within the Contentful content, or to other resources on the internet")

    const contentLinkName = contentLink.createField("name");
    contentLinkName
        .name("Name")
        .type("Symbol")
        .localized(false)
        .required(true)
        .validations([{ "unique": true }])
        .disabled(false)
        .omitted(false)

    const contentLinkUri = contentLink.createField("uri");
    contentLinkUri
        .name("URI")
        .type("Symbol")
        .localized(false)
        .required(true)
        .validations([])
        .disabled(false)
        .omitted(false)
    contentLink.changeFieldControl("name", "builtin", "singleLine", { "helpText": "The display text of the link" })
    contentLink.changeFieldControl("uri", "builtin", "singleLine", { "helpText": "Can either be a Content item's name e.g. 'roles/supervisor', or a link to a website e.g. 'http://www.google.com'" })

    const contentSeparator = migration.createContentType("contentSeparator");
    contentSeparator
        .displayField("type")
        .name("Content Separator")
        .description("A visible separator for content. Large, medium & small exist. You should not create any more.")

    const contentSeparatorType = contentSeparator.createField("type");
    contentSeparatorType
        .name("Type")
        .type("Symbol")
        .localized(false)
        .required(true)
        .validations([{ "in": ["Extra Large", "Large", "Medium", "Small"] }, { "unique": true }])
        .disabled(false)
        .omitted(false)
    contentSeparator.changeFieldControl("type", "builtin", "radio")

    const richTextBlock = migration.createContentType("richTextBlock");
    richTextBlock
        .displayField("title")
        .name("Rich Text Block")
        .description("A block of text that has additional formatting options such as tables, where the title can appear as an optional header")

    const richTextBlockTitle = richTextBlock.createField("title");
    richTextBlockTitle
        .name("Title")
        .type("Symbol")
        .localized(false)
        .required(true)
        .validations([{ "unique": true }])
        .disabled(false)
        .omitted(false)

    const richTextBlockDisplayTitle = richTextBlock.createField("displayTitle");
    richTextBlockDisplayTitle
        .name("Display Title")
        .type("Boolean")
        .localized(false)
        .required(true)
        .validations([])
        .defaultValue({ "en-US": false })
        .disabled(false)
        .omitted(false)

    const richTextBlockTitleLevel = richTextBlock.createField("titleLevel");
    richTextBlockTitleLevel
        .name("Title Level")
        .type("Integer")
        .localized(false)
        .required(false)
        .validations([{ "in": [1, 2, 3] }])
        .defaultValue({ "en-US": 2 })
        .disabled(false)
        .omitted(false)

    const richTextBlockDocument = richTextBlock.createField("document");
    richTextBlockDocument
        .name("Document")
        .type("RichText")
        .localized(false)
        .required(false)
        .validations([{ "enabledMarks": ["bold", "italic", "underline", "code"], "message": "Only bold, italic, underline, and code marks are allowed" }, { "enabledNodeTypes": ["heading-1", "heading-2", "heading-3", "heading-4", "heading-5", "heading-6", "ordered-list", "unordered-list", "hr", "blockquote", "embedded-entry-block", "embedded-asset-block", "table", "hyperlink", "entry-hyperlink", "asset-hyperlink", "embedded-entry-inline"], "message": "Only heading 1, heading 2, heading 3, heading 4, heading 5, heading 6, ordered list, unordered list, horizontal rule, quote, block entry, asset, table, link to Url, link to entry, link to asset, and inline entry nodes are allowed" }, { "nodes": {} }])
        .disabled(false)
        .omitted(false)
    richTextBlock.changeFieldControl("title", "builtin", "singleLine")
    richTextBlock.changeFieldControl("displayTitle", "builtin", "boolean", { "helpText": "Indicates whether the content should be displayed with the Title as a heading", "trueLabel": "Yes", "falseLabel": "No" })
    richTextBlock.changeFieldControl("titleLevel", "builtin", "radio", { "helpText": "The importance of the title, 1 is the most important" })
    richTextBlock.changeFieldControl("document", "builtin", "richTextEditor")

    const textBlock = migration.createContentType("textBlock");
    textBlock
        .displayField("title")
        .name("Text Block")
        .description("A basic block of text that can have a title")

    const textBlockTitle = textBlock.createField("title");
    textBlockTitle
        .name("Title")
        .type("Symbol")
        .localized(false)
        .required(true)
        .validations([{ "unique": true }])
        .disabled(false)
        .omitted(false)

    const textBlockDisplayTitle = textBlock.createField("displayTitle");
    textBlockDisplayTitle
        .name("Display Title")
        .type("Boolean")
        .localized(false)
        .required(true)
        .validations([])
        .defaultValue({ "en-US": false })
        .disabled(false)
        .omitted(false)

    const textBlockTitleLevel = textBlock.createField("titleLevel");
    textBlockTitleLevel
        .name("Title Level")
        .type("Integer")
        .localized(false)
        .required(false)
        .validations([{ "in": [1, 2, 3] }])
        .defaultValue({ "en-US": 2 })
        .disabled(false)
        .omitted(false)

    const textBlockText = textBlock.createField("text");
    textBlockText
        .name("Text")
        .type("Text")
        .localized(false)
        .required(true)
        .validations([])
        .disabled(false)
        .omitted(false)
    textBlock.changeFieldControl("title", "builtin", "singleLine")
    textBlock.changeFieldControl("displayTitle", "builtin", "boolean", { "helpText": "Indicates whether the content should be displayed with the Title as a heading", "trueLabel": "Yes", "falseLabel": "No" })
    textBlock.changeFieldControl("titleLevel", "builtin", "radio", { "helpText": "The importance of the title, 1 is the most important" })
    textBlock.changeFieldControl("text", "builtin", "multipleLine")

    const sideMenu = migration.createContentType("sideMenu");
    sideMenu
        .displayField("name")
        .name("Side Menu")
        .description("A side menu to display on the page - these should only be used as a sections sub page navigation")

    const sideMenuName = sideMenu.createField("name");
    sideMenuName
        .name("Name")
        .type("Symbol")
        .localized(false)
        .required(true)
        .validations([{ "unique": true }])
        .disabled(false)
        .omitted(false)

    const sideMenuItems = sideMenu.createField("items");
    sideMenuItems
        .name("Items")
        .type("Array")
        .localized(false)
        .required(true)
        .validations([])
        .disabled(false)
        .omitted(false)
        .items({ "type": "Link", "validations": [{ "linkContentType": ["contentLink"] }], "linkType": "Entry" })
    sideMenu.changeFieldControl("name", "builtin", "singleLine", { "helpText": "The name of the submenu" })
    sideMenu.changeFieldControl("items", "builtin", "entryLinksEditor", { "helpText": "Each Content Link added will appear as a menu item", "bulkEditing": false, "showLinkEntityAction": true, "showCreateEntityAction": true })

    const roleList = migration.createContentType("roleList");
    roleList
        .displayField("title")
        .name("Role List")
        .description("A list which displays a summary of the role along with its title")

    const roleListTitle = roleList.createField("title");
    roleListTitle
        .name("Title")
        .type("Symbol")
        .localized(false)
        .required(false)
        .validations([])
        .disabled(false)
        .omitted(false)

    const roleListRoles = roleList.createField("roles");
    roleListRoles
        .name("Roles")
        .type("Array")
        .localized(false)
        .required(false)
        .validations([])
        .disabled(false)
        .omitted(false)
        .items({ "type": "Link", "validations": [{ "linkContentType": ["content"] }], "linkType": "Entry" })
    roleList.changeFieldControl("title", "builtin", "singleLine")
    roleList.changeFieldControl("roles", "builtin", "entryLinksEditor", { "helpText": "You should select a content item that is the root page for the role. The page should have a single Detailed Role as its content.", "bulkEditing": false, "showLinkEntityAction": true, "showCreateEntityAction": true })

    const detailedRole = migration.createContentType("detailedRole");
    detailedRole
        .displayField("title")
        .name("Detailed Role")
        .description("A detailed job description")

    const detailedRoleTitle = detailedRole.createField("title");
    detailedRoleTitle
        .name("Title")
        .type("Symbol")
        .localized(false)
        .required(true)
        .validations([{ "unique": true }])
        .disabled(false)
        .omitted(false)

    const detailedRoleSalaryRange = detailedRole.createField("salaryRange");
    detailedRoleSalaryRange
        .name("SalaryRange")
        .type("Symbol")
        .localized(false)
        .required(true)
        .validations([])
        .disabled(false)
        .omitted(false)

    const detailedRoleSummary = detailedRole.createField("summary");
    detailedRoleSummary
        .name("Summary")
        .type("Text")
        .localized(false)
        .required(true)
        .validations([{ "size": { "min": 1, "max": 500 } }])
        .disabled(false)
        .omitted(false)

    const detailedRoleOtherNames = detailedRole.createField("otherNames");
    detailedRoleOtherNames
        .name("Other names")
        .type("Symbol")
        .localized(false)
        .required(false)
        .validations([])
        .disabled(false)
        .omitted(false)

    const detailedRoleWhatYoullDo = detailedRole.createField("whatYoullDo");
    detailedRoleWhatYoullDo
        .name("What you'll do")
        .type("RichText")
        .localized(false)
        .required(true)
        .validations([{ "enabledMarks": ["bold", "italic", "underline", "code", "superscript", "subscript"], "message": "Only bold, italic, underline, code, superscript, and subscript marks are allowed" }, { "enabledNodeTypes": ["heading-3", "heading-4", "heading-5", "heading-6", "ordered-list", "unordered-list", "hr", "blockquote", "embedded-entry-block", "embedded-asset-block", "table", "hyperlink", "entry-hyperlink", "asset-hyperlink", "embedded-entry-inline"], "message": "Only heading 3, heading 4, heading 5, heading 6, ordered list, unordered list, horizontal rule, quote, block entry, asset, table, link to Url, link to entry, link to asset, and inline entry nodes are allowed" }, { "nodes": {} }])
        .disabled(false)
        .omitted(false)

    const detailedRoleSkillsAndKnowledge = detailedRole.createField("skillsAndKnowledge");
    detailedRoleSkillsAndKnowledge
        .name("Skills and knowledge")
        .type("RichText")
        .localized(false)
        .required(true)
        .validations([{ "enabledMarks": ["bold", "italic", "underline", "code", "superscript", "subscript"], "message": "Only bold, italic, underline, code, superscript, and subscript marks are allowed" }, { "enabledNodeTypes": ["heading-3", "heading-4", "heading-5", "heading-6", "ordered-list", "unordered-list", "hr", "blockquote", "embedded-entry-block", "embedded-asset-block", "table", "hyperlink", "entry-hyperlink", "asset-hyperlink", "embedded-entry-inline"], "message": "Only heading 3, heading 4, heading 5, heading 6, ordered list, unordered list, horizontal rule, quote, block entry, asset, table, link to Url, link to entry, link to asset, and inline entry nodes are allowed" }, { "nodes": {} }])
        .disabled(false)
        .omitted(false)

    const detailedRoleHowToBecomeOne = detailedRole.createField("howToBecomeOne");
    detailedRoleHowToBecomeOne
        .name("How to become one")
        .type("RichText")
        .localized(false)
        .required(true)
        .validations([{ "enabledMarks": ["bold", "italic", "underline", "code", "superscript", "subscript"], "message": "Only bold, italic, underline, code, superscript, and subscript marks are allowed" }, { "enabledNodeTypes": ["heading-3", "heading-4", "heading-5", "heading-6", "ordered-list", "unordered-list", "hr", "blockquote", "embedded-entry-block", "embedded-asset-block", "table", "hyperlink", "entry-hyperlink", "asset-hyperlink", "embedded-entry-inline"], "message": "Only heading 3, heading 4, heading 5, heading 6, ordered list, unordered list, horizontal rule, quote, block entry, asset, table, link to Url, link to entry, link to asset, and inline entry nodes are allowed" }, { "nodes": {} }])
        .disabled(false)
        .omitted(false)

    const detailedRoleCareerPathsAndProgression = detailedRole.createField("careerPathsAndProgression");
    detailedRoleCareerPathsAndProgression
        .name("Career paths and progression")
        .type("RichText")
        .localized(false)
        .required(true)
        .validations([{ "enabledMarks": ["bold", "italic", "underline", "code", "superscript", "subscript"], "message": "Only bold, italic, underline, code, superscript, and subscript marks are allowed" }, { "enabledNodeTypes": ["heading-3", "heading-4", "heading-5", "heading-6", "ordered-list", "unordered-list", "hr", "blockquote", "embedded-entry-block", "embedded-asset-block", "table", "hyperlink", "entry-hyperlink", "asset-hyperlink", "embedded-entry-inline"], "message": "Only heading 3, heading 4, heading 5, heading 6, ordered list, unordered list, horizontal rule, quote, block entry, asset, table, link to Url, link to entry, link to asset, and inline entry nodes are allowed" }, { "nodes": {} }])
        .disabled(false)
        .omitted(false)

    const detailedRoleCurrentOpportunities = detailedRole.createField("currentOpportunities");
    detailedRoleCurrentOpportunities
        .name("Current opportunities")
        .type("RichText")
        .localized(false)
        .required(true)
        .validations([{ "enabledMarks": ["bold", "italic", "underline", "code", "superscript", "subscript"], "message": "Only bold, italic, underline, code, superscript, and subscript marks are allowed" }, { "enabledNodeTypes": ["heading-3", "heading-4", "heading-5", "heading-6", "ordered-list", "unordered-list", "hr", "blockquote", "embedded-entry-block", "embedded-asset-block", "table", "hyperlink", "entry-hyperlink", "asset-hyperlink", "embedded-entry-inline"], "message": "Only heading 3, heading 4, heading 5, heading 6, ordered list, unordered list, horizontal rule, quote, block entry, asset, table, link to Url, link to entry, link to asset, and inline entry nodes are allowed" }, { "nodes": {} }])
        .disabled(false)
        .omitted(false)
    detailedRole.changeFieldControl("title", "builtin", "singleLine", { "helpText": "The job title" })
    detailedRole.changeFieldControl("salaryRange", "builtin", "singleLine")
    detailedRole.changeFieldControl("summary", "builtin", "multipleLine")
    detailedRole.changeFieldControl("otherNames", "builtin", "singleLine")
    detailedRole.changeFieldControl("whatYoullDo", "builtin", "richTextEditor")
    detailedRole.changeFieldControl("skillsAndKnowledge", "builtin", "richTextEditor")
    detailedRole.changeFieldControl("howToBecomeOne", "builtin", "richTextEditor")
    detailedRole.changeFieldControl("careerPathsAndProgression", "builtin", "richTextEditor")
    detailedRole.changeFieldControl("currentOpportunities", "builtin", "richTextEditor")

    const content = migration.createContentType("content");
    content
        .displayField("id")
        .name("Content")
        .description("A content container")

    const contentId = content.createField("id");
    contentId
        .name("Id")
        .type("Symbol")
        .localized(false)
        .required(true)
        .validations([{ "regexp": { "pattern": "^[0-9a-z](\\/?[0-9a-z\\-])*\\/?$", "flags": null }, "message": "Please only use lower case letters, dash (-) or forward slash (/) separated words and numbers. Ids can only begin with letters or numbers. e.g. 'roles-list', 'roles/practitioners-1'" }, { "unique": true }])
        .disabled(false)
        .omitted(false)

    const contentTitle = content.createField("title");
    contentTitle
        .name("Title")
        .type("Symbol")
        .localized(false)
        .required(true)
        .validations([])
        .disabled(false)
        .omitted(false)

    const contentCategory = content.createField("category");
    contentCategory
        .name("Category")
        .type("Symbol")
        .localized(false)
        .required(true)
        .validations([{ "in": ["Home", "Career information", "Development programmes", "Explore roles"] }])
        .disabled(false)
        .omitted(false)

    const contentSideMenu = content.createField("sideMenu");
    contentSideMenu
        .name("SideMenu")
        .type("Link")
        .localized(false)
        .required(false)
        .validations([{ "linkContentType": ["sideMenu"] }])
        .disabled(false)
        .omitted(false)
        .linkType("Entry")

    const contentItems = content.createField("items");
    contentItems
        .name("Items")
        .type("Array")
        .localized(false)
        .required(false)
        .validations([])
        .disabled(false)
        .omitted(false)
        .items({ "type": "Link", "validations": [{ "linkContentType": ["columnLayout", "content", "contentLink", "contentSeparator", "detailedRole", "heroBanner", "linkListCard", "richTextBlock", "textBlock", "section", "roleList"] }], "linkType": "Entry" })
    content.changeFieldControl("id", "builtin", "singleLine")
    content.changeFieldControl("title", "builtin", "singleLine", { "helpText": "The title of the content contained within this item, it will become the page title in the website" })
    content.changeFieldControl("category", "builtin", "radio")
    content.changeFieldControl("sideMenu", "builtin", "entryCardEditor", { "helpText": "Note side menus are only shown when this content item is the root item being viewed", "showLinkEntityAction": true, "showCreateEntityAction": true })
    content.changeFieldControl("items", "builtin", "entryLinksEditor", { "helpText": "The content of this item", "bulkEditing": false, "showLinkEntityAction": true, "showCreateEntityAction": true })

    const columnLayout = migration.createContentType("columnLayout");
    columnLayout
        .displayField("name")
        .name("Column Layout")
        .description("A column layout for content items.")

    const columnLayoutName = columnLayout.createField("name");
    columnLayoutName
        .name("Name")
        .type("Symbol")
        .localized(false)
        .required(true)
        .validations([{ "unique": true }])
        .disabled(false)
        .omitted(false)

    const columnLayoutColumnCount = columnLayout.createField("columnCount");
    columnLayoutColumnCount
        .name("Column Count")
        .type("Integer")
        .localized(false)
        .required(true)
        .validations([{ "in": [2, 3] }, { "range": { "min": 2, "max": 3 }, "message": "Can only have a 2 or 3 column layout" }])
        .disabled(false)
        .omitted(false)

    const columnLayoutItems = columnLayout.createField("items");
    columnLayoutItems
        .name("Items")
        .type("Array")
        .localized(false)
        .required(false)
        .validations([])
        .disabled(false)
        .omitted(false)
        .items({ "type": "Link", "validations": [], "linkType": "Entry" })
    columnLayout.changeFieldControl("name", "builtin", "singleLine", { "helpText": "The name of the column layout" })
    columnLayout.changeFieldControl("columnCount", "builtin", "dropdown", { "helpText": "The number of columns the layout should have" })
    columnLayout.changeFieldControl("items", "builtin", "entryLinksEditor", { "helpText": "The items that should appear in the layout", "bulkEditing": false, "showLinkEntityAction": true, "showCreateEntityAction": true })

    const linkCard = migration.createContentType("linkCard");
    linkCard
        .displayField("name")
        .name("Link Card")
        .description("A card that has a link as the title and a block of text to describe the link's purpose")

    const linkCardName = linkCard.createField("name");
    linkCardName
        .name("Name")
        .type("Symbol")
        .localized(false)
        .required(true)
        .validations([{ "unique": true }])
        .disabled(false)
        .omitted(false)

    const linkCardTitleLink = linkCard.createField("titleLink");
    linkCardTitleLink
        .name("Title Link")
        .type("Link")
        .localized(false)
        .required(true)
        .validations([{ "linkContentType": ["contentLink"] }])
        .disabled(false)
        .omitted(false)
        .linkType("Entry")

    const linkCardTitleLevel = linkCard.createField("titleLevel");
        linkCardTitleLevel
            .name("Title Level")
            .type("Integer")
            .localized(false)
            .required(false)
            .validations([{ "in": [2, 3, 4] }])
            .defaultValue({ "en-US": 2 })
            .disabled(false)
            .omitted(false)

    const linkCardText = linkCard.createField("text");
    linkCardText
        .name("Text")
        .type("Text")
        .localized(false)
        .required(true)
        .validations([{ "size": { "max": 500 } }])
        .disabled(false)
        .omitted(false)
    linkCard.changeFieldControl("name", "builtin", "singleLine", { "helpText": "The name of this card" })
    linkCard.changeFieldControl("titleLink", "builtin", "entryLinkEditor", { "helpText": "Select a content link for this card", "showLinkEntityAction": true, "showCreateEntityAction": true })
    linkCard.changeFieldControl("titleLevel", "builtin", "radio")
    linkCard.changeFieldControl("text", "builtin", "multipleLine")
}
module.exports = migrationFunction;
