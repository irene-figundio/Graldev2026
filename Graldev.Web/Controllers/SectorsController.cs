using Microsoft.AspNetCore.Mvc;

namespace Graldev.Web.Controllers
{
    public class SectorsController : BaseController
    {
        [Route("settori")]
        [Route("en/sectors")]
        public IActionResult Index()
        {
            SetupSeo(
                titleIt: "Settori di Intervento di Consulenza Informatica | Graldev",
                titleEn: "IT Consulting Verticals & Industries | Graldev",
                descIt: "Competenza tecnologica applicata a settori complessi: Industria, Retail, eCommerce e Telecomunicazioni con System Integration e AI.",
                descEn: "Applied technical expertise for complex sectors: Industry, Retail, eCommerce and Telecommunications through System Integration and AI.",
                canonicalPathIt: "/settori",
                canonicalPathEn: "/en/sectors"
            );
            return View();
        }

        [Route("settori/industria")]
        [Route("en/sectors/industry")]
        public IActionResult Industria()
        {
            SetupSeo(
                titleIt: "Consulenza IT per il Settore Industriale | Graldev",
                titleEn: "IT Consulting for Industrial Sector | Graldev",
                descIt: "Ottimizzazione dei flussi produttivi industriali: System Integration, flussi dati hardware/software, automazione di processo e analisi dati.",
                descEn: "Optimization of industrial production workflows: System Integration, hardware/software data flows, process automation and data analysis.",
                canonicalPathIt: "/settori/industria",
                canonicalPathEn: "/en/sectors/industry"
            );
            return View();
        }

        [Route("settori/retail-ecommerce")]
        [Route("en/sectors/retail-ecommerce")]
        public IActionResult RetailEcommerce()
        {
            SetupSeo(
                titleIt: "Integrazione Sistemi Retail ed eCommerce | Graldev",
                titleEn: "Retail and eCommerce Systems Integration | Graldev",
                descIt: "Sincronizzazione di ERP aziendali, inventari, vendite multi-canale, Shopify, Amazon e marketplace per flussi eCommerce complessi.",
                descEn: "Synchronization of corporate ERP, inventory, multi-channel sales, Shopify, Amazon and marketplaces for complex eCommerce workflows.",
                canonicalPathIt: "/settori/retail-ecommerce",
                canonicalPathEn: "/en/sectors/retail-ecommerce"
            );
            return View();
        }

        [Route("settori/telco")]
        [Route("en/sectors/telco")]
        public IActionResult Telco()
        {
            SetupSeo(
                titleIt: "Consulenza Informatica e Soluzioni per Telecomunicazioni | Graldev",
                titleEn: "IT Consulting and Solutions for Telecommunications | Graldev",
                descIt: "Soluzioni d'integrazione e architetture dati scalabili per il settore Telco e infrastrutture di comunicazione avanzate.",
                descEn: "Scalable integration solutions and data architectures for the Telco sector and advanced communication infrastructures.",
                canonicalPathIt: "/settori/telco",
                canonicalPathEn: "/en/sectors/telco"
            );
            return View();
        }
    }
}
