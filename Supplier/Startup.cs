using System.Reflection;
using System.Text;
using CommonDataContext;
using CommonInterface;
using ConnectionGateway;
using LSCrud;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SupplierDataContext;
using SupplierInterface;

namespace Supplier
{
    public class Startup
    {
        public static string ConnectionString { get; private set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMvc().AddApplicationPart(Assembly.Load(new AssemblyName(assemblyName: "SupplierDataContext")));
            services.AddMvc().AddApplicationPart(Assembly.Load(new AssemblyName(assemblyName: "SupplierModel")));
            services.AddSingleton<ISupplierDbContext, SupplierDbContext>();
            services.AddScoped<IStaticListRepository, StaticListRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<ICRUD, CRUDEngine>();
            services.AddScoped<IIDGenCriteriaInfo, IDGenCriteriaInfoDC>();

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
            //app.UseDefaultFiles();
            //app.UseStaticFiles();
            DefaultFilesOptions options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("Supplier.html");
            app.UseDefaultFiles(options);
            app.UseStaticFiles();
            //app.UseDefaultFiles(new DefaultFilesOptions
            //{
            //    DefaultFileNames = new
            //   List<string> { "Supplier.html" }
            //});
            app.UseCors("AllowSpecificOrigin");
            app.UseCors("AllowAllMethods");
            app.UseCors("AllowAllHeaders");
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
