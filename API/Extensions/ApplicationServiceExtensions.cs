using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Contracts;
using API.Data;
using API.IService;
using API.Repositories;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
{
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRelationshipService, UserRelationshipService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ILikeService, LikeService>();
            services.AddScoped <ICommentService, CommentService>();

        // Add AutoMapper configuration
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }
}

}