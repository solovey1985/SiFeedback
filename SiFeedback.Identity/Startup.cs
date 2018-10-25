using System;
using System.Linq;
using System.Reflection;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SiFeedback.Identity.Data;
using SiFeedback.Identity.Models;

namespace SiFeedback.Identity {
    public class Startup {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public Startup (IConfiguration configuration, IHostingEnvironment environment) {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices (IServiceCollection services) {
            string connectionString = Configuration.GetConnectionString ("Local");

            var migrationsAssembly = typeof (Startup).GetTypeInfo ().Assembly.GetName ().Name;

            services.AddDbContext<ApplicationDbContext> (options =>
                options.UseSqlServer (connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole> ()
                .AddEntityFrameworkStores<ApplicationDbContext> ()
                .AddDefaultTokenProviders ();

            services.AddMvc ();

            services.Configure<IISOptions> (iis => {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            var builder = services.AddIdentityServer (options => {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddAspNetIdentity<ApplicationUser> ()

                .AddConfigurationStore (options => {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer (connectionString,
                            sql => sql.MigrationsAssembly (migrationsAssembly));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore (options => {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer (connectionString,
                            sql => sql.MigrationsAssembly (migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 15; // frequency in seconds to cleanup stale grants. 15 is useful during debugging
                });

            if (Environment.IsDevelopment ()) {
                builder.AddDeveloperSigningCredential ();
            } else {
                throw new Exception ("need to configure key material");
            }

        }

        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            InitializeDatabase (app);

            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
                app.UseDatabaseErrorPage ();
            } else {
                app.UseExceptionHandler ("/Home/Error");
            }

            app.UseStaticFiles ();
            app.UseIdentityServer ();
            app.UseMvcWithDefaultRoute ();
        }

        private void InitializeDatabase (IApplicationBuilder app) {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory> ().CreateScope ()) {
                
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext> ().Database.Migrate ();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext> ();
                context.Database.Migrate ();
                if (!context.Clients.Any ()) {
                    foreach (var client in Config.GetClients ()) {
                        context.Clients.Add (client.ToEntity ());
                    }
                    context.SaveChanges ();
                }

                if (!context.IdentityResources.Any ()) {
                    foreach (var resource in Config.GetIdentityResources ()) {
                        context.IdentityResources.Add (resource.ToEntity ());
                    }
                    context.SaveChanges ();
                }

                if (!context.ApiResources.Any ()) {
                    foreach (var resource in Config.GetApiResources ()) {
                        context.ApiResources.Add (resource.ToEntity ());
                    }
                    context.SaveChanges ();
                }
            }
        }
    }
}