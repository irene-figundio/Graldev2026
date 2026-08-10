using System.Collections.Generic;

namespace Graldev.Web.Seo
{
    public class PageSeoMetadata
    {
        public string Title { get; set; } = "";
        public string MetaDescription { get; set; } = "";
        public string CanonicalUrl { get; set; } = "";
        public string Robots { get; set; } = "index, follow";
        public string OgTitle { get; set; } = "";
        public string OgDescription { get; set; } = "";
        public string OgImage { get; set; } = "/images/og-default.png";
        public string TwitterCard { get; set; } = "summary_large_image";
        public string Language { get; set; } = "it";
        public Dictionary<string, string> AlternateLanguages { get; set; } = new Dictionary<string, string>();
        public string SchemaJson { get; set; } = "";
        public bool NoIndex { get; set; } = false;
    }
}
