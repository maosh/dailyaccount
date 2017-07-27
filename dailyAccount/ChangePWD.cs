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
    public partial class ChangePWD : Form
    {

        private Request req_ = null;
        public ChangePWD()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
            req_ = new Request(connStr);
            req_.ChangePWD(acc_.Text, Encrypt.MD5(oldPwd_.Text.Trim()), Encrypt.MD5(newPwd_.Text.Trim()));


        }
    }
}
