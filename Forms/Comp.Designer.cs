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
            this.picCell.Location = new System.Drawing.Point(56, 8);
            this.picCell.Name = "picCell";
            this.picCell.Size = new System.Drawing.Size(213, 97);
            this.picCell.TabIndex = 9;
            this.picCell.TabStop = false;
            this.picCell.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picCell_MouseDown);
            this.picCell.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picCell_MouseMove);
            this.picCell.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picCell_MouseUp);
            // 
            // picFooter
            // 
            this.picFooter.BackColor = System.Drawing.Color.Gray;
            this.picFooter.Location = new System.Drawing.Point(56, 110);
            this.picFooter.Name = "picFooter";
            this.picFooter.Size = new System.Drawing.Size(213, 38);
            this.picFooter.TabIndex = 8;
            this.picFooter.TabStop = false;
            // 
            // picRow
            // 
            this.picRow.BackColor = System.Drawing.Color.DarkGray;
            this.picRow.Location = new System.Drawing.Point(7, 8);
            this.picRow.Name = "picRow";
            this.picRow.Size = new System.Drawing.Size(42, 97);
            this.picRow.TabIndex = 6;
            this.picRow.TabStop = false;
            this.picRow.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picRow_MouseDown);
            this.picRow.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picRow_MouseMove);
            this.picRow.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picRow_MouseUp);
            // 
            // Comp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(276, 156);
            this.Controls.Add(this.picCell);
            this.Controls.Add(this.picFooter);
            this.Controls.Add(this.picRow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
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