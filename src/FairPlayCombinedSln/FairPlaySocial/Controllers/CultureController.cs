using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace FairPlaySocial.Controllers
{
    [Route("[controller]/[action]")]
    public class CultureController : Controller
    {
        [HttpGet]
#pragma warning disable S6967 // ModelState.IsValid should be called in controller actions
        public IActionResult Set(string culture, string redirectUri)
#pragma warning restore S6967 // ModelState.IsValid should be called in controller actions
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
