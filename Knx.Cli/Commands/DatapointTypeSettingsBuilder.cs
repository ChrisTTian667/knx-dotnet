using System.ComponentModel;
using System.Reflection;
using System.Reflection.Emit;
using Knx.Common.Attribute;
using Spectre.Console.Cli;

namespace Knx.Cli.Commands;

internal sealed class DatapointTypeSettingsBuilder
{
    private readonly Type _datapointType;
    private readonly PropertyInfo[] _properties;
    private readonly string[] _propertyNames;

    public DatapointTypeSettingsBuilder(Type datapointType)
    {
        _datapointType = datapointType;

        _properties = datapointType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.GetCustomAttribute<DatapointPropertyAttribute>() != null)
            .DistinctBy(p => p.Name)
            .OrderBy(p => p.Name)
            .ToArray();

        _propertyNames = _properties
            .Select(p => p.Name.ToLower())
            .ToArray();
    }

    public Type CreateSettings()
    {
        // Create a dynamic assembly and module
        var assemblyName = new AssemblyName("Knx.Cli.Commands");
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");

        // Define the base type
        var baseType = typeof(WriteCommandSettings);

        // Create the derived type and define the 'Value' property
        var typeBuilder = moduleBuilder.DefineType(
            $"{_datapointType.Name}CommandSetting",
            TypeAttributes.Public,
            baseType);

        // Define the 'get' and 'set' accessors for the 'Value' property
        const MethodAttributes propertyMethodAttributes =
            MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

        int index = 1;
        foreach (var property in _properties)
        {
            var propertyType = property.PropertyType;
            var propertyName = property.Name;

            var (getMethod, setMethod) = CreateAutoImplementedProperty(
                typeBuilder,
                $"_{propertyName}",
                propertyType,
                propertyMethodAttributes);

            var propertyBuilder = typeBuilder
                .DefineProperty(
                    propertyName,
                    PropertyAttributes.HasDefault,
                    propertyType,
                    null);

            propertyBuilder.SetGetMethod(getMethod);
            propertyBuilder.SetSetMethod(setMethod);

            // add Description attribute
            var descriptionAttributeCtor =
                typeof(DescriptionAttribute).GetConstructor(new[] { typeof(string) });
            var descriptionAttributeBuilder = new CustomAttributeBuilder(
                descriptionAttributeCtor!,
                new object[] { $"Payload: {propertyName}" });
            propertyBuilder.SetCustomAttribute(descriptionAttributeBuilder);

            // add argument attribute
            var argumentAttributeCtor =
                typeof(CommandArgumentAttribute)
                    .GetConstructor(new[] { typeof(int), typeof(string) });
            var argumentAttributeBuilder =
                new CustomAttributeBuilder(
                    argumentAttributeCtor!,
                    new object[] { index, $"<{propertyName.ToUpper()}>" });
            propertyBuilder.SetCustomAttribute(argumentAttributeBuilder);

            // // add command option attribute
            // var commandOptionAttributeCtor =
            //     typeof(CommandOptionAttribute).GetConstructor(new[] { typeof(string) });
            // var commandOptionAttributeBuilder =
            //     new CustomAttributeBuilder(
            //         commandOptionAttributeCtor!,
            //         new object[] { BuildCommandOption(propertyName) });
            // propertyBuilder.SetCustomAttribute(commandOptionAttributeBuilder);
        }

        // Create the dynamic type
        return typeBuilder.CreateType();
    }

    private string BuildCommandOption(string propertyName)
    {
        var lowerPropertyName = propertyName.ToLower();
        var shortOption = lowerPropertyName[0];
        var multipleShorts = _propertyNames.Count(name => name[0] == shortOption) > 1;

        var shortOptionPart = multipleShorts
            ? ""
            : $"-{shortOption}|";

        var longOptionPart = $"--{lowerPropertyName.PadLeft(2, 'p')}";

        return $"{shortOptionPart}{longOptionPart}";
    }

    private static (MethodBuilder getMethod, MethodBuilder setMethod) CreateAutoImplementedProperty(
        TypeBuilder typeBuilder,
        string fieldName,
        Type fieldType,
        MethodAttributes propertyMethodAttributes)
    {
        var fieldBuilder = typeBuilder.DefineField(fieldName, fieldType, FieldAttributes.Private);

        var getMethodBuilder = typeBuilder.DefineMethod(
            $"get_{fieldName}",
            propertyMethodAttributes,
            fieldType,
            Type.EmptyTypes);

        var getIl = getMethodBuilder.GetILGenerator();
        getIl.Emit(OpCodes.Ldarg_0);
        getIl.Emit(OpCodes.Ldfld, fieldBuilder);
        getIl.Emit(OpCodes.Ret);

        var setMethodBuilder = typeBuilder.DefineMethod(
            $"set_{fieldName}",
            propertyMethodAttributes,
            null,
            new[] { fieldType });

        var setIl = setMethodBuilder.GetILGenerator();
        setIl.Emit(OpCodes.Ldarg_0);
        setIl.Emit(OpCodes.Ldarg_1);
        setIl.Emit(OpCodes.Stfld, fieldBuilder);
        setIl.Emit(OpCodes.Ret);

        return (getMethodBuilder, setMethodBuilder);
    }
}
