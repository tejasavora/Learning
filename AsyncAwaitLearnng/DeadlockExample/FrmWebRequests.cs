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

        #region TAP

        /// <summary>
        /// 'Get using TAP - Deadlock' button click event
        /// </summary>
        private void btnTAP_Click(object sender, EventArgs e)
        {
            // Clear result
            txtResults.Clear();

            // Set progress
            pgbRetrieve.Style = ProgressBarStyle.Continuous;
            pgbRetrieve.Value = 0;
            pgbRetrieve.Maximum = 100;

            var task = SumPageSizesAsync();
            txtResults.Text += string.Format("\r\n\r\nTotal bytes returned:  {0}\r\n", task.Result);
            txtResults.Text += "\r\n" + @"Done !!!";
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

            // Set progress
            pgbRetrieve.Style = ProgressBarStyle.Continuous;
            pgbRetrieve.Value = 0;
            pgbRetrieve.Maximum = 100;

            var totalBytes = await SumPageSizesAsync();

            txtResults.Text += string.Format("\r\n\r\nTotal bytes returned:  {0}\r\n", totalBytes);
            txtResults.Text += "\r\n" + @"Done !!!";
            pgbRetrieve.Value = 0;
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
                Invoke(d, new object[] { text });
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
                Invoke(d, new object[] {progress});
            }
            else
            {
                pgbRetrieve.Style = ProgressBarStyle.Continuous;
                pgbRetrieve.Value = progress;
            }
        }

        #endregion
    }
}
