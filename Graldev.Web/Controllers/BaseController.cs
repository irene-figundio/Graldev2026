using Microsoft.AspNetCore.Mvc;
using Graldev.Web.Seo;
using System;

namespace Graldev.Web.Controllers
{
    public class BaseController : Controller
    {
        protected bool IsEn => HttpContext.Request.Path.Value?.StartsWith("/en", StringComparison.OrdinalIgnoreCase) == true;

        protected void SetupSeo(string titleIt, string titleEn, string descIt, string descEn, string canonicalPathIt, string canonicalPathEn, bool noIndex = false)
        {
            var seo = new PageSeoMetadata
            {
                Language = IsEn ? "en" : "it",
                NoIndex = noIndex,
                Robots = noIndex ? "noindex, nofollow" : "index, follow"
            };

            var baseUrl = "https://www.graldev.com";

            if (IsEn)
            {
                seo.Title = titleEn;
                seo.MetaDescription = descEn;
                seo.CanonicalUrl = $"{baseUrl}{canonicalPathEn}";
            }
            else
            {
                seo.Title = titleIt;
                seo.MetaDescription = descIt;
                seo.CanonicalUrl = $"{baseUrl}{canonicalPathIt}";
            }

            seo.OgTitle = seo.Title;
            seo.OgDescription = seo.MetaDescription;

            // Alternate languages
            seo.AlternateLanguages["it"] = $"{baseUrl}{canonicalPathIt}";
            seo.AlternateLanguages["en"] = $"{baseUrl}{canonicalPathEn}";
            seo.AlternateLanguages["x-default"] = $"{baseUrl}{canonicalPathIt}"; // Default to Italian

            ViewData["SeoMetadata"] = seo;
            ViewData["IsEn"] = IsEn;
        }
    }
}
