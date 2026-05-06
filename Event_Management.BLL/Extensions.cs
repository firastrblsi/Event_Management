using Event_Management.Application.Validators;
using Event_Management.BLL.Interfaces.Services;
using Event_Management.BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Event_Management.BLL
{
    public static class Extensions
    {
        public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IBookingService, BookingService>();

            // new services
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ILocationService, LocationService>();

            return services;
        }

    }
}
