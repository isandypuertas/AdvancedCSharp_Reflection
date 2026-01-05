namespace PrintAssemblyReference
{
    public class ReflectionPrint
    {
        private string privateField;
        public string PrivateProperty => privateField;
        public static string StaticPublicProperty => "Static Public Property Value";
        public void Print()
        {
            Console.WriteLine($"            Writing from Print class");
        }

        public string GetValue()
        {
            return this.privateField;
        }

        public void PrintValue()
        {
            Console.WriteLine($"            Field value: {this.privateField}");
        }

        public void PrintValueWithParameter(string parameter = "default_defined_on_method")
        {
            Console.WriteLine($"            Parameter value: {parameter}");
        }

        public void PrintValueWithMultipleParameters(string parameter1 = "param1_default_value", int parameter2 = 10, double parameter3 = 10)
        {
            Console.WriteLine($"            Parameters values parameter1:{parameter1} parameter2-{parameter2} parameter3-{parameter3}");
        }

        private void PrintPrivate()
        {
            Console.WriteLine($"            Writing from private method");
        }
    }
}
