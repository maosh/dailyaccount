using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Owc11;
using System.Collections;
namespace dailyAccount
{
    public partial class Form1 : Form
    {
        private string header = "序号	网站套次	网名	账号	资料人	负责人	IP地	网站总额	可打额度	充值	反水	第一次投注	赔率	赢亏	第二次投注	赔率	赢亏	第三次投注	赔率	赢亏	第四次投注	赔率	赢亏	第五次	赔率	赢亏	第六次	赔率	赢亏	第七次	赔率	赢亏	第八次	赔率	赢亏	第九次	赔率	赢亏	第十次	赔率	赢亏	第十一次	赔率	赢亏	一次提款	是否到	二次提款	是否到	剩余金额	盈亏	被扣	状态	备注";
        private string header_tj = "姓名	第1套	可打	第2套	可打	其他	三升	沙巴	365	总额";
        private Boolean isArchived = false;

        public  ArrayList users = new ArrayList();

        public Boolean IsLogin;
        public string SiteOwner;
        public int userClass;
        public int userID;
        private Request req_ = null;
        private DataTable info_ = null;
        private System.Timers.Timer getData_ = null;

        private string archiveDate;
        private int archiveTimes;

        public int[]tzStatus = new int[11];
        public Form1()
        {
            InitializeComponent();
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Range cells = ss_.Cells;

                LogUtl.info("解析表格。。。");
            DataTable dt = toDT(cells);
            LogUtl.info($"解析结果：共{dt.Rows.Count}");
            try
            {
                string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
                req_ = new Request(connStr);

                int nTotalRow = 0;
                int number = 0;

                //      req_.clear( SiteOwner);

                //userclass 不被锁定
                if (userClass == 0)
                {
                    for (int i = 0; i < 11; i++)
                    {
                        tzStatus[i] = 0;
                    }

                }else {

                getStatus();
                }
                foreach (DataRow dr in dt.Rows)
                {
                    item item_, item_db;
                    item_ = dr2ar(dr);
                    //   number = req_.add(item_);
                //    number = req_.add(item_, getitem(item_.User,item_.Id));
                    number = req_.add(item_, getitem(item_.User, item_.Id), userClass, tzStatus);
                    if (number == 1)
                    {
                        LogUtl.info($"添加：<{item_.Website}><{item_.Card_owner}>");
                  //      req_.clientSetStatus(item_.User, 1);
                    } else if(number == 2)
                    {
                        LogUtl.info($"更新：<{item_.Website}><{item_.Card_owner}>");
                  //      req_.clientSetStatus(item_.User, 1);
                    }
                    else
                    {
                        LogUtl.err($"添加失败：<{item_.Website}><{item_.Card_owner}>");
                    }
                }
        

                if (userClass == 1)
                {
                    req_.clientSetStatus(SiteOwner, 1);
                }
                else if (userClass == 0)
                {
                    req_.serverSetStatus("", 1);
                }

                setdeposite();

            }
            catch (Exception ex)
            {
                //LogUtl.err("异常", ex);
                //try
                //{
                //    db.Rollback();
                //    LogUtl.info("回滚：成功");
                //}
                //catch (Exception ex2)
                //{
                //    LogUtl.err("回滚：失败", ex2);
                //}
            }


        }

        private void SetHeader()
        {
            string tips = "XXXX	XXXX	XX	从	第三行	开始	提交	数据	此行	不	可用	从	第三行	开始	提交	数据	此行	不	可用	从	第三行	开始	提交	数据	此行	不	可用";
            ss_.Cells.ClearContents();
            (ss_.Cells[1, 1] as Range).ParseText(header, "\t");
            (ss_.Cells[1, 1] as Range).EntireRow.set_HorizontalAlignment(XlHAlign.xlHAlignCenter);
            (ss_.Cells[1, 1] as Range).EntireRow.Font.set_Color("red");
            (ss_.Cells[2, 1] as Range).ParseText(tips, "\t");


        }

        private void SetHeader_tj()
        {
            ss_.Cells.ClearContents();
            (ss_.Cells[1, 1] as Range).ParseText(header_tj, "\t");
            (ss_.Cells[1, 1] as Range).EntireRow.set_HorizontalAlignment(XlHAlign.xlHAlignCenter);
            (ss_.Cells[1, 1] as Range).EntireRow.Font.set_Color("red");

        }


        private void setContent()
        {

            string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
            req_ = new Request(connStr);
            DataTable dt = req_.getData(SiteOwner, userClass, users);
           int tempi =  dt.Rows.Count;
            SetHeader();

            setbalance();
            setnowmoney();


            int i = 1;
            string temp = null;
            ((Microsoft.Office.Interop.Owc11.Worksheet)this.ss_.Worksheets["Sheet1"]).Range["A1:zz65536"].CopyFromRecordset(ADONETtoADO.ConvertDataTableToRecordset(dt), 1000,100);
            //   (ss_.Cells[2, 1] as Range).CopyFromRecordset(ADONETtoADO.ConvertDataTableToRecordset(dt));
            (ss_.Cells[1, 1] as Range).ParseText(header, "\t");
            (ss_.Cells[2, 1] as Range).EntireRow.Insert();

            string tztitle = getTzTitle(1);
   
            (ss_.Cells[2, 1] as Range).ParseText(tztitle, "\t");
            (ss_.Cells[2, 1] as Range).EntireRow.ClearFormats();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            // 初始化日志
            var root = Path.GetDirectoryName(Application.ExecutablePath);
            var fileName = "log_" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            LogUtl.initLogger(root + "\\log\\" + fileName, log_);
            SetHeader();


         

            info_ = new DataTable();
            dataitemView_.DataSource = info_;

            DataColumn name = new DataColumn("姓名", typeof(string));
            info_.Columns.Add(name);

            DataColumn server = new DataColumn("服务器", typeof(string));
            info_.Columns.Add(server);
            DataColumn client = new DataColumn("用户", typeof(string));
            info_.Columns.Add(client);

            dataitemView_.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            getData_ = new System.Timers.Timer();
            getData_.Interval = 1 * 1000;
            getData_.Elapsed += GetData__Elapsed;
            getData_.Start();

            if (userClass != 0)
            {
                archive_BTN.Visible = false;
                reset.Visible = false;

                tzIndex_.Visible = false;
                result_.Visible = false;
                setResult_.Visible = false;
                eName_.Visible = false;
                delete_.Visible = false;

                locklog_.Visible = false;
                lock_.Visible = false;
                unlock_.Visible = false;
                clearTz_.Visible = false;


                
            }
            getArchiveFlag();
            getUserProfile();

            initDropList();

        }

        private void GetData__Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            getData_.Stop();
            string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];

            req_ = new Request(connStr);

            DataTable dt = new DataTable();
            dt = req_.selectAll("select * from notice");





            DoUIJob(new Action(() =>
            {

                dataitemView_.DataSource = dt;

            }));






        }

        private void DoUIJob(Action act, int timeout = 5 * 1000)
        {

            this.Invoke(new Action(() =>
            {   // 把UI线程获得的异常转移到用户线程
                try
                {
                    act();
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    getData_.Start();
                }

            }));

        }

        enum BetHead
        {
            begin = 0,
            id, websitetype, website, loginname, cardowner, user, iplocation, initmoney, avaimoney, deposit, rebate,
            tzmoney1, odds1, result1,
            tzmoney2, odds2, result2,
            tzmoney3, odds3, result3,
            tzmoney4, odds4, result4,
            tzmoney5, odds5, result5,
            tzmoney6, odds6, result6,
            tzmoney7, odds7, result7,
            tzmoney8, odds8, result8,
            tzmoney9, odds9, result9,
            tzmoney10, odds10, result10,
            tzmoney11, odds11, result11,

            withdraw1, wdresult1,
            withdraw2, wdresult2,
            nowmoney,

            balance, block, status, comment,
            end
        }

        private DataTable toDT(Range r)
        {
            DataTable dt = new DataTable();

            // 创建表头
            // 网站	网站套次	网名	网站用户名	负责人	IP地	昨总额	充值	充值备注	网站金额	下注1	赔率	赢亏	下注2	赔率	赢亏	下注3	赔率	赢亏	下注4	赔率	赢亏	下注5	赔率	赢亏	下注6	赔率	赢亏	一次提款	是否到	二次提款	是否到	余额
            for (int i = (int)BetHead.begin; i < (int)BetHead.end; i++)
            {
                DataColumn dc = new DataColumn(((BetHead)i).ToString());
                dt.Columns.Add(dc);
            }

            var CheckEmptyLine = new Func<Range, int, bool>((_r, nrow) =>
            {
                //序号
                string index = (_r[nrow, (int)BetHead.id] as Range)?.Text?.ToString();
                //类型
                string type = (_r[nrow, (int)BetHead.websitetype] as Range)?.Text?.ToString();
                // 网站
                string site = (_r[nrow, (int)BetHead.website] as Range)?.Text?.ToString();
                // 网名
                string account = (_r[nrow, (int)BetHead.loginname] as Range)?.Text?.ToString();
                // 网站资料人
                string cardOwner = (_r[nrow, (int)BetHead.cardowner] as Range)?.Text?.ToString();

                if (string.IsNullOrWhiteSpace(index)
                    ||string.IsNullOrWhiteSpace(type)
                    ||string.IsNullOrWhiteSpace(site)
                    || string.IsNullOrWhiteSpace(account)
                    || string.IsNullOrWhiteSpace(cardOwner))
                {
                    return true;
                }

                return false;
            });

            // Range (2, 1)->(n, m)
            // 1：Head，从2开始
            if(userClass == 0)
            {
             //   insert into game(account, initamount, memo) values('{account}',{ money}, '{(auto ? "自动" : "手动")}') ON DUPLICATE KEY UPDATE nowamount = { money}, memo = '{(auto ? "自动" : "手动")}'";

                DataRow dr = dt.NewRow();
                StringBuilder tztitle = new StringBuilder("insert into tztitle(id, title) value (0,  '");
                string temp = "";
                for (int col = 1; col < dt.Columns.Count; ++col)
                {
                    temp += (r[2, col] as Range)?.Text.ToString().Trim();
                    temp += "\t";
                   


                  

                }
                tztitle.Append(temp);
                tztitle.Append("') ON DUPLICATE KEY UPDATE title = ' ").Append(temp).Append("'");
                string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
                req_ = new Request(connStr);
                req_.update(tztitle.ToString());

            }


            int emptyLineCnt = 0;
            for (int row = 3; row > 0; row++) // 无限循环
            {
                if (CheckEmptyLine(r, row))
                {
                    if (emptyLineCnt == 4)
                    {
                        break;
                    }
                    else
                    {
                        emptyLineCnt++;
                        continue;
                    }
                }

                emptyLineCnt = 0;

                DataRow dr = dt.NewRow();
                for (int col = 1; col < dt.Columns.Count; ++col)
                {
                    if (col == (int)BetHead.website)
                    {
                        dr[col] = CorrectSite2((r[row, col] as Range)?.Text.ToString().Trim());
                    }
                    else
                    {
                        dr[col] = (r[row, col] as Range)?.Text.ToString().Trim();
                    }
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }

        static private string CorrectSite2(string site)
        {
            // 先移除 http://www. 或者 https://www.
            string _url = site;
            if (_url.StartsWith("http://"))
            {
                _url = _url.Substring("http://".Length);
            }
            if (_url.StartsWith("https://"))
            {
                _url = _url.Substring("https://".Length);
            }
            if (_url.StartsWith("www."))
            {
                _url = _url.Substring("www.".Length);
            }

            // 然后移除 最后的"/"
            int lastSlash = _url.IndexOf("/");
            if (lastSlash > 0)
            {
                _url = _url.Substring(0, lastSlash);
            }

            return _url.Trim();
        }

        private item dr2ar(DataRow dr)
        {
            item item_ =new item();
            item_.Id = Convert.ToInt32(dr[BetHead.id.ToString()]);
            item_.Websitetype = dr[BetHead.websitetype.ToString()] as string;
            item_.Website = dr[BetHead.website.ToString()] as string;
            item_.Loginname = dr[BetHead.loginname.ToString()] as string;
            item_.Card_owner = dr[BetHead.cardowner.ToString()] as string;
            if(userClass == 0)
            {
                item_.User = dr[BetHead.user.ToString()] as string;
            }
            else { item_.User = SiteOwner; }
        
            item_.Iplocation = dr[BetHead.iplocation.ToString()] as string;
            item_.Initmoney = Convert.ToInt32(dr[BetHead.initmoney.ToString()]);

            if (dr[BetHead.avaimoney.ToString()] as string == "" || dr[BetHead.avaimoney.ToString()] == DBNull.Value)
            { item_.Avaimoney = null; }
            else
            { item_.Avaimoney = Convert.ToInt32(dr[BetHead.avaimoney.ToString()]); }

            if (dr[BetHead.deposit.ToString()] as string == "" || dr[BetHead.deposit.ToString()] == DBNull.Value)
            { item_.Deposit = null; }
            else
            { item_.Deposit = Convert.ToInt32(dr[BetHead.deposit.ToString()]); }

            if (dr[BetHead.rebate.ToString()] as string == "" || dr[BetHead.rebate.ToString()] == DBNull.Value) { item_.Rebate = null; }
            else { item_.Rebate = Convert.ToInt32(dr[BetHead.rebate.ToString()]); }

            if (dr[BetHead.tzmoney1.ToString()] as string == "" || dr[BetHead.tzmoney1.ToString()] == DBNull.Value) { item_.Tzmoney1 = null; }
            else { item_.Tzmoney1 = Convert.ToInt32(dr[BetHead.tzmoney1.ToString()]); }
            if (dr[BetHead.odds1.ToString()] as string == "" || dr[BetHead.odds1.ToString()] == DBNull.Value) { item_.Odds1 = null; }
            else { item_.Odds1 = Convert.ToDouble(dr[BetHead.odds1.ToString()]); }
            if (dr[BetHead.result1.ToString()] as string == "" || dr[BetHead.result1.ToString()] == DBNull.Value) { item_.Result1 = null; }
            else { item_.Result1 = Convert.ToInt32(dr[BetHead.result1.ToString()]); }


            if (dr[BetHead.tzmoney2.ToString()] as string == "" || dr[BetHead.tzmoney2.ToString()] == DBNull.Value) { item_.Tzmoney2 = null; }
            else { item_.Tzmoney2 = Convert.ToInt32(dr[BetHead.tzmoney2.ToString()]); }
            if (dr[BetHead.odds2.ToString()] as string == "" || dr[BetHead.odds2.ToString()] == DBNull.Value) { item_.Odds2 = null; }
            else { item_.Odds2 = Convert.ToDouble(dr[BetHead.odds2.ToString()]); }
            if (dr[BetHead.result2.ToString()] as string == "" || dr[BetHead.result2.ToString()] == DBNull.Value) { item_.Result2 = null; }
            else { item_.Result2 = Convert.ToInt32(dr[BetHead.result2.ToString()]); }

            if (dr[BetHead.tzmoney3.ToString()] as string == "" || dr[BetHead.tzmoney3.ToString()] == DBNull.Value) { item_.Tzmoney3 = null; }
            else { item_.Tzmoney3 = Convert.ToInt32(dr[BetHead.tzmoney3.ToString()]); }
            if (dr[BetHead.odds3.ToString()] as string == "" || dr[BetHead.odds3.ToString()] == DBNull.Value) { item_.Odds3 = null; }
            else { item_.Odds3 = Convert.ToDouble(dr[BetHead.odds3.ToString()]); }
            if (dr[BetHead.result3.ToString()] as string == "" || dr[BetHead.result3.ToString()] == DBNull.Value) { item_.Result3 = null; }
            else { item_.Result3 = Convert.ToInt32(dr[BetHead.result3.ToString()]); }

            if (dr[BetHead.tzmoney4.ToString()] as string == "" || dr[BetHead.tzmoney4.ToString()] == DBNull.Value) { item_.Tzmoney4 = null; }
            else { item_.Tzmoney4 = Convert.ToInt32(dr[BetHead.tzmoney4.ToString()]); }
            if (dr[BetHead.odds4.ToString()] as string == "" || dr[BetHead.odds4.ToString()] == DBNull.Value) { item_.Odds4 = null; }
            else { item_.Odds4 = Convert.ToDouble(dr[BetHead.odds4.ToString()]); }
            if (dr[BetHead.result4.ToString()] as string == "" || dr[BetHead.result4.ToString()] == DBNull.Value) { item_.Result4 = null; }
            else { item_.Result4 = Convert.ToInt32(dr[BetHead.result4.ToString()]); }

            if (dr[BetHead.tzmoney5.ToString()] as string == "" || dr[BetHead.tzmoney5.ToString()] == DBNull.Value) { item_.Tzmoney5 = null; }
            else { item_.Tzmoney5 = Convert.ToInt32(dr[BetHead.tzmoney5.ToString()]); }
            if (dr[BetHead.odds5.ToString()] as string == "" || dr[BetHead.odds5.ToString()] == DBNull.Value) { item_.Odds5 = null; }
            else { item_.Odds5 = Convert.ToDouble(dr[BetHead.odds5.ToString()]); }
            if (dr[BetHead.result5.ToString()] as string == "" || dr[BetHead.result5.ToString()] == DBNull.Value) { item_.Result5 = null; }
            else { item_.Result5 = Convert.ToInt32(dr[BetHead.result5.ToString()]); }

            if (dr[BetHead.tzmoney6.ToString()] as string == "" || dr[BetHead.tzmoney6.ToString()] == DBNull.Value) { item_.Tzmoney6 = null; }
            else { item_.Tzmoney6 = Convert.ToInt32(dr[BetHead.tzmoney6.ToString()]); }
            if (dr[BetHead.odds6.ToString()] as string == "" || dr[BetHead.odds6.ToString()] == DBNull.Value) { item_.Odds6 = null; }
            else { item_.Odds6 = Convert.ToDouble(dr[BetHead.odds6.ToString()]); }
            if (dr[BetHead.result6.ToString()] as string == "" || dr[BetHead.result6.ToString()] == DBNull.Value) { item_.Result6 = null; }
            else { item_.Result6 = Convert.ToInt32(dr[BetHead.result6.ToString()]); }

            if (dr[BetHead.tzmoney7.ToString()] as string == "" || dr[BetHead.tzmoney7.ToString()] == DBNull.Value) { item_.Tzmoney7 = null; }
            else { item_.Tzmoney7 = Convert.ToInt32(dr[BetHead.tzmoney7.ToString()]); }
            if (dr[BetHead.odds7.ToString()] as string == "" || dr[BetHead.odds7.ToString()] == DBNull.Value) { item_.Odds7 = null; }
            else { item_.Odds7 = Convert.ToDouble(dr[BetHead.odds7.ToString()]); }
            if (dr[BetHead.result7.ToString()] as string == "" || dr[BetHead.result7.ToString()] == DBNull.Value) { item_.Result7 = null; }
            else { item_.Result7 = Convert.ToInt32(dr[BetHead.result7.ToString()]); }

            if (dr[BetHead.tzmoney8.ToString()] as string == "" || dr[BetHead.tzmoney8.ToString()] == DBNull.Value) { item_.Tzmoney8 = null; }
            else { item_.Tzmoney8 = Convert.ToInt32(dr[BetHead.tzmoney8.ToString()]); }
            if (dr[BetHead.odds8.ToString()] as string == "" || dr[BetHead.odds8.ToString()] == DBNull.Value) { item_.Odds8 = null; }
            else { item_.Odds8 = Convert.ToDouble(dr[BetHead.odds8.ToString()]); }
            if (dr[BetHead.result8.ToString()] as string == "" || dr[BetHead.result8.ToString()] == DBNull.Value) { item_.Result8 = null; }
            else { item_.Result8 = Convert.ToInt32(dr[BetHead.result8.ToString()]); }

            if (dr[BetHead.tzmoney9.ToString()] as string == "" || dr[BetHead.tzmoney9.ToString()] == DBNull.Value) { item_.Tzmoney9 = null; }
            else { item_.Tzmoney9 = Convert.ToInt32(dr[BetHead.tzmoney9.ToString()]); }
            if (dr[BetHead.odds9.ToString()] as string == "" || dr[BetHead.odds9.ToString()] == DBNull.Value) { item_.Odds9 = null; }
            else { item_.Odds9 = Convert.ToDouble(dr[BetHead.odds9.ToString()]); }
            if (dr[BetHead.result9.ToString()] as string == "" || dr[BetHead.result9.ToString()] == DBNull.Value) { item_.Result9 = null; }
            else { item_.Result9 = Convert.ToInt32(dr[BetHead.result9.ToString()]); }

            if (dr[BetHead.tzmoney10.ToString()] as string == "" || dr[BetHead.tzmoney10.ToString()] == DBNull.Value) { item_.Tzmoney10 = null; }
            else { item_.Tzmoney10 = Convert.ToInt32(dr[BetHead.tzmoney10.ToString()]); }
            if (dr[BetHead.odds10.ToString()] as string == "" || dr[BetHead.odds10.ToString()] == DBNull.Value) { item_.Odds10 = null; }
            else { item_.Odds10 = Convert.ToDouble(dr[BetHead.odds10.ToString()]); }
            if (dr[BetHead.result10.ToString()] as string == "" || dr[BetHead.result10.ToString()] == DBNull.Value) { item_.Result10 = null; }
            else { item_.Result10 = Convert.ToInt32(dr[BetHead.result10.ToString()]); }

            if (dr[BetHead.tzmoney11.ToString()] as string == "" || dr[BetHead.tzmoney11.ToString()] == DBNull.Value) { item_.Tzmoney11 = null; }
            else { item_.Tzmoney11 = Convert.ToInt32(dr[BetHead.tzmoney11.ToString()]); }
            if (dr[BetHead.odds11.ToString()] as string == "" || dr[BetHead.odds11.ToString()] == DBNull.Value) { item_.Odds11 = null; }
            else { item_.Odds11 = Convert.ToDouble(dr[BetHead.odds11.ToString()]); }
            if (dr[BetHead.result11.ToString()] as string == "" || dr[BetHead.result11.ToString()] == DBNull.Value) { item_.Result11 = null; }
            else { item_.Result11 = Convert.ToInt32(dr[BetHead.result11.ToString()]); }


            if (dr[BetHead.withdraw1.ToString()] as string == "" || dr[BetHead.withdraw1.ToString()] == DBNull.Value) { item_.Withdraw1 = null; }
            else {
             
                item_.Withdraw1 = Convert.ToInt32(dr[BetHead.withdraw1.ToString()]);
                
            }

         //   if (userClass == 0)
            {
                if (dr[BetHead.wdresult1.ToString()] as string == "" || dr[BetHead.wdresult1.ToString()] == DBNull.Value) { item_.Wdresult1 = null; }
                else { item_.Wdresult1 = dr[BetHead.wdresult1.ToString()] as string; }

                if (dr[BetHead.wdresult2.ToString()] as string == "" || dr[BetHead.wdresult2.ToString()] == DBNull.Value) { item_.Wdresult2 = null; }
                else { item_.Wdresult2 = dr[BetHead.wdresult2.ToString()] as string; }
            }

            if (dr[BetHead.withdraw2.ToString()] as string == "" || dr[BetHead.withdraw2.ToString()] == DBNull.Value) { item_.Withdraw2 = null; }
            else {
               
                item_.Withdraw2 = Convert.ToInt32(dr[BetHead.withdraw2.ToString()]); 
            }


            if (dr[BetHead.odds11.ToString()] as string == "" || dr[BetHead.odds11.ToString()] == DBNull.Value) { item_.Odds11 = null; }
            else { item_.Odds11 = Convert.ToDouble(dr[BetHead.odds11.ToString()]); }
            if (dr[BetHead.result11.ToString()] as string == "" || dr[BetHead.result11.ToString()] == DBNull.Value) { item_.Result11 = null; }
            else { item_.Result11 = Convert.ToInt32(dr[BetHead.result11.ToString()]); }

               if (dr[BetHead.nowmoney.ToString()] as string == "" || dr[BetHead.nowmoney.ToString()] == DBNull.Value) { item_.Nowmoney = null; }
               else { item_.Nowmoney = Convert.ToInt32(dr[BetHead.nowmoney.ToString()]); }

            if (dr[BetHead.balance.ToString()] as string == "" || dr[BetHead.balance.ToString()] == DBNull.Value) { item_.Balance = null; }
            else { item_.Balance = Convert.ToInt32(dr[BetHead.balance.ToString()]); }
            if (dr[BetHead.block.ToString()] as string == "" || dr[BetHead.block.ToString()] == DBNull.Value) { item_.Block = null; }
            else { item_.Block = Convert.ToInt32(dr[BetHead.block.ToString()]); }
            if (dr[BetHead.status.ToString()] as string == "" || dr[BetHead.status.ToString()] == DBNull.Value) { item_.Status = null; }
            else { item_.Status = Convert.ToInt32(dr[BetHead.status.ToString()]); }
            if (dr[BetHead.comment.ToString()] as string == "" || dr[BetHead.comment.ToString()] == DBNull.Value) { item_.Comment = null; }
            else { item_.Comment = dr[BetHead.comment.ToString()] as string; }



            return item_;

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            ss_.Width = this.Width - 24 - dataitemView_.Width;
            ss_.Height = this.ClientSize.Height - 180;
            

            dataitemView_.Left = ss_.Width;

   

            bottomLayout_.Left = 0;
            bottomLayout_.Top = this.ClientSize.Height - bottomLayout_.Height;
            
        }

        private void refresh_BTN_Click(object sender, EventArgs e)
        {
            DoUIJob(new Action(() =>
            {
                setContent();
               

                if (userClass == 1)
                {
                    req_.serverSetStatus(SiteOwner, 0);
                }
                else if (userClass == 0)
                {
                    req_.clientSetStatus("", 0);
                }
            }));



        }

        private string dr2string(DataRow dr)
        {
            item item_ = new item();
            string tabStr;
            tabStr = dr[BetHead.id.ToString()] + ",";
            tabStr += dr[BetHead.websitetype.ToString()] + ",";
            tabStr += dr[BetHead.website.ToString()] + ",";
            tabStr += dr[BetHead.loginname.ToString()] + ",";
            tabStr += dr[BetHead.cardowner.ToString()] + ",";
            tabStr += dr[BetHead.user.ToString()] + ",";

            tabStr += dr[BetHead.iplocation.ToString()] + ",";
            tabStr += dr[BetHead.initmoney.ToString()] + ",";
            tabStr += dr[BetHead.avaimoney.ToString()] + ",";
            tabStr += dr[BetHead.deposit.ToString()] + ",";
            tabStr += dr[BetHead.rebate.ToString()] + ",";



            tabStr += dr[BetHead.tzmoney1.ToString()] + ",";
            tabStr += dr[BetHead.odds1.ToString()] + ",";
            tabStr += dr[BetHead.result1.ToString()] + ",";

            tabStr += dr[BetHead.tzmoney2.ToString()] + ",";
            tabStr += dr[BetHead.odds2.ToString()] + ",";
            tabStr += dr[BetHead.result2.ToString()] + ",";

            tabStr += dr[BetHead.tzmoney3.ToString()] + ",";
            tabStr += dr[BetHead.odds3.ToString()] + ",";
            tabStr += dr[BetHead.result3.ToString()] + ",";

            tabStr += dr[BetHead.tzmoney4.ToString()] + ",";
            tabStr += dr[BetHead.odds4.ToString()] + ",";
            tabStr += dr[BetHead.result4.ToString()] + ",";

            tabStr += dr[BetHead.tzmoney5.ToString()] + ",";
            tabStr += dr[BetHead.odds5.ToString()] + ",";
            tabStr += dr[BetHead.result5.ToString()] + ",";

            tabStr += dr[BetHead.tzmoney6.ToString()] + ",";
            tabStr += dr[BetHead.odds6.ToString()] + ",";
            tabStr += dr[BetHead.result6.ToString()] + ",";

            tabStr += dr[BetHead.tzmoney7.ToString()] + ",";
            tabStr += dr[BetHead.odds7.ToString()] + ",";
            tabStr += dr[BetHead.result7.ToString()] + ",";

            tabStr += dr[BetHead.tzmoney8.ToString()] + ",";
            tabStr += dr[BetHead.odds8.ToString()] + ",";
            tabStr += dr[BetHead.result8.ToString()] + ",";

            tabStr += dr[BetHead.tzmoney9.ToString()] + ",";
            tabStr += dr[BetHead.odds9.ToString()] + ",";
            tabStr += dr[BetHead.result9.ToString()] + ",";

            tabStr += dr[BetHead.tzmoney10.ToString()] + ",";
            tabStr += dr[BetHead.odds10.ToString()] + ",";
            tabStr += dr[BetHead.result10.ToString()] + ",";

            tabStr += dr[BetHead.tzmoney11.ToString()] + ",";
            tabStr += dr[BetHead.odds11.ToString()] + ",";
            tabStr += dr[BetHead.result11.ToString()] + ",";

            tabStr += dr[BetHead.withdraw1.ToString()] + ",";
            tabStr += dr[BetHead.wdresult1.ToString()] + ",";
            tabStr += dr[BetHead.withdraw2.ToString()] + ",";
            tabStr += dr[BetHead.wdresult2.ToString()] + ",";


            tabStr += dr[BetHead.nowmoney.ToString()] + ",";



            tabStr += dr[BetHead.balance.ToString()] + ",";
            tabStr += dr[BetHead.block.ToString()] + ",";
            tabStr += dr[BetHead.status.ToString()] + ",";
            tabStr += dr[BetHead.comment.ToString()] ;





            return tabStr;

        }

        private string dr2string_tj(DataRow dr)
        {
            item item_ = new item();
            string tabStr;
            tabStr = dr["name"] + ",";
            tabStr += dr["1"] + ",";
            tabStr += dr["1a"] + ",";
            tabStr += dr["2"] + ",";
            tabStr += dr["2a"] + ",";
            tabStr += dr["3"] + ",";

         
            tabStr += dr["4"] + ",";
     
            tabStr += dr["5"] + ",";




            tabStr += dr["6"] + ",";
            tabStr += dr["total"] ;






            return tabStr;

        }

        private void query_BTN_Click(object sender, EventArgs e)
        {
            string dateStr = dateTimePicker_.Value.ToShortDateString();
            string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
            req_ = new Request(connStr);
            DataTable dt = req_.getHistoryData(SiteOwner, userClass, dateStr, "",users, "");
            SetHeader();
            int i = 1;

            ((Microsoft.Office.Interop.Owc11.Worksheet)this.ss_.Worksheets["Sheet1"]).Range["A1:zz65536"].CopyFromRecordset(ADONETtoADO.ConvertDataTableToRecordset(dt));
    
            (ss_.Cells[1, 1] as Range).ParseText(header, "\t");
            (ss_.Cells[2, 1] as Range).EntireRow.Insert();

            string tztitle = getTzTitle(-1);

            (ss_.Cells[2, 1] as Range).ParseText(tztitle, "\t");
            (ss_.Cells[2, 1] as Range).EntireRow.ClearFormats();
        }

        private void archive_BTN_Click(object sender, EventArgs e)
        {

            getArchiveFlag();
            string dateStr = dateTimePicker_.Value.ToShortDateString();
            if (archiveTimes == 0)
            {
                string temp = archiveDate.Replace("/0", "/");
                if (temp.Equals(dateStr))
                {
                    MessageBox.Show(dateStr + "已存在记录，请选择其他日期");
                    return;
                }else
                {
                    if(MessageBox.Show("该记录将第一次保存在 " + dateStr + "，继续保存?", "标题", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                {

                        archive(dateStr);

                    }
                 
                }
            }

            if(archiveTimes > 0)
            {
                string temp = archiveDate.Replace("/0", "/");
                if (!temp.Equals(dateStr))
                {
                    MessageBox.Show("数据已保存在" + temp + "，你选择的日期：" + dateStr+"日期冲突，请与数据库日期一致");
                    return;
                }
                else
                {
                    if (MessageBox.Show("该记录已保存"  + archiveTimes + "次，继续更新?", "标题", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                    {

                        archive(dateStr);

                    }

                }
            }
      
        }

        private void archive(string dateStr)
        {
            Range cells = ss_.Cells;
            DataTable dt = toDT(cells);
            try
            {
                string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
                req_ = new Request(connStr);

                int nTotalRow = 0;


                foreach (DataRow dr in dt.Rows)
                {
                    item item_;
                    item_ = dr2ar(dr);
                    req_.archive(item_, dateStr);
                    isArchived = true;
                }

                //    insert into tztitle_history(id, title, date)  select id, title, '2017' from tztitle on DUPLICATE key update title = (select title from tztitle where id = 0)
                StringBuilder sb = new StringBuilder();
                sb.Append("insert into tztitle_history  (id, title, date)  select id, title , '").Append(dateStr).Append("' from tztitle on DUPLICATE key update title = (select title from tztitle where id = 0) ");
                req_.update(sb.ToString());
                setArchiveFlag(dateStr, archiveTimes + 1);

            }

            catch (Exception ex)
            {

            }
        }

        private item getitem(string owner, int id)
        {
            item item_temp = new item();
            string dateStr = dateTimePicker_.Value.ToShortDateString();
            string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
            req_ = new Request(connStr);
            DataTable dt = req_.getItemData( owner, id);
            if(dt.Rows.Count == 1)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    item_temp = dr2ar(dr);
                    return item_temp;
                }
            }

            return item_temp;
        }

        private void tj_BTN_Click(object sender, EventArgs e)
        {
            string dateStr = dateTimePicker_.Value.ToShortDateString();
            string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
            req_ = new Request(connStr);
            setnowmoney();

            StringBuilder sql = new StringBuilder("select  ");
            sql.Append("IFNULL(user,'name') as name,");
            sql.Append("SUM(IF(websitetype = '1', init, 0)) as '111111',");
            sql.Append("SUM(IF(websitetype = '1', avail, 0)) as '1aaaaaa',");
            sql.Append("SUM(IF(websitetype = '2', init, 0)) as '222222',");
            sql.Append("SUM(IF(websitetype = '2', avail, 0)) as '2aaaaaa',");
            sql.Append("SUM(IF(websitetype = '3', init, 0)) as '333333',");

            sql.Append("SUM(IF(websitetype = '4', init, 0)) as '444444',");
  
            sql.Append("SUM(IF(websitetype = '5', init, 0)) as '555555',");

            sql.Append("SUM(IF(websitetype = '6', init, 0)) as '666666',");
   
            sql.Append("sum(init) as total_total ");
            //    sql.Append("from(select user, websitetype, sum(initmoney + ifnull(deposit, 0)) as init, ifnull(sum(avaimoney),0) as avail from dailyaccount GROUP BY  user, websitetype) as A GROUP BY user  order by field (`user` ,'杨东杰', '高国强', '杨飞', '郭科峰', '吴娜' , '聂俊勇', '张洪佳', '张孝猛', '申玉龙', '段龙辉', '王利强', '黄意','杨晓丹', '张朝生', '毛帅'); ");
            
            if (userClass == 0)
            {
                sql.Append("from(select user, websitetype, sum(nowmoney) as init, ifnull(sum(avaimoney),0) as avail from dailyaccount GROUP BY  user, websitetype) as A GROUP BY user  order by field (`user` ,'杨东杰', '高国强', '杨飞', '郭科峰', '吴娜' , '聂俊勇', '张洪佳', '张孝猛', '申玉龙', '段龙辉', '王利强', '黄意','杨晓丹', '张朝生', '毛帅'); ");
            }
            else
            {

                sql.Append("from(select user, websitetype, sum(nowmoney) as init, ifnull(sum(avaimoney),0) as avail from dailyaccount  where user = '");
                    sql.Append(SiteOwner);
                    sql.Append("' GROUP BY  user, websitetype) as A GROUP BY user  order by field (`user` ,'杨东杰', '高国强', '杨飞', '郭科峰', '吴娜' , '聂俊勇', '张洪佳', '张孝猛', '申玉龙', '段龙辉', '王利强', '黄意','杨晓丹', '张朝生', '毛帅'); ");

            }
            DataTable dt = req_.select(sql.ToString());
            SetHeader_tj();
            int i = 1;
            int a =0, aa =0 , b =0, bb =0 , c =0  , d =0 ,  f = 0,  g = 0, gg = 0;
            StringBuilder tempStr = new StringBuilder("总计,");
            //  dt.ImportRow();
            DataRow drt = dt.NewRow();
            drt[1] = 0; drt[2] = 0; drt[3] = 0; drt[4] = 0; drt[5] = 0; drt[6] = 0; drt[7] = 0; drt[8] = 0; drt[9] = 0;

            //    dt.ImportRow(drt);a{
            dt.Rows.Add(drt);
           
            ((Microsoft.Office.Interop.Owc11.Worksheet)this.ss_.Worksheets["Sheet1"]).Range["A1:zz65536"].CopyFromRecordset(ADONETtoADO.ConvertDataTableToRecordset(dt));
            (ss_.Cells[1, 1] as Range).ParseText(header_tj, "\t");



            foreach (DataRow dr in dt.Rows)
            {
                i++;
                a += Convert.ToInt32(dr["111111"]);
                aa += Convert.ToInt32(dr["1aaaaaa"]);

                b += Convert.ToInt32(dr["222222"]);
                bb += Convert.ToInt32(dr["2aaaaaa"]);

                c += Convert.ToInt32(dr["333333"]);
  

                d += Convert.ToInt32(dr["444444"]);
        

                f += Convert.ToInt32(dr["555555"]);


                g += Convert.ToInt32(dr["666666"]);
                gg += Convert.ToInt32(dr["total_total"]);


             

               
               
      

            }
            tempStr.Append(a).Append(",");
            tempStr.Append(aa).Append(",");
            tempStr.Append(b).Append(",");
            tempStr.Append(bb).Append(",");
            tempStr.Append(c).Append(",");


            tempStr.Append(d).Append(",");

            tempStr.Append(f).Append(",");
      
            tempStr.Append(g).Append(",");
            tempStr.Append(gg).Append(",");
            tempStr.Append("").Append(",");
            tempStr.Append(a + b + c + d + f + g).Append(",");

            (ss_.Cells[i , 1] as Range).ParseText(tempStr.ToString(), ",");
            (ss_.Cells[i, 1] as Range).EntireRow.Font.set_Color("red");

        

            ss_.Width = 10000;

        }

        private void Form1_Shown(object sender, EventArgs e)
        {

            ss_.Cells.Range["A1"].Activate();
        //    ss_.ActiveWindow.FreezePanes = true;
        }

        private void reset_Click(object sender, EventArgs e)
        {
            if(!isArchived)
            {
                MessageBox.Show("请先存档");
                return;
            }

          //  string dateStr = dateTimePicker_.Value.ToShortDateString();
            Range cells = ss_.Cells;
            DataTable dt = toDT(cells);
            try
            {
                string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
                req_ = new Request(connStr);

                int nTotalRow = 0;


                req_.update("update tztitle set title = '' ");
                foreach (DataRow dr in dt.Rows)
                {
                    item item_;
                    item_ = dr2ar(dr);
                    req_.clear();
                    isArchived = false;
                }

                setArchiveFlag(archiveDate, 0);

            }
            catch (Exception ex)
            {

            }
        }


        private void add_blankRow(int i)
        {
           
        }

        private void add_blankColumn(int i)
        {

        }

        private void freeze()
        {
         
        
        }

        private void reeze__Click(object sender, EventArgs e)
        {
            ss_.Cells.Range["H2"].Activate();

        //    ss_.ActiveSheet.Range("A1").EntireRow.Insert;


            if (ss_.ActiveWindow.FreezePanes == false) { 
            ss_.ActiveWindow.FreezePanes = true;
              
            }
            else
            {
                ss_.ActiveWindow.FreezePanes = false;
            }
        }

        private void setResult__Click(object sender, EventArgs e)
        {
            int tzi = -1; int result = -1;
           tzi =  tzIndex_.SelectedIndex;
            result = result_.SelectedIndex;
            string[,] temp = new string[11, 3]{
                { "tzmoney1", "odds1", "result1" },
{ "tzmoney2", "odds2", "result2" },
{ "tzmoney3", "odds3", "result3" },
{ "tzmoney4", "odds4", "result4" },
{ "tzmoney5", "odds5", "result5" },
{ "tzmoney6", "odds6", "result6" },
{ "tzmoney7", "odds7", "result7" },
{ "tzmoney8", "odds8", "result8" },
{ "tzmoney9", "odds9", "result9" },
{ "tzmoney10", "odds10", "result10" },
{ "tzmoney11", "odds11", "result11" } };

            double[] temp_wight = new double[] { 1, 0.5, 0, -0.5, -1 };


            if(tzi == -1 || result == -1)
            {
                MessageBox.Show("请选择场次或者结果");
                return;
            }

            string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
            req_ = new Request(connStr);
            double wight = temp_wight[result];


            StringBuilder sql = new StringBuilder("update dailyaccount set   ");
            sql.Append(temp[tzi,2]).Append("=").Append(temp[tzi,0]).Append("*").Append(wight);
            if(result <= 2)
            {
                sql.Append("*").Append(temp[tzi, 1]);
            };

            req_.update(sql.ToString());

            setbalance();
            setnowmoney();




        }

        private void initDropList()
        {
            tzIndex_.Items.Add("1");
            tzIndex_.Items.Add("2");
            tzIndex_.Items.Add("3");
            tzIndex_.Items.Add("4");
            tzIndex_.Items.Add("5");
            tzIndex_.Items.Add("6");
            tzIndex_.Items.Add("7");
            tzIndex_.Items.Add("8");
            tzIndex_.Items.Add("9");
            tzIndex_.Items.Add("10");
            tzIndex_.Items.Add("11");


            result_.Items.Add("全赢");
            result_.Items.Add("赢一半");
            result_.Items.Add("和局");
            result_.Items.Add("输一半");
            result_.Items.Add("全输");


            //eName_.Items.Add("杨东杰");
            //eName_.Items.Add("高国强");
            //eName_.Items.Add("杨飞");
            //eName_.Items.Add("郭科峰");
            //eName_.Items.Add("吴娜");
            //eName_.Items.Add("聂俊勇");
            //eName_.Items.Add("张洪佳");
            //eName_.Items.Add("张孝猛");
            //eName_.Items.Add("申玉龙");
            //eName_.Items.Add("段龙辉");
            //eName_.Items.Add("王利强");
            //eName_.Items.Add("黄意");
            //eName_.Items.Add("杨晓丹");
            //eName_.Items.Add("张朝生");
            //eName_.Items.Add("毛帅");

            foreach(user use in users)
            {
                if(use.Userclass > 0)
                {
                    eName_.Items.Add(use.Username);
                }
            }
    

        }



        private void setnowmoney()
        {
            string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
            req_ = new Request(connStr);



            StringBuilder sql = new StringBuilder("update dailyaccount set nowmoney = initmoney + ifnull(deposit,0) + ifnull(rebate,0)");
            //sql.Append(" + ifnull(result1,0-ifnull(tzmoney1,0))");
            //sql.Append(" + ifnull(result2,0-ifnull(tzmoney2,0))");
            //sql.Append(" + ifnull(result3,0-ifnull(tzmoney3,0))");
            //sql.Append(" + ifnull(result4,0-ifnull(tzmoney4,0))");
            //sql.Append(" + ifnull(result5,0-ifnull(tzmoney5,0))");
            //sql.Append(" + ifnull(result6,0-ifnull(tzmoney6,0))");
            //sql.Append(" + ifnull(result7,0-ifnull(tzmoney7,0))");
            //sql.Append(" + ifnull(result8,0-ifnull(tzmoney8,0) )");
            //sql.Append(" + ifnull(result9,0-ifnull(tzmoney9,0))");
            //sql.Append(" + ifnull(result10,0-ifnull(tzmoney10,0))");
            //sql.Append(" + ifnull(result11,0-ifnull(tzmoney11,0))");
            //sql.Append("-IF(LENGTH(wdresult1) > 0,withdraw1,0)");
            //sql.Append("-IF(LENGTH(wdresult2) > 0,withdraw2,0)");

            sql.Append(" + ifnull(result1,0)");
            sql.Append(" + ifnull(result2,0)");
            sql.Append(" + ifnull(result3,0)");
            sql.Append(" + ifnull(result4,0)");
            sql.Append(" + ifnull(result5,0)");
            sql.Append(" + ifnull(result6,0)");
            sql.Append(" + ifnull(result7,0)");
            sql.Append(" + ifnull(result8,0)");
            sql.Append(" + ifnull(result9,0)");
            sql.Append(" + ifnull(result10,0)");
            sql.Append(" + ifnull(result11,0)");
            sql.Append("-IF(LENGTH(wdresult1) > 0,withdraw1,0)");
            sql.Append("-IF(LENGTH(wdresult2) > 0,withdraw2,0)");










            req_.update(sql.ToString());


        }

        private void setbalance()
        {
            string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
            req_ = new Request(connStr);



            StringBuilder sql = new StringBuilder("update dailyaccount set balance =  ifnull(rebate,0)");
            sql.Append(" + ifnull(result1,0)");
            sql.Append(" + ifnull(result2,0)");
            sql.Append(" + ifnull(result3,0)");
            sql.Append(" + ifnull(result4,0)");
            sql.Append(" + ifnull(result5,0)");
            sql.Append(" + ifnull(result6,0)");
            sql.Append(" + ifnull(result7,0)");
            sql.Append(" + ifnull(result8,0)");
            sql.Append(" + ifnull(result9,0)");
            sql.Append(" + ifnull(result10,0)");
            sql.Append(" + ifnull(result11,0)");
        //    sql.Append("-IF(LENGTH(wdresult1) > 0,withdraw1,0)");
        //    sql.Append("-IF(LENGTH(wdresult2) > 0,withdraw2,0)");


            req_.update(sql.ToString());


        }

        private void clear()
        {
            ;
        }

        private void delete__Click(object sender, EventArgs e)
        {
            string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
            req_ = new Request(connStr);

            string name = eName_.SelectedItem.ToString();
            

            StringBuilder sql = new StringBuilder("delete from  dailyaccount where user = '");

            sql.Append(name);
            sql.Append("'");

            //  int num = req_.update(sql.ToString());




            if (MessageBox.Show("确定清空 "+name+"?","标题", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                int num = req_.update(sql.ToString());
                LogUtl.info($"删除：<{name}><{num}>");
            }



           
        }



        private void set_color(Range r, int count)
        {

            string name = (r[2, 6] as Range)?.Text.ToString().Trim();
            string newName = "";

          

            for (int i = 3; i < count+5; i++)
            {
                newName = (r[i, 6] as Range)?.Text.ToString().Trim();
                if (name.Equals(newName))
                {
                    (ss_.Cells[i, 1] as Range).EntireRow.Font.set_Color("black");
                }
                else
                {
                    name = newName;
                    (ss_.Cells[i, 1] as Range).EntireRow.Font.set_Color("red");
                }
           
            }
           
        }

        private void setStatus(int index, int value)
        {

            tzStatus[index] =value;


            string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
            req_ = new Request(connStr);

       


            string[] tz = new string[11] { "tz1status", "tz2status", "tz3status", "tz4status", "tz5status", "tz6status", "tz7status", "tz8status", "tz9status", "tz10status", "tz11status"};

            StringBuilder sql = new StringBuilder("update    tzstatus set ");
            sql.Append(tz[index]).Append("=").Append(value);


            req_.update(sql.ToString());

        }


        private void getStatus()
        {
            string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
            req_ = new Request(connStr);

            DataTable dt = req_.select("select * from tzstatus");

            foreach (DataRow req in dt.Rows)
            {
                tzStatus[0] = int.Parse(req["tz1status"].ToString());
                tzStatus[1] = int.Parse(req["tz2status"].ToString());
                tzStatus[2] = int.Parse(req["tz3status"].ToString());
                tzStatus[3] = int.Parse(req["tz4status"].ToString());
                tzStatus[4] = int.Parse(req["tz5status"].ToString());
                tzStatus[5] = int.Parse(req["tz6status"].ToString());
                tzStatus[6] = int.Parse(req["tz7status"].ToString());
                tzStatus[7] = int.Parse(req["tz8status"].ToString());
                tzStatus[8] = int.Parse(req["tz9status"].ToString());
                tzStatus[9] = int.Parse(req["tz10status"].ToString());
                tzStatus[10] = int.Parse(req["tz11status"].ToString());
  
            }
            string ss="";

            for(int i = 0; i < 11; i++)
            {
                int j = i + 1;
                if (tzStatus[i] == 1)
                    ss += j+", ";
            }
            locklog_.Text = ss;

        }

        private void lock__Click(object sender, EventArgs e)
        {
            locklog_.Text = "test";

            int tzi = -1;
            tzi = tzIndex_.SelectedIndex;
        


            if (tzi == -1)
            {
                MessageBox.Show("请选冻结场次");
                return;
            }


            setStatus(tzi, 1);
            getStatus();






        }

        private void unlock__Click(object sender, EventArgs e)
        {
            locklog_.Text = "unlock";


            int tzi = -1;
            tzi = tzIndex_.SelectedIndex;

            if (tzi == -1)
            {
                MessageBox.Show("请选解冻场次");
                return;
            }


            setStatus(tzi, 0);
            getStatus();
        }

        private void clearTz__Click(object sender, EventArgs e)
        {
            int tzi = -1; int result = -1;
            tzi = tzIndex_.SelectedIndex;
            result = result_.SelectedIndex;
            string[,] temp = new string[11, 3]{
                { "tzmoney1", "odds1", "result1" },
{ "tzmoney2", "odds2", "result2" },
{ "tzmoney3", "odds3", "result3" },
{ "tzmoney4", "odds4", "result4" },
{ "tzmoney5", "odds5", "result5" },
{ "tzmoney6", "odds6", "result6" },
{ "tzmoney7", "odds7", "result7" },
{ "tzmoney8", "odds8", "result8" },
{ "tzmoney9", "odds9", "result9" },
{ "tzmoney10", "odds10", "result10" },
{ "tzmoney11", "odds11", "result11" } };

            double[] temp_wight = new double[] { 1, 0.5, 0, -0.5, -1 };


            if (tzi == -1 )
            {
                MessageBox.Show("请选择场次");
                return;
            }

            string name = "";
            if (eName_.SelectedItem == null)
            {
                name = "所有人";
            }
            else { name = eName_.SelectedItem.ToString(); }
     
     

            string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
            req_ = new Request(connStr);
          //  double wight = temp_wight[result];


            StringBuilder sql = new StringBuilder("update dailyaccount set   ");
            sql.Append(temp[tzi, 0]).Append("= null ,").Append(temp[tzi, 1]).Append("= null ,").Append(temp[tzi, 2]).Append("= null ");
            if (!name.Equals("所有人"))
            {
                sql.Append(" where user = '").Append(name).Append("'");
            }

            int jj = tzi + 1;
            if (MessageBox.Show("确定清空 " + name + "的第 "+jj+" 次投注？", "标题", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                int num = req_.update(sql.ToString());
        

            }


        

            setbalance();
            setnowmoney();



        }

        private string getTzTitle(int index)
        {
            string ss ="";
        //    DataTable dt = req_.getData(SiteOwner, userClass);
            DataTable dt ;
            if (index > 0) {
            dt = req_.select("select * from tztitle");
            }
            else
            {

                string dateStr = dateTimePicker_.Value.ToShortDateString();
                dt = req_.select($"select * from tztitle_history where date = '{dateStr}' ");
            }

            foreach (DataRow req in dt.Rows)
            {
                ss = req["title"].ToString();


            }
            return ss;
        }

        private void getUserProfile()
        {
            string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];

            req_ = new Request(connStr);

            DataTable dt = new DataTable();
            dt = req_.selectAll("select * from user, userprofile where user.userid = userprofile.userid order by userprofile.location");

            foreach(DataRow dr in dt.Rows)
            {
                user us = new user();
                us.Userid = int.Parse(dr["userid"].ToString());
                us.Username = dr["username"].ToString();
                us.Location = int.Parse(dr["location"].ToString());
                us.Commission = float.Parse(dr["commission"].ToString());
                us.Persion365com = float.Parse(dr["persion365com"].ToString());
                us.Comp365com = float.Parse(dr["comp365com"].ToString());
                us.Userclass = int.Parse(dr["class"].ToString());
                users.Add(us);
            }

        }

        private void queryAllBTN__Click(object sender, EventArgs e)
        {

            string name;
            if (eName_.SelectedItem == null)
            {
                name = "";
            }
            else { name = eName_.SelectedItem.ToString(); }

            string startDateStr = startDate_.Value.ToShortDateString();
            string endDateStr = endDate_.Value.ToShortDateString();
            string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
            req_ = new Request(connStr);
            DataTable dt = req_.getHistoryData(SiteOwner, userClass, startDateStr,endDateStr, users, name);
            SetHeader();
            int i = 1;

            ((Microsoft.Office.Interop.Owc11.Worksheet)this.ss_.Worksheets["Sheet1"]).Range["A1:zz65536"].CopyFromRecordset(ADONETtoADO.ConvertDataTableToRecordset(dt));

            (ss_.Cells[1, 1] as Range).ParseText(header, "\t");
            (ss_.Cells[2, 1] as Range).EntireRow.Insert();

            string tztitle = getTzTitle(-1);

            (ss_.Cells[2, 1] as Range).ParseText(tztitle, "\t");
            (ss_.Cells[2, 1] as Range).EntireRow.ClearFormats();
        
    }

        private void setArchiveFlag(string date, int times)
        {
            string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];

            req_ = new Request(connStr);


            DataTable dt = new DataTable();
            req_.update($"update archiveflag set date = '{date}' , times = {times}");

        }
        private void getArchiveFlag()
        {
            string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];

            req_ = new Request(connStr);


            DataTable dt = new DataTable();
            dt = req_.select($"select DATE_FORMAT(date,'%Y/%m/%d') date, times from archiveflag");
            foreach(DataRow dr in dt.Rows)
            {
                archiveDate = dr["date"].ToString();
                archiveTimes = int.Parse(dr["times"].ToString());
            }
        }


        private void setdeposite()
        {
            getArchiveFlag();
            if (archiveTimes > 0)
            {
                return;
            }
            int lastMoney;
            int nowMonwy;
            int rebate;
            int index;
            string sql;
            sql = $"select sum(nowMoney) money from dailyaccount_history where user = '{SiteOwner}' and date = '{archiveDate}'";
            DataTable dt = new DataTable();
            dt = req_.select(sql );
            lastMoney = int.Parse(dt.Rows[0]["money"].ToString());

            sql = $"select sum(initmoney) money from dailyAccount where user = '{SiteOwner}'";
            dt = req_.select(sql);
            nowMonwy = int.Parse(dt.Rows[0]["money"].ToString());

            rebate = nowMonwy - lastMoney;

            sql = $"select id  from dailyAccount where user = '{SiteOwner}' order by id desc limit 1";
            dt = req_.select(sql);
            if(dt.Rows.Count > 0) { 
            index = int.Parse(dt.Rows[0]["id"].ToString());

            sql = $"update dailyAccount set rebate = {rebate}  where id = {index} and user ='{SiteOwner}'";

                req_.update(sql);
            }

        }

    }
}
