using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Introduction
{
    public partial class FrmWebRequests : Form
    {
        #region Variables and Constants

        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void SetTextCallback(string text);
        delegate void SetProgressCallback(int progress);

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FrmWebRequests()
        {
            InitializeComponent();
        }

        #endregion Constructor

        #region Windows Form Events

        /// <summary>
        /// On Form Load Event
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            pgbRetrieve.Style = ProgressBarStyle.Continuous;
            pgbRetrieve.Value = 0;
            txtResults.Clear();
        }

        #endregion

        #region Synchronous

        /// <summary>
        /// 'Get Synchronous' button click
        /// </summary>
        private void btnSynchronous_Click(object sender, EventArgs e)
        {
            // Clear result
            txtResults.Clear();

            // Set progress
            pgbRetrieve.Style = ProgressBarStyle.Marquee;

            // Download
            SumPageSizes();

            // Stop progress
            pgbRetrieve.Style = ProgressBarStyle.Continuous;
            pgbRetrieve.Value = 0;
        }

        #endregion Synchronous

        #region Background Worker

        /// <summary>
        /// 'Get using Background Worker' button click event
        /// </summary>
        private void btnBackgroundWorker_Click(object sender, EventArgs e)
        {
            // Clear result
            txtResults.Clear();

            // Set progress
            pgbRetrieve.Style = ProgressBarStyle.Continuous;
            pgbRetrieve.Value = 0;
            pgbRetrieve.Maximum = 100;

            // Background worker
            var bgWorker = new BackgroundWorker
                {
                    WorkerSupportsCancellation = true, 
                    WorkerReportsProgress = true
                };

            // Do Work - Download
            bgWorker.DoWork += (o, args) =>
                {
                    var worker = o as BackgroundWorker;
                    if (worker != null && worker.CancellationPending)
                    {
                        args.Cancel = true;
                        return;
                    }

                    SumPageSizes(i =>
                        {
                            if (worker != null)
                                worker.ReportProgress(i);
                        });
                };

            // Progress changed
            bgWorker.ProgressChanged += (o, args) => { pgbRetrieve.Value = args.ProgressPercentage; };

            // Worker completed
            bgWorker.RunWorkerCompleted += (o, args) =>
                {
                    if ((args.Cancelled))
                        SetText(txtResults.Text + "\r\nCancelled !!!");

                    else if (args.Error != null)
                        SetText(txtResults.Text + "\r\nError: " + args.Error.Message);

                    else
                    {
                        SetText(txtResults.Text + "\r\nDone");
                        pgbRetrieve.Style = ProgressBarStyle.Continuous;
                        pgbRetrieve.Maximum = 0;
                    }
                };

            // Run worker
            bgWorker.RunWorkerAsync();
        }

        #endregion

        #region APM - Buggy Code

        /// <summary>
        /// 'Get using APM' button click event
        /// </summary>
        private void btnAPM_Click(object sender, EventArgs e)
        {
            // Make a list of web addresses.
            var urlList = SetUpURLList();

            var index = 0;
            foreach (var url in urlList)
            {
                // Initialize an HttpWebRequest for the current URL. 
                var webReq = (HttpWebRequest)WebRequest.Create(url);

                var myRequestState = new RequestState { request = webReq, currentIndex = ++index, url = url, totalUrls = urlList.Count };
                webReq.BeginGetResponse(RespCallback, myRequestState);
            }
        }

        #endregion

        #region Thread EAP

        private void btnEAP_Click(object sender, EventArgs e)
        {
            // Clear result
            txtResults.Clear();

            // Set progress
            pgbRetrieve.Style = ProgressBarStyle.Continuous;
            pgbRetrieve.Value = 0;
            pgbRetrieve.Maximum = 100;

            // Thread with EAP
            var thread = new ThreadEventBasedRequest();
            thread.PartialRequestCompleted += ThreadOnPartialRequestCompleted;
            thread.RequestCompleted += ThreadOnRequestCompleted;
            thread.Start(SetUpURLList());
        }

        private void ThreadOnPartialRequestCompleted(object sender, RequestResultEventArgs requestResultEventArgs)
        {
            SetProgress(requestResultEventArgs.Progress);
            DisplayResults(requestResultEventArgs.Url, requestResultEventArgs.Contents);
        }

        private void ThreadOnRequestCompleted(object sender, RequestCompletedEventArgs requestCompletedEventArgs)
        {
            SetText(txtResults.Text + string.Format("\r\n\r\nTotal bytes returned:  {0}\r\n", requestCompletedEventArgs.TotalBytes));
            SetText(txtResults.Text + "\r\nDone");
            SetProgress(100);
            SetProgress(0);
        }

        #endregion

        #region TAP

        /// <summary>
        /// 'Get using TAP' button click event
        /// </summary>
        private void btnTAP_Click(object sender, EventArgs e)
        {
            // Clear result
            txtResults.Clear();

            // Set progress
            pgbRetrieve.Style = ProgressBarStyle.Continuous;
            pgbRetrieve.Value = 0;
            pgbRetrieve.Maximum = 100;

            var task = new Task(() => SumPageSizes(SetProgress));
            task.ContinueWith(task1 => SetText(txtResults.Text + "\r\nDone !!!"));
            task.Start();

            // Zero progress
            pgbRetrieve.Value = 0;
        }

        #endregion

        #region TAP - async/await

        /// <summary>
        /// 'Get using TAP - async/wait' button click event
        /// </summary>
        private async void btnAsyncAwait_Click(object sender, EventArgs e)
        {
            // Clear result
            txtResults.Clear();
            string something = "testing !!!";

            // Set progress
            pgbRetrieve.Style = ProgressBarStyle.Continuous;
            pgbRetrieve.Value = 0;
            pgbRetrieve.Maximum = 100;

            int totalBytes = await SumPageSizesAsync();

            txtResults.Text += string.Format("\r\n\r\nTotal bytes returned:  {0}\r\n", totalBytes);
            txtResults.Text += "\r\n" + @"Done !!!" + " " + something;
            pgbRetrieve.Value = 0;
        }


        #endregion

        #region Normal Synchronous GET methods

        /// <summary>
        /// Sums up retrived page sizes
        /// </summary>
        private void SumPageSizes(Action<int> progressCallbback = null)
        {
            // Make a list of web addresses.
            var urlList = SetUpURLList();

            var index = 0;
            var total = 0;
            foreach (var url in urlList)
            {
                var urlContents = GetURLContents(url);
                DisplayResults(url, urlContents);

                // Update the total.          
                total += urlContents.Length;
                index++;

                // Progress
                if (progressCallbback != null)
                    progressCallbback(Convert.ToInt32((double) index/urlList.Count * 100));
            }
            // Display the total count for all of the websites.
            SetText(txtResults.Text + string.Format("\r\n\r\nTotal bytes returned:  {0}\r\n", total));
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

        #endregion

        #region APM GET methods

        private void RespCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                // State of request is asynchronous.
                var myRequestState = (RequestState)asynchronousResult.AsyncState;
                var myHttpWebRequest = myRequestState.request;
                myRequestState.response = (HttpWebResponse)myHttpWebRequest.EndGetResponse(asynchronousResult);

                // Read the response into a Stream object.
                var responseStream = myRequestState.response.GetResponseStream();

                var content = new MemoryStream();
                responseStream.CopyTo(content);

                myRequestState.totalBytes += content.Length;
                DisplayResults(myRequestState.url, content.ToArray());
                SetProgress(Convert.ToInt32((double)myRequestState.currentIndex / myRequestState.totalUrls * 100));

                if (myRequestState.currentIndex >= myRequestState.totalUrls)
                {
                    SetText(txtResults.Text + string.Format("\r\n\r\nTotal bytes returned:  {0}\r\n", myRequestState.totalBytes));
                    SetText(txtResults.Text + "\r\nDone !!!");
                    SetProgress(0);
                }
            }
            catch (WebException e)
            {
                Console.WriteLine("\nRespCallback Exception raised!");
                Console.WriteLine("\nMessage:{0}", e.Message);
                Console.WriteLine("\nStatus:{0}", e.Status);
            }            
        }

        #endregion

        #region Task Asyn/AWait GET Methods

        /// <summary>
        /// Sums up retrived page sizes asynchronously
        /// </summary>
        private async Task<int> SumPageSizesAsync()
        {
            // Make a list of web addresses.
            var urlList = SetUpURLList();

            var index = 0;
            var total = 0;

            foreach (var url in urlList)
            {
                try
                {
                    var urlContents = await GetURLContentsAsync(url); //.ConfigureAwait(false);

                    // The previous line abbreviates the following two assignment statements. 
                    // GetURLContentsAsync returns a Task<T>. At completion, the task 
                    // produces a byte array. 
                    //Task<byte[]> getContentsTask = GetURLContentsAsync(url); 
                    //byte[] urlContents = await getContentsTask;
                    DisplayResults(url, urlContents);

                    // Update the total.          
                    total += urlContents.Length;
                }
                catch (Exception ex)
                {
                    SetText(txtResults.Text + string.Format("\r\nUrl: {0}, Error: {1}, Type: {2}", url, ex.Message, ex.GetType().Name));
                }
                index++;

                // Progress
                pgbRetrieve.Value = Convert.ToInt32((double) index/urlList.Count*100);
            }

            return total;
            // Display the total count for all of the websites.
            //txtResults.Text += string.Format("\r\n\r\nTotal bytes returned:  {0}\r\n", total);
        }

        /// <summary>
        /// Gets URL contents asynchronously
        /// </summary>
        /// <param name="url">URL to be retrived</param>
        /// <returns>Awaitable task encapsulating result of URL contents</returns>
        private static async Task<byte[]> GetURLContentsAsync(string url)
        {
            // The downloaded resource ends up in the variable named content. 
            var content = new MemoryStream();

            // Initialize an HttpWebRequest for the current URL. 
            var webReq = (HttpWebRequest)WebRequest.Create(url);

            // Send the request to the Internet resource and wait for 
            // the response.                 
            using (var response = await webReq.GetResponseAsync())

            // The previous statement abbreviates the following two statements.
            //Task<WebResponse> responseTask = webReq.GetResponseAsync(); 
            //using (WebResponse response = await responseTask)
            {
                var responseStream = response.GetResponseStream();
                if (responseStream == null)
                    return new byte[0];

                // Get the data stream that is associated with the specified url. 
                using (responseStream)
                {
                    // Read the bytes in responseStream and copy them to content. 
                    await responseStream.CopyToAsync(content);

                    // The previous statement abbreviates the following two statements. 

                    // CopyToAsync returns a Task, not a Task<T>. 
                    //Task copyTask = responseStream.CopyToAsync(content); 
                    // When copyTask is completed, content contains a copy of 
                    // responseStream. 
                    //await copyTask;
                }
            }
            // Return the result as a byte array. 
            return content.ToArray();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets list of URLs to be downloaded
        /// </summary>
        /// <returns>List of URLs</returns>
        private static List<string> SetUpURLList()
        {
            return new List<string> 
            { 
                "http://msdn.microsoft.com/library/windows/apps/br211380.aspx",
                "http://msdn.microsoft.com",
                "http://msdn.microsoft.com/en-us/library/hh290136.aspx",
                "http://msdn.microsoft.com/en-us/library/ee256749.aspx",
                "http://msdn.microsoft.com/en-us/library/hh290138.aspx",
                "http://msdn.microsoft.com/en-us/library/hh290140.aspx",
                "http://msdn.microsoft.com/en-us/library/dd470362.aspx",
                "http://msdn.microsoft.com/en-us/library/aa578028.aspx",
                "http://msdn.microsoft.com/en-us/library/ms404677.aspx",
                "http://msdn.microsoft.com/en-us/library/ff730837.aspx"
            };
        }

        /// <summary>
        /// Displays results
        /// </summary>
        private void DisplayResults(string url, byte[] content)
        {
            // Display the length of each website. The string format  
            // is designed to be used with a monospaced font, such as 
            // Lucida Console or Global Monospace. 
            var bytes = content.Length;
            // Strip off the "http://".
            var displayURL = url.Replace("http://", "");
            SetText(txtResults.Text + string.Format("\r\n{0,-80} {1, 8}", displayURL, bytes));
        }

        // This method demonstrates a pattern for making thread-safe
        // calls on a Windows Forms control. 
        //
        // If the calling thread is different from the thread that
        // created the TextBox control, this method creates a
        // SetTextCallback and calls itself asynchronously using the
        // Invoke method.
        //
        // If the calling thread is the same as the thread that created
        // the TextBox control, the Text property is set directly. 
        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (txtResults.InvokeRequired)
            {
                var d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
               txtResults.Text = text;
            }
        }

        private void SetProgress(int progress)
        {
            if (pgbRetrieve.InvokeRequired)
            {
                var d = new SetProgressCallback(SetProgress);
                this.Invoke(d, new object[] {progress});
            }
            else
            {
                pgbRetrieve.Style = ProgressBarStyle.Continuous;
                pgbRetrieve.Value = progress;
            }
        }

        #endregion
    }

    public class RequestState
    {
        // This class stores the State of the request. 
        const int BUFFER_SIZE = 1024;
        public StringBuilder requestData;
        public byte[] BufferRead;
        public HttpWebRequest request;
        public HttpWebResponse response;
        public Stream streamResponse;
        public int totalUrls = 0;
        public int currentIndex = 0;
        public long totalBytes = 0;
        public string url = string.Empty;

        public RequestState()
        {
            BufferRead = new byte[BUFFER_SIZE];
            requestData = new StringBuilder("");
            request = null;
            streamResponse = null;
        }
    }
}
