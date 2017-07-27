namespace dailyAccount
{
    partial class ChangePWD
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
            this.lable = new System.Windows.Forms.Label();
            this.acc_ = new System.Windows.Forms.TextBox();
            this.lable1 = new System.Windows.Forms.Label();
            this.oldPwd_ = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.newPwd_ = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lable
            // 
            this.lable.AutoSize = true;
            this.lable.Location = new System.Drawing.Point(53, 47);
            this.lable.Name = "lable";
            this.lable.Size = new System.Drawing.Size(41, 12);
            this.lable.TabIndex = 0;
            this.lable.Text = "账号：";
            // 
            // acc_
            // 
            this.acc_.Location = new System.Drawing.Point(104, 44);
            this.acc_.Name = "acc_";
            this.acc_.Size = new System.Drawing.Size(128, 21);
            this.acc_.TabIndex = 1;
            // 
            // lable1
            // 
            this.lable1.AutoSize = true;
            this.lable1.Location = new System.Drawing.Point(53, 74);
            this.lable1.Name = "lable1";
            this.lable1.Size = new System.Drawing.Size(41, 12);
            this.lable1.TabIndex = 0;
            this.lable1.Text = "老密码";
            // 
            // oldPwd_
            // 
            this.oldPwd_.Location = new System.Drawing.Point(104, 71);
            this.oldPwd_.Name = "oldPwd_";
            this.oldPwd_.PasswordChar = '*';
            this.oldPwd_.Size = new System.Drawing.Size(128, 21);
            this.oldPwd_.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(53, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "新密码";
            // 
            // newPwd_
            // 
            this.newPwd_.Location = new System.Drawing.Point(104, 98);
            this.newPwd_.Name = "newPwd_";
            this.newPwd_.PasswordChar = '*';
            this.newPwd_.Size = new System.Drawing.Size(128, 21);
            this.newPwd_.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(104, 134);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.TabStop = false;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ChangePWD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 187);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.newPwd_);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.oldPwd_);
            this.Controls.Add(this.lable1);
            this.Controls.Add(this.acc_);
            this.Controls.Add(this.lable);
            this.Name = "ChangePWD";
            this.Text = "ChangePWD";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lable;
        private System.Windows.Forms.TextBox acc_;
        private System.Windows.Forms.Label lable1;
        private System.Windows.Forms.TextBox oldPwd_;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox newPwd_;
        private System.Windows.Forms.Button button1;
    }
}