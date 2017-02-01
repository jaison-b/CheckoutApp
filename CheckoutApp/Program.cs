using System;
using Fclp;

namespace CheckoutApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var parser = GetParser();
            var parseResult = parser.Parse(args);
            if (parseResult.HasErrors)
            {
                parser.HelpOption.ShowHelp(parser.Options);
                Environment.Exit(-1);
            }

            Console.WriteLine("Executing: {0}", parser.Object.InputFile);
        }

        private static FluentCommandLineParser<ApplicationArguments> GetParser()
        {
            var parser = new FluentCommandLineParser<ApplicationArguments>();
            parser.Setup(arg => arg.InputFile)
                .As('i', "input-file")
                .Required()
                .WithDescription("Order file to process");
            parser.Setup(arg => arg.ProductsFile)
                .As('p', "products-file")
                .Required()
                .WithDescription("Products file to lookup");
            parser.Setup(arg => arg.PromotionsFile)
                .As('d', "discounts-file")
                .Required()
                .WithDescription("Discounts file to apply to products");
            parser.SetupHelp("?", "help")
                .Callback(text => Console.WriteLine(text))
                .WithHeader("Usage: CheckoutApp -i ordersfile.txt -p prodsfile.txt -d promosfile.txt\n" +
                            "       CheckoutApp --input-File ordersfile.txt --products-file prodsfile.txt --discounts-file promosfile.txt");

            return parser;
        }
    }

    public class ApplicationArguments
    {
        public string InputFile { get; set; }
        public string ProductsFile { get; set; }
        public string PromotionsFile { get; set; }
    }

}
