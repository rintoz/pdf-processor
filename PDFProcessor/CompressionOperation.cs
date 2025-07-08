using System.IO.Compression;
using System.Text;

namespace PDFProcessor
{
    internal class CompressionOperation
    {
        public CompressionOperation()
        {
        }

        internal void CompressFolder(string inputFolderPath, string outputFilePath)
        {
            if (!Directory.Exists(inputFolderPath))
            {
                Console.WriteLine($"Input folder does not exist: {inputFolderPath}");
                return;
            }

            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }

            using var memoryStream = new MemoryStream();

            ZipFile.CreateFromDirectory(
                sourceDirectoryName: inputFolderPath,
                destination: memoryStream,
                compressionLevel: CompressionLevel.Optimal,
                includeBaseDirectory: true,
                entryNameEncoding: Encoding.UTF8
            );

            memoryStream.Position = 0;
            using var fileStream = File.Create(outputFilePath);
            memoryStream.CopyTo(fileStream);

            memoryStream.Flush();
            memoryStream.Close();

            fileStream.Close();

            Console.WriteLine($"Folder successfully compressed to: {outputFilePath}");
        }

        #region Old code, Not for encrypted drives 

        // This method works for notmal scenario, but it is failing on encrypted drive
        //internal void CompressFolder(string inputFolderPath, string outputFilePath)
        //{
        //    try
        //    {
        //        if (!Directory.Exists(inputFolderPath))
        //        {
        //            Console.WriteLine($"Input folder does not exist: {inputFolderPath}");
        //            return;
        //        }

        //        if (File.Exists(outputFilePath))
        //        {
        //            File.Delete(outputFilePath);
        //        }

        //        ZipFile.CreateFromDirectory(inputFolderPath, outputFilePath, CompressionLevel.Optimal, includeBaseDirectory: true);

        //        Console.WriteLine($"Folder successfully compressed to: {outputFilePath}");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error while compressing folder: {ex.Message}");
        //    }
        //}

        #endregion
    }
}