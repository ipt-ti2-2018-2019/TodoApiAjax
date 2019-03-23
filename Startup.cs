using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoApi.Data;

namespace TodoApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Adicionar ligação à BD
            services.AddDbContext<TodoDbContext>(dbOptions =>
            {
                // Configurar ligação para usar SQL Server
                // Outras ligações: https://docs.microsoft.com/en-gb/ef/core/providers/
                // - Ex (MySQL): https://www.nuget.org/packages/MySql.Data.EntityFrameworkCore
                dbOptions.UseSqlServer(Configuration.GetConnectionString("TodoDb"));
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Permitir que a aplicação funcione entre servidores
            app.UseCors(corsOptions =>
            {
                corsOptions.AllowAnyHeader();
                corsOptions.AllowAnyMethod();
                corsOptions.AllowCredentials();
                corsOptions.AllowAnyOrigin();
            });

            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
