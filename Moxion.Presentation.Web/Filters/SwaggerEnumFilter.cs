using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Moxion.Presentation.Web.Filters;

public class SwaggerEnumFilter : ISchemaFilter
{
  public void Apply( OpenApiSchema schema, SchemaFilterContext context )
  {
    if (!context.Type.IsEnum)
    {
      return;
    }

    schema.Enum.Clear();

    foreach (var name in Enum.GetNames( context.Type ))
    {
      if (Convert.ToInt32( Enum.Parse( context.Type, name ) ) != 0)
      {
        schema.Enum.Add( new OpenApiString( name ) );
      }
    }
  }
}