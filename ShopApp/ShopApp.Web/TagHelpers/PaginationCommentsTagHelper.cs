using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ShopApp.Web.TagHelpers;

public class PaginationCommentsTagHelper : TagHelper
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public string Url { get; set; } = null!;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "nav"; // 'nav' etiketi olarak render et
        output.Attributes.SetAttribute("aria-label", "Page navigation");

        var ulTag = new TagBuilder("ul");
        ulTag.AddCssClass("pagination");

        for (var i = 1; i <= TotalPages; i++)
        {
            var liTag = new TagBuilder("li");
            liTag.AddCssClass("page-item");

            var aTag = new TagBuilder("a");
            aTag.AddCssClass("page-link");
            aTag.Attributes["href"] = $"{Url}?page={i}";
            aTag.InnerHtml.Append(i.ToString());

            if (i == CurrentPage)
            {
                liTag.AddCssClass("active");
            }

            liTag.InnerHtml.AppendHtml(aTag);
            ulTag.InnerHtml.AppendHtml(liTag);
        }

        output.Content.AppendHtml(ulTag);
    }
}
