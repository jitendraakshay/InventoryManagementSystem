using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Threading.Tasks;
using InventoryManagementSystem.Helper;

namespace TicketBookingSystem
{
    public class CookieValidator
    {       
        public static async Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            //_seenCount++;
            //if(_seenCount>=5)
            //{
            //    context.RejectPrincipal();
            //    await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //    _seenCount = 0;
            //}

            var binding = context.HttpContext.Features.Get<ITlsTokenBindingFeature>()?.GetProvidedTokenBindingId();
            var tlsTokenBinding = binding == null ? null : Convert.ToBase64String(binding);
            var cookie = context.Options.CookieManager.GetRequestCookie(context.HttpContext, context.Options.CookieName);            
            if (cookie == null)
            {
                context.RejectPrincipal();
                await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

        }
    }
}
