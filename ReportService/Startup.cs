using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using ReportService.Consts;
using ReportService.Data;
using ReportService.Domain;
using ReportService.Services.Abstract;
using ReportService.Services.Implementations;
using System;
using System.Net.Http;
using System.Reflection;

namespace ReportService
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
            services.AddSingleton(new DbOptions(Configuration.GetConnectionString("dbconnect")));
            services.AddSingleton<IRepository, Repository>();
            services.AddControllers();
            services.Configure<Settings>(Configuration.GetSection(nameof(Settings)));

            var accountingServiceUrl = Configuration.GetValue<string>("Services:AccountingServiceUrl");
            services.AddHttpClient(Constants.ACCOUNTING_HTTP_CLIENT, c =>
            {
                c.BaseAddress = new Uri(accountingServiceUrl);
            }).AddPolicyHandler(GetRetryPolicy());

            AddHttpClient(services, "Services:SalaryServiceUrl", Constants.SALARY_HTTP_CLIENT);
            AddHttpClient(services, "Services:AccountingServiceUrl", Constants.ACCOUNTING_HTTP_CLIENT);

            services.AddSingleton<IReportGenerator, ReportGenerator>();
            services.AddSingleton<ISalaryService, SalaryService>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(1000));
        }

        private void AddHttpClient(IServiceCollection services, string settingUrlPath, string clientAlias)
        {
            var serviceUrl = Configuration.GetValue<string>(settingUrlPath);
            services.AddHttpClient(clientAlias, c =>
            {
                c.BaseAddress = new Uri(serviceUrl);
            }).AddPolicyHandler(GetRetryPolicy());
        }
    }
}