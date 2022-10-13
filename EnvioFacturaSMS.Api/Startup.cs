using EnvioFacturaSMS.Aplications.Middleware;
using EnvioFacturaSMS.Applications.Middleware;
using EnvioFacturaSMS.Domain.Interfaces;
using EnvioFacturaSMS.Domain.Models;
using EnvioFacturaSMS.Domain.Services;
using EnvioFacturaSMS.Infraestructure.Context;
using EnvioFacturaSMS.Infraestructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;

namespace EnvioFacturaSMS.Api
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
            services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            services.AddControllers(options => {
                options.Filters.Add(typeof(ErrorHandlerMiddleware));
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            CreateDependencyInjection(services);
        }

        public void CreateDependencyInjection(IServiceCollection services)
        {
            services.Configure<AppConfigurations>(Configuration.GetSection("AppConfigurations"));
            services.AddDbContext<PrivadaContext>(x => 
                x.UseSqlServer(Environment.GetEnvironmentVariable("DB_SERVER") ?? Configuration.GetConnectionString("SqlConection")), ServiceLifetime.Transient);
            services.AddMediatR(Assembly.Load("EnvioFacturaSMS.Applications"), Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient(typeof(IAzureService), typeof(AzureService));
            services.AddTransient(typeof(IAbonoService), typeof(AbonosServices));
            services.AddTransient(typeof(IPagareService), typeof(PagareService));
            services.AddTransient(typeof(IContratoService), typeof(ContratoService));
            services.AddTransient(typeof(IMasivianService), typeof(MasivianService));
            services.AddTransient(typeof(ILogService), typeof(LogService));
            services.AddTransient(typeof(IParametrosService), typeof(ParametrosService));
            services.AddTransient<IAuthorizationService, AuthorizationService>();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                );
            });

            
            services.AddSwaggerGen();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Envio SMS V1.0");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors("MyPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
