using System;
using System.Net;
using System.Security.Principal;
using System.Text;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SoccerManager.Auth;
using SoccerManager.Auth.Handlers;
using SoccerManager.Auth.Requirements;
using SoccerManager.Data;
using SoccerManager.Data.DTO.Identity;
using SoccerManager.Extensions;
using SoccerManager.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace SoccerManager
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
            //services.AddDbContext<ApplicationDbContext>(opts => opts.UseSqlite("Data Source=soccer.db"));
            services.AddDbContext<ApplicationDbContext>(opts => opts.UseInMemoryDatabase("soccer"));

            services.AddSingleton<IJwtFactory, JwtFactory>();

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            var sKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAppSettingOptions["Key"]));

            services.Configure<JwtIssuerOptions>(opt =>
            {
                opt.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                opt.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];

                opt.SigningCredentials = new SigningCredentials(sKey, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = sKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            services.AddAuthorization(options =>
            {
                //options.AddPolicy("ApiUser", policy => policy.RequireClaim(   , Helpers.Constants.Strings.JwtClaims.ApiAccess));
                options.AddPolicy("UserManagement", policy => policy.Requirements.Add(new UserManagementRequirement()));
            });

            var builder = services.AddIdentityCore<User>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
            builder.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.AddAutoMapper();
            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "SoccerManager API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme { In = "header", Description = "Please insert JWT with Bearer into field", Name = "Authorization", Type = "apiKey" });
            });

            services.AddTransient<IAuthorizationHandler, UserManagementHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler(builder =>
            {
                builder.Run(
                          async context =>
                          {
                              context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                              context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                              var error = context.Features.Get<IExceptionHandlerFeature>();
                              if (error != null)
                              {
                                  context.Response.AddApplicationError(error.Error.Message);
                                  await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                              }
                          });
            });

            app.UseAuthentication();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SoccerManager API");
            });

            app.UseMvc();
        }
    }
}
