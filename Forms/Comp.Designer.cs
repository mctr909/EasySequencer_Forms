namespace Forms {
    partial class Comp {
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
            this.picCell.Location = new System.Drawing.Point(121, 15);
            this.picCell.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.picCell.Name = "picCell";
            this.picCell.Size = new System.Drawing.Size(462, 194);
            this.picCell.TabIndex = 9;
            this.picCell.TabStop = false;
            this.picCell.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picCell_MouseDown);
            this.picCell.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picCell_MouseMove);
            this.picCell.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picCell_MouseUp);
            // 
            // picFooter
            // 
            this.picFooter.BackColor = System.Drawing.Color.Gray;
            this.picFooter.Location = new System.Drawing.Point(121, 221);
            this.picFooter.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.picFooter.Name = "picFooter";
            this.picFooter.Size = new System.Drawing.Size(462, 76);
            this.picFooter.TabIndex = 8;
            this.picFooter.TabStop = false;
            // 
            // picRow
            // 
            this.picRow.BackColor = System.Drawing.Color.DarkGray;
            this.picRow.Location = new System.Drawing.Point(16, 15);
            this.picRow.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.picRow.Name = "picRow";
            this.picRow.Size = new System.Drawing.Size(91, 194);
            this.picRow.TabIndex = 6;
            this.picRow.TabStop = false;
            // 
            // Comp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 312);
            this.Controls.Add(this.picCell);
            this.Controls.Add(this.picFooter);
            this.Controls.Add(this.picRow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Name = "Comp";
            this.Text = "Comp";
            this.Load += new System.EventHandler(this.Comp_Load);
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