using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace Graldev.Web.Tests
{
    public class SeoAndRoutingTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public SeoAndRoutingTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/en")]
        [InlineData("/consulenza-informatica")]
        [InlineData("/en/it-consulting")]
        [InlineData("/servizi/system-integration")]
        [InlineData("/en/services/system-integration")]
        [InlineData("/servizi/ai-integration")]
        [InlineData("/en/services/ai-integration")]
        [InlineData("/servizi/software-engineering")]
        [InlineData("/en/services/software-engineering")]
        [InlineData("/servizi/api-data-integration")]
        [InlineData("/en/services/api-data-integration")]
        [InlineData("/servizi/digital-commerce-integration")]
        [InlineData("/en/services/digital-commerce-integration")]
        [InlineData("/servizi/cloud-architecture")]
        [InlineData("/en/services/cloud-architecture")]
        [InlineData("/servizi/business-applications")]
        [InlineData("/en/services/business-applications")]
        [InlineData("/servizi/ar-vr-3d")]
        [InlineData("/en/services/ar-vr-3d")]
        [InlineData("/case-study")]
        [InlineData("/en/case-studies")]
        [InlineData("/case-study/geordie")]
        [InlineData("/en/case-studies/geordie")]
        [InlineData("/case-study/vitinerario")]
        [InlineData("/en/case-studies/vitinerario")]
        [InlineData("/case-study/gralcall")]
        [InlineData("/en/case-studies/gralcall")]
        [InlineData("/settori")]
        [InlineData("/en/sectors")]
        [InlineData("/settori/industria")]
        [InlineData("/en/sectors/industry")]
        [InlineData("/settori/retail-ecommerce")]
        [InlineData("/en/sectors/retail-ecommerce")]
        [InlineData("/settori/telco")]
        [InlineData("/en/sectors/telco")]
        [InlineData("/chi-siamo")]
        [InlineData("/en/about")]
        [InlineData("/insights")]
        [InlineData("/en/insights")]
        [InlineData("/insights/system-integration-quando-serve")]
        [InlineData("/en/insights/system-integration-quando-serve")]
        [InlineData("/contatti")]
        [InlineData("/en/contact")]
        [InlineData("/consulenza-informatica-potenza")]
        [InlineData("/en/it-consulting-potenza")]
        [InlineData("/labs")]
        [InlineData("/en/labs")]
        [InlineData("/privacy")]
        [InlineData("/en/privacy")]
        public async Task MainRoutes_ReturnSuccessAndCorrectSeo(string url)
        {
            // Arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var html = await response.Content.ReadAsStringAsync();

            // Perform robust SEO string checks
            Assert.Contains("<title>", html);
            Assert.Contains("</title>", html);
            Assert.Contains("meta name=\"description\"", html);
            Assert.Contains("link rel=\"canonical\"", html);

            // Check that canonical URL doesn't cross culture boundaries
            if (url.StartsWith("/en"))
            {
                Assert.DoesNotContain("canonical\" href=\"https://www.graldev.com/consulenza-informatica\"", html);
            }
            else if (url != "/" && !url.Contains("robots.txt") && !url.Contains("sitemap.xml"))
            {
                Assert.DoesNotContain("canonical\" href=\"https://www.graldev.com/en\"", html);
            }
        }

        [Theory]
        [InlineData("/Home/Index", "/")]
        [InlineData("/Home/CicDetails", "/consulenza-informatica")]
        [InlineData("/Project/Geordie", "/case-study/geordie")]
        [InlineData("/Project/Ludirex", "/labs")]
        [InlineData("/Project/AR", "/case-study/gralcall")]
        [InlineData("/Project/Parcor", "/labs")]
        [InlineData("/Home/ChangeLanguage?lang=EN", "/en")]
        [InlineData("/Home/ChangeLanguage?lang=IT", "/")]
        public async Task LegacyRoutes_ReturnPermanentRedirect(string oldUrl, string expectedNewUrl)
        {
            // Arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            // Act
            var response = await client.GetAsync(oldUrl);

            // Assert
            Assert.Equal(HttpStatusCode.MovedPermanently, response.StatusCode);
            var location = response.Headers.Location?.ToString();
            Assert.Equal(expectedNewUrl, location);
        }

        [Fact]
        public async Task NonExistingRoute_ReturnsCustom404()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/non-existing-page-abc");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var html = await response.Content.ReadAsStringAsync();
            Assert.Contains("404", html);
            Assert.Contains("Pagina non trovata", html);
        }

        [Theory]
        [InlineData("/case-study/gralcall", "/en/case-studies/gralcall")]
        [InlineData("/en/case-studies/gralcall", "/case-study/gralcall")]
        [InlineData("/servizi/system-integration", "/en/services/system-integration")]
        [InlineData("/en/services/system-integration", "/servizi/system-integration")]
        public async Task LanguageSwitchLinks_PointToCorrespondingLocalizedRoute(string pageUrl, string expectedTargetUrl)
        {
            // Arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            // Act
            var response = await client.GetAsync(pageUrl);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var html = await response.Content.ReadAsStringAsync();

            Assert.Contains($"href=\"{expectedTargetUrl}\"", html);
        }

        [Fact]
        public async Task Sitemap_ReturnsValidXml()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/sitemap.xml");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/xml", response.Content.Headers.ContentType?.MediaType);
            var xml = await response.Content.ReadAsStringAsync();
            Assert.Contains("<urlset", xml);
            Assert.Contains("<loc>", xml);
        }

        [Fact]
        public async Task RobotsTxt_ReturnsValidContent()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/robots.txt");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("text/plain", response.Content.Headers.ContentType?.MediaType);
            var text = await response.Content.ReadAsStringAsync();
            Assert.Contains("User-agent: *", text);
            Assert.Contains("Sitemap:", text);
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/en")]
        public async Task Layout_RendersGraldevTetrisFloatingButtonAndModal(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var html = await response.Content.ReadAsStringAsync();

            Assert.Contains("id=\"graldevTetrisBtn\"", html);
            Assert.Contains("id=\"graldevTetrisModal\"", html);
            Assert.Contains("graldev-tetris.css", html);
            Assert.Contains("graldev-tetris.js", html);
        }
    }
}
