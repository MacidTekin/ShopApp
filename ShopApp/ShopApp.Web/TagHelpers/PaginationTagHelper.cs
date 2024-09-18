using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ShopApp.Web.TagHelpers;

[HtmlTargetElement("pagination")]
public class PaginationTagHelper : TagHelper
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public string Url { get; set; } = null!; 

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.SetAttribute("class", "pagination");

        var content = new StringBuilder();

        // Eğer kategori URL'si boşsa, sadece sayfa parametresi kullanılır
        string pageUrl;
        for (int i = 1; i <= TotalPages; i++)
        {
            // Kategori varsa kategori URL'sini kullan, yoksa ana sayfayı göster
            pageUrl = string.IsNullOrEmpty(Url)
                ? $"/?page={i}" // Ana sayfa için sayfalama
                : $"/products/category/{Url}?page={i}"; // Kategori için sayfalama

            var activeClass = i == CurrentPage ? "active" : "";
            content.AppendFormat($"<a href='{pageUrl}' class='{activeClass}'>{i}</a>");
        }


        output.Content.SetHtmlContent(content.ToString());
    }
}
