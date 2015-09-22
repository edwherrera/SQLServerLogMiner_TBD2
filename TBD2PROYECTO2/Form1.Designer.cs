namespace TBD2PROYECTO2
{
    partial class Form1
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.grid = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.insertListView = new System.Windows.Forms.ListView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.deleteListView = new System.Windows.Forms.ListView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.updateListView = new System.Windows.Forms.ListView();
            this.grid.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 46);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(177, 46);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 1;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(389, 43);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 2;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.button1_Click);
            // 
            // grid
            // 
            this.grid.Controls.Add(this.tabPage1);
            this.grid.Controls.Add(this.tabPage2);
            this.grid.Controls.Add(this.tabPage3);
            this.grid.Location = new System.Drawing.Point(0, 85);
            this.grid.Name = "grid";
            this.grid.SelectedIndex = 0;
            this.grid.Size = new System.Drawing.Size(619, 317);
            this.grid.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.insertListView);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(611, 291);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Insert";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // insertListView
            // 
            this.insertListView.Location = new System.Drawing.Point(0, 0);
            this.insertListView.Name = "insertListView";
            this.insertListView.Size = new System.Drawing.Size(611, 285);
            this.insertListView.TabIndex = 0;
            this.insertListView.UseCompatibleStateImageBehavior = false;
            this.insertListView.View = System.Windows.Forms.View.Details;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.deleteListView);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(611, 291);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Delete";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // deleteListView
            // 
            this.deleteListView.Location = new System.Drawing.Point(3, 3);
            this.deleteListView.Name = "deleteListView";
            this.deleteListView.Size = new System.Drawing.Size(608, 282);
            this.deleteListView.TabIndex = 0;
            this.deleteListView.UseCompatibleStateImageBehavior = false;
            this.deleteListView.View = System.Windows.Forms.View.Details;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.updateListView);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(611, 291);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Update";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // updateListView
            // 
            this.updateListView.Location = new System.Drawing.Point(0, 0);
            this.updateListView.Name = "updateListView";
            this.updateListView.Size = new System.Drawing.Size(611, 285);
            this.updateListView.TabIndex = 1;
            this.updateListView.UseCompatibleStateImageBehavior = false;
            this.updateListView.View = System.Windows.Forms.View.Details;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 414);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.grid.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.TabControl grid;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListView insertListView;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListView deleteListView;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListView updateListView;
    }
}

