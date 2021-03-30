using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace country.back {

    public class SwaggerDefaultValues : IOperationFilter {
        public void Apply(OpenApiOperation operation, OperationFilterContext context) {
            ApiDescription apiDescription = context.ApiDescription;
            operation.Deprecated |= apiDescription.IsDeprecated();
            if (operation.Parameters == null) {
                return;
            }
            foreach (OpenApiParameter parameter in operation.Parameters) {
                ApiParameterDescription description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);
                if (parameter.Description == null) {
                    parameter.Description = description.ModelMetadata?.Description;
                }
                if (parameter.Schema.Default == null && description.DefaultValue != null) {
                    parameter.Schema.Default = new OpenApiString(description.DefaultValue.ToString());
                }
                parameter.Required |= description.IsRequired;
            }
        }
    }

    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions> {
        private readonly IApiVersionDescriptionProvider _provider;
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;
        public void Configure(SwaggerGenOptions options) {
            foreach (ApiVersionDescription description in _provider.ApiVersionDescriptions) {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }
        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description) {
            OpenApiInfo info = new OpenApiInfo() {
                Title = "Country API",
                Version = description.ApiVersion.ToString(),
            };
            if (description.IsDeprecated) {
                info.Description += " This API version has been deprecated.";
            }
            return info;
        }
    }
}
