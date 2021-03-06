using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Riverbed.Test.FreshBrewApi.Models;
using Riverbed.Test.FreshBrewApi.Services;
using Riverbed.Test.FreshBrew.Models;

namespace Riverbed.Test.FreshBrewApi
{
    public class Startup
    {       

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<OrderContext>(opt => opt.UseInMemoryDatabase("OrderList"));
            services.AddTransient<IOrderItem, InMemoryDb>();
            //services.AddMvc();
            services.AddRazorPages();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //  app.UseMvc();
            app.UseRouting();
            app.UseAuthentication();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
