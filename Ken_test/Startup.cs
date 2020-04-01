using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ken_test.Bos;
using Ken_test.Middlewares;
using Ken_test.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Swashbuckle.AspNetCore.Swagger;

namespace Ken_test
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public IHostingEnvironment _hostingEnvironment;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<BoPriver>();
            services.AddScoped<UserInfoRepo>();
            services.AddScoped<MessageLogRepo>();
            services.AddAutoMapper();

            services.AddCors(options =>
            {
                options.AddPolicy(
                    "AllowDomain",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                        // builder.WithOrigins("http://localhost:8080") //AllowAnyOrigin 与 WithOrigins 冲突
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .AllowAnyMethod();
                    });
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContextPool<Ken_testContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseMySql(Configuration.GetConnectionString("Ken_test"));
                options.EnableSensitiveDataLogging();//增加参数输出
            }, 64);

            if (!_hostingEnvironment.IsProduction())
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info { Title = "Ken_test API", Version = "v1",
                       Contact = new Contact
                        {
                            Name = "本是青灯不归客，却因浊酒留风尘。赶路已有清风伴，莫叹岁月不饶人。",
                            Email = "",
                            Url = "http://www.93yz95rz.club"
                       }
                    });
                    var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                    c.DescribeAllEnumsAsStrings();
                });

            if (_hostingEnvironment.IsDevelopment() || _hostingEnvironment.IsEnvironment("QA")
               || _hostingEnvironment.IsProduction())
                services.AddDirectoryBrowser();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var fileProvider = new PhysicalFileProvider(Path.Combine(env.WebRootPath, "index"));
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowDomain");

            app.UseDefaultFiles(new DefaultFilesOptions()
            {
                FileProvider = fileProvider,
                DefaultFileNames = new[] { "login.html" }
            });
      
            app.UseCookiePolicy();

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 40 * 1024
            };
            app.UseWebSockets(webSocketOptions);

            //app.Use(async (context, next) =>
            //{
            //    if (context.Request.Path == "/ws")
            //    {
            //        if (context.WebSockets.IsWebSocketRequest)
            //        {
            //            WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
            //            //var socketFinishedTcs = new TaskCompletionSource<WebSocket>(webSocket);
            //            //await socketFinishedTcs.Task;
            //            var result = await RecvAsync(webSocket, CancellationToken.None);
            //        }
            //        else
            //        {
            //            context.Response.StatusCode = 400;
            //        }
            //    }
            //    else
            //    {
            //        await next();
            //    }

            //});

            app.Map("/ws", WebSocketHandler.Map);
      


            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = fileProvider
            });
            app.UseStaticFiles();

            if (!env.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {                    
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ken API");
                });
            }            
            app.UseMvc();
            

        }
    }
}
