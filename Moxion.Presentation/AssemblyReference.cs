using System.Reflection;

namespace Moxion.Presentation;

internal class AssemblyReference
{
  public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}