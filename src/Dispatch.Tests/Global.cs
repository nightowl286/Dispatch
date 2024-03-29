global using Microsoft.VisualStudio.TestTools.UnitTesting;
global using Moq;
global using TNO.DependencyInjection.Abstractions.Components;
global using TNO.Dispatch.Abstractions;
global using TNO.Dispatch.Tests.TestImplementations;
global using TNO.Tests.Moq;
global using TNO.Tests.Moq.Dispatch.Abstractions;

#if DEBUG
[assembly: Parallelize(Scope = ExecutionScope.ClassLevel, Workers = 1)]
#else
[assembly: Parallelize(Scope = ExecutionScope.MethodLevel, Workers = 0)]
#endif

internal static class Category
{
   public const string Dispatch = "Dispatch";
   public const string Decorator = "Decorator";
}