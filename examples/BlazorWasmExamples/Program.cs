using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using BBComponents.Services;
using System.Globalization;

namespace BlazorWasmExamples
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var culture = new CultureInfo("en-US");
            CultureInfo.CurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddBaseAddressHttpClient();

            // Service to add alerts.
            builder.Services.AddScoped<IAlertService, AlertService>();

            await builder.Build().RunAsync();
        }
    }
}
