var builder = WebApplication.CreateBuilder(args);

// Variables
var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
var appName = Assembly.GetExecutingAssembly().GetName().Name;


// Add services to the container.
builder.Services.AddTransient<IAppRepository<Employee>, EmployeeRepository>();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();

// Configre Services
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
    {
        opt.Password.RequiredLength = 1;
        opt.Password.RequireDigit = false;
        opt.Password.RequireLowercase = false;
        opt.Password.RequireUppercase = false;
        opt.Password.RequiredUniqueChars = 0;
    })
       .AddEntityFrameworkStores<AppDbContext>()
       .AddDefaultTokenProviders();

builder.Services.AddSwaggerGen(c =>
    {
        //c.SwaggerDoc("v1", new OpenApiInfo
        //{
        //    Title = "Demo .NET 6 API",
        //    Description = "Create first application with .NET 6 Web APIs."
        //});

        c.OperationFilter<SwaggerDefaultValues>();
    });

builder.Services.AddVersionedApiExplorer(opt => opt.GroupNameFormat = "'v'VVV");
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;

    opt.ApiVersionReader = new HeaderApiVersionReader("X-API-Version");
});


// Configure the HTTP request pipeline.
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwagger();
    using (var scope = app.Services.CreateScope()) 
    app.UseSwaggerUI(c => c.SwaggerUIEndpointsDescription(scope.ServiceProvider));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Seed Data To Database.
using (var scope = app.Services.CreateScope())
{
    try
    { await SeedData.InitializeAsync(scope.ServiceProvider); }
    catch (Exception ex)
    {
        var logger = app.Logger;
        logger?.LogError(ex, ex.Message);
    }
}

app.Run();
