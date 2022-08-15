using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Visitor.Pages {
    [AllowAnonymous]
    public class LoginModel : PageModel {
        public string ReturnUrl { get; set; }
        public async Task<IActionResult> OnGetAsync(string paramUsername, string paramPassword) {
            string returnUrl = Url.Content("~/admin");
            try {
                // Clear the existing external cookie
                await HttpContext
                    .SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch { }

            // This is where you will validate the user
            if (paramUsername == "admin" && paramPassword == "password1234") {
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, paramUsername),
                    new Claim(ClaimTypes.Role, "Administrator"),
                };
                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties {
                    IsPersistent = true,
                    RedirectUri = this.Request.Host.Value
                };
                try {
                    await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                } catch (Exception ex) {
                    string error = ex.Message;
                }
                return LocalRedirect(returnUrl);
            }
            return LocalRedirect(Url.Content("~/"));
        }
    }
}