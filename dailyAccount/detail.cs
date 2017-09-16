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
    public partial class detail : Form
    {
        private DataTable info_ = null;
        private Request req_ = null;
        private int id;
        private int selectedId;
        private int uc;
        string dateStr;
        private string eName;
        public detail(int userclass,int eid, string name, string date)
        {
            uc = userclass;
            id = eid;
            dateStr = date;
            this.eName = name;
            this.Text = name;
            
            InitializeComponent();
        }

        private void commit_Click(object sender, EventArgs e)
        {
            string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];

            req_ = new Request(connStr);
            int number = int.Parse(amount.Text);
            if (MessageBox.Show("确定提交:" + eName+"金额："+ number, "   标题", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                if (uc == 0)
                {
                    req_.updateNumber(id, number, 0, 0);
                }
                else
                {
                    if (number > 0)
                    {
                        req_.updateNumber(id, 0, number, 0);
                    }
                    else
                    {
                        req_.updateNumber(id, 0, 0, number);
                    }
                }

                StringBuilder sql = new StringBuilder();
           
                sql.Append("select a.id, b.username 姓名, a.apply 申请, a.sent 已转, a.back 退回, a.comment 备注, a.time 时间 from deposit a, user b  where a.eid = b.userid and a.eid = ").Append(id);
                DataTable dt = new DataTable();
                dt = req_.selectAll(sql.ToString());

                dataGridView1.DataSource = dt;

            }

       
         
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try { 
            string ss = dataGridView1.CurrentRow.Cells["id"].Value.ToString();
            int back = int.Parse(dataGridView1.CurrentRow.Cells["退回"].Value.ToString());
            string comment = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            if(back <0 && comment.Length == 0)
            {

                if (MessageBox.Show("收到该笔回款?" +back, "标题", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                {
                    string sql1 = $"update deposit set comment='ok' where id={ss}";
                    req_.update(sql1);

                }


                StringBuilder sql = new StringBuilder();
                //    sql.Append("select a.id, b.username, a.sent, a.back, a.time from deposit a, user b  where a.eid = b.userid and a.eid = ").Append(id);
                sql.Append("select a.id, b.username 姓名, a.apply 申请, a.sent 已转, a.back 退回, a.comment 备注, a.time 时间 from deposit a, user b  where a.eid = b.userid and a.eid = ").Append(id);
                DataTable dt = new DataTable();
                dt = req_.selectAll(sql.ToString());

                dataGridView1.DataSource = dt;

            }
            }
            catch (Exception ex)
            {

            }





        }

        private void detail_Load(object sender, EventArgs e)
        {

            this.Text = eName;
            StringBuilder sql = new StringBuilder();

            info_ = new DataTable();


            // 添加行
            DataColumn name = new DataColumn("ID", typeof(string));
            info_.Columns.Add(name);
            DataColumn number = new DataColumn("eid", typeof(string));
            info_.Columns.Add(number);

            DataColumn apply = new DataColumn("apply", typeof(string));
            info_.Columns.Add(apply);

            DataColumn amount = new DataColumn("amount", typeof(int));
            info_.Columns.Add(amount);
            DataColumn comment = new DataColumn("comment", typeof(int));
            info_.Columns.Add(comment);
            DataColumn time = new DataColumn("time", typeof(string));
            info_.Columns.Add(time);
            dataGridView1.DataSource = info_;
            string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];

            req_ = new Request(connStr);
            if (dateStr.Equals("")) { 
            sql.Append("select a.id, b.username 姓名, a.apply 申请, a.sent 已转, a.back 退回, a.comment 备注, a.time 时间 from deposit a, user b  where a.eid = b.userid and a.eid = ").Append(id);
            }
            else
            {
                sql.Append("select a.id, b.username 姓名, a.apply 申请, a.sent 已转, a.back 退回, a.comment 备注, a.time 时间 from deposit_history a, user b  where a.eid = b.userid and a.eid = ").Append(id).Append(" and  a.date = '").Append(dateStr).Append("'");
            }
            DataTable dt = new DataTable();
            dt = req_.selectAll(sql.ToString());

            dataGridView1.DataSource = dt;
        }

        private void delete_Click(object sender, EventArgs e)
        {
            string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];

            req_ = new Request(connStr);



            if (MessageBox.Show("确定删除 id: " + selectedId + "这条充值信息?", "标题", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("delete from deposit where id = ").Append(selectedId);
                int num = req_.update(sql.ToString());

                sql.Clear();
                sql.Append("select a.id, b.username, a.amount, a.comment, a.time from deposit a, user b  where a.eid = b.userid and a.eid = ").Append(id);
                DataTable dt = new DataTable();
                dt = req_.selectAll(sql.ToString());

                dataGridView1.DataSource = dt;

            }
        }
    }
}
