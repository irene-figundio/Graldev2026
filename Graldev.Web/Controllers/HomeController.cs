using Microsoft.AspNetCore.Mvc;
using Graldev.Web.Models;
using System.Text.Json;
using System.Text;
using System.Collections.Generic;

namespace Graldev.Web.Controllers
{
    public class HomeController : BaseController
    {
        [Route("")]
        [Route("en")]
        public IActionResult Index()
        {
            SetupSeo(
                titleIt: "Consulenza Informatica, System Integration e AI | Graldev",
                titleEn: "IT Consulting, System Integration and AI | Graldev",
                descIt: "Graldev affianca le aziende con consulenza informatica, System Integration, AI, software engineering, API e cloud per evolvere sistemi e processi digitali.",
                descEn: "Graldev partners with enterprises providing IT consulting, System Integration, AI, software engineering, API and cloud to evolve digital systems.",
                canonicalPathIt: "",
                canonicalPathEn: "/en"
            );
            return View();
        }

        [Route("chi-siamo")]
        [Route("en/about")]
        public IActionResult About()
        {
            SetupSeo(
                titleIt: "Graldev | Consulenza Informatica, System Integration e AI",
                titleEn: "About Graldev | IT Consulting, System Integration and AI",
                descIt: "Graldev è un'azienda di consulenza informatica con sede a Potenza. Affianchiamo aziende con System Integration, AI, software engineering e soluzioni IT.",
                descEn: "Graldev is an IT consulting company based in Potenza, Italy. We partner with businesses for System Integration, AI, software engineering and IT solutions.",
                canonicalPathIt: "/chi-siamo",
                canonicalPathEn: "/en/about"
            );
            return View();
        }

        [Route("consulenza-informatica-potenza")]
        [Route("en/it-consulting-potenza")]
        public IActionResult LocalSeoPotenza()
        {
            SetupSeo(
                titleIt: "Consulenza Informatica Potenza | System Integration e AI | Graldev",
                titleEn: "IT Consulting Potenza | System Integration and AI | Graldev",
                descIt: "Graldev offre consulenza informatica a Potenza e in tutta Italia: System Integration, AI, software engineering, API, cloud e soluzioni IT per aziende.",
                descEn: "Graldev provides IT consulting in Potenza and all of Italy: System Integration, AI, software engineering, API, cloud and IT solutions for companies.",
                canonicalPathIt: "/consulenza-informatica-potenza",
                canonicalPathEn: "/en/it-consulting-potenza"
            );
            return View();
        }

        [Route("contatti")]
        [Route("en/contact")]
        public IActionResult Contacts()
        {
            SetupSeo(
                titleIt: "Contatta Graldev | Consulenza Informatica e System Integration",
                titleEn: "Contact Graldev | IT Consulting and System Integration",
                descIt: "Raccontaci la tua esigenza IT. Graldev affianca aziende in System Integration, AI, software engineering, API, cloud e modernizzazione.",
                descEn: "Tell us about your IT needs. Graldev supports companies with System Integration, AI, software engineering, API, cloud and modernization.",
                canonicalPathIt: "/contatti",
                canonicalPathEn: "/en/contact"
            );
            return View(new ContactViewModel());
        }

        [HttpPost]
        [Route("contatti")]
        [Route("en/contact")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitContact(ContactViewModel model, [FromServices] IHttpClientFactory httpClientFactory, [FromServices] IConfiguration configuration)
        {
            bool isEn = IsEn;

            // Check Honeypot spam field
            if (!string.IsNullOrEmpty(model.Website))
            {
                // Silent success to mislead bots
                TempData["SuccessMessage"] = isEn
                    ? "Thank you! Your request has been received."
                    : "Grazie! La tua richiesta è stata presa in carico.";
                return RedirectToAction("Contacts");
            }

            if (!ModelState.IsValid)
            {
                SetupSeo(
                    titleIt: "Contatta Graldev | Consulenza Informatica e System Integration",
                    titleEn: "Contact Graldev | IT Consulting and System Integration",
                    descIt: "Raccontaci la tua esigenza IT. Graldev affianca aziende in System Integration, AI, software engineering, API, cloud e modernizzazione.",
                    descEn: "Tell us about your IT needs. Graldev supports companies with System Integration, AI, software engineering, API, cloud and modernization.",
                    canonicalPathIt: "/contatti",
                    canonicalPathEn: "/en/contact"
                );
                return View("Contacts", model);
            }

            // Server-side Google reCAPTCHA v3 verification
            var secretKey = configuration["Recaptcha:SecretKey"];
            if (!string.IsNullOrEmpty(secretKey) && !string.IsNullOrEmpty(model.RecaptchaToken))
            {
                try
                {
                    var client = httpClientFactory.CreateClient();
                    var response = await client.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={model.RecaptchaToken}", null);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        using var doc = JsonDocument.Parse(json);
                        var root = doc.RootElement;
                        bool success = root.TryGetProperty("success", out var sProp) && sProp.GetBoolean();
                        double score = root.TryGetProperty("score", out var scProp) ? scProp.GetDouble() : 0.0;
                        double minScore = double.TryParse(configuration["Recaptcha:MinimumScore"], out var ms) ? ms : 0.5;

                        if (!success || score < minScore)
                        {
                            ModelState.AddModelError("", isEn ? "Security verification failed. Please try again." : "Verifica di sicurezza non riuscita. Riprova.");
                            SetupSeo(
                                titleIt: "Contatta Graldev | Consulenza Informatica e System Integration",
                                titleEn: "Contact Graldev | IT Consulting and System Integration",
                                descIt: "Raccontaci la tua esigenza IT. Graldev affianca aziende in System Integration, AI, software engineering, API, cloud e modernizzazione.",
                                descEn: "Tell us about your IT needs. Graldev supports companies with System Integration, AI, software engineering, API, cloud and modernization.",
                                canonicalPathIt: "/contatti",
                                canonicalPathEn: "/en/contact"
                            );
                            return View("Contacts", model);
                        }
                    }
                }
                catch
                {
                    // Fail-safe logging
                }
            }

            // Perform Mail Sending / File Submission Logging
            try
            {
                var logPath = Path.Combine(Directory.GetCurrentDirectory(), "contact_submissions.json");
                var submissionJson = JsonSerializer.Serialize(model, new JsonSerializerOptions { WriteIndented = true });
                System.IO.File.AppendAllText(logPath, submissionJson + Environment.NewLine);
            }
            catch
            {
                // Fail-safe
            }

            TempData["SuccessMessage"] = isEn
                ? "Thank you! Your message has been sent successfully. We will contact you soon."
                : "Grazie! Il tuo messaggio è stato inviato con successo. Ti ricontatteremo al più presto.";

            return RedirectToAction("Contacts");
        }

        [Route("studios")]
        [Route("graldev-studios")]
        [Route("en/studios")]
        [Route("en/graldev-studios")]
        public IActionResult Studios()
        {
            SetupSeo(
                titleIt: "Graldev Studios | Creative AI & Produzione Multimediale",
                titleEn: "Graldev Studios | Creative AI & Multimedia Production",
                descIt: "Graldev Studios è la divisione creativa di Graldev dedicata a produzioni video, audio, media digitali, 3D interattivo ed intelligenza artificiale generativa.",
                descEn: "Graldev Studios is Graldev's creative division dedicated to video, audio, digital media, interactive 3D, and generative AI production.",
                canonicalPathIt: "/studios",
                canonicalPathEn: "/en/studios"
            );
            return View();
        }

        [Route("labs")]
        [Route("en/labs")]
        public IActionResult Labs()
        {
            SetupSeo(
                titleIt: "Graldev Labs | Progetti Sperimentali e Tecnologia Interattiva",
                titleEn: "Graldev Labs | Experimental Projects & Interactive Tech",
                descIt: "Progetti sperimentali, interactive technology, gaming, educational e ricerca applicata di Graldev.",
                descEn: "Graldev's experimental projects, interactive technology, gaming, educational and applied research.",
                canonicalPathIt: "/labs",
                canonicalPathEn: "/en/labs"
            );
            return View();
        }

        [Route("privacy")]
        [Route("en/privacy")]
        public IActionResult Privacy()
        {
            SetupSeo(
                titleIt: "Informativa sulla Privacy e sui Cookie | Graldev",
                titleEn: "Privacy & Cookie Policy | Graldev",
                descIt: "Informazioni su come Graldev raccoglie, tratta e protegge i dati personali e sull'utilizzo dei cookie nelle proprie piattaforme digitali.",
                descEn: "Information on how Graldev collects, processes and protects personal data and how cookies are used across our digital platforms.",
                canonicalPathIt: "/privacy",
                canonicalPathEn: "/en/privacy"
            );
            return View();
        }

        [Route("Error404")]
        public IActionResult Error404()
        {
            SetupSeo(
                titleIt: "Pagina Non Trovata | Graldev",
                titleEn: "Page Not Found | Graldev",
                descIt: "La pagina che stai cercando non esiste.",
                descEn: "The page you are looking for does not exist.",
                canonicalPathIt: "/Error404",
                canonicalPathEn: "/Error404",
                noIndex: true
            );
            Response.StatusCode = 404;
            return View();
        }

        [Route("Error500")]
        public IActionResult Error500()
        {
            SetupSeo(
                titleIt: "Errore di Sistema | Graldev",
                titleEn: "System Error | Graldev",
                descIt: "Si è verificato un errore di sistema.",
                descEn: "A system error has occurred.",
                canonicalPathIt: "/Error500",
                canonicalPathEn: "/Error500",
                noIndex: true
            );
            Response.StatusCode = 500;
            return View();
        }

        // Programmatic robots.txt
        [HttpGet]
        [Route("robots.txt")]
        public IActionResult RobotsTxt()
        {
            var sb = new StringBuilder();
            sb.AppendLine("User-agent: *");
            sb.AppendLine("Allow: /");
            sb.AppendLine("");
            sb.AppendLine("Sitemap: https://www.graldev.com/sitemap.xml");
            return Content(sb.ToString(), "text/plain", Encoding.UTF8);
        }

        // Programmatic sitemap.xml
        [HttpGet]
        [Route("sitemap.xml")]
        public IActionResult SitemapXml()
        {
            var baseUrl = "https://www.graldev.com";
            var urls = new List<string>
            {
                // Italian
                "",
                "/consulenza-informatica",
                "/servizi/system-integration",
                "/servizi/ai-integration",
                "/servizi/software-engineering",
                "/servizi/api-data-integration",
                "/servizi/digital-commerce-integration",
                "/servizi/cloud-architecture",
                "/servizi/business-applications",
                "/servizi/ar-vr-3d",
                "/case-study",
                "/case-study/geordie",
                "/case-study/vitinerario",
                "/case-study/gralcall",
                "/settori",
                "/settori/industria",
                "/settori/retail-ecommerce",
                "/settori/telco",
                "/chi-siamo",
                "/insights",
                "/insights/system-integration-quando-serve",
                "/insights/software-standard-sviluppo-custom",
                "/insights/introduzione-ai-processi-aziendali",
                "/contatti",
                "/consulenza-informatica-potenza",
                "/labs",
                "/privacy",

                // English
                "/en",
                "/en/it-consulting",
                "/en/services/system-integration",
                "/en/services/ai-integration",
                "/en/services/software-engineering",
                "/en/services/api-data-integration",
                "/en/services/digital-commerce-integration",
                "/en/services/cloud-architecture",
                "/en/services/business-applications",
                "/en/services/ar-vr-3d",
                "/en/case-studies",
                "/en/case-studies/geordie",
                "/en/case-studies/vitinerario",
                "/en/case-studies/gralcall",
                "/en/sectors",
                "/en/sectors/industry",
                "/en/sectors/retail-ecommerce",
                "/en/sectors/telco",
                "/en/about",
                "/en/insights",
                "/en/insights/system-integration-quando-serve",
                "/en/insights/software-standard-sviluppo-custom",
                "/en/insights/introduzione-ai-processi-aziendali",
                "/en/contact",
                "/en/it-consulting-potenza",
                "/en/labs",
                "/en/privacy"
            };

            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

            foreach (var url in urls)
            {
                sb.AppendLine("  <url>");
                sb.AppendLine($"    <loc>{baseUrl}{url}</loc>");
                sb.AppendLine("    <changefreq>daily</changefreq>");
                sb.AppendLine("    <priority>0.8</priority>");
                sb.AppendLine("  </url>");
            }

            sb.AppendLine("</urlset>");

            return Content(sb.ToString(), "application/xml", Encoding.UTF8);
        }
    }
}
