using Contentful.Core.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Childrens_Social_Care_CPD.Contentful.Renderers;

public class AssetStructureRenderer : IRenderer<AssetStructure>
{
    public IHtmlContent Render(AssetStructure item)
    {
        switch (item.Data)
        {
            case AssetStructureData assetStructureData:
                {
                    switch (assetStructureData.Target)
                    {
                        case Asset asset:
                            {
                                var contentType = asset.File?.ContentType.ToLower();

                                if (contentType.StartsWith("image/"))
                                {
                                    var img = new TagBuilder("img");
                                    img.Attributes.Add("src", asset.File.Url);
                                    if (!string.IsNullOrEmpty(asset.Description))
                                    {
                                        img.Attributes.Add("alt", asset.Description);
                                    }

                                    return img;
                                }
                                break;
                            }
                    }
                    break;
                }
        }

        return null;
    }
}
