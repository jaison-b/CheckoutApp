using System;
using System.IO;
using CheckoutApp.Repository;
using CommandLine;
using CommandLine.Text;

namespace CheckoutApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var options = new Options();
            if (Parser.Default.ParseArguments(args, options))
            {
                Console.WriteLine("Checking Out...");
                CheckOut(options);
            }
        }

        private static void CheckOut(Options options)
        {
            IProductRepository productRepository = new ProductRepository(File.OpenRead(options.ProductInputFile));
            IPromotionRepository promotionRepository = new PromotionRepository(File.OpenRead(options.PromotionInputFile));
            ICartFactory cartFactory = new CartFactory(promotionRepository, productRepository);
            var cart = cartFactory.CreateCart(File.OpenRead(options.OrderInputFile), DateTime.Now);
            cart.Checkout();
        }

        internal sealed class Options
        {
            [Option('i', "input-file", Required = true, HelpText = "Order input file")]
            public string OrderInputFile { get; set; }

            [Option('p', "product-file", Required = true, HelpText = "Product catalog file")]
            public string ProductInputFile { get; set; }

            [Option('d', "discount-file", Required = true, HelpText = "Promotion input file")]
            public string PromotionInputFile { get; set; }

            [ParserState]
            public IParserState LastParserState { get; set; }

            [HelpOption]
            public string GetUsage()
            {
                var help = new HelpText
                {
                    Heading = new HeadingInfo("CheckoutApp"),
                    Copyright = new CopyrightInfo("Jaison.B", 2012),
                    AdditionalNewLineAfterOption = true,
                    AddDashesToOption = true
                };
                help.AddOptions(this);
                if (LastParserState.Errors.Count <= 0) return help;
                var errors = help.RenderParsingErrorsText(this, 2); // indent with two spaces
                if (string.IsNullOrEmpty(errors)) return help;
                help.AddPreOptionsLine(string.Concat(Environment.NewLine, "ERROR(S):"));
                help.AddPreOptionsLine(errors);
                return help;
            }
        }
    }
}