namespace Graldev.Web.Models
{
        public class MetalHeroViewModel
        {
            public string Id { get; set; } = "default";

            public string Size { get; set; } = "medium";
            public string Eyebrow { get; set; } = string.Empty;

            public string Title { get; set; } = string.Empty;

            public string Subtitle { get; set; } = string.Empty;

            public bool Centered { get; set; }

            public List<MetalHeroBreadcrumbItem> Breadcrumbs { get; set; } = new();
        }

        public class MetalHeroBreadcrumbItem
        {
            public string Label { get; set; } = string.Empty;

            public string? Url { get; set; }

            public bool IsCurrent { get; set; }
        }
    }
