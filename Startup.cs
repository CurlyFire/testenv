using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace testenv
{
    public static class ConfigurationProviderExtensions
    {
        public static HashSet<string> GetFullKeyNames(this IConfigurationProvider provider, string rootKey, HashSet<string> initialKeys)
        {
            foreach (var key in provider.GetChildKeys(Enumerable.Empty<string>(), rootKey))
            {
                string surrogateKey = key;
                if (rootKey != null)
                {
                    surrogateKey = rootKey + ":" + key;
                }

                GetFullKeyNames(provider, surrogateKey, initialKeys);

                if (!initialKeys.Any(k => k.StartsWith(surrogateKey)))
                {
                    initialKeys.Add(surrogateKey);
                }
            }

            return initialKeys;
        }
    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var root = (ConfigurationRoot)configuration;
            var builder = new StringBuilder();
            builder.AppendLine("Voici les providers de configuration et leurs valeurs:");
            foreach (var provider in root.Providers)
            {
                builder.AppendLine(string.Empty);
                builder.AppendLine(provider.ToString());
                var cles = provider.GetFullKeyNames(null, new HashSet<string>());
                foreach (var cle in cles)
                {
                    if (provider.TryGet(cle, out var value))
                    {
                        builder.AppendLine($"\t{cle}={value}");
                    }
                }
            }
            Console.Write(builder.ToString());

        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
