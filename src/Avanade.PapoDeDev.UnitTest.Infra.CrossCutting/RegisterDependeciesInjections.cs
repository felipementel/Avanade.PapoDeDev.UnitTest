using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Account.Interfaces.Repositories;
using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Bank.Interfaces.Repositories;
using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Bank.Interfaces.Services;
using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Bank.Services;
using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Customer.Interfaces.Repositories;
using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Vault.Interfaces.Services;
using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Walltet.Services;
using Avanade.PapoDeDev.UnitTest.Domain.Configs;
using Avanade.PapoDeDev.UnitTest.Domain.Logging;
using Avanade.PapoDeDev.UnitTest.Infra.Data.Repositories;
using Avanade.PapoDeDev.UnitTest.Infra.HttpService;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace Avanade.PapoDeDev.UnitTest.Infra.CrossCutting
{
    public static class RegisterDependeciesInjections
    {
        public static void AddDependeciesInjections(
            this IServiceCollection services,
            ExternalConfigurations options)
        {
            services.AddScoped<IBankService, BankService>();
            services.AddScoped<ICalculatorService, CalculatorService>();

            services.AddSingleton<IBankLogger, BankLogger>();

            services.AddScoped<IBankRepository, BankRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();

            services
                .AddSingleton<IValidateCustomerRegistry, ValidateCustomerRegistry>()
                .AddHttpClient("ExternalValidation", config =>
                {
                    config.BaseAddress = new Uri(options.ValidateCustomerRegistry);
                    config.DefaultRequestHeaders.Add("Accept", "application/json");
                    config.DefaultRequestHeaders.Add("code", "5SHVXi/oxtVX2p4/IYReeupsko4wzrHpi1CVDi37TXu9WUSFSfgdww==");
                })
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());
        }

        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(
                retryCount: 6,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (ex, ts, retryCount, context) =>
                {
                    Console.WriteLine($"Retry Policy - Attempt {retryCount} - Error {ex.Exception?.Message}");
                });
        }

        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(3, TimeSpan.FromSeconds(2),
                onBreak: (ex, ts) => Console.WriteLine($"CircuitBreaker Policy onBreak: - {ex.Exception?.Message} - Time {ts} - Open Circuit"),
                onHalfOpen: () => Console.WriteLine("CircuitBreaker Policy - onHalfOpen: Attempt"),
                onReset: () => Console.WriteLine("CircuitBreaker Policy - onReset: Successful request - Closed Circuit"));
        }
    }
}
