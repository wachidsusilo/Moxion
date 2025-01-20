using System.Reflection;

namespace Moxion.Application.Feature;

internal class AssemblyReference
{
  internal static Assembly Assembly => typeof(AssemblyReference).Assembly;
}