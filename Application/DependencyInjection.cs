using Application.Interfaces;
using Application.Interfaces.Advert;
using Application.Interfaces.Category;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<UserResolverService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //services.AddTransient<IUserService, UserService>();
            services.AddScoped<IAdvertService, AdvertService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPictureService, PictureService>();

            return services;
        }
    }
}