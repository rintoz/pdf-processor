using System.CommandLine;
using System.CommandLine.Parsing;

namespace PDFProcessor
{
    public class Program
    {
        static void Main(string[] args)
        {
            var inputFileOption = new Option<string>(
                name: "--inputfilepath",
                description: "Fully qualified file path to the input PDF document.");

            var outputFileOption = new Option<string>(
                name: "--outputfilepath",
                description: "Fully qualified file path to the output PDF document.");

            var watermarkOption = new Option<string>(
                name: "--watermark",
                description: "Watermark for the PDF document.");

            var guidOption = new Option<string>(
                name: "--guid",
                description: "Enable guid stamp on the PDF document.");


            var zipinputFolderOption = new Option<string>(
                name: "--zipinputfolder",
                description: "Fully qualified folder path to zip.");

            var zipOutputFileOption = new Option<string>(
               name: "--zipoutputfile",
               description: "Output zip file name.");

            var rootCommand = new RootCommand("PDFProcessor - Utility to process the PDF documents.")
            {
                inputFileOption,
                outputFileOption,
                watermarkOption,
                guidOption,
                zipinputFolderOption,
                zipOutputFileOption,
            };


            // this is to parse the options 
            //var parser = new Parser(rootCommand);
            //var result = parser.Parse(args);
            //var inputFilePath = result.GetValueForOption(inputFileOption);
            //var outputFilePath = result.GetValueForOption(outputFileOption);
            bool isZipOperation = false;
            rootCommand.SetHandler((string inputFolderPath,string outputFilePath) =>
            {
                if (string.IsNullOrWhiteSpace(inputFolderPath) || string.IsNullOrWhiteSpace(outputFilePath))
                {
                    Console.WriteLine("Input Folder Path or Output File Path is empty!");
                    return;
                }
                isZipOperation = true;
                CompressionOperation compression = new CompressionOperation();
                compression.CompressFolder(inputFolderPath, outputFilePath);

            }, zipinputFolderOption,zipOutputFileOption);
            rootCommand.Invoke(args);

            if (isZipOperation)
            {
                return;
            }

            PDFOperations pdfOperations = null;
            rootCommand.SetHandler((string inputFilePath, string outputFilePath) =>
            {
                pdfOperations = new PDFOperations(inputFilePath, outputFilePath);
            }, inputFileOption, outputFileOption);

            var parseResult = rootCommand.Parse(args);
            rootCommand.Invoke(args);
            if (parseResult.Errors.Count > 0)
            {                
                return;
            }

            rootCommand.SetHandler((string watermark) =>
            {
                if (string.IsNullOrWhiteSpace(watermark))
                {
                    return;
                }
                pdfOperations.Watermark(watermark);
            }, watermarkOption);
            rootCommand.Invoke(args);


            rootCommand.SetHandler((string guid) =>
            {
                if (string.IsNullOrWhiteSpace(guid))
                {
                    return;
                }
                pdfOperations.GuidStamp(guid);
            }, guidOption);
            rootCommand.Invoke(args);

            pdfOperations.Save();
            pdfOperations.Dispose();
        } 
    }
}