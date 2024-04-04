using System.Globalization;
using System.Net;
using System.Reflection.Metadata;
using HtmlAgilityPack;

namespace UrbanScienceScraper
{
    
    class Program
    {
        
        static void Main(string[] args)
        {
            
            string industryOutput = scrapeIndustrySales();
            List<string> brandOutput = scrapeBrandSales();
            string sharesOutput = scrapeBrandShares();
            
            string brandOutputString = string.Join(",", brandOutput);
            
            string[] jsonDataArray = new string[]
            {
                industryOutput,
                brandOutputString, // Add the combined string for brandOutput
                sharesOutput
            };


           string filePath = "/Users/cadekennedy/Documents/Capstone Folder/output2.json";
           WriteToJsonFile(jsonDataArray, filePath);
        }
        static string scrapeBrandShares()
        {
            string htmlFilePath = "/Users/cadekennedy/Documents/Capstone Folder/parser.html";
            string testString = "INFINITI Share";
            
            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(htmlFilePath);
            
            var brandSharesElements = htmlDocument.DocumentNode.SelectNodes("//span[contains(text(),'')]");
            
            List<double> extractedNumbers = new List<double>();

            if (brandSharesElements != null)
            {
                foreach (var element in brandSharesElements)
                {
                    string innerText = element.InnerText;
                    string cleanedText = innerText.Trim();
                    cleanedText = innerText.Replace(" ", "");

                    string pattern = @"-?\d+(\.\d+)?%";
                    if (System.Text.RegularExpressions.Regex.IsMatch(cleanedText, pattern))
                    {
                        // Remove '%' symbol before parsing
                        cleanedText = cleanedText.Replace("%", "");
                        // Parse the cleaned text to extract numbers
                        if (double.TryParse(cleanedText, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double number))
                        {
                            // Only add numbers greater than or equal to 0 and less than 100
                            if (number >= 0.0001 && number < 100)
                            {
                                extractedNumbers.Add(number);
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                
                Console.WriteLine("List of numbers from brand shares: " + string.Join(", ", extractedNumbers));
            }
            else
            {
                Console.WriteLine("No span elements found.");
            }

            var brandSharesIndicators = htmlDocument.DocumentNode.SelectNodes("//span[contains(@class, 'mat-subheading-2') and contains(@class, 'dark:!text-gray-300') and text()='INFINITI Share']");

            if (brandSharesElements != null && brandSharesIndicators != null)
            {
                Console.WriteLine("Got here.");
                for (int i = 0; i < brandSharesElements.Count && i < brandSharesIndicators.Count; i++)
                {
                    var brandShare = brandSharesElements[i+2].InnerText.Trim();
                    var textString = brandSharesIndicators[i].InnerText.Trim();
                    //Console.WriteLine(brandSales);
                    //Console.WriteLine(textString);
            
                    if (System.Text.RegularExpressions.Regex.IsMatch(brandShare, @"^\d{1,2}\.\d$") && String.Equals(textString, testString))
                    {
                        Console.WriteLine("Brand Share: " + brandShare);
                        string jsonData = "{\"brand_share_INFINITI\": \"" + brandShare + "\"}";
                        return jsonData;
                    }
                }
            }
            else
            {
                Console.WriteLine("No data found");
            }

            return "Error";
        }
        static List<string> scrapeBrandSales()
        {
            string htmlFilePath = "/Users/cadekennedy/Documents/Capstone Folder/parser.html";
            string[] testStringArray = { "Honda", "Toyota", "Ford", "Chevrolet", "Nissan", "Mercedes-Benz", "Jeep", "Subaru", "Audi", "Hyundai", "INFINITI" };

            
            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(htmlFilePath);
            
            //var brandSalesElements = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class,'usai-widget-delta__value')]");
            //var brandSalesIndicators = htmlDocument.DocumentNode.SelectNodes("//span[contains(@class, 'mat-subheading-2') and contains(@class, 'dark:!text-gray-300') and text()='INFINITI Sales']");

            var brandSalesElements = htmlDocument.DocumentNode.SelectNodes("//span[contains(text(),'')]");

            List<int> extractedNumbers = new List<int>();

            if (brandSalesElements != null)
            {
                foreach (var element in brandSalesElements)
                {
                    // Get inner text of the span element
                    string innerText = element.InnerText;

                    // Remove leading and trailing whitespace
                    string cleanedText = innerText.Trim();

                    // Check if the cleaned text matches the regex pattern
                    string pattern = @"^\d{1,3},\d{3}$";
                    if (System.Text.RegularExpressions.Regex.IsMatch(cleanedText, pattern))
                    {
                        // Parse the cleaned text to extract numbers
                        // Remove commas only before parsing
                        cleanedText = cleanedText.Replace(",", "");
                        if (int.TryParse(cleanedText, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out int number))
                        {
                            // Only add numbers greater than 10
                            if (number > 10)
                            {
                                extractedNumbers.Add(number);
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                Console.WriteLine("List of numbers from brand sales: " + string.Join(", ", extractedNumbers));
            }
            else
            {
                Console.WriteLine("No span elements found.");
            }
            // var hondaSalesIndicators = htmlDocument.DocumentNode.SelectNodes("//span[@class='ng-star-inserted'][normalize-space(text())='Honda']");
            // var toyotaSalesIndicators = htmlDocument.DocumentNode.SelectNodes("//span[@class='ng-star-inserted'][normalize-space(text())='Toyota']");
            // var fordSalesIndicators = htmlDocument.DocumentNode.SelectNodes("//span[@class='ng-star-inserted'][normalize-space(text())='Ford']");
            // var chevroletSalesIndicators = htmlDocument.DocumentNode.SelectNodes("//span[@class='ng-star-inserted'][normalize-space(text())='Chevrolet']");
            // var nissanSalesIndicators = htmlDocument.DocumentNode.SelectNodes("//span[@class='ng-star-inserted'][normalize-space(text())='Nissan']");
            // var mercedesSalesIndicators = htmlDocument.DocumentNode.SelectNodes("//span[@class='ng-star-inserted'][normalize-space(text())='Mercedes-Benz']");
            // var jeepSalesIndicators = htmlDocument.DocumentNode.SelectNodes("//span[@class='ng-star-inserted'][normalize-space(text())='Jeep']");
            // var subaruSalesIndicators = htmlDocument.DocumentNode.SelectNodes("//span[@class='ng-star-inserted'][normalize-space(text())='Subaru']");
            // var audiSalesIndicators = htmlDocument.DocumentNode.SelectNodes("//span[@class='ng-star-inserted'][normalize-space(text())='Audi']");
            // var hyundaiSalesIndicators = htmlDocument.DocumentNode.SelectNodes("//span[@class='ng-star-inserted'][normalize-space(text())='Hyundai']");
            // var infinitiSalesIndicators = htmlDocument.DocumentNode.SelectNodes("//span[@class='ng-star-inserted'][normalize-space(text())='INFINITI']");




            var brandSalesIndicators = new Dictionary<string, HtmlNodeCollection>()
            {
                { "Honda", htmlDocument.DocumentNode.SelectNodes("//span[@class='ng-star-inserted'][normalize-space(text())='Honda']") },
                { "Toyota", htmlDocument.DocumentNode.SelectNodes("//span[@class='ng-star-inserted'][normalize-space(text())='Toyota']") },
                { "Ford", htmlDocument.DocumentNode.SelectNodes("//span[@class='ng-star-inserted'][normalize-space(text())='Ford']") },
                { "Chevrolet", htmlDocument.DocumentNode.SelectNodes("//span[@class='ng-star-inserted'][normalize-space(text())='Chevrolet']") },
                { "Nissan", htmlDocument.DocumentNode.SelectNodes("//span[@class='ng-star-inserted'][normalize-space(text())='Nissan']") },
                { "Mercedes-Benz", htmlDocument.DocumentNode.SelectNodes("//span[@class='ng-star-inserted'][normalize-space(text())='Mercedes-Benz']") },
                { "Jeep", htmlDocument.DocumentNode.SelectNodes("//span[@class='ng-star-inserted'][normalize-space(text())='Jeep']") },
                { "Subaru", htmlDocument.DocumentNode.SelectNodes("//span[@class='ng-star-inserted'][normalize-space(text())='Subaru']") },
                { "Audi", htmlDocument.DocumentNode.SelectNodes("//span[@class='ng-star-inserted'][normalize-space(text())='Audi']") },
                { "Hyundai", htmlDocument.DocumentNode.SelectNodes("//span[@class='ng-star-inserted'][normalize-space(text())='Hyundai']") },
                { "INFINITI", htmlDocument.DocumentNode.SelectNodes("//span[@class='ng-star-inserted'][normalize-space(text())='INFINITI']") }
            };


            var jsonDataList = new List<string>();
            
            if (brandSalesElements != null && brandSalesIndicators != null)
            {
                Console.WriteLine("Got here.");
                foreach (var brandEntry in brandSalesIndicators)
                {
                    var brandName = brandEntry.Key;
                    var salesIndicators = brandEntry.Value;

                    if (salesIndicators != null)
                    {
                        Console.WriteLine("Got here.");
                        //determine the minimum count between extractedNumbers and salesIndicators
                        for (int j = 0; j < testStringArray.Length; j++)
                        {
                            int minCount = Math.Min(extractedNumbers.Count, salesIndicators.Count);

                            for (int i = 0; i < minCount; i++)
                            {
                                var brandSales = extractedNumbers[i];  
                                var textString = salesIndicators[i].InnerText.Trim(); 
                                var testString = testStringArray[j];
                                // Console.WriteLine("Sales element: " + brandSales);
                                // Console.WriteLine("Text string: " + textString);
                                // Console.WriteLine("Test string: " + testString);

                                if (textString.Equals(testString))
                                {
                                    foreach (int extractedNumber in extractedNumbers)
                                    {
                                        if (extractedNumber >= 1000)
                                        {
                                            Console.WriteLine("Finally reached inside of if statement\\n\\n");
                                            Console.WriteLine($"Brand Sales for {testString}: " + extractedNumber);
                                            string jsonData = $"{{\"brand_sales_{testString}\": \"{extractedNumber}\"}}";
                                            jsonDataList.Add(jsonData);
                                        }
                                        else
                                        {
                                            continue;
                                            // Console.WriteLine("Condition not met!");
                                            // Console.WriteLine($"textString: {textString}, testString: {testString}, equals: {textString.Equals(testString)}");
                                        }
                                    }
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                    }
                }
                return jsonDataList;
            }
            else
            {
                Console.WriteLine("No data found");
            }

            return new List<string> { "Error" };
        }
        static string scrapeIndustrySales()
        {
            string htmlFilePath = "/Users/cadekennedy/Documents/Capstone Folder/parser.html";
            string testString = "Industry Sales";
            
            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(htmlFilePath);
            
            var industrySalesElements = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class,'usai-widget-delta__value')]");
            var industrySalesIndicators = htmlDocument.DocumentNode.SelectNodes("//span[contains(@class, 'mat-subheading-2') and contains(@class, 'dark:!text-gray-300') and text()='Industry Sales']");

            if (industrySalesElements != null && industrySalesIndicators != null)
            {
                Console.WriteLine("Got here.");
                for (int i = 0; i < industrySalesElements.Count && i < industrySalesIndicators.Count; i++)
                {
                    var industrySales = industrySalesElements[i].InnerText.Trim();
                    var textString = industrySalesIndicators[i].InnerText.Trim();
            
                    if (System.Text.RegularExpressions.Regex.IsMatch(industrySales, @"^\d{1,3}(?:,\d{3})$") && String.Equals(textString, testString))
                    {
                        Console.WriteLine("Industry Sales: " + industrySales);
                        string jsonData = "{\"total_segment_sales\": \"" + industrySales + "\"}";
                        return jsonData;
                    }
                }
            }
            else
            {
                Console.WriteLine("No data found");
            }

            return "Error";
        }
        static async Task<string> Login(string url, string username, string password)
        {
            try
            {
                var handler = new HttpClientHandler
                {
                    AllowAutoRedirect = true
                };
                using (var httpClient = new HttpClient(handler))
                {
                    var loginPageResponse = await httpClient.GetAsync(url);
                    loginPageResponse.EnsureSuccessStatusCode();
                    
                    var loginData = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("username", username),
                        new KeyValuePair<string, string>("password", password)
                    });
                    
                    var response = await httpClient.PostAsync(url, loginData);
                    response.EnsureSuccessStatusCode();

                    // Check for errors in the request
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Login failed with status code: {response.StatusCode}");
                    }

                    
                    if (response.StatusCode == HttpStatusCode.Redirect || response.StatusCode == HttpStatusCode.RedirectMethod)
                    {
                        // Get the redirect URL
                        string redirectUrl = response.Headers.Location.AbsoluteUri;

                        
                        var redirectResponse = await httpClient.GetAsync(redirectUrl);

                        // Check for errors in the redirect response
                        if (!redirectResponse.IsSuccessStatusCode)
                        {
                            throw new Exception($"Login redirect failed with status code: {redirectResponse.StatusCode}");
                        }

                        
                        var cookies = redirectResponse.Headers.GetValues("Set-Cookie");

                        
                        if (cookies == null || !cookies.Any())
                        {
                            throw new Exception("No cookies found in the redirect response.");
                        }

                        
                        var cookieString = string.Join("; ", cookies);

                        return cookieString;
                    }
                    else
                    {
                        // Get cookies from the response headers
                        var cookies = response.Headers.GetValues("Set-Cookie");

                        // Ensure there's at least one cookie
                        if (cookies == null || !cookies.Any())
                        {
                            throw new Exception("No cookies found in the response.");
                        }

                        // Extracting the cookies
                        var cookieString = string.Join("; ", cookies);

                        return cookieString;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during login: {ex.Message}");
                return null;
            }
        }


        
        static async Task<string> GetData(string url, string cookies)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Cookie", cookies);
                return await httpClient.GetStringAsync(url);
            }
        }
        
        static void WriteToJsonFile(string[] jsonDataArray, string filePath)
        {
            try
            {
                // Open the file for writing
                using (StreamWriter file = File.CreateText(filePath))
                {
                    // Start the JSON array
                    file.Write("{\"queryResult\":[");

                    // Loop through each JSON data string
                    for (int i = 0; i < jsonDataArray.Length; i++)
                    {
                        // If not the first element, add a comma separator
                        if (i > 0)
                            file.Write(",");

                        // Write the current JSON data to the file
                        file.Write(jsonDataArray[i]);
                    }

                    // Close the JSON array
                    file.Write("]}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

    }
}