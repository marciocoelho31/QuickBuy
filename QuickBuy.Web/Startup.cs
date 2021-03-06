using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using QuickBuy.Dominio.Contratos;
using QuickBuy.Repositorio.Contexto;
using QuickBuy.Repositorio.Repositorios;

namespace QuickBuy.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup()//IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("config.json", optional: false, reloadOnChange: true);

            //Configuration = configuration;
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                //.AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddNewtonsoftJson(o =>
                {
                    o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();     // contexto da requisi��o - instancia unica do httpcontextaccessor

            var connectionString = Configuration.GetConnectionString("QuickBuyDB"); // pegando no config.json
            services.AddDbContext<QuickBuyContexto>(option => option
                                                    .UseLazyLoadingProxies()
                                                    .UseMySql(connectionString,
                                                        m => m.MigrationsAssembly("QuickBuy.Repositorio")));

            services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>(); // inje��o de depend�ncia
            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            services.AddScoped<IPedidoRepositorio, PedidoRepositorio>();

            // In production, the Angular files will be served from this directory

            //ServiceProvider serviceProvider = services.BuildServiceProvider();
            //IWebHostEnvironment env = serviceProvider.GetService<IWebHostEnvironment>();
            //if (env.IsProduction())
            //{
                services.AddSpaStaticFiles(configuration =>
                {
                    configuration.RootPath = "ClientApp/dist";
                });
            //}
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            //if (env.IsProduction())
            {
                app.UseStaticFiles();
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller}/{action=Index}/{id?}");
            //});
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";  // <----------

                if (env.IsDevelopment())
                {

                    //spa.UseAngularCliServer(npmScript: "start");   // <==========================
                    // ou... 
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");  // <=== pra rodar o angular por fora (usar npm start - melhor - ou ng serve) - na pasta ClientApp

                }
            });
        }
    }
}
