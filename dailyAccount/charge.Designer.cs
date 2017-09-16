namespace dailyAccount
{
    partial class charge
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
            this.components = new System.ComponentModel.Container();
            this.query_ = new System.Windows.Forms.Button();
            this.chargeTimePicks_ = new System.Windows.Forms.DateTimePicker();
            this.dataitemView_ = new System.Windows.Forms.DataGridView();
            this.amount_ = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.refresh_ = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataitemView_)).BeginInit();
            this.SuspendLayout();
            // 
            // query_
            // 
            this.query_.Location = new System.Drawing.Point(298, 13);
            this.query_.Name = "query_";
            this.query_.Size = new System.Drawing.Size(75, 23);
            this.query_.TabIndex = 0;
            this.query_.Text = "查询";
            this.query_.UseVisualStyleBackColor = true;
            this.query_.Click += new System.EventHandler(this.query__Click);
            // 
            // chargeTimePicks_
            // 
            this.chargeTimePicks_.Location = new System.Drawing.Point(183, 13);
            this.chargeTimePicks_.Name = "chargeTimePicks_";
            this.chargeTimePicks_.Size = new System.Drawing.Size(109, 21);
            this.chargeTimePicks_.TabIndex = 1;
            // 
            // dataitemView_
            // 
            this.dataitemView_.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataitemView_.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataitemView_.Location = new System.Drawing.Point(0, 83);
            this.dataitemView_.Name = "dataitemView_";
            this.dataitemView_.RowTemplate.Height = 23;
            this.dataitemView_.Size = new System.Drawing.Size(489, 552);
            this.dataitemView_.TabIndex = 2;
            this.dataitemView_.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataitemView__CellContentClick);
            // 
            // amount_
            // 
            this.amount_.AutoSize = true;
            this.amount_.Location = new System.Drawing.Point(13, 23);
            this.amount_.Name = "amount_";
            this.amount_.Size = new System.Drawing.Size(0, 12);
            this.amount_.TabIndex = 3;
            // 
            // refresh_
            // 
            this.refresh_.Location = new System.Drawing.Point(394, 11);
            this.refresh_.Name = "refresh_";
            this.refresh_.Size = new System.Drawing.Size(75, 23);
            this.refresh_.TabIndex = 4;
            this.refresh_.Text = "今日";
            this.refresh_.UseVisualStyleBackColor = true;
            this.refresh_.Click += new System.EventHandler(this.refresh__Click);
            // 
            // charge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 634);
            this.Controls.Add(this.refresh_);
            this.Controls.Add(this.amount_);
            this.Controls.Add(this.dataitemView_);
            this.Controls.Add(this.chargeTimePicks_);
            this.Controls.Add(this.query_);
            this.Name = "charge";
            this.Text = "充值";
            this.Load += new System.EventHandler(this.charge_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataitemView_)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button query_;
        private System.Windows.Forms.DateTimePicker chargeTimePicks_;
        private System.Windows.Forms.DataGridView dataitemView_;
        private System.Windows.Forms.Label amount_;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button refresh_;
    }
}