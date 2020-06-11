using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlazorServerSideForDebug
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //var culture = "en";
            //Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);
            //Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(culture);
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStaticWebAssets();
                    webBuilder.UseStartup<Startup>();
                });
    }
}
