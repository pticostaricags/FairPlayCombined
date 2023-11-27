using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace FairPlayTube.Controllers
{
    [Route("[controller]/[action]")]
    public class CultureController : Controller
    {
        [HttpGet]
        public IActionResult Set(string culture, string redirectUri)
        {
            if (culture != null)
            {
                HttpContext.Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(
                        new RequestCulture(culture, culture)));
            }

            return LocalRedirect(redirectUri);
        }
    }
}
