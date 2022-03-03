using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shared.Models.Dto;

namespace Shared.Extensions
{
    public static class ValidationResponseExtension
    {
        public static void UseValidationResponse(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Values.Where(w => w.Errors.Count > 0).SelectMany(s => s.Errors).Select(s => s.ErrorMessage);

                    var response = Response<string>.Fail(new Error(errors.ToList(), true), 400);

                    return new BadRequestObjectResult(response);
                };
            });
        }
    }
}