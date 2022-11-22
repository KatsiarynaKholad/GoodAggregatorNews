using GoodAggregatorNews.Abstractions;
using GoodAggregatorNews.Abstractions.Repositories;
using GoodAggregatorNews.Business.ServicesImplementations;
using GoodAggregatorNews.Core.Abstractions;
using GoodAggregatorNews.Database;
using GoodAggregatorNews.Database.Entities;
using GoodAggregatorNews.Repositories;
using GoodAggregatorNews.WebAPI.Filters;
using GoodAggregatorNews.WebAPI.Utils;
using Hangfire;
using Hangfire.SqlServer;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System.Text;

namespace GoodAggregatorNews.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((ctx, lc)
                => lc.WriteTo.File(@"D:\data.log", LogEventLevel.Information)
                    .WriteTo.Console(LogEventLevel.Verbose));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            var connectionString = builder.Configuration.GetConnectionString("Default");

            builder.Services.AddDbContext<GoodAggregatorNewsContext>(optionBuilder =>
            optionBuilder.UseSqlServer(connectionString));

            builder.Services.AddHangfire(configuration => configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(connectionString,
             new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(30),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(30),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true,
                    }));


            builder.Services.AddHangfireServer();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddScoped<IArticleService, ArticleService>();
            builder.Services.AddScoped<ISourceService, SourceService>();
            builder.Services.AddScoped<IClientService, ClientService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<ICommentService, CommentService>();


            builder.Services.AddScoped<IAdditionArticleRepository, AdditionArticleRepository>();
            builder.Services.AddScoped<IRepository<Source>, Repository<Source>>();
            builder.Services.AddScoped<IRepository<Client>, Repository<Client>>();
            builder.Services.AddScoped<IRepository<Comment>, Repository<Comment>>();
            builder.Services.AddScoped<IRepository<Role>, Repository<Role>>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IParseService, ParseService>();

            builder.Services.AddScoped<IJwtUtil, JwtUtilSha256>();

            //add mediatr

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt=>
            {
                opt.IncludeXmlComments(builder.Configuration["XmlDoc"]);
            });

            builder.Services
                .AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opt =>
                {
                    opt.RequireHttpsMetadata = false;
                    opt.SaveToken = true;
                    opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = builder.Configuration["Token:Issuer"],
                        ValidAudience = builder.Configuration["Token:Issuer"],
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:JwtToken"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            var app = builder.Build();

            app.UseStaticFiles();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new AuthorizationFilter() }
            });

            app.UseRouting();

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapHangfireDashboard();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}