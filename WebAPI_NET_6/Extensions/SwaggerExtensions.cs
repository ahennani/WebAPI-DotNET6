using Swashbuckle.AspNetCore.SwaggerUI;

namespace WebAPI_NET_6.Extensions;

public static class SwaggerExtensions
{
    public static void SwaggerUIEndpointsDescription(this SwaggerUIOptions option, IServiceProvider service)
    {
        var provider = service.GetRequiredService<IApiVersionDescriptionProvider>();

        foreach (var desc in provider.ApiVersionDescriptions)
        {
            option.SwaggerEndpoint($"/swagger/{desc.GroupName.ToLower()}/swagger.json", desc.GroupName.ToLower());
        }
    }
}