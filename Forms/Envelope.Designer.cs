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
            this.picRow = new System.Windows.Forms.PictureBox();
            this.picHeader = new System.Windows.Forms.PictureBox();
            this.picCell = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picTabButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCell)).BeginInit();
            this.SuspendLayout();
            // 
            // picTabButton
            // 
            this.picTabButton.BackColor = System.Drawing.Color.Transparent;
            this.picTabButton.Location = new System.Drawing.Point(16, 15);
            this.picTabButton.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.picTabButton.Name = "picTabButton";
            this.picTabButton.Size = new System.Drawing.Size(567, 56);
            this.picTabButton.TabIndex = 0;
            this.picTabButton.TabStop = false;
            // 
            // picRow
            // 
            this.picRow.BackColor = System.Drawing.Color.DarkGray;
            this.picRow.Location = new System.Drawing.Point(16, 83);
            this.picRow.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.picRow.Name = "picRow";
            this.picRow.Size = new System.Drawing.Size(91, 282);
            this.picRow.TabIndex = 1;
            this.picRow.TabStop = false;
            // 
            // picHeader
            // 
            this.picHeader.BackColor = System.Drawing.Color.Gray;
            this.picHeader.Location = new System.Drawing.Point(121, 83);
            this.picHeader.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.picHeader.Name = "picHeader";
            this.picHeader.Size = new System.Drawing.Size(462, 76);
            this.picHeader.TabIndex = 3;
            this.picHeader.TabStop = false;
            this.picHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picHeader_MouseDown);
            this.picHeader.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picHeader_MouseMove);
            this.picHeader.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picHeader_MouseUp);
            // 
            // picCell
            // 
            this.picCell.BackColor = System.Drawing.Color.Silver;
            this.picCell.Location = new System.Drawing.Point(121, 171);
            this.picCell.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.picCell.Name = "picCell";
            this.picCell.Size = new System.Drawing.Size(462, 194);
            this.picCell.TabIndex = 4;
            this.picCell.TabStop = false;
            this.picCell.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picCell_MouseDown);
            this.picCell.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picCell_MouseMove);
            this.picCell.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picCell_MouseUp);
            // 
            // Envelope
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(600, 380);
            this.Controls.Add(this.picCell);
            this.Controls.Add(this.picHeader);
            this.Controls.Add(this.picRow);
            this.Controls.Add(this.picTabButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Name = "Envelope";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Envelope_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picTabButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCell)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picTabButton;
        private System.Windows.Forms.PictureBox picRow;
        private System.Windows.Forms.PictureBox picHeader;
        private System.Windows.Forms.PictureBox picCell;
    }
}

