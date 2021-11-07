namespace WebAPI_NET_6.Helpers;

public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;
    public ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }
    public void Configure(SwaggerGenOptions options)
    {
        var appName = Assembly.GetExecutingAssembly().GetName().Name;

        var info = new OpenApiInfo()
        {
            Title = appName.Replace('_', ' '),
            Description = "First .NET 6 Web API application.",
            Contact = new OpenApiContact()
            {
                Name = "Contact me",
                Email = "ah.hennani@gmail.com",
                Url = new Uri("https://www.linkedin.com/in/ahmed-hennani")
            }
        };

        foreach (var description in _provider.ApiVersionDescriptions)
        {
            info.Version = description.ApiVersion.ToString();

            options.SwaggerDoc($"{description.GroupName.ToLower()}", info);
        }
    }
}