using System.Data.Common;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Cryptography;

namespace AdvancedCSharp_Reflection
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*insert here your dll path, not mine*/
            var assembly = Assembly.LoadFrom(@"...\AdvancedCSharp_Reflection\PrintAssemblyReference\bin\Debug\net8.0\PrintAssemblyReference.dll");

            foreach (var type in assembly.GetTypes())
            {
                //WriteAllInformation(type);
                ActivateMethods(type);
            }
        }

        private static void ActivateMethods(Type type)
        {
            Console.WriteLine($"Type: {type.FullName}");
            Console.WriteLine("-----------------------------------------");
            var instance = Activator.CreateInstance(type);

            // | is used to check it all
            foreach (var field in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                Console.WriteLine($"Field: {field.Name} Value set: myValue");
                field.SetValue(instance, "myValue");

                Console.WriteLine($"    Field: {field.Name} Value: {field.GetValue(instance)}");
            }
            Console.WriteLine("-----------------------------------------");

            foreach (var property in type.GetProperties())
            {
                //checking static property (not needed)
                //bool isStatic = property.GetMethod?.IsStatic == true || property.SetMethod?.IsStatic == true;
                //object value = property.GetValue(isStatic ? null : instance);

                Console.WriteLine($"Property: {property.Name}");
                Console.WriteLine($"        Property Value: {property.GetValue(instance)}");
                Console.WriteLine();
            }
            Console.WriteLine("-----------------------------------------");

            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                if (method.IsSpecialName)
                {
                    Console.WriteLine($"Internal Method: {method.Name}");
                }
                else
                {
                    Console.WriteLine($"Method: {method.Name}");

                    if (method.GetParameters().Length > 0)
                    {
                        List<object> parameters = new List<object>();

                        foreach (var parameter in method.GetParameters())
                        {
                            object typeImplicitDefaultValue = null;
                            if (parameter.ParameterType.IsValueType) /*int, double...*/
                            {
                                typeImplicitDefaultValue = Activator.CreateInstance(parameter.ParameterType);
                                Console.WriteLine($"    Parameter: {parameter.Name} Type: {parameter.ParameterType} Default Value (by type): {typeImplicitDefaultValue}");
                            }
                            else if (parameter.ParameterType == typeof(string)) /*!parameter.ParameterType.IsValueTyp - class, string...*/
                            {
                                typeImplicitDefaultValue = parameter.DefaultValue;
                                Console.WriteLine($"    Parameter: {parameter.Name} Type: {parameter.ParameterType} Default Value (by definition): {typeImplicitDefaultValue}");
                            }

                            //same as above
                            //object typeImplicitDefaultValue = parameter.ParameterType.IsValueType ? Activator.CreateInstance(parameter.ParameterType) : null;

                            parameters.Add(typeImplicitDefaultValue);
                        }

                        Console.WriteLine($"        Invoke results:");
                        method.Invoke(instance, parameters.ToArray());
                    }
                    else if (method.ReturnType.Name != "Void")
                    {
                        var returnedValue = method.Invoke(instance, null);
                        Console.WriteLine($"        Invoke results:");
                        Console.WriteLine($"            Returned value: {returnedValue}");
                    }
                    else
                    {
                        Console.WriteLine($"        Invoke results:");
                        method.Invoke(instance, null);
                    }

                    Console.WriteLine();
                }
            }
            Console.WriteLine("-----------------------------------------");
        }

        private static void WriteAllInformation(Type type)
        {
            Console.WriteLine($"Type: {type.FullName}");
            Console.WriteLine("-----------------------------------------");

            // | is used to check it all
            foreach (var field in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                Console.WriteLine($"Field: {field.Name}");
            }
            Console.WriteLine("-----------------------------------------");

            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                if (method.IsSpecialName)
                {
                    Console.WriteLine($"Internal Method: {method.Name}");
                }
                else
                {
                    Console.WriteLine($"Method: {method.Name}");
                }

                foreach (var parameter in method.GetParameters())
                {
                    Console.WriteLine($"    Parameter: {parameter.Name} Type: {parameter.ParameterType}");
                }
            }
            Console.WriteLine("-----------------------------------------");

            foreach (var property in type.GetProperties())
            {
                Console.WriteLine($"Property: {property.Name}");
            }
            Console.WriteLine("-----------------------------------------");
        }
    }
}

