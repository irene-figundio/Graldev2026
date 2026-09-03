using Microsoft.AspNetCore.Mvc;

namespace Graldev.Web.Controllers
{
    public class CaseStudiesController : BaseController
    {
        [Route("case-study")]
        [Route("en/case-studies")]
        public IActionResult Index()
        {
            SetupSeo(
                titleIt: "Case Study di Consulenza IT e System Integration | Graldev",
                titleEn: "IT Consulting and System Integration Case Studies | Graldev",
                descIt: "Scopri come Graldev aiuta le aziende con progetti reali di System Integration, AI, software engineering e soluzioni IT su misura.",
                descEn: "Discover how Graldev helps businesses with real-world projects in System Integration, AI, software engineering and custom IT solutions.",
                canonicalPathIt: "/case-study",
                canonicalPathEn: "/en/case-studies"
            );
            return View();
        }

        [Route("case-study/geordie")]
        [Route("en/case-studies/geordie")]
        public IActionResult Geordie()
        {
            SetupSeo(
                titleIt: "Case Study Geordie: Integrazione ERP e Multi-Store eCommerce | Graldev",
                titleEn: "Geordie Case Study: ERP and Multi-Store eCommerce Integration | Graldev",
                descIt: "Analisi di Geordie: integrazione automatica e sincronizzazione di cataloghi, magazzini e ordini tra ERP e canali di vendita digitali.",
                descEn: "Geordie Case Study: automated integration and synchronization of catalogs, inventory and orders between ERP and digital sales channels.",
                canonicalPathIt: "/case-study/geordie",
                canonicalPathEn: "/en/case-studies/geordie"
            );
            return View();
        }

        [Route("case-study/vitinerario")]
        [Route("en/case-studies/vitinerario")]
        public IActionResult Vitinerario()
        {
            SetupSeo(
                titleIt: "Case Study Vitinerario: Assistente Sommelier AI e LLM | Graldev",
                titleEn: "Vitinerario Case Study: AI Sommelier Assistant and LLMs | Graldev",
                descIt: "Come Graldev ha integrato intelligenza artificiale, Large Language Models e RAG per creare un sommelier virtuale interattivo per la cultura vinicola.",
                descEn: "How Graldev integrated artificial intelligence, Large Language Models and RAG to build an interactive virtual sommelier for wine culture.",
                canonicalPathIt: "/case-study/vitinerario",
                canonicalPathEn: "/en/case-studies/vitinerario"
            );
            return View();
        }

    }
}
