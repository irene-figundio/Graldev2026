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

            seo.PathIt = string.IsNullOrEmpty(canonicalPathIt) ? "/" : canonicalPathIt;
            seo.PathEn = string.IsNullOrEmpty(canonicalPathEn) ? "/en" : canonicalPathEn;

            if (IsEn)
            {
                seo.Title = titleEn;
                seo.MetaDescription = descEn;
                seo.CanonicalUrl = $"{baseUrl}{seo.PathEn}";
            }
            else
            {
                seo.Title = titleIt;
                seo.MetaDescription = descIt;
                seo.CanonicalUrl = $"{baseUrl}{seo.PathIt}";
            }

            seo.OgTitle = seo.Title;
            seo.OgDescription = seo.MetaDescription;

            // Alternate languages for SEO link tags
            seo.AlternateLanguages["it"] = $"{baseUrl}{seo.PathIt}";
            seo.AlternateLanguages["en"] = $"{baseUrl}{seo.PathEn}";
            seo.AlternateLanguages["x-default"] = $"{baseUrl}{seo.PathIt}"; // Default to Italian

            ViewData["SeoMetadata"] = seo;
            ViewData["IsEn"] = IsEn;
        }
    }
}
