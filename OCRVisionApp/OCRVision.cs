using System;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OCRVisionApp
{
    class OCRVision
    {
        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }

        private static void SaveJson(ImageAnalysis imageMetadata, string fileName)
        {
            var jsonResults = JsonConvert.SerializeObject(imageMetadata);
            Console.WriteLine(imageMetadata);
            string workingDirectory = Environment.CurrentDirectory;
            string projectBinDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
            string projectDirectory = Directory.GetParent(projectBinDirectory).FullName;
            string MetadataPath = Path.Combine(projectDirectory, "ImageMetadata");
            if (!Directory.Exists(MetadataPath))
                Directory.CreateDirectory(MetadataPath);

            string JsonFilePath = Path.Combine(MetadataPath, fileName);

            string metadataJson = JsonConvert.SerializeObject(imageMetadata, Formatting.Indented);

            File.WriteAllText(JsonFilePath, metadataJson);
        }

        private static void SaveJsonOCR(ReadOperationResult imageMetadata, string fileName)
        {
            var jsonResults = JsonConvert.SerializeObject(imageMetadata);
            Console.WriteLine(imageMetadata);
            string workingDirectory = Environment.CurrentDirectory;
            string projectBinDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
            string projectDirectory = Directory.GetParent(projectBinDirectory).FullName;
            string MetadataPath = Path.Combine(projectDirectory, "ImageMetadata");
            if (!Directory.Exists(MetadataPath))
                Directory.CreateDirectory(MetadataPath);

            string JsonFilePath = Path.Combine(MetadataPath, fileName);

            string metadataJson = JsonConvert.SerializeObject(imageMetadata, Formatting.Indented);

            File.WriteAllText(JsonFilePath, metadataJson);
        }


        private static void ShowResults(ImageAnalysis imageMetadata)
        {
            // Sunmarizes the image content.
            Console.WriteLine("Summary:");
            foreach (var caption in imageMetadata.Description.Captions)
            {
                Console.WriteLine($"{caption.Text} with confidence {caption.Confidence}");
            }
            Console.WriteLine();

            // Display categories the image is divided into.
            Console.WriteLine("Categories:");
            foreach (var category in imageMetadata.Categories)
            {
                Console.WriteLine($"{category.Name} with confidence {category.Score}");
            }
            Console.WriteLine();
            // </snippet_categorize>

            // <snippet_tags>
            // Image tags and their confidence score
            Console.WriteLine("Tags:");
            foreach (var tag in imageMetadata.Tags)
            {
                Console.WriteLine($"{tag.Name} {tag.Confidence}");
            }
            Console.WriteLine();
            // </snippet_tags>

            // <snippet_objects>
            // Objects
            Console.WriteLine("Objects:");
            foreach (var obj in imageMetadata.Objects)
            {
                Console.WriteLine($"{obj.ObjectProperty} with confidence {obj.Confidence} at location {obj.Rectangle.X}, " +
                  $"{obj.Rectangle.X + obj.Rectangle.W}, {obj.Rectangle.Y}, {obj.Rectangle.Y + obj.Rectangle.H}");
            }
            Console.WriteLine();
            // </snippet_objects>

            // <snippet_faces>
            // Faces
            Console.WriteLine("Faces:");
            foreach (var face in imageMetadata.Faces)
            {
                Console.WriteLine($"A {face.Gender} of age {face.Age} at location {face.FaceRectangle.Left}, " +
                  $"{face.FaceRectangle.Left}, {face.FaceRectangle.Top + face.FaceRectangle.Width}, " +
                  $"{face.FaceRectangle.Top + face.FaceRectangle.Height}");
            }
            Console.WriteLine();
            // </snippet_faces>

            // <snippet_adult>
            // Adult or racy content, if any.
            Console.WriteLine("Adult:");
            Console.WriteLine($"Has adult content: {imageMetadata.Adult.IsAdultContent} with confidence {imageMetadata.Adult.AdultScore}");
            Console.WriteLine($"Has racy content: {imageMetadata.Adult.IsRacyContent} with confidence {imageMetadata.Adult.RacyScore}");
            Console.WriteLine();
            // </snippet_adult>

            // <snippet_brands>
            // Well-known (or custom, if set) brands.
            Console.WriteLine("Brands:");
            foreach (var brand in imageMetadata.Brands)
            {
                Console.WriteLine($"Logo of {brand.Name} with confidence {brand.Confidence} at location {brand.Rectangle.X}, " +
                  $"{brand.Rectangle.X + brand.Rectangle.W}, {brand.Rectangle.Y}, {brand.Rectangle.Y + brand.Rectangle.H}");
            }
            Console.WriteLine();
            // </snippet_brands>

            // <snippet_celebs>
            // Celebrities in image, if any.
            Console.WriteLine("Celebrities:");
            foreach (var category in imageMetadata.Categories)
            {
                if (category.Detail?.Celebrities != null)
                {
                    foreach (var celeb in category.Detail.Celebrities)
                    {
                        Console.WriteLine($"{celeb.Name} with confidence {celeb.Confidence} at location {celeb.FaceRectangle.Left}, " +
                          $"{celeb.FaceRectangle.Top}, {celeb.FaceRectangle.Height}, {celeb.FaceRectangle.Width}");
                    }
                }
            }
            Console.WriteLine();
            // </snippet_celebs>


            // <snippet_landmarks>
            // Popular landmarks in image, if any.
            Console.WriteLine("Landmarks:");
            foreach (var category in imageMetadata.Categories)
            {
                if (category.Detail?.Landmarks != null)
                {
                    foreach (var landmark in category.Detail.Landmarks)
                    {
                        Console.WriteLine($"{landmark.Name} with confidence {landmark.Confidence}");
                    }
                }
            }
            Console.WriteLine();
            // </snippet_landmarks>

            // <snippet_color>
            // Identifies the color scheme.
            Console.WriteLine("Color Scheme:");
            Console.WriteLine("Is black and white?: " + imageMetadata.Color.IsBWImg);
            Console.WriteLine("Accent color: " + imageMetadata.Color.AccentColor);
            Console.WriteLine("Dominant background color: " + imageMetadata.Color.DominantColorBackground);
            Console.WriteLine("Dominant foreground color: " + imageMetadata.Color.DominantColorForeground);
            Console.WriteLine("Dominant colors: " + string.Join(",", imageMetadata.Color.DominantColors));
            Console.WriteLine();
            // </snippet_color>

            // <snippet_type>
            // Detects the image types.
            Console.WriteLine("Image Type:");
            Console.WriteLine("Clip Art Type: " + imageMetadata.ImageType.ClipArtType);
            Console.WriteLine("Line Drawing Type: " + imageMetadata.ImageType.LineDrawingType);
            Console.WriteLine();
        }

        public static async Task AnalyzeImageWithUrl(ComputerVisionClient client, string imageUrl)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("ANALYZE IMAGE - URL");
            Console.WriteLine();

            // Creating a list that defines the features to be extracted from the image. 
            List<VisualFeatureTypes> features = new List<VisualFeatureTypes>()
            {
                  VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
                  VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
                  VisualFeatureTypes.Tags, VisualFeatureTypes.Adult,
                  VisualFeatureTypes.Color, VisualFeatureTypes.Brands,
                  VisualFeatureTypes.Objects
            };

            Console.WriteLine($"Analyzing the image {Path.GetFileName(imageUrl)}...");
            Console.WriteLine();
            // Analyze the URL image 
            ImageAnalysis results = await client.AnalyzeImageAsync(imageUrl, features);

            ShowResults(results);
            string fileName = $"{Path.GetFileName(imageUrl)}.json";
            SaveJson(results, fileName);
        }

        public static async Task AnalyzeImageLocal(ComputerVisionClient client, string localImage)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("ANALYZE IMAGE - LOCAL IMAGE");
            Console.WriteLine();

            // Creating a list that defines the features to be extracted from the image. 
            List<VisualFeatureTypes> features = new List<VisualFeatureTypes>()
            {
                VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
                VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
                VisualFeatureTypes.Tags, VisualFeatureTypes.Adult,
                VisualFeatureTypes.Color, VisualFeatureTypes.Brands,
                VisualFeatureTypes.Objects
            };

            Console.WriteLine($"Analyzing the local image {Path.GetFileName(localImage)}...");
            Console.WriteLine();

            using (Stream analyzeImageStream = File.OpenRead(localImage))
            {
                // Analyze the local image.
                ImageAnalysis results = await client.AnalyzeImageInStreamAsync(analyzeImageStream, features);

                ShowResults(results);
                string fileName = $"{Path.GetFileName(localImage)}.json";
                SaveJson(results, fileName);
            }
        }

        public static async Task DetectObjectsUrl(ComputerVisionClient client, string urlImage)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("DETECT OBJECTS - URL IMAGE");
            Console.WriteLine();

            Console.WriteLine($"Detecting objects in URL image {Path.GetFileName(urlImage)}...");
            Console.WriteLine();
            // Detect the objects
            DetectResult detectObjectAnalysis = await client.DetectObjectsAsync(urlImage);

            // For each detected object in the picture, print out the bounding object detected, confidence of that detection and bounding box within the image
            Console.WriteLine("Detected objects:");
            foreach (var obj in detectObjectAnalysis.Objects)
            {
                Console.WriteLine($"{obj.ObjectProperty} with confidence {obj.Confidence} at location {obj.Rectangle.X}, " +
                  $"{obj.Rectangle.X + obj.Rectangle.W}, {obj.Rectangle.Y}, {obj.Rectangle.Y + obj.Rectangle.H}");
            }
            Console.WriteLine();
        }

        public static async Task DetectObjectsLocal(ComputerVisionClient client, string localImage)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("DETECT OBJECTS - LOCAL IMAGE");
            Console.WriteLine();

            using (Stream stream = File.OpenRead(localImage))
            {
                // Make a call to the Computer Vision service using the local file
                DetectResult results = await client.DetectObjectsInStreamAsync(stream);

                Console.WriteLine($"Detecting objects in local image {Path.GetFileName(localImage)}...");
                Console.WriteLine();

                // For each detected object in the picture, print out the bounding object detected, confidence of that detection and bounding box within the image
                Console.WriteLine("Detected objects:");
                foreach (var obj in results.Objects)
                {
                    Console.WriteLine($"{obj.ObjectProperty} with confidence {obj.Confidence} at location {obj.Rectangle.X}, " +
                      $"{obj.Rectangle.X + obj.Rectangle.W}, {obj.Rectangle.Y}, {obj.Rectangle.Y + obj.Rectangle.H}");
                }
                Console.WriteLine();
            }
        }

        public static async Task BatchReadFileUrl(ComputerVisionClient client, string urlImage)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("BATCH READ FILE - URL IMAGE");
            Console.WriteLine();

            // Read text from URL
            BatchReadFileHeaders textHeaders = await client.BatchReadFileAsync(urlImage);
            // After the request, get the operation location (operation ID)
            string operationLocation = textHeaders.OperationLocation;
            // </snippet_extract_call>

            // <snippet_extract_response>
            // Retrieve the URI where the recognized text will be stored from the Operation-Location header.
            // We only need the ID and not the full URL
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            // Extract the text
            // Delay is between iterations and tries a maximum of 10 times.
            int i = 0;
            int maxRetries = 10;
            ReadOperationResult results;
            Console.WriteLine($"Extracting text from URL image {Path.GetFileName(urlImage)}...");
            Console.WriteLine();
            do
            {
                results = await client.GetReadOperationResultAsync(operationId);
                Console.WriteLine("Server status: {0}, waiting {1} seconds...", results.Status, i);
                await Task.Delay(1000);
                if (i == 9)
                {
                    Console.WriteLine("Server timed out.");
                }
            }
            while ((results.Status == TextOperationStatusCodes.Running ||
                results.Status == TextOperationStatusCodes.NotStarted) && i++ < maxRetries);
            // </snippet_extract_response>

            // <snippet_extract_display>
            // Display the found text.
            Console.WriteLine();
            var textRecognitionLocalFileResults = results.RecognitionResults;
            foreach (TextRecognitionResult recResult in textRecognitionLocalFileResults)
            {
                foreach (Line line in recResult.Lines)
                {
                    Console.WriteLine(line.Text);
                }
            }
            Console.WriteLine();
        }

        public static async Task BatchReadFileLocal(ComputerVisionClient client, string localImage)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("BATCH READ FILE - LOCAL IMAGE");
            Console.WriteLine();

            // Helps calucalte starting index to retrieve operation ID
            const int numberOfCharsInOperationId = 36;

            Console.WriteLine($"Extracting text from local image {Path.GetFileName(localImage)}...");
            Console.WriteLine();
            using (Stream imageStream = File.OpenRead(localImage))
            {
                // Read the text from the local image
                BatchReadFileInStreamHeaders localFileTextHeaders = await client.BatchReadFileInStreamAsync(imageStream);
                // Get the operation location (operation ID)
                string operationLocation = localFileTextHeaders.OperationLocation;

                // Retrieve the URI where the recognized text will be stored from the Operation-Location header.
                string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

                // Extract text, wait for it to complete.
                int i = 0;
                int maxRetries = 10;
                ReadOperationResult results;
                do
                {
                    results = await client.GetReadOperationResultAsync(operationId);
                    Console.WriteLine("Server status: {0}, waiting {1} seconds...", results.Status, i);
                    await Task.Delay(1000);
                    if (i == 9)
                    {
                        Console.WriteLine("Server timed out.");
                    }
                }
                while ((results.Status == TextOperationStatusCodes.Running ||
                    results.Status == TextOperationStatusCodes.NotStarted) && i++ < maxRetries);

                // Display the found text.
                Console.WriteLine();
                var textRecognitionLocalFileResults = results.RecognitionResults;
                foreach (TextRecognitionResult recResult in textRecognitionLocalFileResults)
                {
                    foreach (Line line in recResult.Lines)
                    {
                        Console.WriteLine(line.Text);
                    }
                }
                Console.WriteLine();

                string fileName = $"{Path.GetFileName(localImage)}.json";
                SaveJsonOCR(results, fileName);
            }
        }

        public static async Task RecognizePrintedTextUrl(ComputerVisionClient client, string imageUrl)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("RECOGNIZE PRINTED TEXT - URL IMAGE");
            Console.WriteLine();

            Console.WriteLine($"Performing OCR on URL image {Path.GetFileName(imageUrl)}...");
            Console.WriteLine();

            // Perform OCR on image
            OcrResult remoteOcrResult = await client.RecognizePrintedTextAsync(true, imageUrl);

            // Print the recognized text
            Console.WriteLine("Text:");
            Console.WriteLine("Language: " + remoteOcrResult.Language);
            Console.WriteLine("Text Angle: " + remoteOcrResult.TextAngle);
            Console.WriteLine("Orientation: " + remoteOcrResult.Orientation);
            Console.WriteLine();
            Console.WriteLine("Text regions: ");
            foreach (var remoteRegion in remoteOcrResult.Regions)
            {
                Console.WriteLine("Region bounding box: " + remoteRegion.BoundingBox);
                foreach (var line in remoteRegion.Lines)
                {
                    Console.WriteLine("Line bounding box: " + line.BoundingBox);

                    foreach (var word in line.Words)
                    {
                        Console.WriteLine("Word bounding box: " + word.BoundingBox);
                        Console.WriteLine("Text: " + word.Text);
                    }
                    Console.WriteLine();
                }
            }
        }

        public static async Task RecognizePrintedTextLocal(ComputerVisionClient client, string localImage)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("RECOGNIZE PRINTED TEXT - LOCAL IMAGE");
            Console.WriteLine();

            using (Stream stream = File.OpenRead(localImage))
            {
                Console.WriteLine($"Performing OCR on local image {Path.GetFileName(localImage)}...");
                Console.WriteLine();
                // Get the recognized text
                OcrResult localFileOcrResult = await client.RecognizePrintedTextInStreamAsync(true, stream);

                // Display text, language, angle, orientation, and regions of text from the results.
                Console.WriteLine("Text:");
                Console.WriteLine("Language: " + localFileOcrResult.Language);
                Console.WriteLine("Text Angle: " + localFileOcrResult.TextAngle);
                Console.WriteLine("Orientation: " + localFileOcrResult.Orientation);
                Console.WriteLine();
                Console.WriteLine("Text regions: ");

                // Getting only one line of text for testing purposes. To see full demonstration, remove the counter & conditional.
                int counter = 0;
                foreach (var localRegion in localFileOcrResult.Regions)
                {
                    Console.WriteLine("Region bounding box: " + localRegion.BoundingBox);
                    foreach (var line in localRegion.Lines)
                    {
                        Console.WriteLine("Line bounding box: " + line.BoundingBox);
                        if (counter == 1)
                        {
                            Console.WriteLine();
                            return;
                        }
                        counter++;
                        foreach (var word in line.Words)
                        {
                            Console.WriteLine("Word bounding box: " + word.BoundingBox);
                            Console.WriteLine("Text: " + word.Text);
                        }
                    }
                }
            }
        }
    }
}
