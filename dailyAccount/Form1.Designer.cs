namespace dailyAccount
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.refresh_BTN = new System.Windows.Forms.Button();
            this.dataitemView_ = new System.Windows.Forms.DataGridView();
            this.bottomLayout_ = new System.Windows.Forms.Panel();
            this.materialOwner_ = new System.Windows.Forms.TextBox();
            this.updateHistoryBTN_ = new System.Windows.Forms.Button();
            this.reportBTN_ = new System.Windows.Forms.Button();
            this.queryAllBTN_ = new System.Windows.Forms.Button();
            this.endDate_ = new System.Windows.Forms.DateTimePicker();
            this.startDate_ = new System.Windows.Forms.DateTimePicker();
            this.clearTz_ = new System.Windows.Forms.Button();
            this.unlock_ = new System.Windows.Forms.Button();
            this.lock_ = new System.Windows.Forms.Button();
            this.locklog_ = new System.Windows.Forms.RichTextBox();
            this.delete_ = new System.Windows.Forms.Button();
            this.eName_ = new System.Windows.Forms.ComboBox();
            this.setResult_ = new System.Windows.Forms.Button();
            this.result_ = new System.Windows.Forms.ComboBox();
            this.tzIndex_ = new System.Windows.Forms.ComboBox();
            this.reeze_ = new System.Windows.Forms.Button();
            this.reset = new System.Windows.Forms.Button();
            this.tj_BTN = new System.Windows.Forms.Button();
            this.log_ = new System.Windows.Forms.RichTextBox();
            this.archive_BTN = new System.Windows.Forms.Button();
            this.query_BTN = new System.Windows.Forms.Button();
            this.dateTimePicker_ = new System.Windows.Forms.DateTimePicker();
            this.ss_ = new AxMicrosoft.Office.Interop.Owc11.AxSpreadsheet();
            ((System.ComponentModel.ISupportInitialize)(this.dataitemView_)).BeginInit();
            this.bottomLayout_.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss_)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(93, 28);
            this.button1.TabIndex = 0;
            this.button1.Text = "提交";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // refresh_BTN
            // 
            this.refresh_BTN.Location = new System.Drawing.Point(111, 19);
            this.refresh_BTN.Name = "refresh_BTN";
            this.refresh_BTN.Size = new System.Drawing.Size(93, 28);
            this.refresh_BTN.TabIndex = 2;
            this.refresh_BTN.Text = "刷新";
            this.refresh_BTN.UseVisualStyleBackColor = true;
            this.refresh_BTN.Click += new System.EventHandler(this.refresh_BTN_Click);
            // 
            // dataitemView_
            // 
            this.dataitemView_.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataitemView_.Location = new System.Drawing.Point(1551, 12);
            this.dataitemView_.Name = "dataitemView_";
            this.dataitemView_.RowTemplate.Height = 23;
            this.dataitemView_.Size = new System.Drawing.Size(179, 633);
            this.dataitemView_.TabIndex = 3;
            // 
            // bottomLayout_
            // 
            this.bottomLayout_.Controls.Add(this.materialOwner_);
            this.bottomLayout_.Controls.Add(this.updateHistoryBTN_);
            this.bottomLayout_.Controls.Add(this.reportBTN_);
            this.bottomLayout_.Controls.Add(this.queryAllBTN_);
            this.bottomLayout_.Controls.Add(this.endDate_);
            this.bottomLayout_.Controls.Add(this.startDate_);
            this.bottomLayout_.Controls.Add(this.clearTz_);
            this.bottomLayout_.Controls.Add(this.unlock_);
            this.bottomLayout_.Controls.Add(this.lock_);
            this.bottomLayout_.Controls.Add(this.locklog_);
            this.bottomLayout_.Controls.Add(this.delete_);
            this.bottomLayout_.Controls.Add(this.eName_);
            this.bottomLayout_.Controls.Add(this.setResult_);
            this.bottomLayout_.Controls.Add(this.result_);
            this.bottomLayout_.Controls.Add(this.tzIndex_);
            this.bottomLayout_.Controls.Add(this.reeze_);
            this.bottomLayout_.Controls.Add(this.reset);
            this.bottomLayout_.Controls.Add(this.tj_BTN);
            this.bottomLayout_.Controls.Add(this.log_);
            this.bottomLayout_.Controls.Add(this.archive_BTN);
            this.bottomLayout_.Controls.Add(this.query_BTN);
            this.bottomLayout_.Controls.Add(this.dateTimePicker_);
            this.bottomLayout_.Controls.Add(this.refresh_BTN);
            this.bottomLayout_.Controls.Add(this.button1);
            this.bottomLayout_.Location = new System.Drawing.Point(3, 851);
            this.bottomLayout_.Name = "bottomLayout_";
            this.bottomLayout_.Size = new System.Drawing.Size(1727, 165);
            this.bottomLayout_.TabIndex = 4;
            // 
            // materialOwner_
            // 
            this.materialOwner_.Location = new System.Drawing.Point(1578, 21);
            this.materialOwner_.Name = "materialOwner_";
            this.materialOwner_.Size = new System.Drawing.Size(100, 21);
            this.materialOwner_.TabIndex = 22;
            // 
            // updateHistoryBTN_
            // 
            this.updateHistoryBTN_.Location = new System.Drawing.Point(1488, 19);
            this.updateHistoryBTN_.Name = "updateHistoryBTN_";
            this.updateHistoryBTN_.Size = new System.Drawing.Size(87, 23);
            this.updateHistoryBTN_.TabIndex = 21;
            this.updateHistoryBTN_.Text = "更新历史纪录";
            this.updateHistoryBTN_.UseVisualStyleBackColor = true;
            this.updateHistoryBTN_.Click += new System.EventHandler(this.updateHistoryBTN__Click);
            // 
            // reportBTN_
            // 
            this.reportBTN_.Location = new System.Drawing.Point(1578, 55);
            this.reportBTN_.Name = "reportBTN_";
            this.reportBTN_.Size = new System.Drawing.Size(75, 23);
            this.reportBTN_.TabIndex = 20;
            this.reportBTN_.Text = "财务";
            this.reportBTN_.UseVisualStyleBackColor = true;
            this.reportBTN_.Click += new System.EventHandler(this.reportBTN__Click);
            // 
            // queryAllBTN_
            // 
            this.queryAllBTN_.Location = new System.Drawing.Point(1488, 54);
            this.queryAllBTN_.Name = "queryAllBTN_";
            this.queryAllBTN_.Size = new System.Drawing.Size(75, 23);
            this.queryAllBTN_.TabIndex = 19;
            this.queryAllBTN_.Text = "查询";
            this.queryAllBTN_.UseVisualStyleBackColor = true;
            this.queryAllBTN_.Click += new System.EventHandler(this.queryAllBTN__Click);
            // 
            // endDate_
            // 
            this.endDate_.Location = new System.Drawing.Point(1366, 56);
            this.endDate_.Name = "endDate_";
            this.endDate_.Size = new System.Drawing.Size(116, 21);
            this.endDate_.TabIndex = 18;
            // 
            // startDate_
            // 
            this.startDate_.Location = new System.Drawing.Point(1256, 57);
            this.startDate_.Name = "startDate_";
            this.startDate_.Size = new System.Drawing.Size(104, 21);
            this.startDate_.TabIndex = 17;
            // 
            // clearTz_
            // 
            this.clearTz_.Location = new System.Drawing.Point(759, 76);
            this.clearTz_.Name = "clearTz_";
            this.clearTz_.Size = new System.Drawing.Size(75, 23);
            this.clearTz_.TabIndex = 16;
            this.clearTz_.Text = "清除投注";
            this.clearTz_.UseVisualStyleBackColor = true;
            this.clearTz_.Click += new System.EventHandler(this.clearTz__Click);
            // 
            // unlock_
            // 
            this.unlock_.Location = new System.Drawing.Point(672, 122);
            this.unlock_.Name = "unlock_";
            this.unlock_.Size = new System.Drawing.Size(65, 23);
            this.unlock_.TabIndex = 15;
            this.unlock_.Text = "解锁";
            this.unlock_.UseVisualStyleBackColor = true;
            this.unlock_.Click += new System.EventHandler(this.unlock__Click);
            // 
            // lock_
            // 
            this.lock_.Location = new System.Drawing.Point(579, 122);
            this.lock_.Name = "lock_";
            this.lock_.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lock_.Size = new System.Drawing.Size(65, 23);
            this.lock_.TabIndex = 14;
            this.lock_.Text = "锁定";
            this.lock_.UseVisualStyleBackColor = true;
            this.lock_.Click += new System.EventHandler(this.lock__Click);
            // 
            // locklog_
            // 
            this.locklog_.Location = new System.Drawing.Point(356, 57);
            this.locklog_.Name = "locklog_";
            this.locklog_.Size = new System.Drawing.Size(109, 96);
            this.locklog_.TabIndex = 13;
            this.locklog_.Text = "";
            // 
            // delete_
            // 
            this.delete_.Location = new System.Drawing.Point(1392, 19);
            this.delete_.Name = "delete_";
            this.delete_.Size = new System.Drawing.Size(75, 23);
            this.delete_.TabIndex = 12;
            this.delete_.Text = "删除";
            this.delete_.UseVisualStyleBackColor = true;
            this.delete_.Click += new System.EventHandler(this.delete__Click);
            // 
            // eName_
            // 
            this.eName_.FormattingEnabled = true;
            this.eName_.Location = new System.Drawing.Point(1255, 19);
            this.eName_.Name = "eName_";
            this.eName_.Size = new System.Drawing.Size(121, 20);
            this.eName_.TabIndex = 11;
            // 
            // setResult_
            // 
            this.setResult_.Location = new System.Drawing.Point(662, 76);
            this.setResult_.Name = "setResult_";
            this.setResult_.Size = new System.Drawing.Size(87, 23);
            this.setResult_.TabIndex = 10;
            this.setResult_.Text = "设置比赛结果";
            this.setResult_.UseVisualStyleBackColor = true;
            this.setResult_.Click += new System.EventHandler(this.setResult__Click);
            // 
            // result_
            // 
            this.result_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.result_.FormattingEnabled = true;
            this.result_.Location = new System.Drawing.Point(579, 76);
            this.result_.Name = "result_";
            this.result_.Size = new System.Drawing.Size(65, 20);
            this.result_.TabIndex = 9;
            // 
            // tzIndex_
            // 
            this.tzIndex_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tzIndex_.FormattingEnabled = true;
            this.tzIndex_.Location = new System.Drawing.Point(504, 76);
            this.tzIndex_.Name = "tzIndex_";
            this.tzIndex_.Size = new System.Drawing.Size(65, 20);
            this.tzIndex_.TabIndex = 9;
            // 
            // reeze_
            // 
            this.reeze_.Location = new System.Drawing.Point(3, 75);
            this.reeze_.Name = "reeze_";
            this.reeze_.Size = new System.Drawing.Size(75, 23);
            this.reeze_.TabIndex = 8;
            this.reeze_.Text = "冻结";
            this.reeze_.UseVisualStyleBackColor = true;
            this.reeze_.Click += new System.EventHandler(this.reeze__Click);
            // 
            // reset
            // 
            this.reset.Location = new System.Drawing.Point(850, 26);
            this.reset.Name = "reset";
            this.reset.Size = new System.Drawing.Size(75, 23);
            this.reset.TabIndex = 7;
            this.reset.Text = "清空";
            this.reset.UseVisualStyleBackColor = true;
            this.reset.Click += new System.EventHandler(this.reset_Click);
            // 
            // tj_BTN
            // 
            this.tj_BTN.Location = new System.Drawing.Point(228, 19);
            this.tj_BTN.Name = "tj_BTN";
            this.tj_BTN.Size = new System.Drawing.Size(93, 28);
            this.tj_BTN.TabIndex = 6;
            this.tj_BTN.Text = "统计";
            this.tj_BTN.UseVisualStyleBackColor = true;
            this.tj_BTN.Click += new System.EventHandler(this.tj_BTN_Click);
            // 
            // log_
            // 
            this.log_.Location = new System.Drawing.Point(941, 3);
            this.log_.Name = "log_";
            this.log_.Size = new System.Drawing.Size(308, 164);
            this.log_.TabIndex = 5;
            this.log_.Text = "";
            // 
            // archive_BTN
            // 
            this.archive_BTN.Location = new System.Drawing.Point(759, 26);
            this.archive_BTN.Name = "archive_BTN";
            this.archive_BTN.Size = new System.Drawing.Size(75, 23);
            this.archive_BTN.TabIndex = 4;
            this.archive_BTN.Text = "存档";
            this.archive_BTN.UseVisualStyleBackColor = true;
            this.archive_BTN.Click += new System.EventHandler(this.archive_BTN_Click);
            // 
            // query_BTN
            // 
            this.query_BTN.Location = new System.Drawing.Point(662, 26);
            this.query_BTN.Name = "query_BTN";
            this.query_BTN.Size = new System.Drawing.Size(75, 23);
            this.query_BTN.TabIndex = 4;
            this.query_BTN.Text = "查询";
            this.query_BTN.UseVisualStyleBackColor = true;
            this.query_BTN.Click += new System.EventHandler(this.query_BTN_Click);
            // 
            // dateTimePicker_
            // 
            this.dateTimePicker_.Location = new System.Drawing.Point(504, 27);
            this.dateTimePicker_.Name = "dateTimePicker_";
            this.dateTimePicker_.Size = new System.Drawing.Size(140, 21);
            this.dateTimePicker_.TabIndex = 3;
            // 
            // ss_
            // 
        //    this.ss_.DataSource = null;
            this.ss_.Enabled = true;
            this.ss_.Location = new System.Drawing.Point(1, 1);
            this.ss_.Name = "ss_";
            this.ss_.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("ss_.OcxState")));
            this.ss_.Size = new System.Drawing.Size(1551, 794);
            this.ss_.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1730, 1012);
            this.Controls.Add(this.ss_);
            this.Controls.Add(this.dataitemView_);
            this.Controls.Add(this.bottomLayout_);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.dataitemView_)).EndInit();
            this.bottomLayout_.ResumeLayout(false);
            this.bottomLayout_.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss_)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button refresh_BTN;
        private System.Windows.Forms.DataGridView dataitemView_;
        private System.Windows.Forms.Panel bottomLayout_;
        private System.Windows.Forms.DateTimePicker dateTimePicker_;
        private System.Windows.Forms.Button archive_BTN;
        private System.Windows.Forms.Button query_BTN;
        private System.Windows.Forms.RichTextBox log_;
        private System.Windows.Forms.Button tj_BTN;
        private AxMicrosoft.Office.Interop.Owc11.AxSpreadsheet ss_;
        private System.Windows.Forms.Button reset;
        private System.Windows.Forms.Button reeze_;
        private System.Windows.Forms.ComboBox result_;
        private System.Windows.Forms.ComboBox tzIndex_;
        private System.Windows.Forms.Button setResult_;
        private System.Windows.Forms.Button delete_;
        private System.Windows.Forms.ComboBox eName_;
        private System.Windows.Forms.RichTextBox locklog_;
        private System.Windows.Forms.Button unlock_;
        private System.Windows.Forms.Button lock_;
        private System.Windows.Forms.Button clearTz_;
        private System.Windows.Forms.Button reportBTN_;
        private System.Windows.Forms.Button queryAllBTN_;
        private System.Windows.Forms.DateTimePicker endDate_;
        private System.Windows.Forms.DateTimePicker startDate_;
        private System.Windows.Forms.Button updateHistoryBTN_;
        private System.Windows.Forms.TextBox materialOwner_;
    }
}

