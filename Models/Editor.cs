using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Models
{
    public static class Editor
    {
        public static IHtmlContent GetHtml(this IHtmlHelper helper, string input)
            => new HtmlContentBuilder().AppendHtml(input);
    }
}