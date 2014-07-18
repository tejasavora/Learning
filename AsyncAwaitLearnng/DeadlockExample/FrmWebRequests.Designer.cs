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
            this.pgbRetrieve = new System.Windows.Forms.ProgressBar();
            this.txtResults = new System.Windows.Forms.TextBox();
            this.btnAsyncAwait = new System.Windows.Forms.Button();
            this.btnTAP = new System.Windows.Forms.Button();
            this.SuspendLayout();
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
            this.btnAsyncAwait.Location = new System.Drawing.Point(263, 12);
            this.btnAsyncAwait.Name = "btnAsyncAwait";
            this.btnAsyncAwait.Size = new System.Drawing.Size(169, 44);
            this.btnAsyncAwait.TabIndex = 8;
            this.btnAsyncAwait.Text = "Get using TAP (async/await)";
            this.btnAsyncAwait.UseVisualStyleBackColor = true;
            this.btnAsyncAwait.Click += new System.EventHandler(this.btnAsyncAwait_Click);
            // 
            // btnTAP
            // 
            this.btnTAP.Location = new System.Drawing.Point(12, 12);
            this.btnTAP.Name = "btnTAP";
            this.btnTAP.Size = new System.Drawing.Size(245, 44);
            this.btnTAP.TabIndex = 5;
            this.btnTAP.Text = "Get using TAP (async/await) - Deadlock";
            this.btnTAP.UseVisualStyleBackColor = true;
            this.btnTAP.Click += new System.EventHandler(this.btnTAP_Click);
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
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FrmWebRequests";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Web Request Demo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar pgbRetrieve;
        private System.Windows.Forms.TextBox txtResults;
        private System.Windows.Forms.Button btnAsyncAwait;
        private System.Windows.Forms.Button btnTAP;

    }
}

