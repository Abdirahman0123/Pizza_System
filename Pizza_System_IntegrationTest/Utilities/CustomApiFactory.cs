using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Pizza_System.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;

namespace Pizza_System_IntegrationTest.Utilities
{
    public class CustomApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        public WebApplicationFactory<Program> Application;
        public HttpClient Client;
        public AppDbContext dbContext;
        
        public AppDbContext AppDbContext => dbContext;
        public readonly PostgreSqlContainer _dbContainer =
            new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("The_Pizza")
            .WithUsername("postgres")
            .WithPassword("Awale123")
            .WithExposedPort(5432)
            .Build();

        public CustomApiFactory ()
        {
            Application = new WebApplicationFactory<Program>();
            Application = Application.WithWebHostBuilder(builder =>
            {

                //jwt token configuration 
                builder.ConfigureServices(services =>
                {
                    services.Configure<JwtBearerOptions>(
                        JwtBearerDefaults.AuthenticationScheme,
                        options =>
                        {
                            options.Configuration = new OpenIdConnectConfiguration
                            {
                                Issuer = JwtTokenProvider.Issuer,
                            };
                            options.TokenValidationParameters.ValidIssuer = JwtTokenProvider.Issuer;
                            options.TokenValidationParameters.ValidAudience = JwtTokenProvider.Issuer;
                            options.Configuration.SigningKeys.Add(JwtTokenProvider.SecurityKey);
                        }
                    );
                });

            });
            Client = Application.CreateClient();

        }

        // configure the test to Postgres Docker Testcontainers 
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // get the dbcontext and removed from 
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                services.Remove(dbContextDescriptor!);

                var ctx = services.SingleOrDefault(d => d.ServiceType == typeof(AppDbContext));
                services.Remove(ctx!);

                // add back the container-based dbContext
                services.AddDbContext<AppDbContext>(opts =>
                    opts.UseNpgsql(_dbContainer.GetConnectionString()));
            });

       }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(_dbContainer.GetConnectionString());
            dbContext = new AppDbContext(optionsBuilder.Options);
            await dbContext.Database.MigrateAsync();
        }

        public new async Task DisposeAsync()
        {
            await _dbContainer.DisposeAsync();
        }
    }
}
