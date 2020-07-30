using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaymentGateway.Adapters;
using PaymentGateway.Command;
using PaymentGateway.Interfaces;
using PaymentGateway.Query;
using PaymentGateway.Validators;

namespace PaymentGateway
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
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddControllers();
            services.AddScoped<AppSettings>();
            services.AddHttpClient<IPaymentProcessor, BankPaymentProcessor>();
            services.AddScoped<IPaymentQueryHandler, PaymentQueryHandler>();
            services.AddScoped<IPaymentCommandHandler, PaymentCommandHandler>();
            services.AddScoped<IPaymentCommandRepository, PaymentCommandRepository>();
            services.AddScoped<IPaymentQueryRepository, PaymentQueryRepository>();
            services.AddScoped<PaymentCommandValidator>();
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment Gateway API V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}