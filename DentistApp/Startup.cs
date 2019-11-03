using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using DentistApp.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore.Extensions;

namespace DentistApp
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

            Environment.SetEnvironmentVariable("TestLecturer", "this_is_Lecturer_key");
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("TestLecturer")));

             Environment.SetEnvironmentVariable("TestIntern", "this_is_Intern_key");

            var symmetricSecurityKeyIntern = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("TestIntern")));

            Environment.SetEnvironmentVariable("TestClient", "this_is_Client_key");

            var symmetricSecurityKeyClient = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("TestClient")));

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "dentist.lt",
                    ValidAudience = "readers",
                    IssuerSigningKey = symmetricSecurityKey
                };
            }).AddJwtBearer("intern", options =>
            { 
                 options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "dentist.lt",
                    ValidAudience = "readers",
                    IssuerSigningKey = symmetricSecurityKeyIntern
                };
            }).AddJwtBearer("client", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "dentist.lt",
                    ValidAudience = "readers",
                    IssuerSigningKey = symmetricSecurityKeyClient
                };
               
            } ).AddJwtBearer("lecturer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "dentist.lt",
                    ValidAudience = "readers",
                    IssuerSigningKey = symmetricSecurityKey
                };
            });


            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                    JwtBearerDefaults.AuthenticationScheme,
                    "intern");
                defaultAuthorizationPolicyBuilder =
                    defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });

            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                    JwtBearerDefaults.AuthenticationScheme,
                    "client");
                defaultAuthorizationPolicyBuilder =
                    defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });
            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                    JwtBearerDefaults.AuthenticationScheme,
                    "lecturer");
                defaultAuthorizationPolicyBuilder =
                    defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });

         //  var Connection = new MySqlConnection("server=us-cdbr-iron-east-05.cleardb.net;port=3306;database=heroku_d20a76ba102b272;uid=b52a535439180b;password=4c4ae86f");

            services.AddDbContext<dentistdbContext>(opt =>
          //  opt.UseMySQL("server=us-cdbr-iron-east-05.cleardb.net;port=3306;database=heroku_d20a76ba102b272;uid=b52a535439180b;password=4c4ae86f"));
             opt.UseSqlServer(Configuration.GetConnectionString("Development")));


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

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

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
