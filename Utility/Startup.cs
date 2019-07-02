using ApprovalDataContext;
using ApprovalInterface;
using CommonDataContext;
using CommonInterface;
using CommonModel;
using ConnectionGateway;
using HRDataContext;
using HRInterface;
using LSCrud;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupplierDataContext;
using SupplierInterface;
using PMSDataContext;
using PMSInterface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Utility
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<ISupplierDbContext, SupplierDbContext>();
            services.AddScoped<ICRUD, CRUDEngine>();
            services.AddScoped<IGeneralCodeFile, GeneralCodeFileDC>();
            services.AddScoped<IIDGenCriteriaInfo, IDGenCriteriaInfoDC>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IAppModuleObjectMapping, BusinessObjectMappingRepository>();
            services.AddScoped<IGeneralCodeFileType, GeneralCodeFileTypeDC>();
            services.AddScoped<IGeneralCodeFileLevel, GeneralCodeFileLevelDC>();
            services.AddScoped<IUserMapEmployee, UserMapEmployeeDC>();
            services.AddScoped<IAppLevelDefinition, AppLevelDefinitionDC>();
            services.AddScoped<IAppObjInfoMap_Logic, AppLevelDefinitionDC>();
            services.AddScoped<IAppLevelDefDetAppType, ApproverSelectionDC>();
            services.AddScoped<IGeneralWaitingForApproval, AppObjectInfoMapDC>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IEmailHelper, EmailHelper>();
            services.AddScoped<IUploadedFileRepository, UploadFileRepository>();
            services.AddScoped<IPurchaseRequsitionRepository, PurchaseRequsitionRepository>();
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
            options.DefaultFileNames.Add("Common.html");
            app.UseDefaultFiles(options);
            app.UseStaticFiles();
            app.UseCors();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
