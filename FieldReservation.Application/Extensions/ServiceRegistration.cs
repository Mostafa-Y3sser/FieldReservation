using Mapster;
using MediatR;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using FieldReservation.Application.Common.Behaviors;

namespace FieldReservation.Application.Extentions
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly));

            services.AddValidatorsFromAssembly(typeof(ServiceRegistration).Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddMapster();
        }
    }
}