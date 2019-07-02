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
using Newtonsoft.Json.Serialization;
using PMSDataContext;
using PMSInterface;
using System.Text;

namespace ProcurementManagement
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(options =>
        options.SerializerSettings.ContractResolver
         = new DefaultContractResolver());
            services.AddSingleton<ISupplierDbContext, SupplierDbContext>();
            services.AddScoped<IStaticListRepository, StaticListRepository>();
            services.AddScoped<IPurchaseRequsitionRepository, PurchaseRequsitionRepository>();
            services.AddScoped<ITermsConditionRepository, TermsConditionRepository>();
            services.AddScoped<ICRUD, CRUDEngine>();
            services.AddScoped<IIDGenCriteriaInfo, IDGenCriteriaInfoDC>();
            services.AddScoped<IProduct, ProductRepository>();
            services.AddScoped<IRFPProcessingRepository, RFPProcessingRepository>();
            services.AddScoped<IQuotationRepository, QuotationRepository>();
            services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
            services.AddScoped<IItemReceiveRepository, ItemReceiveRepository>();
            services.AddScoped<ISupplierInvoiceRepository, SupplierInvoiceRepository>();

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
                    builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());//.WithOrigins("https://localhost:44379"));
                                                                                           //options.AddPolicy("AllowAllMethods",
                                                                                           //        builder =>
                                                                                           //        {
                                                                                           //            builder.WithOrigins("https://localhost:44379")
                                                                                           //                       .AllowAnyMethod();
                                                                                           //        });
                                                                                           //options.AddPolicy("AllowAllHeaders",
                                                                                           //        builder =>
                                                                                           //        {
                                                                                           //            builder.WithOrigins("https://localhost:44379")
                                                                                           //                   .AllowAnyHeader();
                                                                                           //        });
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
                app.UseHsts();
            }
            DefaultFilesOptions options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("Procurement.html");
            app.UseDefaultFiles(options);
            app.UseStaticFiles();
            app.UseCors();
            //app.UseCors("AllowAllMethods");
            //app.UseCors("AllowAllHeaders");
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
