namespace Forms {
    partial class Envelope {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.picTabButton = new System.Windows.Forms.PictureBox();
            this.picLeftFrame = new System.Windows.Forms.PictureBox();
            this.picTime = new System.Windows.Forms.PictureBox();
            this.picValue = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picTabButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLeftFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picValue)).BeginInit();
            this.SuspendLayout();
            // 
            // picTabButton
            // 
            this.picTabButton.BackColor = System.Drawing.Color.Transparent;
            this.picTabButton.Location = new System.Drawing.Point(7, 8);
            this.picTabButton.Name = "picTabButton";
            this.picTabButton.Size = new System.Drawing.Size(262, 28);
            this.picTabButton.TabIndex = 0;
            this.picTabButton.TabStop = false;
            // 
            // picLeftFrame
            // 
            this.picLeftFrame.BackColor = System.Drawing.Color.DarkGray;
            this.picLeftFrame.Location = new System.Drawing.Point(7, 42);
            this.picLeftFrame.Name = "picLeftFrame";
            this.picLeftFrame.Size = new System.Drawing.Size(42, 141);
            this.picLeftFrame.TabIndex = 1;
            this.picLeftFrame.TabStop = false;
            // 
            // picTime
            // 
            this.picTime.BackColor = System.Drawing.Color.Gray;
            this.picTime.Location = new System.Drawing.Point(56, 42);
            this.picTime.Name = "picTime";
            this.picTime.Size = new System.Drawing.Size(213, 38);
            this.picTime.TabIndex = 3;
            this.picTime.TabStop = false;
            this.picTime.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picHeader_MouseDown);
            this.picTime.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picHeader_MouseMove);
            this.picTime.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picHeader_MouseUp);
            // 
            // picValue
            // 
            this.picValue.BackColor = System.Drawing.Color.Silver;
            this.picValue.Location = new System.Drawing.Point(56, 86);
            this.picValue.Name = "picValue";
            this.picValue.Size = new System.Drawing.Size(213, 97);
            this.picValue.TabIndex = 4;
            this.picValue.TabStop = false;
            this.picValue.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picCell_MouseDown);
            this.picValue.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picCell_MouseMove);
            this.picValue.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picCell_MouseUp);
            // 
            // Envelope
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(277, 190);
            this.Controls.Add(this.picValue);
            this.Controls.Add(this.picTime);
            this.Controls.Add(this.picLeftFrame);
            this.Controls.Add(this.picTabButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Envelope";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Envelope_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picTabButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLeftFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picTabButton;
        private System.Windows.Forms.PictureBox picLeftFrame;
        private System.Windows.Forms.PictureBox picTime;
        private System.Windows.Forms.PictureBox picValue;
    }
}

