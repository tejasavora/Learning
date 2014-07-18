using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace AsyncConsoleApp
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                return AsyncContext.Run(() => MainAsync(args));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return -1;
            }
        }

        private static async Task<int> MainAsync(string[] args)
        {
            await Task.Delay(1000);
            await SumPageSizesAsync();
            Console.WriteLine("All Done !!!");
            return 0;
        }

        private static async Task SumPageSizesAsync()
        {
            // Make a list of web addresses.
            IEnumerable<string> urlList = SetUpURLList();

            // Create a query. 
            IEnumerable<Task<int>> downloadTasksQuery = 
                from url in urlList select ProcessURLAsync(url);

            // Use ToArray to execute the query and start the download tasks.
            Task<int>[] downloadTasks = downloadTasksQuery.ToArray();

            // You can do other work here before awaiting. 

            // Await the completion of all the running tasks. 
            int[] lengths = await Task.WhenAll(downloadTasks);

            //// The previous line is equivalent to the following two statements. 
            //Task<int[]> whenAllTask = Task.WhenAll(downloadTasks); 
            //int[] lengths = await whenAllTask; 

            int total = lengths.Sum();

            //var total = 0; 
            //foreach (var url in urlList) 
            //{ 
            //    byte[] urlContents = await GetURLContentsAsync(url); 

            //    // The previous line abbreviates the following two assignment statements. 
            //    // GetURLContentsAsync returns a Task<T>. At completion, the task 
            //    // produces a byte array. 
            //    //Task<byte[]> getContentsTask = GetURLContentsAsync(url); 
            //    //byte[] urlContents = await getContentsTask; 

            //    DisplayResults(url, urlContents); 

            //    // Update the total.           
            //    total += urlContents.Length; 
            //} 

            // Display the total count for all of the websites.
            Console.WriteLine("Total bytes returned:  {0}", total);
        }


        private static IEnumerable<string> SetUpURLList()
        {
            var urls = new List<string> 
            { 
                "http://msdn.microsoft.com",
                "http://msdn.microsoft.com/library/windows/apps/br211380.aspx",
                "http://msdn.microsoft.com/en-us/library/hh290136.aspx",
                "http://msdn.microsoft.com/en-us/library/ee256749.aspx",
                "http://msdn.microsoft.com/en-us/library/hh290138.aspx",
                "http://msdn.microsoft.com/en-us/library/hh290140.aspx",
                "http://msdn.microsoft.com/en-us/library/dd470362.aspx",
                "http://msdn.microsoft.com/en-us/library/aa578028.aspx",
                "http://msdn.microsoft.com/en-us/library/ms404677.aspx",
                "http://msdn.microsoft.com/en-us/library/ff730837.aspx"
            };
            return urls;
        }

        // The actions from the foreach loop are moved to this async method. 
        private static async Task<int> ProcessURLAsync(string url)
        {
            var byteArray = await GetURLContentsAsync(url);
            DisplayResults(url, byteArray);
            return byteArray.Length;
        }

        private static async Task<byte[]> GetURLContentsAsync(string url)
        {
            // The downloaded resource ends up in the variable named content. 
            var content = new MemoryStream();

            // Initialize an HttpWebRequest for the current URL. 
            var webReq = (HttpWebRequest)WebRequest.Create(url);

            // Send the request to the Internet resource and wait for 
            // the response. 
            using (WebResponse response = await webReq.GetResponseAsync())
            {
                // Get the data stream that is associated with the specified url. 
                using (Stream responseStream = response.GetResponseStream())
                {
                    await responseStream.CopyToAsync(content);
                }
            }

            // Return the result as a byte array. 
            return content.ToArray();

        }

        private static void DisplayResults(string url, byte[] content)
        {
            // Display the length of each website. The string format  
            // is designed to be used with a monospaced font, such as 
            // Lucida Console or Global Monospace. 
            var bytes = content.Length;
            // Strip off the "http://".
            var displayURL = url.Replace("http://", "");
            Console.WriteLine("{0,-58} {1,8}", displayURL, bytes);

        }
    }
}
