using FairPlayCombined.DataAccess.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Xml;

namespace FairPlayTube.Controllers
{
    /// <summary>
    /// Dynamically generates a sitemap
    /// </summary>
    [Route("")]
    public class SitemapController(
        IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        ILogger<SitemapController> logger) : Controller
    {

        /// <summary>
        /// Generates a sitemap
        /// </summary>
        /// <returns></returns>
        [HttpGet("Sitemap.xml")]
        public async Task<FileContentResult> Sitemap(CancellationToken cancellationToken)
        {
            StringBuilder stringBuilder = new();
            using var xmlWriter = XmlWriter.Create(stringBuilder, new XmlWriterSettings
            {
                Indent = true,
                Async = true,
                
            });
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement(localName: "urlset", ns: "http://www.sitemaps.org/schemas/sitemap/0.9");
            var encodedUrl = new Uri(this.HttpContext.Request.GetEncodedUrl());
            var baseUrl = encodedUrl.AbsoluteUri.Replace(encodedUrl.AbsolutePath, String.Empty);
            AddLocation(xmlWriter: xmlWriter, url: baseUrl);
            try
            {
                var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                var allVideos = dbContext.VideoInfo.OrderByDescending(p => p.VideoInfoId);
                foreach (var singleVideo in allVideos)
                {
                    AddLocation(xmlWriter: xmlWriter, url: $"{baseUrl}/Public/WatchVideo/{singleVideo.VideoId}");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error: {ErrorMessage}.", ex.Message);
            }
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            var result = stringBuilder.ToString();
            return File(Encoding.UTF8.GetBytes(result),
                System.Net.Mime.MediaTypeNames.Application.Xml);
        }

        private static void AddLocation(XmlWriter xmlWriter, string url)
        {
            xmlWriter.WriteStartElement(localName: "url");
            xmlWriter.WriteElementString(localName: "loc", value: url);
            xmlWriter.WriteEndElement();
        }
    }
}
