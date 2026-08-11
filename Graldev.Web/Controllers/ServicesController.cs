using Microsoft.AspNetCore.Mvc;

namespace Graldev.Web.Controllers
{
    public class ServicesController : BaseController
    {
        [Route("consulenza-informatica")]
        [Route("en/it-consulting")]
        public IActionResult Consulenza()
        {
            SetupSeo(
                titleIt: "Consulenza Informatica per Aziende | Graldev",
                titleEn: "IT Consulting for Businesses | Graldev",
                descIt: "Consulenza informatica per aziende: assessment, architettura, System Integration, AI, modernizzazione e sviluppo di soluzioni IT integrate.",
                descEn: "IT consulting for businesses: assessment, architecture, System Integration, AI, modernization and development of integrated IT solutions.",
                canonicalPathIt: "/consulenza-informatica",
                canonicalPathEn: "/en/it-consulting"
            );
            return View();
        }

        [Route("servizi/system-integration")]
        [Route("en/services/system-integration")]
        public IActionResult SystemIntegration()
        {
            SetupSeo(
                titleIt: "System Integration per Aziende | Graldev",
                titleEn: "System Integration for Businesses | Graldev",
                descIt: "Graldev integra software, ERP, CRM, API, database, eCommerce, cloud e sistemi industriali per creare processi digitali più efficienti.",
                descEn: "Graldev integrates software, ERP, CRM, APIs, databases, eCommerce, cloud and industrial systems to create more efficient digital processes.",
                canonicalPathIt: "/servizi/system-integration",
                canonicalPathEn: "/en/services/system-integration"
            );
            return View();
        }

        [Route("servizi/ai-integration")]
        [Route("en/services/ai-integration")]
        public IActionResult AiIntegration()
        {
            SetupSeo(
                titleIt: "AI Integration e Automazione per Aziende | Graldev",
                titleEn: "AI Integration and Automation for Businesses | Graldev",
                descIt: "Graldev integra AI, LLM, agenti e automazioni nei software e processi aziendali, collegandoli a dati, API e sistemi esistenti.",
                descEn: "Graldev integrates AI, LLMs, agents and automations into software and business processes, connecting them with data, APIs and legacy systems.",
                canonicalPathIt: "/servizi/ai-integration",
                canonicalPathEn: "/en/services/ai-integration"
            );
            return View();
        }

        [Route("servizi/software-engineering")]
        [Route("en/services/software-engineering")]
        public IActionResult SoftwareEngineering()
        {
            SetupSeo(
                titleIt: "Software Engineering e Soluzioni Custom | Graldev",
                titleEn: "Software Engineering & Custom Solutions | Graldev",
                descIt: "Graldev progetta e sviluppa componenti software, piattaforme e applicazioni custom quando le soluzioni standard non coprono i processi aziendali.",
                descEn: "Graldev designs and develops software components, platforms and custom applications when standard software does not fit corporate processes.",
                canonicalPathIt: "/servizi/software-engineering",
                canonicalPathEn: "/en/services/software-engineering"
            );
            return View();
        }

        [Route("servizi/api-data-integration")]
        [Route("en/services/api-data-integration")]
        public IActionResult ApiDataIntegration()
        {
            SetupSeo(
                titleIt: "API Integration e Data Integration per Aziende | Graldev",
                titleEn: "API and Data Integration for Businesses | Graldev",
                descIt: "Integrazione API, database e sistemi aziendali per sincronizzare dati e automatizzare processi tra applicazioni, ERP, CRM e piattaforme digitali.",
                descEn: "API integration, databases and enterprise systems to synchronize data and automate processes between apps, ERP, CRM and digital platforms.",
                canonicalPathIt: "/servizi/api-data-integration",
                canonicalPathEn: "/en/services/api-data-integration"
            );
            return View();
        }

        [Route("servizi/digital-commerce-integration")]
        [Route("en/services/digital-commerce-integration")]
        public IActionResult DigitalCommerceIntegration()
        {
            SetupSeo(
                titleIt: "Integrazione ERP, Shopify, Amazon ed eBay | Graldev",
                titleEn: "ERP, Shopify, Amazon and eBay Integration | Graldev",
                descIt: "Graldev collega ERP, produzione, Shopify, Amazon, eBay e marketplace attraverso API e automazioni progettate per flussi eCommerce complessi.",
                descEn: "Graldev connects ERP, production, Shopify, Amazon, eBay and marketplaces through APIs and automations built for complex eCommerce workflows.",
                canonicalPathIt: "/servizi/digital-commerce-integration",
                canonicalPathEn: "/en/services/digital-commerce-integration"
            );
            return View();
        }

        [Route("servizi/cloud-architecture")]
        [Route("en/services/cloud-architecture")]
        public IActionResult CloudArchitecture()
        {
            SetupSeo(
                titleIt: "Cloud, Software Architecture e Modernizzazione IT | Graldev",
                titleEn: "Cloud, Software Architecture and IT Modernization | Graldev",
                descIt: "Consulenza su architetture software, cloud, modernizzazione applicativa, legacy systems e progettazione di infrastrutture scalabili.",
                descEn: "Consulting on software architecture, cloud, application modernization, legacy systems and designing scalable infrastructures.",
                canonicalPathIt: "/servizi/cloud-architecture",
                canonicalPathEn: "/en/services/cloud-architecture"
            );
            return View();
        }

        [Route("servizi/business-applications")]
        [Route("en/services/business-applications")]
        public IActionResult BusinessApplications()
        {
            SetupSeo(
                titleIt: "Applicazioni Business e Gestionali Custom | Graldev",
                titleEn: "Custom Business Applications & Software | Graldev",
                descIt: "Applicazioni business, gestionali custom, portali e strumenti operativi progettati per i processi reali dell'azienda.",
                descEn: "Business applications, custom management software, portals and operational tools designed for actual business processes.",
                canonicalPathIt: "/servizi/business-applications",
                canonicalPathEn: "/en/services/business-applications"
            );
            return View();
        }

        [Route("servizi/ar-vr-3d")]
        [Route("en/services/ar-vr-3d")]
        public IActionResult ArVr3d()
        {
            SetupSeo(
                titleIt: "Sviluppo AR, VR e 3D per Aziende | Graldev",
                titleEn: "AR, VR and 3D Development for Businesses | Graldev",
                descIt: "Soluzioni AR, VR, 3D e applicazioni interattive per turismo, cultura, retail, formazione e comunicazione digitale.",
                descEn: "AR, VR, 3D solutions and interactive applications for tourism, culture, retail, training and digital communication.",
                canonicalPathIt: "/servizi/ar-vr-3d",
                canonicalPathEn: "/en/services/ar-vr-3d"
            );
            return View();
        }
    }
}
