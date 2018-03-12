using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SoccerManager.Data;
using SoccerManager.Data.DTO.Identity;
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

            services.AddIdentity<User, UserRole>(opts => {
                opts.Password.RequireDigit = false;
                opts.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication()
                .AddJwtBearer(opt => {
                    opt.RequireHttpsMetadata = false;
                    opt.SaveToken = true;
                    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidIssuer = Configuration["Tokens:Issuer"],
                        ValidAudience = Configuration["Tokens:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                    };
                });

            services.AddMvc();

            services.AddAuthorization(opt => {
                opt.AddPolicy("RequireElevatedRights", policy => policy.RequireRole("admin"));
            });

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info { Title = "SoccerManager API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SoccerManager API");
            });

            app.UseMvcWithDefaultRoute();
        }
    }
}
