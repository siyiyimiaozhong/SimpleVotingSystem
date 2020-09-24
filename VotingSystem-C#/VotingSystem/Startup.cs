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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VotingSystem.conf;
using VotingSystem.Mapper;
using VotingSystem.Mapper.MapperImpl;
using VotingSystem.Service;
using VotingSystem.Service.ServiceImpl;

namespace VotingSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            DbConfig.Init();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()/*.AddNewtonsoftJson()*/;
            services.AddSession();
            services.AddMvc();

            services.AddSingleton<UserMapper, UserMapperImpl>()
                .AddSingleton<UserService, UserServiceImpl>()
                .AddSingleton<VoteService, VoteServiceImpl>()
                .AddSingleton<VoteMapper, VoteMapperImpl>()
                .AddSingleton<OptionMapper, OptionMapperImpl>()
                .AddSingleton<PollService, PollServiceImpl>()
                .AddSingleton<PollMapper, PollMapperImpl>();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
