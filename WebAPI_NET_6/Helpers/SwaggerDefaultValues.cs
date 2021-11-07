namespace WebAPI_NET_6.Helpers;

public class SwaggerDefaultValues : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var apiDescription = context.ApiDescription;

        if (operation.Parameters is null)
            return;

        operation.Deprecated |= apiDescription.IsDeprecated();

        foreach (var param in operation.Parameters)
        {
            var apiDesc = apiDescription.ParameterDescriptions.Where(d => d.Name == param.Name).FirstOrDefault();

            if (param.Description is null)
            {
                param.Description = apiDesc.ModelMetadata?.Description;
            }

            if (param.Schema.Default is not null && apiDesc.DefaultValue is not null)
            {
                param.Schema.Default = new OpenApiString(apiDesc.DefaultValue.ToString());
            }

            param.Required |= apiDesc.IsRequired;
        }
    }
}