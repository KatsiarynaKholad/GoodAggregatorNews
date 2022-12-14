using GoodAggregatorNews.Abstractions;
using GoodAggregatorNews.Abstractions.Repositories;
using GoodAggregatorNews.Business.ServicesImplementations;
using GoodAggregatorNews.Core.Abstractions;
using GoodAggregatorNews.Data.CQS.Commands;
using GoodAggregatorNews.Data.CQS.Queries;
using GoodAggregatorNews.Database;
using GoodAggregatorNews.Database.Entities;
using GoodAggregatorNews.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

namespace GoodAggregatorNews
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((ctx, lc)
                => lc.WriteTo.File(@"D:\data.log", LogEventLevel.Information)
                .WriteTo.Console(LogEventLevel.Information));

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options =>
               {
                   options.LoginPath = new PathString(@"/Account/Login");
                   options.LogoutPath = new PathString(@"/Account/Logout");
                   options.AccessDeniedPath = new PathString(@"/Account/Login");
               });

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var connectionString = builder.Configuration.GetConnectionString("Default");

            builder.Services.AddDbContext<GoodAggregatorNewsContext>(optionBuilder =>
            optionBuilder.UseSqlServer(connectionString));

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

            builder.Services.AddMediatR(typeof(AddArticleDataFromRssFeedCommand).Assembly);
            builder.Services.AddMediatR(typeof(AddRefreshTokenCommand).Assembly);
            builder.Services.AddMediatR(typeof(AddArticleCommand).Assembly);
            builder.Services.AddMediatR(typeof(UpdateArticleRateCommand).Assembly);
            builder.Services.AddMediatR(typeof(RemoveRefreshTokenCommand).Assembly);

            builder.Services.AddMediatR(typeof(GetAllArticlesWithoutTextIdsQuery).Assembly);
            builder.Services.AddMediatR(typeof(GetArticleByIdQuery).Assembly);
            builder.Services.AddMediatR(typeof(GetAllSourcesQuery).Assembly);
            builder.Services.AddMediatR(typeof(GetArticlesWithEmptyRateIdQuery).Assembly);
            builder.Services.AddMediatR(typeof(GetClientByRefreshTokenQuery).Assembly);



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Article}/{action=Index}/{id?}");

            app.Run();
        }
    }
}