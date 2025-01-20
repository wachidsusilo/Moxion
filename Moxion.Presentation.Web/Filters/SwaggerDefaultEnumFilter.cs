using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Moxion.Common.Units;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Moxion.Presentation.Web.Filters;

public class SwaggerDefaultEnumFilter : ISchemaFilter
{
  public void Apply( OpenApiSchema schema, SchemaFilterContext context )
  {
    if (context.Type == typeof(PositionUnit))
    {
      schema.Default = new OpenApiString( PositionUnit.Millimeter.ToString() );
      return;
    }

    if (context.Type == typeof(TimeUnit))
    {
      schema.Default = new OpenApiString( TimeUnit.Second.ToString() );
      return;
    }

    if (context.Type == typeof(VolumeUnit))
    {
      schema.Default = new OpenApiString( VolumeUnit.Microliter.ToString() );
    }
  }
}