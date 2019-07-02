using CenterPointCoreSecurity.Data;
using CenterPointCoreSecurity.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Security.Model;
using System.Reflection;
using System.Text;

namespace CenterPointCoreSecurity
{
    public class Startup
    {
        public static string ConnectionString { get; private set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
           
            ConnectionString = configuration
                   .GetSection("ConnectionStrings:DefaultConnection").Value;
            
        }

        public IConfiguration Configuration { get; }


        //public static string GetConnectionString()
        //{
        //    string connString = String.Empty;

        //    return connString;
        //}

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(options =>
        options.SerializerSettings.ContractResolver
         = new DefaultContractResolver());
            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddDefaultTokenProviders();
            MyConfiguration configuration = new MyConfiguration();
            Configuration.GetSection("ConnectionStrings").Bind(configuration);
            services.AddSingleton(configuration);
            // services.Configure<MyConfiguration>(Configuration.GetSection("ConnectionStrings"));

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.SaveToken = true;
                option.RequireHttpsMetadata = false;
                option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = "Shanta",
                    ValidIssuer = "Parama",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authnetication"))
                };
            });
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("https://192.168.2.8/CoreSolution"));
                options.AddPolicy("AllowAllMethods",
                        builder =>
                       {
                           builder.WithOrigins("https://192.168.2.8/CoreSolution")
                                      .AllowAnyMethod();
                       });
                options.AddPolicy("AllowAllHeaders",
                        builder =>
                        {
                            builder.WithOrigins("https://192.168.2.8/CoreSolution")
                                   .AllowAnyHeader();
                        });
            });
            //services.AddSingleton<IMyDependency, MyDependency>();
            //services.AddTransient<IMyDependency, MyDependency>();

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Permissions", builder =>
            //    {
            //        builder.Requirements.Add(new PermissionsRequirement("SysAdmin"));
            //    });
            //    options.AddPolicy("Supplier", builder =>
            //    {
            //        builder.Requirements.Add(new PermissionsRequirement("Supplier"));
            //    });
            //});

            //services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

            //services.AddScope<IAuthorizationHandler, PermissionHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for 
                // production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCors("AllowSpecificOrigin");
            app.UseCors("AllowAllMethods");
            app.UseCors("AllowAllHeaders");
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
