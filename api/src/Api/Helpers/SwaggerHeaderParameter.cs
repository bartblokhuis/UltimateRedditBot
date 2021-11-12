
using Api.Consts;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Helpers;
public class SwaggerHeaderParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = AuthorizationConsts.AuthorizationHeaderName,
            In = ParameterLocation.Header,
            Required = false
        });
    }
}