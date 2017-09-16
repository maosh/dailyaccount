using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dailyAccount
{
    public partial class charge : Form
    {
        private DataTable info_ = null;
        private System.Timers.Timer getData_ = null;
        private Request req_ = null;
        int uc;
        int eid;

        int queryStatus = 0;
        public charge(int userClass, int id)
        {
            uc = userClass;
            eid = id;
            InitializeComponent();
        }

        private void query__Click(object sender, EventArgs e)
        {
            queryStatus = 1;
            getData_.Stop();
       //     dataitemView_.Rows.Clear();
            string DateStr = chargeTimePicks_.Value.ToShortDateString();
            string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];

            req_ = new Request(connStr);

            DataTable dt = new DataTable();
            StringBuilder sql = new StringBuilder();
            sql.Append("select userid id, username 姓名, sum(ifnull(deposit,0)) 充值, apply-ifnull(sent,0)  仍需, sent 已转, back 退回, backok 已确认 , sent +backok -sum(ifnull(deposit,0)) 余留 from dailyaccount right join  (select sum(ifnull(apply,0)) apply, sum(ifnull(sent,0))sent, sum(ifnull(back,0)) back, sum(if(length(comment)>0, back,0))   backok,    userid, `user`.username username from deposit_history right join user  on  deposit_history.eid = `user`.userid where date='");
            sql.Append(DateStr);
            sql.Append("' group BY userid) as tablea on dailyaccount.`user` = tablea.username ");
            
   
            // select userid, username, sum(deposit), apply, sent, back from dailyaccount right join  (select sum(apply) apply, sum(sent)sent, sum(back) back, userid, `user`.username username from deposit right join user  on deposit.eid = `user`.userid group BY userid) as tablea on dailyaccount.`user` = tablea.username    GROUP BY userid;




            if (uc > 0)
            {
                sql.Append(" where userid = ");
                sql.Append(eid);
            }
            sql.Append(" GROUP BY userid;");
            dt = req_.selectAll(sql.ToString());
           
            dataitemView_.DataSource = dt;
        }

        private void charge_Load(object sender, EventArgs e)
        {
            info_ = new DataTable();
            dataitemView_.DataSource = info_;
            amount_.Font = new Font(amount_.Font.FontFamily, 20, amount_.Font.Style);
            // 添加行
            DataColumn id = new DataColumn("id", typeof(string));
            info_.Columns.Add(id);
            DataColumn name = new DataColumn("姓名", typeof(string));
            info_.Columns.Add(name);
            DataColumn number = new DataColumn("已账", typeof(string));
            info_.Columns.Add(number);
            DataColumn depos = new DataColumn("充值", typeof(string));
            info_.Columns.Add(depos);

            DataColumn apply = new DataColumn("仍需", typeof(string));
            info_.Columns.Add(apply);
            DataColumn back = new DataColumn("退回", typeof(string));
            info_.Columns.Add(back);

            DataColumn backok = new DataColumn("已确认", typeof(string));
            info_.Columns.Add(backok);

            DataColumn remaind = new DataColumn("余留", typeof(string));
            info_.Columns.Add(remaind);

            //     getData();

            getData_ = new System.Timers.Timer();
            getData_.Interval = 1 * 1000;
            getData_.Elapsed += GetData__Elapsed;
            getData_.Start();
        }

        private void GetData__Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            getData_.Stop();
            string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];

            req_ = new Request(connStr);

            DataTable dt = new DataTable();
            StringBuilder sql = new StringBuilder();
            sql.Append("select userid id, username 姓名, sum(ifnull(deposit,0)) 充值, apply-ifnull(sent,0)  仍需, sent 已转, back 退回, backok 已确认 , sent +backok -sum(ifnull(deposit,0)) 余留 from dailyaccount right join  (select sum(ifnull(apply,0)) apply, sum(ifnull(sent,0))sent, sum(ifnull(back,0)) back, sum(if(length(comment)>0, back,0))   backok,    userid, `user`.username username from deposit right join user  on  deposit.eid = `user`.userid group BY userid) as tablea on dailyaccount.`user` = tablea.username   ");
           // select userid, username, sum(deposit), apply, sent, back from dailyaccount right join  (select sum(apply) apply, sum(sent)sent, sum(back) back, userid, `user`.username username from deposit right join user  on deposit.eid = `user`.userid group BY userid) as tablea on dailyaccount.`user` = tablea.username    GROUP BY userid;


          
                
           if(uc > 0)
            {
                sql.Append(" where userid = ");
                sql.Append(eid);
            }
                sql.Append(" GROUP BY userid;");
                dt = req_.selectAll(sql.ToString());




            DoUIJob(new Action(() =>
            {

                dataitemView_.DataSource = dt;
                int number = 0;
                int i = 0;
                foreach (DataRow req in dt.Rows)
                {
                    number += int.Parse(req["金额"].ToString());
                    i++;
                }
                //     string numstr = 
                amount_.Text = number.ToString("C");
          //      itemCount_.Text = i.ToString() + "条记录";
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

        private void dataitemView__CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ss = dataitemView_.CurrentRow.Cells["id"].Value.ToString();
            string name = dataitemView_.CurrentRow.Cells["姓名"].Value.ToString();
            string date = "";
            if (!ss.Equals(""))
            {
                if(queryStatus == 1)
                {
                    date = chargeTimePicks_.Value.ToShortDateString();
                }
                detail dt = new detail(uc,int.Parse(ss),name, date);

                dt.Show();
            }
        }

        private void refresh__Click(object sender, EventArgs e)
        {
            queryStatus = 0;
            getData_.Start();
        }
    }


}
