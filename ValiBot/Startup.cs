using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ValiBot.Commands;
using ValiBot.Services;

namespace ValiBot
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            services.AddDbContext<DataContext>(opt => 
                opt.UseSqlServer(_configuration.GetConnectionString("Db")),ServiceLifetime.Singleton);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ValiBot API", Version = "v1" });
            });
            services.AddSingleton<TelegramBot>();
            services.AddSingleton<ICommandExecutor, CommandExecutor>();
            services.AddSingleton<IOperationService, OperationService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IAnalyticService, AnalyticService>();
            services.AddSingleton<BaseCommand, StartCommand>();
            services.AddSingleton<BaseCommand, FinishOperationCommand>();
            services.AddSingleton<BaseCommand, SelectCategoryCommand>();
            services.AddSingleton<BaseCommand, AddOperationCommand>();
            services.AddSingleton<BaseCommand, GetOperationsCommand>();
            services.AddSingleton<BaseCommand, GetAnalyticsCommand>();
            services.AddSingleton<BaseCommand, SelectAnalyticDaysCommand>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ValiBot API"));
            serviceProvider.GetRequiredService<TelegramBot>().GetBot().Wait();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}