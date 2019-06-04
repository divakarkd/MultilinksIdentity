﻿namespace Multilinks.TokenService.Models
{
   public class WebConsoleClientOptions
   {
      public string AllowedCorsOriginsIdp { get; set; }

      public string AllowedCorsOriginsApi { get; set; }

      public string LoginRedirectUri { get; set; }

      public string SilentLoginRedirectUri { get; set; }

      public string LogoutRedirectUri { get; set; }

      public string RegistrationConfirmedRedirectUri { get; set; }

      public string ResetPasswordSuccessfulRedirectUri { get; set; }
   }
}
