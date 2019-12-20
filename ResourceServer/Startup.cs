/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๑๘/๑๒/๒๕๖๒>
Modify date : <๒๐/๑๒/๒๕๖๒>
Description : <>
=============================================
*/

using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ResourceServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://devadfs.mahidol.ac.th/adfs";
                    options.Audience = "e43a62d7-381a-453d-841c-2ec769f9cc8e";
                    options.RequireHttpsMetadata = false;
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            string authorization = context.Request.Headers["Authorization"];

                            if (String.IsNullOrEmpty(authorization))
                            {
                                context.NoResult();

                                return Task.CompletedTask;
                            }

                            if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                            {
                                context.Token = authorization.Substring("Bearer ".Length).Trim();
                            }

                            if (string.IsNullOrEmpty(context.Token))
                            {
                                context.NoResult();

                                return Task.CompletedTask;
                            }

                            try
                            {
                                byte[] decDataByte = Convert.FromBase64String(context.Token);
                                context.Token = Encoding.UTF8.GetString(decDataByte);

                                char[] tokenArray = context.Token.ToCharArray();
                                Array.Reverse(tokenArray);
                                context.Token = new string(tokenArray);
                            }
                            catch
                            {
                                context.NoResult();
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
            );
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
