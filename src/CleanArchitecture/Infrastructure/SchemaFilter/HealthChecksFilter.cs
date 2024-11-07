using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CleanArchitecture.Infrastructure.SchemaFilter;

public class HealthChecksFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument openApiDocument, DocumentFilterContext context)
    {
        // Add the /healthz endpoint to Swagger
        openApiDocument.Paths.Add("/healthz", new OpenApiPathItem
        {
            Operations =
            {
                [OperationType.Get] = new OpenApiOperation
                {
                    Summary = "Custom Health Check",
                    Description = "Displays the application's health status.",
                    Responses = new OpenApiResponses
                    {
                        ["200"] = new OpenApiResponse { Description = "Healthy" },
                        ["429"] = new OpenApiResponse { Description = "Degraded" },
                        ["503"] = new OpenApiResponse { Description = "Unhealthy" }
                    }
                }
            }
        });

        // Add the /health endpoint to Swagger
        openApiDocument.Paths.Add("/health", new OpenApiPathItem
        {
            Operations =
            {
                [OperationType.Get] = new OpenApiOperation
                {
                    Summary = "Health Endpoint",
                    Description = "Returns a plain text health status.",
                    Responses = new OpenApiResponses
                    {
                        ["200"] = new OpenApiResponse { Description = "Health" }
                    }
                }
            }
        });
    }
}
