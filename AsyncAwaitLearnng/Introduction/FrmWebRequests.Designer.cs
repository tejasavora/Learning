namespace Introduction
{
    partial class FrmWebRequests
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSynchronous = new System.Windows.Forms.Button();
            this.btnBackgroundWorker = new System.Windows.Forms.Button();
            this.btnAPM = new System.Windows.Forms.Button();
            this.btnEAP = new System.Windows.Forms.Button();
            this.btnTAP = new System.Windows.Forms.Button();
            this.pgbRetrieve = new System.Windows.Forms.ProgressBar();
            this.txtResults = new System.Windows.Forms.TextBox();
            this.btnAsyncAwait = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSynchronous
            // 
            this.btnSynchronous.Location = new System.Drawing.Point(12, 12);
            this.btnSynchronous.Name = "btnSynchronous";
            this.btnSynchronous.Size = new System.Drawing.Size(101, 44);
            this.btnSynchronous.TabIndex = 0;
            this.btnSynchronous.Text = "Get Synchronous";
            this.btnSynchronous.UseVisualStyleBackColor = true;
            this.btnSynchronous.Click += new System.EventHandler(this.btnSynchronous_Click);
            // 
            // btnBackgroundWorker
            // 
            this.btnBackgroundWorker.Location = new System.Drawing.Point(119, 12);
            this.btnBackgroundWorker.Name = "btnBackgroundWorker";
            this.btnBackgroundWorker.Size = new System.Drawing.Size(164, 44);
            this.btnBackgroundWorker.TabIndex = 1;
            this.btnBackgroundWorker.Text = "Get using Background Worker";
            this.btnBackgroundWorker.UseVisualStyleBackColor = true;
            this.btnBackgroundWorker.Click += new System.EventHandler(this.btnBackgroundWorker_Click);
            // 
            // btnAPM
            // 
            this.btnAPM.Location = new System.Drawing.Point(289, 12);
            this.btnAPM.Name = "btnAPM";
            this.btnAPM.Size = new System.Drawing.Size(113, 44);
            this.btnAPM.TabIndex = 2;
            this.btnAPM.Text = "Get using APM";
            this.btnAPM.UseVisualStyleBackColor = true;
            this.btnAPM.Click += new System.EventHandler(this.btnAPM_Click);
            // 
            // btnEAP
            // 
            this.btnEAP.Location = new System.Drawing.Point(408, 12);
            this.btnEAP.Name = "btnEAP";
            this.btnEAP.Size = new System.Drawing.Size(146, 44);
            this.btnEAP.TabIndex = 3;
            this.btnEAP.Text = "Get using Thread / EAP";
            this.btnEAP.UseVisualStyleBackColor = true;
            this.btnEAP.Click += new System.EventHandler(this.btnEAP_Click);
            // 
            // btnTAP
            // 
            this.btnTAP.Location = new System.Drawing.Point(560, 12);
            this.btnTAP.Name = "btnTAP";
            this.btnTAP.Size = new System.Drawing.Size(115, 44);
            this.btnTAP.TabIndex = 5;
            this.btnTAP.Text = "Get using Tasks";
            this.btnTAP.UseVisualStyleBackColor = true;
            this.btnTAP.Click += new System.EventHandler(this.btnTAP_Click);
            // 
            // pgbRetrieve
            // 
            this.pgbRetrieve.Location = new System.Drawing.Point(12, 62);
            this.pgbRetrieve.Name = "pgbRetrieve";
            this.pgbRetrieve.Size = new System.Drawing.Size(838, 23);
            this.pgbRetrieve.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pgbRetrieve.TabIndex = 6;
            // 
            // txtResults
            // 
            this.txtResults.BackColor = System.Drawing.Color.White;
            this.txtResults.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtResults.Location = new System.Drawing.Point(12, 91);
            this.txtResults.Multiline = true;
            this.txtResults.Name = "txtResults";
            this.txtResults.ReadOnly = true;
            this.txtResults.Size = new System.Drawing.Size(838, 413);
            this.txtResults.TabIndex = 7;
            // 
            // btnAsyncAwait
            // 
            this.btnAsyncAwait.Location = new System.Drawing.Point(681, 12);
            this.btnAsyncAwait.Name = "btnAsyncAwait";
            this.btnAsyncAwait.Size = new System.Drawing.Size(169, 44);
            this.btnAsyncAwait.TabIndex = 8;
            this.btnAsyncAwait.Text = "Get using TAP (async/await)";
            this.btnAsyncAwait.UseVisualStyleBackColor = true;
            this.btnAsyncAwait.Click += new System.EventHandler(this.btnAsyncAwait_Click);
            // 
            // FrmWebRequests
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(865, 514);
            this.Controls.Add(this.btnAsyncAwait);
            this.Controls.Add(this.txtResults);
            this.Controls.Add(this.pgbRetrieve);
            this.Controls.Add(this.btnTAP);
            this.Controls.Add(this.btnEAP);
            this.Controls.Add(this.btnAPM);
            this.Controls.Add(this.btnBackgroundWorker);
            this.Controls.Add(this.btnSynchronous);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FrmWebRequests";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Web Request Demo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSynchronous;
        private System.Windows.Forms.Button btnBackgroundWorker;
        private System.Windows.Forms.Button btnAPM;
        private System.Windows.Forms.Button btnEAP;
        private System.Windows.Forms.Button btnTAP;
        private System.Windows.Forms.ProgressBar pgbRetrieve;
        private System.Windows.Forms.TextBox txtResults;
        private System.Windows.Forms.Button btnAsyncAwait;

    }
}

