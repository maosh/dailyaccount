using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dailyAccount
{
    public partial class Login : Form
    {
        private Form1 MainFrame_ = null;
        private Request req_ = null;

        private string version = "1.0.2";
        public Login(Form1 mainFrame)
        {
            MainFrame_ = mainFrame;
            InitializeComponent();
        }


        private void login_BTN_Click(object sender, EventArgs e)
        {

            string acc = acc_.Text.Trim();
            string pwdOrigin = pwd_.Text.Trim();

            string pwd = Encrypt.MD5(pwdOrigin);

          //  string connStr = ConfigurationManager.AppSettings["AccountConnectionString"];
          //  DBTransaction db = new DBTransaction(connStr);
            string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
            req_ = new Request(connStr);
            if (!req_.valid())
            {
                MessageBox.Show("数据库连接失败！");
            }

                DataTable dt = null;
            if (req_.version(version, out dt))
            {

            }else
            {

                MessageBox.Show("请使用最新版！");
                return;
            }
             dt = null;
            if (req_.Login(acc, pwd,out dt))
            {
                // 保存账号密码
                Write2Config("Account", acc);
                Write2Config("Password", pwdOrigin);

                MainFrame_.IsLogin = true;
                MainFrame_.SiteOwner = acc;
                MainFrame_.userID = Convert.ToInt32( dt.Rows[0]["userid"]);
                MainFrame_.userClass = Convert.ToInt32(dt.Rows[0]["class"]);
                                 


                this.Close();
            }
            else
            {
                MessageBox.Show(this, "登录失败，请检查账号密码");
            }
        }

        private void cancel__Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            // 加载账号密码

      
        }

        private void Add2Config(Configuration config, string key, string value)
        {
            if (config.AppSettings.Settings[key] == null)
            {
                config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                config.AppSettings.Settings[key].Value = value;
            }
        }
        private void Write2Config(string key, string value)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[key] == null)
            {
                config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                config.AppSettings.Settings[key].Value = value;
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void 取消_Click(object sender, EventArgs e)
        {
            ChangePWD cpwd = new ChangePWD();//实例化窗体
            cpwd.Show();//将窗体显示出来
           
        }

        private void Login_Load_1(object sender, EventArgs e)
        {
            var acc = System.Configuration.ConfigurationManager.AppSettings["Account"];
            var pwd = System.Configuration.ConfigurationManager.AppSettings["Password"];
            acc_.Text = acc;
            pwd_.Text = pwd;
        }

        private Boolean version_vailde()
        {

            return true;
        }
    }
}
