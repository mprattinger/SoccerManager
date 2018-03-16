using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SoccerManager.Extensions
{
    public static class HelperExtension
    {
        public static string StringifyModelStateErrors(this ModelStateDictionary modelState)
        {
            var errors = modelState.Where(x => x.Value.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
                        .Select(x => String.Join(", ", x.Value.Errors.Select(e => e.ErrorMessage))).ToList();
            return String.Join(", ", errors);
        }

        public static bool IsValidEmail(this string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            // CORS
            response.Headers.Add("access-control-expose-headers", "Application-Error");
        }

        public static string StringifyIdentityErrors(this List<IdentityError> identityErrors)
        {
            var errors = identityErrors.Select(e => String.Join(", ", $"{e.Code}: {e.Description}")).ToList();
            return String.Join(", ", errors);
        }
    }
}
