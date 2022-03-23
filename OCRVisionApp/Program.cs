using System;
using System.Collections.Generic;
using System.Text;
using OCRVisionApp;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace OCRVisionApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string subscriptionName = Environment.GetEnvironmentVariable("COGNITIVE_SERVICE_NAME");
            string endpoint = Environment.GetEnvironmentVariable("COGNITIVE_SERVICE_ENDPOINT");
            string key = Environment.GetEnvironmentVariable("COGNITIVE_SERVICE_KEY");
            Console.WriteLine(subscriptionName);
            Console.WriteLine(endpoint);
            Console.WriteLine(key);
            ComputerVisionClient client = OCRVision.Authenticate(endpoint, key);

            Console.WriteLine(client);

            string ANALYZE_URL_IMAGE = "https://moderatorsampleimages.blob.core.windows.net/samples/sample16.png";
            string DETECT_URL_IMAGE = "https://moderatorsampleimages.blob.core.windows.net/samples/sample9.png";
            string EXTRACT_TEXT_URL_IMAGE = "https://moderatorsampleimages.blob.core.windows.net/samples/sample2.jpg";

            //OCRVision.AnalyzeImageWithUrl(client, ANALYZE_URL_IMAGE).Wait();
            string localImage = "C:/Users/r.bandpey/source/repos/azure-computer-vision/OCRVisionApp/InputImages/TestImages/Farmer_meme.jpg";
            string localLetterImage = "C:/Users/r.bandpey/source/repos/azure-computer-vision/OCRVisionApp/InputImages/imagesa/aaa00a00/60008366.tif";
            var localLetterMetadata = "";
            //OCRVision.AnalyzeImageLocal(client, localImage).Wait();

            //OCRVision.DetectObjectsUrl(client, DETECT_URL_IMAGE).Wait();
            //OCRVision.DetectObjectsLocal(client, localImage).Wait();
            //OCRVision.BatchReadFileUrl(client, EXTRACT_TEXT_URL_IMAGE).Wait();
            //OCRVision.BatchReadFileLocal(client, localImage).Wait();
            //OCRVision.RecognizePrintedTextUrl(client, EXTRACT_TEXT_URL_IMAGE).Wait();
            //OCRVision.RecognizePrintedTextLocal(client, localImage).Wait();

            OCRVision.BatchReadFileLocal(client, localLetterImage).Wait();
        }
    }
}
