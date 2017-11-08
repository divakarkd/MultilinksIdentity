﻿using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Formatters;
using Multilinks.ApiService.Infrastructure;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Multilinks.ApiService.Filters;

namespace Multilinks.ApiService
{
   public class Startup
   {
      private IConfiguration _configuration { get; }
      private IHostingEnvironment _env { get; }

      public Startup(IConfiguration configuration, IHostingEnvironment env)
      {
         _configuration = configuration;
         _env = env;
      }

      // This method gets called by the runtime. Use this method to add services to the container.
      public void ConfigureServices(IServiceCollection services)
      {
         services.AddMvcCore()
            .AddAuthorization()
            .AddJsonFormatters()
            .AddMvcOptions(opt =>
            {
               var jsonFormatter = opt.OutputFormatters.OfType<JsonOutputFormatter>().Single();
               opt.OutputFormatters.Remove(jsonFormatter);
               opt.OutputFormatters.Add(new IonOutputFormatter(jsonFormatter));

               opt.Filters.Add(typeof(JsonExceptionFilter));

               if(!_env.IsProduction())
               {
                  opt.SslPort = 44301;
               }
               opt.Filters.Add(new RequireHttpsAttribute());
            });

         services.AddRouting(opt => opt.LowercaseUrls = true);

         services.AddApiVersioning(opt =>
         {
            opt.ApiVersionReader = new MediaTypeApiVersionReader();
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.ReportApiVersions = true;
            opt.DefaultApiVersion = new ApiVersion(1, 0);
            opt.ApiVersionSelector = new CurrentImplementationApiVersionSelector(opt);
         });

         services.AddAuthentication("Bearer")
            .AddIdentityServerAuthentication(options =>
            {
               options.Authority = "http://localhost:5000";
               options.RequireHttpsMetadata = false;

               options.ApiName = "api1";
            });
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app)
      {
         if(_env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }

         app.UseAuthentication();

         app.UseMvc();
      }
   }
}
