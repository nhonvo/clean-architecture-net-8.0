using System.Text.Json.Serialization;
using CleanArchitecture.Domain.Utilities;
using CleanArchitecture.Infrastructure.SchemaFilter;
using CleanArchitecture.Web.Validations;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.Extensions;

public static class MvcExtension
{
    public static void SetupMvc(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

        services.AddControllers(options => options.Filters.Add(typeof(ValidateModelFilter))).AddJsonOptions(options =>
        {
            // Ignore null values
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

            // Avoid reference loop issues
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

            // Use ISO 8601 format for DateTime and DateTimeOffset
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.Converters.Add(new JsonDateTimeOffsetConverter());

            // Handle reference loops
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

            // null strings → ""
            // null numbers → 0
            // null objects → Empty instance of the type.
            options.JsonSerializerOptions.Converters.Add(new NullToDefaultConverter());
            // Name: " John Doe " → "John Doe" (trimmed).
            options.JsonSerializerOptions.Converters.Add(new TrimmingConverter());
            // Price: 12.34567 → 12.35 (2 decimal places).
            options.JsonSerializerOptions.Converters.Add(new DecimalPrecisionConverter(2));


        });
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<Program>();
        services.AddTransient<IValidatorInterceptor, ValidatorInterceptor>();
    }
}
