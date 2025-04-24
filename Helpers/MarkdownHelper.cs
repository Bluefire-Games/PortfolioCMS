using Markdig;

namespace PortfolioCMS.Helpers
{
    public static class MarkdownHelper
    {
        public static string ToHtml(string markdown)
        {
            return Markdown.ToHtml(markdown ?? string.Empty);
        }
    }
}
