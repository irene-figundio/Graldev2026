using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Graldev.Web.Seo;

namespace Graldev.Web.Controllers
{
    public class InsightArticle
    {
        public string Slug { get; set; } = "";
        public string Title { get; set; } = "";
        public string Excerpt { get; set; } = "";
        public string Content { get; set; } = "";
        public string Category { get; set; } = "";
        public string PublishedAt { get; set; } = "";
        public string Author { get; set; } = "Graldev Consulting";
    }

    public class InsightsController : BaseController
    {
        private readonly List<InsightArticle> ArticlesIt = new()
        {
            new() {
                Slug = "system-integration-quando-serve",
                Title = "System Integration: quando serve davvero?",
                Excerpt = "Sistemi scollegati e dati ridondanti rallentano la crescita della tua azienda. Scopri come l'integrazione di sistemi elimina l'operatività manuale.",
                Category = "System Integration",
                PublishedAt = "15/01/2026",
                Content = @"<p>Molte aziende affrontano una crescita disordinata dei propri sistemi informatici: un ERP acquistato anni fa, un CRM per la gestione dei clienti, un e-commerce integrato all'ultimo minuto e innumerevoli fogli Excel per connettere il tutto. Il risultato? Gli operatori passano ore a inserire manualmente lo stesso dato in più piattaforme, rischiando errori e rallentando i processi.</p>
                            <h3>I segnali che indicano la necessità di una System Integration</h3>
                            <p>Coinvolgere un partner di System Integration ha senso quando:</p>
                            <ul>
                                <li>I dati di magazzino sull'e-commerce non sono sincronizzati in tempo reale con l'ERP.</li>
                                <li>Le informazioni sui clienti rimangono isolate nel CRM senza confluire nei sistemi di fatturazione o assistenza.</li>
                                <li>Molti flussi operativi quotidiani dipendono interamente da un operatore che fa copia-incolla tra sistemi diversi.</li>
                            </ul>
                            <h3>L'approccio di Graldev</h3>
                            <p>Graldev non propone software pronti all'uso preconfezionati. Analizziamo l'architettura dei vostri sistemi attuali (database, API REST/SOAP, file piatti) e progettiamo un layer di integrazione o middleware capace di far fluire i dati in modo automatico, sicuro e monitorabile. In questo modo si ottimizza l'investimento tecnologico già effettuato, rendendolo scalabile.</p>"
            },
            new() {
                Slug = "software-standard-sviluppo-custom",
                Title = "Software standard, integrazione o sviluppo custom?",
                Excerpt = "Acquistare un software pronto (Buy) o svilupparlo su misura (Build)? Analizziamo vantaggi e svantaggi dal punto di vista strategico ed economico.",
                Category = "IT Strategy",
                PublishedAt = "22/01/2026",
                Content = @"<p>Nel decidere come digitalizzare un nuovo processo aziendale, il dilemma classico è: 'Build, Buy or Integrate?'. Comprare una soluzione sul mercato (SaaS o licenza standard) è spesso percepito come più economico e rapido, mentre lo sviluppo custom da zero è visto come ideale ma costoso.</p>
                            <h3>La terza via: l'integrazione intelligente</h3>
                            <p>Spesso non è necessario scegliere tra gli estremi. La soluzione ottimale risiede nell'integrazione di piattaforme esistenti e consolidate (es. Shopify per il commercio o Salesforce per il CRM) attraverso componenti custom leggeri e mirati (middleware). Questo consente di avere la robustezza delle grandi soluzioni standard e l'unicità di flussi di lavoro adatti al vostro modello di business specifico.</p>
                            <h3>Quando conviene davvero sviluppare software custom?</h3>
                            <p>Lo sviluppo custom è la strada corretta solo quando il processo di business rappresenta il vostro reale vantaggio competitivo sul mercato. Se il vostro workflow è unico e non configurabile in nessun software commerciale, allora sviluppare un componente personalizzato è l'unico modo per non forzare l'azienda a processi inefficienti.</p>"
            },
            new() {
                Slug = "introduzione-ai-processi-aziendali",
                Title = "Come introdurre l'AI nei processi aziendali in modo strategico",
                Excerpt = "L'Intelligenza Artificiale non deve essere un progetto isolato. Scopri come l'integrazione di LLM e RAG automatizza compiti reali partendo dai tuoi dati.",
                Category = "AI & Automation",
                PublishedAt = "05/02/2026",
                Content = @"<p>L'hype attorno all'Intelligenza Artificiale Generativa spinge molte aziende a lanciare progetti pilota isolati: un semplice chatbot sul sito pubblico o l'accesso a strumenti di chat generici per i dipendenti. Tuttavia, per generare valore reale, l'AI deve essere integrata all'interno dei flussi e dei sistemi informativi aziendali esistenti.</p>
                            <h3>Portare l'AI dentro i processi, non accanto</h3>
                            <p>L'AI enterprise si basa sulla connessione sicura tra modelli di linguaggio (LLM) e i dati interni dell'azienda. Utilizzando architetture come RAG (Retrieval-Augmented Generation), è possibile creare assistenti virtuali capaci di rispondere a domande di supporto tecnico o commerciale basandosi esclusivamente su manuali d'uso, procedure interne o contratti depositati nei vostri database, mantenendo la totale riservatezza del dato (AI Governance).</p>
                            <h3>Casi d'uso concreti ad alto ROI</h3>
                            <p>I progetti di maggior successo riguardano l'estrazione automatica di dati strutturati da documenti non strutturati (es. ordini scritti via mail o fatture passive complesse), la classificazione automatica dei ticket di assistenza e il supporto agli operatori di backoffice nella ricerca rapida all'interno di vaste knowledge base aziendali.</p>"
            }
        };

        private readonly List<InsightArticle> ArticlesEn = new()
        {
            new() {
                Slug = "system-integration-quando-serve",
                Title = "System Integration: when does it really help?",
                Excerpt = "Disconnected systems and redundant data slow down company growth. Discover how systems integration eliminates manual operations.",
                Category = "System Integration",
                PublishedAt = "15/01/2026",
                Content = @"<p>Many businesses face uncoordinated growth of their IT systems: an ERP purchased years ago, a CRM for customer relations, an e-commerce added at the last minute and endless Excel sheets to tie it all together. The result? Operators spend hours manually re-entering the same data across platforms, risking errors and slowing down operations.</p>
                            <h3>Signs you need System Integration</h3>
                            <p>Involving a System Integration partner is critical when:</p>
                            <ul>
                                <li>E-commerce stock levels are not synchronized in real-time with the ERP.</li>
                                <li>Customer information remains isolated in the CRM without flowing to invoicing or support systems.</li>
                                <li>Many daily operations rely entirely on an operator copying and pasting between different systems.</li>
                            </ul>
                            <h3>The Graldev Approach</h3>
                            <p>Graldev doesn't propose rigid pre-packaged software. We analyze your current architecture (databases, REST/SOAP APIs, flat files) and design an integration layer or middleware capable of flowing data automatically, securely and traceably. This optimizes the technology investments you've already made, making them scalable.</p>"
            },
            new() {
                Slug = "software-standard-sviluppo-custom",
                Title = "Standard software, integration or custom development?",
                Excerpt = "Buy ready-made software (Buy) or build it customized (Build)? We analyze the strategic and economic advantages of each path.",
                Category = "IT Strategy",
                PublishedAt = "22/01/2026",
                Content = @"<p>When deciding how to digitize a new business process, the classic dilemma is: 'Build, Buy or Integrate?'. Buying a market-ready solution (SaaS or standard license) is often perceived as cheaper and faster, while custom development from scratch is seen as ideal but expensive.</p>
                            <h3>The third way: Intelligent Integration</h3>
                            <p>Often, you don't have to choose between the two extremes. The optimal solution lies in integrating existing, proven platforms (e.g., Shopify for commerce or Salesforce for CRM) through lightweight, targeted custom components (middleware). This gives you the robustness of standard software and the uniqueness of workflows fit for your specific business model.</p>
                            <h3>When is custom software development truly worth it?</h3>
                            <p>Custom development is the correct path only when the business process represents your actual competitive advantage in the market. If your workflow is unique and cannot be configured in any commercial software, then developing a customized component is the only way to avoid forcing inefficiency onto your organization.</p>"
            },
            new() {
                Slug = "introduzione-ai-processi-aziendali",
                Title = "How to introduce AI into business processes strategically",
                Excerpt = "Artificial Intelligence shouldn't be an isolated project. Discover how integrating LLMs and RAG automates real tasks using your data.",
                Category = "AI & Automation",
                PublishedAt = "05/02/2026",
                Content = @"<p>The hype surrounding Generative AI drives many companies to launch isolated pilots: a simple chatbot on the public website or giving employees access to generic chat tools. However, to generate real value, AI must be integrated into your existing business workflows and information systems.</p>
                            <h3>Bringing AI inside processes, not beside them</h3>
                            <p>Enterprise AI relies on the secure connection between language models (LLMs) and internal company data. Using architectures like RAG (Retrieval-Augmented Generation), you can create virtual assistants capable of answering technical or commercial support questions based strictly on user manuals, internal procedures or contracts stored in your databases, while maintaining total data privacy (AI Governance).</p>
                            <h3>Real use cases with high ROI</h3>
                            <p>The most successful projects involve automatically extracting structured data from unstructured documents (such as orders written via email or complex vendor invoices), classifying customer support tickets automatically, and assisting backoffice operators in rapidly searching extensive corporate knowledge bases.</p>"
            }
        };

        [Route("insights")]
        [Route("en/insights")]
        public IActionResult Index()
        {
            var articles = IsEn ? ArticlesEn : ArticlesIt;

            SetupSeo(
                titleIt: "Insights su Consulenza IT, System Integration e AI | Graldev",
                titleEn: "Insights on IT Consulting, System Integration and AI | Graldev",
                descIt: "Articoli, guide e insights di Graldev per comprendere l'integrazione dei sistemi, l'AI enterprise e la modernizzazione delle infrastrutture digitali.",
                descEn: "Articles, guides and insights from Graldev to understand systems integration, enterprise AI and digital infrastructure modernization.",
                canonicalPathIt: "/insights",
                canonicalPathEn: "/en/insights"
            );
            return View(articles);
        }

        [Route("insights/{slug}")]
        [Route("en/insights/{slug}")]
        public IActionResult Detail(string slug)
        {
            var articles = IsEn ? ArticlesEn : ArticlesIt;
            var article = articles.FirstOrDefault(a => a.Slug.Equals(slug, System.StringComparison.OrdinalIgnoreCase));

            if (article == null)
            {
                return RedirectToAction("Error404", "Home");
            }

            SetupSeo(
                titleIt: $"{article.Title} | Insights Graldev",
                titleEn: $"{article.Title} | Graldev Insights",
                descIt: article.Excerpt,
                descEn: article.Excerpt,
                canonicalPathIt: $"/insights/{slug}",
                canonicalPathEn: $"/en/insights/{slug}"
            );

            // Setup BlogPosting Structured Data Schema
            var baseUrl = "https://www.graldev.com";
            var schema = $@"{{
              ""@context"": ""https://schema.org"",
              ""@type"": ""BlogPosting"",
              ""headline"": ""{article.Title}"",
              ""description"": ""{article.Excerpt}"",
              ""author"": {{
                ""@type"": ""Organization"",
                ""name"": ""{article.Author}""
              }},
              ""publisher"": {{
                ""@type"": ""Organization"",
                ""name"": ""Graldev"",
                ""logo"": {{
                  ""@type"": ""ImageObject"",
                  ""url"": ""{baseUrl}/images/logo.png""
                }}
              }},
              ""datePublished"": ""2026-02-05"",
              ""mainEntityOfPage"": ""{baseUrl}{(IsEn ? $"/en/insights/{slug}" : $"/insights/{slug}")}""
            }}";

            if (ViewData["SeoMetadata"] is PageSeoMetadata seo)
            {
                seo.SchemaJson = schema;
            }

            return View(article);
        }
    }
}
