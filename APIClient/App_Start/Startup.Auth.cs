using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using APIClient.Models;

namespace APIClient
{
    public partial class Startup
    {
        // Další informace o konfiguraci ověřování najdete na webu https://go.microsoft.com/fwlink/?LinkId=301864.
        public void ConfigureAuth(IAppBuilder app)
        {
            // Konfigurovat kontext databáze, správce uživatelů a správce přihlášení, aby se používala jedna instance pro každý požadavek
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Povolit aplikaci používat soubor cookie k uložení informací pro přihlášeného uživatele
            // a používat soubor cookie k dočasnému uložení informací o uživateli přihlášeném pomocí zprostředkovatele přihlášení třetí strany
            // Konfigurovat soubor cookie přihlášení
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Umožní aplikaci ověřit razítko zabezpečení při přihlášení uživatele.
                    // Toto je funkce zabezpečení, která je použita, pokud změníte heslo nebo přidáte ke svému účtu externí přihlášení.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Umožňuje aplikaci dočasně uložit informace o uživateli při ověřování druhého faktoru v rámci procesu dvoufaktorového ověřování.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Umožňuje aplikaci zapamatovat druhý faktor přihlašovacího ověření, například telefon nebo e-mail.
            // Jakmile zaškrtnete tuto možnost, druhý krok ověření během procesu přihlášení bude v zařízení, z něhož jste se přihlásili, zapamatován.
            // Tato funkce je obdobou možnosti Zapamatovat uživatele při přihlášení.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Zrušením komentáře u tohoto řádku povolíte protokolování s využitím zprostředkovatelů přihlášení třetích stran
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
    }
}