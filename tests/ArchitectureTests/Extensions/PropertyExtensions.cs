namespace ArchitectureTests.Extensions;

internal static class PropertyExtensions
{
    internal static bool IsInitOnly(this PropertyInfo propertyInfo)
    {
        MethodInfo setMethod = propertyInfo.SetMethod!;

        if (setMethod == null)
        {
            return false;
        }

        Type isExternalInitType = typeof(System.Runtime.CompilerServices.IsExternalInit);

        return setMethod.ReturnParameter.GetRequiredCustomModifiers().Contains(isExternalInitType);
    }
}
