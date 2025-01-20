using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Asp.Versioning;
using Moxion.Presentation;
using Moxion.Presentation.Web.Filters;
using Serilog;

var builder = WebApplication.CreateBuilder( args );

builder.Services.AddInfrastructure( builder.Configuration );
builder.Services.AddAutoMapper( Assembly.GetExecutingAssembly() );
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen( options =>
  {
    options.SchemaFilter<SwaggerEnumFilter>();
    options.SchemaFilter<SwaggerDefaultEnumFilter>();
  }
);

builder.Services.AddApiVersioning( options =>
  {
    options.DefaultApiVersion = new ApiVersion( 1, 0 );
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
  }
);

builder.Services.AddCors( options =>
  options.AddDefaultPolicy( policyBuilder =>
    policyBuilder
      .AllowAnyOrigin()
      .AllowAnyMethod()
      .AllowAnyOrigin()
  )
);

builder.Services
  .AddControllers()
  .AddJsonOptions( options =>
    {
      options.JsonSerializerOptions.WriteIndented = true;
      options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
      options.JsonSerializerOptions.Converters.Add( new JsonStringEnumConverter() );
    }
  );

builder.Services.AddSerilog();

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI( options => options.InjectStylesheet( "/swagger/SwaggerDark.css" ) );
}

app.UseHttpsRedirection();
app.UseCors();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.MapGet( "/", () => "Ready!" );

app.Lifetime.ApplicationStarted.Register( () =>
  {
    var logger = app.Services.GetRequiredService<ILogger<Program>>();

    logger.LogInformation( "Hosting environment: {HostingEnvironment}", app.Environment.EnvironmentName );

    foreach (var appUrl in app.Urls)
    {
      logger.LogInformation( "Now listening on: {Url}", appUrl );
    }

    if (app.Environment.IsDevelopment())
    {
      foreach (var appUrl in app.Urls)
      {
        if (!appUrl.Contains( "https" ))
        {
          continue;
        }

        logger.LogInformation( "Swagger available on: {SwaggerUrl}", new Uri( new Uri( appUrl ), "/swagger" ) );
      }
    }

    logger.LogInformation( "Application started" );
  }
);

app.Lifetime.ApplicationStopped.Register( () =>
  {
    var logger = app.Services.GetRequiredService<ILogger<Program>>();

    logger.LogInformation( "Application stopped" );
  }
);

app.Run();