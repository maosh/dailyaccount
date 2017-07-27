namespace dailyAccount
{
    partial class Login
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
            this.login_BTN = new System.Windows.Forms.Button();
            this.acc_ = new System.Windows.Forms.TextBox();
            this.pwd_ = new System.Windows.Forms.TextBox();
            this.取消 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // login_BTN
            // 
            this.login_BTN.Location = new System.Drawing.Point(80, 120);
            this.login_BTN.Name = "login_BTN";
            this.login_BTN.Size = new System.Drawing.Size(75, 23);
            this.login_BTN.TabIndex = 2;
            this.login_BTN.Text = "登陆";
            this.login_BTN.UseVisualStyleBackColor = true;
            this.login_BTN.Click += new System.EventHandler(this.login_BTN_Click);
            // 
            // acc_
            // 
            this.acc_.Location = new System.Drawing.Point(80, 26);
            this.acc_.Name = "acc_";
            this.acc_.Size = new System.Drawing.Size(157, 21);
            this.acc_.TabIndex = 0;
            // 
            // pwd_
            // 
            this.pwd_.Location = new System.Drawing.Point(80, 65);
            this.pwd_.Name = "pwd_";
            this.pwd_.PasswordChar = '*';
            this.pwd_.Size = new System.Drawing.Size(157, 21);
            this.pwd_.TabIndex = 1;
            // 
            // 取消
            // 
            this.取消.Location = new System.Drawing.Point(162, 120);
            this.取消.Name = "取消";
            this.取消.Size = new System.Drawing.Size(75, 23);
            this.取消.TabIndex = 3;
            this.取消.Text = "改密";
            this.取消.UseVisualStyleBackColor = true;
            this.取消.Click += new System.EventHandler(this.取消_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 195);
            this.Controls.Add(this.pwd_);
            this.Controls.Add(this.acc_);
            this.Controls.Add(this.取消);
            this.Controls.Add(this.login_BTN);
            this.Name = "Login";
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button login_BTN;
        private System.Windows.Forms.TextBox acc_;
        private System.Windows.Forms.TextBox pwd_;
        private System.Windows.Forms.Button 取消;
    }
}