using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace Introduction
{
    internal class RequestResultEventArgs : EventArgs
    {
        public string Url { get; set; }
        public byte[] Contents { get; set; }
        public int Progress { get; set; }
    }

    internal class RequestCompletedEventArgs : EventArgs
    {
        public int TotalBytes { get; set; }
    }

    internal class ThreadEventBasedRequest
    {
        public event EventHandler<RequestResultEventArgs> PartialRequestCompleted;
        public event EventHandler<RequestCompletedEventArgs> RequestCompleted;

        /// <summary>
        /// Starts thread
        /// </summary>
        public void Start(List<string> urlList)
        {
            if (urlList == null)
                throw new ArgumentNullException("urlList");

            var thread = new Thread(o => SumPageSizes(urlList));
            thread.Start();
        }

        /// <summary>
        /// Sums up retrived page sizes
        /// </summary>
        private void SumPageSizes(IReadOnlyCollection<string> urlList)
        {
            var index = 0;
            var total = 0;
            foreach (var url in urlList)
            {
                var urlContents = GetURLContents(url);

                // Update the total.          
                total += urlContents.Length;
                index++;

                OnPartialRequestCompleted(new RequestResultEventArgs
                    {
                        Url = url,
                        Contents = urlContents,
                        Progress = Convert.ToInt32((double)index / urlList.Count * 100)
                    });
            }
            OnRequestCompleted(new RequestCompletedEventArgs
                {
                    TotalBytes = total
                });
        }

        /// <summary>
        /// Gets URL contents synchronously
        /// </summary>
        /// <param name="url">URL to be retrived</param>
        /// <returns>URL contents</returns>
        private static byte[] GetURLContents(string url)
        {
            // The downloaded resource ends up in the variable named content. 
            var content = new MemoryStream();

            // Initialize an HttpWebRequest for the current URL. 
            var webReq = (HttpWebRequest)WebRequest.Create(url);

            // Send the request to the Internet resource and wait for 
            // the response.
            using (var response = webReq.GetResponse())
            {
                var responseStream = response.GetResponseStream();
                if (responseStream == null)
                    return new byte[0];

                // Get the data stream that is associated with the specified url. 
                using (responseStream)
                    responseStream.CopyTo(content);
            }

            // Return the result as a byte array. 
            return content.ToArray();
        }

        /// <summary>
        /// Request completed event handler
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPartialRequestCompleted(RequestResultEventArgs e)
        {
            var handler = PartialRequestCompleted;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnRequestCompleted(RequestCompletedEventArgs e)
        {
            var handler = RequestCompleted;
            if (handler != null) handler(this, e);
        }
    }
}
