namespace Forms {
    partial class EQ {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.picCell = new System.Windows.Forms.PictureBox();
            this.picFooter = new System.Windows.Forms.PictureBox();
            this.picRow = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picCell)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFooter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRow)).BeginInit();
            this.SuspendLayout();
            // 
            // picCell
            // 
            this.picCell.BackColor = System.Drawing.Color.Silver;
            this.picCell.Location = new System.Drawing.Point(59, 12);
            this.picCell.Name = "picCell";
            this.picCell.Size = new System.Drawing.Size(213, 97);
            this.picCell.TabIndex = 12;
            this.picCell.TabStop = false;
            this.picCell.DoubleClick += new System.EventHandler(this.picCell_DoubleClick);
            this.picCell.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picCell_MouseDown);
            this.picCell.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picCell_MouseMove);
            this.picCell.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picCell_MouseUp);
            // 
            // picFooter
            // 
            this.picFooter.BackColor = System.Drawing.Color.Gray;
            this.picFooter.Location = new System.Drawing.Point(59, 114);
            this.picFooter.Name = "picFooter";
            this.picFooter.Size = new System.Drawing.Size(213, 38);
            this.picFooter.TabIndex = 11;
            this.picFooter.TabStop = false;
            // 
            // picRow
            // 
            this.picRow.BackColor = System.Drawing.Color.DarkGray;
            this.picRow.Location = new System.Drawing.Point(10, 12);
            this.picRow.Name = "picRow";
            this.picRow.Size = new System.Drawing.Size(42, 97);
            this.picRow.TabIndex = 10;
            this.picRow.TabStop = false;
            // 
            // EQ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 161);
            this.Controls.Add(this.picCell);
            this.Controls.Add(this.picFooter);
            this.Controls.Add(this.picRow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "EQ";
            this.Text = "EQ";
            this.Load += new System.EventHandler(this.EQ_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picCell)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFooter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRow)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picCell;
        private System.Windows.Forms.PictureBox picFooter;
        private System.Windows.Forms.PictureBox picRow;
    }
}