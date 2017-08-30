using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;

namespace dailyAccount
{
    class Request
    {
        private string connectStr_ = null;
        public Request(string connectionStr)
        {
            connectStr_ = connectionStr;
        }

        public bool valid()
        {
            bool ret = true;
            string sql = "select count(*) from caculator";

            try
            {
                var dt = select(sql);
            }
            catch
            {
                ret = false;
            }

            return ret;
        }

        public DataTable select(string sql)
        {
            var conn = new MySql.Data.MySqlClient.MySqlConnection(connectStr_);
            var adp = new MySql.Data.MySqlClient.MySqlDataAdapter(sql, conn);

            DataTable dt = new DataTable();
            adp.Fill(dt);

            return dt;
        }

        public int update(string sql)
        {
            var conn = new MySql.Data.MySqlClient.MySqlConnection(connectStr_);
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;

            int lines = 0;
            try
            {
                conn.Open();
                lines = cmd.ExecuteNonQuery();
                if (lines == 0)
                {
                  //  System.Windows.Forms.MessageBox.Show("插入数据异常！");
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(sql + "用户名，余额 插入数据库失败！");
            }
            finally
            {
                try
                {
                    conn.Close();
                }
                catch
                {

                }
            }

            return lines;
        }

        public int count(string sql)
        {
            DataTable dt = select(sql);

            return Convert.ToInt32(dt.Rows[0][0]); ;
        }

        public DataTable @get(string account)
        {
            string sql = $"select * from game where account='{account}' and sstat=0";

            return select(sql);
        }

        public int updateMoney(string account, string money, bool auto)
        {


            //"if exists(select * from game where account = '" & mAccount & "') update game set nowamount = " & mMoney & " where account = '" & mAccount & "' else insert into game (account, initamount) VALUES ('" & mAccount & "'," & mMoney & ") "
            //  string sql = $"if exists(select * from game where account = '{account}') update game set nowamount = {money}, memo='{(auto ? "自动" : "手动")}' where account = '{account}' else insert into game (account, initamount, memo) VALUES ('{account}',{money}, '{(auto ? "自动" : "手动")}') ";
            string sql = $"insert into game(account, initamount, memo) values('{account}',{money}, '{(auto ? "自动" : "手动")}') ON DUPLICATE KEY UPDATE nowamount = {money}, memo = '{(auto ? "自动" : "手动")}'";
            return update(sql);
        }

        public int updateOrderResult(string account, bool success, string money, string odds, string winMoney, string siteReturnedMsg = "", string memo = "")
        {
            //vSQL = "update game set sstat='2',betamount=" & mUpMoney & ",stime=getdate(),rtext='网站返回结果字符串',memo='备注，想写点啥都行', odds = " & odds & ", winamount =" & winMoney & "where account='" & mAccount & "'"
            string sql = $"update game set sstat='{(success ? "2" : "3")}',betamount={(string.IsNullOrWhiteSpace(money) ? "null" : money)},stime=now(),rtext='{siteReturnedMsg}',memo='{memo}', odds = {(string.IsNullOrWhiteSpace(odds) ? "null" : odds)}, winamount ={(string.IsNullOrWhiteSpace(winMoney) ? "null" : winMoney)} where account='{account}'";


            return update(sql);
        }

        public int updateNumber(string name, int number)
        {
            string sql = $"insert into  caculator ( name, number) values('{name}','{number}') ON DUPLICATE KEY UPDATE number='{number}'";
            return update(sql);
        }

        private int delete (int id)
        {
            string sql = $"delete from  dailyaccount where user = '{id}'";
            return update(sql);
        }
        private int insert()
        {
            string sql = $"insert into dailyaccount";
            return update(sql);
        }

        public int clear(string user)
        {
            string sql = $"delete from  dailyaccount where user = '{user}'";
            return update(sql);
        }

        public int clear()
        {
            string sql = $"delete from  dailyaccount";
            return update(sql);
        }

        public DataTable getData(string user, int userClass, ArrayList userList)
        {
            string sql;
            string sql1;
            if(userClass == 1)
            {
                sql = $"select * from  dailyaccount where user = '{user}' order by id  ";
                sql1 = $"select '', '','','','', '{user}','',sum(initmoney), sum(avaimoney), sum(deposit), sum(rebate), sum(tzmoney1), sum(result1) / sum(tzmoney1), sum(result1),sum(tzmoney2), sum(result2) / sum(tzmoney2), sum(result2),sum(tzmoney3), sum(result3) / sum(tzmoney3), sum(result3),sum(tzmoney4), sum(result4) / sum(tzmoney4), sum(result4),sum(tzmoney5), sum(result5) / sum(tzmoney5), sum(result5),sum(tzmoney6), sum(result6) / sum(tzmoney6), sum(result6),sum(tzmoney7), sum(result7) / sum(tzmoney7), sum(result7),sum(tzmoney8), sum(result8) / sum(tzmoney8), sum(result8),sum(tzmoney9), sum(result9) / sum(tzmoney9), sum(result9),sum(tzmoney10), sum(result10) / sum(tzmoney10), sum(result10),sum(tzmoney11), sum(result11) / sum(tzmoney11), sum(result11), sum(withdraw1),'',sum(withdraw2),'', sum(nowmoney), sum(balance), sum(balance),'','' from dailyaccount where user = '{user}';";
            }
            else
            {
                string temp="";
             
                foreach (user us in userList)
                {
                    temp += ",'";
                    temp += us.Username;
                    temp += "'";
                }
                //     sql = $"select * from  dailyaccount  order by field (`user` ,'杨东杰', '高国强', '杨飞', '郭科峰', '吴娜' , '聂俊勇', '张洪佳', '张孝猛', '申玉龙', '段龙辉', '王利强', '黄意','杨晓丹', '张朝生', '毛帅') , id";

                sql = "select * from  dailyaccount  order by field (`user`";
                sql += temp;
                sql +=") , id";



                sql1 = $"select '', '','','','', '{user}','',sum(initmoney), sum(avaimoney), sum(deposit), sum(rebate), sum(tzmoney1), sum(result1) / sum(tzmoney1), sum(result1),sum(tzmoney2), sum(result2) / sum(tzmoney2), sum(result2),sum(tzmoney3), sum(result3) / sum(tzmoney3), sum(result3),sum(tzmoney4), sum(result4) / sum(tzmoney4), sum(result4),sum(tzmoney5), sum(result5) / sum(tzmoney5), sum(result5),sum(tzmoney6), sum(result6) / sum(tzmoney6), sum(result6),sum(tzmoney7), sum(result7) / sum(tzmoney7), sum(result7),sum(tzmoney8), sum(result8) / sum(tzmoney8), sum(result8),sum(tzmoney9), sum(result9) / sum(tzmoney9), sum(result9),sum(tzmoney10), sum(result10) / sum(tzmoney10), sum(result10),sum(tzmoney11), sum(result11) / sum(tzmoney11), sum(result11), sum(withdraw1),'',sum(withdraw2),'', sum(nowmoney), sum(balance), sum(balance),'','' from dailyaccount;";
            }
            DataTable dt = select(sql);


            DataRow        drt = dt.NewRow();
            drt[1] = ""; drt[2] = 0; drt[3] = 0; drt[4] = 0; drt[5] = 0; drt[6] = 0; drt[7] = 0; drt[8] = 0; drt[9] = 0; drt[10] = 0;
            drt[11] = 0; drt[12] = 0; drt[13] = 0; drt[14] = 0; drt[15] = 0; drt[16] = 0; drt[17] = 0; drt[18] = 0; drt[19] = 0; drt[20] = 0;
            drt[21] = 0; drt[22] = 0; drt[23] = 0; drt[24] = 0; drt[25] = 0; drt[26] = 0; drt[27] = 0; drt[28] = 0; drt[29] = 0; drt[30] = 0;
            drt[31] = 0; drt[32] = 0; drt[33] = 0; drt[34] = 0; drt[35] = 0; drt[36] = 0; drt[37] = 0; drt[38] = 0; drt[39] = 0; drt[40] = 0;

            drt[41] = 0; drt[42] = 0; drt[43] = 0; drt[44] = 0; drt[45] = 0; drt[46] = 0; drt[47] = 0; drt[48] = 0; drt[49] = 0; drt[50] = 0; drt[51] = 0;
            DataTable  dt2 = select(sql1);

            // drt

        

            for(int i =1; i < 50; i++)
            {
                drt[i] = dt2.Rows[0][i];
            }
            drt[1] = "总计";

            object[] obj = new object[dt.Columns.Count];
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                // dt2.Rows[i].ItemArray.CopyTo(obj, 0);
                drt.ItemArray.CopyTo(obj, 0);
                dt.Rows.Add(obj);
            }

          //  dt.Merge(dt2);
     


            return dt;
        }

        public DataTable getHistoryData(string user, int userClass, string startDate,string endDate, ArrayList userList, string selectedName)
        {
            string sql;
            string sql1;
            if (userClass == 1)
            {
                if (endDate.Equals("")) {
                sql = $"select * from  dailyaccount_history where user = '{user}' and date = '{startDate}' order by id";
                sql1 = $"select '', '','','','', '{user}','',sum(initmoney), sum(avaimoney), sum(deposit), sum(rebate), sum(tzmoney1), sum(result1) / sum(tzmoney1), sum(result1),sum(tzmoney2), sum(result2) / sum(tzmoney2), sum(result2),sum(tzmoney3), sum(result3) / sum(tzmoney3), sum(result3),sum(tzmoney4), sum(result4) / sum(tzmoney4), sum(result4),sum(tzmoney5), sum(result5) / sum(tzmoney5), sum(result5),sum(tzmoney6), sum(result6) / sum(tzmoney6), sum(result6),sum(tzmoney7), sum(result7) / sum(tzmoney7), sum(result7),sum(tzmoney8), sum(result8) / sum(tzmoney8), sum(result8),sum(tzmoney9), sum(result9) / sum(tzmoney9), sum(result9),sum(tzmoney10), sum(result10) / sum(tzmoney10), sum(result10),sum(tzmoney11), sum(result11) / sum(tzmoney11), sum(result11), sum(withdraw1),'',sum(withdraw2),'', sum(nowmoney), sum(balance), sum(balance),'','' from dailyaccount_history where user = '{user}'  and date between '{startDate}' and '{endDate}';";
                }
                else
                {
                    sql = $"select * from  dailyaccount_history where user = '{user}' and date between '{startDate}' and '{endDate}' order by id";
                    sql1 = $"select '', '','','','', '{user}','',sum(initmoney), sum(avaimoney), sum(deposit), sum(rebate), sum(tzmoney1), sum(result1) / sum(tzmoney1), sum(result1),sum(tzmoney2), sum(result2) / sum(tzmoney2), sum(result2),sum(tzmoney3), sum(result3) / sum(tzmoney3), sum(result3),sum(tzmoney4), sum(result4) / sum(tzmoney4), sum(result4),sum(tzmoney5), sum(result5) / sum(tzmoney5), sum(result5),sum(tzmoney6), sum(result6) / sum(tzmoney6), sum(result6),sum(tzmoney7), sum(result7) / sum(tzmoney7), sum(result7),sum(tzmoney8), sum(result8) / sum(tzmoney8), sum(result8),sum(tzmoney9), sum(result9) / sum(tzmoney9), sum(result9),sum(tzmoney10), sum(result10) / sum(tzmoney10), sum(result10),sum(tzmoney11), sum(result11) / sum(tzmoney11), sum(result11), sum(withdraw1),'',sum(withdraw2),'', sum(nowmoney), sum(balance), sum(balance),'','' from dailyaccount_history where user = '{user}'  and date  between '{startDate}' and '{endDate}';";

                }
            }
            else
            {
               


                string temp = "";

                foreach (user us in userList)
                {
                    temp += ",'";
                    temp += us.Username;
                    temp += "'";
                }
                if (endDate.Equals("")) { 
                sql = $"select * from  dailyaccount_history where date = '{startDate}'";
                    sql1 = $"select '', '','','','', '{user}','',sum(initmoney), sum(avaimoney), sum(deposit), sum(rebate), sum(tzmoney1), sum(result1) / sum(tzmoney1), sum(result1),sum(tzmoney2), sum(result2) / sum(tzmoney2), sum(result2),sum(tzmoney3), sum(result3) / sum(tzmoney3), sum(result3),sum(tzmoney4), sum(result4) / sum(tzmoney4), sum(result4),sum(tzmoney5), sum(result5) / sum(tzmoney5), sum(result5),sum(tzmoney6), sum(result6) / sum(tzmoney6), sum(result6),sum(tzmoney7), sum(result7) / sum(tzmoney7), sum(result7),sum(tzmoney8), sum(result8) / sum(tzmoney8), sum(result8),sum(tzmoney9), sum(result9) / sum(tzmoney9), sum(result9),sum(tzmoney10), sum(result10) / sum(tzmoney10), sum(result10),sum(tzmoney11), sum(result11) / sum(tzmoney11), sum(result11), sum(withdraw1),'',sum(withdraw2),'', sum(nowmoney), sum(balance), sum(balance),'','' from dailyaccount_history where   date = '{startDate}'";

                }
                else
                {
                    sql = $"select * from  dailyaccount_history where date between '{startDate}' and '{endDate}' ";
                    sql1 = $"select '', '','','','', '{user}','',sum(initmoney), sum(avaimoney), sum(deposit), sum(rebate), sum(tzmoney1), sum(result1) / sum(tzmoney1), sum(result1),sum(tzmoney2), sum(result2) / sum(tzmoney2), sum(result2),sum(tzmoney3), sum(result3) / sum(tzmoney3), sum(result3),sum(tzmoney4), sum(result4) / sum(tzmoney4), sum(result4),sum(tzmoney5), sum(result5) / sum(tzmoney5), sum(result5),sum(tzmoney6), sum(result6) / sum(tzmoney6), sum(result6),sum(tzmoney7), sum(result7) / sum(tzmoney7), sum(result7),sum(tzmoney8), sum(result8) / sum(tzmoney8), sum(result8),sum(tzmoney9), sum(result9) / sum(tzmoney9), sum(result9),sum(tzmoney10), sum(result10) / sum(tzmoney10), sum(result10),sum(tzmoney11), sum(result11) / sum(tzmoney11), sum(result11), sum(withdraw1),'',sum(withdraw2),'', sum(nowmoney), sum(balance), sum(balance),'','' from dailyaccount_history where   date  between '{startDate}' and '{endDate}'";

                }
                if (!selectedName.Equals(""))
                {
                    sql += $"and user = '{selectedName}'  order by field (`user` ";
                    sql1 += $"and user = '{selectedName}' ";
                }else
                {
                    sql += $" order by field (`user` ";
                }
                sql += temp;
                sql += ") , id";
              
            }
            

            //   return select(sql);

            DataTable dt = select(sql);


            DataRow drt = dt.NewRow();
            drt[1] = ""; drt[2] = 0; drt[3] = 0; drt[4] = 0; drt[5] = 0; drt[6] = 0; drt[7] = 0; drt[8] = 0; drt[9] = 0; drt[10] = 0;
            drt[11] = 0; drt[12] = 0; drt[13] = 0; drt[14] = 0; drt[15] = 0; drt[16] = 0; drt[17] = 0; drt[18] = 0; drt[19] = 0; drt[20] = 0;
            drt[21] = 0; drt[22] = 0; drt[23] = 0; drt[24] = 0; drt[25] = 0; drt[26] = 0; drt[27] = 0; drt[28] = 0; drt[29] = 0; drt[30] = 0;
            drt[31] = 0; drt[32] = 0; drt[33] = 0; drt[34] = 0; drt[35] = 0; drt[36] = 0; drt[37] = 0; drt[38] = 0; drt[39] = 0; drt[40] = 0;

            drt[41] = 0; drt[42] = 0; drt[43] = 0; drt[44] = 0; drt[45] = 0; drt[46] = 0; drt[47] = 0; drt[48] = 0; drt[49] = 0; drt[50] = 0; drt[51] = 0;
            DataTable dt2 = select(sql1);

            // drt



            for (int i = 1; i < 50; i++)
            {
                drt[i] = dt2.Rows[0][i];
            }
            drt[1] = "总计";

            object[] obj = new object[dt.Columns.Count];
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                // dt2.Rows[i].ItemArray.CopyTo(obj, 0);
                drt.ItemArray.CopyTo(obj, 0);
                dt.Rows.Add(obj);
            }

            //  dt.Merge(dt2);



            return dt;

        }
        public DataTable getItemData(string user ,int  id)
        {
            string sql;
       
                sql = $"select * from  dailyaccount where user = '{user}' and id = '{id}'";
            


            return select(sql);
        }
        public bool Login(string acc, string pwd, out DataTable dt)
        {
            string sql = $"select *  from user  where username='{acc}' and password='{pwd}'";
            try
            {
                dt = select(sql);
            }
            finally
            {
               
            }

            if (dt.Rows.Count == 1)
            {
             
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool version(string version, out DataTable dt)
        {
            string sql = $"select *  from version  where id=1 and version='{version}'";
            try
            {
                dt = select(sql);
            }
            finally
            {

            }

            if (dt.Rows.Count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool ChangePWD(string acc, string oldPwd, string newPwd)
        {
            DataTable dt = new DataTable();
            string sql = $"select *  from user  where username='{acc}' and password='{oldPwd}'";
            try
            {
                dt = select(sql);
            }
            finally
            {

            }

            if (dt.Rows.Count == 1)
            {
                int i  = update($"update  user set password='{newPwd}' where username='{acc}'");
                if (i >= 1)
                {
                    System.Windows.Forms.MessageBox.Show("密码更新成功");
                }
                return true;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("用户名或密码不正确");
                return false;
            }
        }


        public int add(item item_)
        {
            StringBuilder sql = new StringBuilder("insert into dailyaccount set  ");
            sql.Append("id = '").Append(item_.Id).Append("',")
.Append("websitetype = '").Append(item_.Websitetype).Append("',")
.Append("website = '").Append(item_.Website).Append("',")
.Append("loginname = '").Append(item_.Loginname).Append("',")
.Append("cardowner = '").Append(item_.Card_owner).Append("',")
.Append("user = '").Append(item_.User).Append("',")
.Append("iplocation = '").Append(item_.Iplocation).Append("',")
.Append("initmoney = '").Append(item_.Initmoney).Append("',");
if(item_.Deposit != null) { sql.Append("deposit = '").Append(item_.Deposit).Append("',"); }
if(item_.Rebate != null) { sql.Append("rebate = '").Append(item_.Rebate).Append("',"); }
if(item_.Tzmoney1 != null) { sql.Append("tzmoney1 = '").Append(item_.Tzmoney1).Append("',");   }
            if (item_.Odds1 != null) { sql.Append("odds1 = '").Append(item_.Odds1).Append("',"); }
if(item_.Result1 != null) { sql.Append("result1 = '").Append(item_.Result1).Append("',"); }

            if (item_.Tzmoney2 != null) { sql.Append("tzmoney2 = '").Append(item_.Tzmoney2).Append("',"); }
            if (item_.Odds2 != null) { sql.Append("odds2 = '").Append(item_.Odds2).Append("',"); }

            if (item_.Result2 != null) { sql.Append("result2 = '").Append(item_.Result2).Append("',"); }

            if (item_.Tzmoney3 != null) { sql.Append("tzmoney3 = '").Append(item_.Tzmoney3).Append("',"); }
            if (item_.Odds3 != null) { sql.Append("odds3 = '").Append(item_.Odds3).Append("',"); }
            if (item_.Result3 != null) { sql.Append("result3 = '").Append(item_.Result3).Append("',"); }

            if (item_.Tzmoney4 != null) { sql.Append("tzmoney4 = '").Append(item_.Tzmoney4).Append("',"); }
            if (item_.Odds4 != null) { sql.Append("odds4 = '").Append(item_.Odds4).Append("',"); }
            if (item_.Result4 != null) { sql.Append("result4 = '").Append(item_.Result4).Append("',"); }

            if (item_.Tzmoney5 != null) { sql.Append("tzmoney5 = '").Append(item_.Tzmoney5).Append("',"); }
            if (item_.Odds5 != null) { sql.Append("odds5 = '").Append(item_.Odds5).Append("',"); }
            if (item_.Result5 != null) { sql.Append("result5 = '").Append(item_.Result5).Append("',"); }

            if (item_.Tzmoney6 != null) { sql.Append("tzmoney6 = '").Append(item_.Tzmoney6).Append("',"); }
            if (item_.Odds6 != null) { sql.Append("odds6 = '").Append(item_.Odds6).Append("',"); }
            if (item_.Result6 != null) { sql.Append("result6 = '").Append(item_.Result6).Append("',"); }

            if (item_.Tzmoney7 != null) { sql.Append("tzmoney7 = '").Append(item_.Tzmoney7).Append("',"); }
            if (item_.Odds7 != null) { sql.Append("odds7 = '").Append(item_.Odds7).Append("',"); }
            if (item_.Result7 != null) { sql.Append("result7 = '").Append(item_.Result7).Append("',"); }

            if (item_.Tzmoney8 != null) { sql.Append("tzmoney8 = '").Append(item_.Tzmoney8).Append("',"); }
            if (item_.Odds8 != null) { sql.Append("odds8 = '").Append(item_.Odds8).Append("',"); }
            if (item_.Result8 != null) { sql.Append("result8 = '").Append(item_.Result8).Append("',"); }

            if (item_.Tzmoney9 != null) { sql.Append("tzmoney9 = '").Append(item_.Tzmoney9).Append("',"); }
            if (item_.Odds9 != null) { sql.Append("odds9 = '").Append(item_.Odds9).Append("',"); }
            if (item_.Result9 != null) { sql.Append("result9 = '").Append(item_.Result9).Append("',"); }

            if (item_.Tzmoney10 != null) { sql.Append("tzmoney10 = '").Append(item_.Tzmoney10).Append("',"); }
            if (item_.Odds10 != null) { sql.Append("odds10 = '").Append(item_.Odds10).Append("',"); }
            if (item_.Result10 != null) { sql.Append("result10 = '").Append(item_.Result10).Append("',"); }

            if (item_.Tzmoney11 != null) { sql.Append("tzmoney11 = '").Append(item_.Tzmoney11).Append("',"); }
            if (item_.Odds11 != null) { sql.Append("odds11 = '").Append(item_.Odds11).Append("',"); }
            if (item_.Result11 != null) { sql.Append("result11 = '").Append(item_.Result11).Append("',"); }

            if (item_.Withdraw1 != null) { sql.Append("withdraw1 = '").Append(item_.Withdraw1).Append("',"); }

            //todo::
            sql.Append("wdresult1 = '").Append(item_.Wdresult1).Append("',");
             if (item_.Withdraw2 != null) { sql.Append("withdraw2 = '").Append(item_.Withdraw2).Append("',"); }
       
            if (item_.Nowmoney  != null) { sql.Append("nowmoney = '").Append(item_.Nowmoney).Append("',"); }
            sql.Append("wdresult2 = '").Append(item_.Wdresult2).Append("'");
            sql.Append("ON DUPLICATE KEY UPDATE  ");
            if (item_.Deposit != null) { sql.Append("deposit = '").Append(item_.Deposit).Append("',"); }
            if (item_.Rebate != null) { sql.Append("rebate = '").Append(item_.Rebate).Append("',"); }
            if (item_.Tzmoney1 != null) { sql.Append("tzmoney1 = '").Append(item_.Tzmoney1).Append("',"); }
            if (item_.Odds1 != null) { sql.Append("odds1 = '").Append(item_.Odds1).Append("',"); }
            if (item_.Result1 != null) { sql.Append("result1 = '").Append(item_.Result1).Append("',"); }

            if (item_.Tzmoney2 != null) { sql.Append("tzmoney2 = '").Append(item_.Tzmoney2).Append("',"); }
            if (item_.Odds2 != null) { sql.Append("odds2 = '").Append(item_.Odds2).Append("',"); }

            if (item_.Result2 != null) { sql.Append("result2 = '").Append(item_.Result2).Append("',"); }

            if (item_.Tzmoney3 != null) { sql.Append("tzmoney3 = '").Append(item_.Tzmoney3).Append("',"); }
            if (item_.Odds3 != null) { sql.Append("odds3 = '").Append(item_.Odds3).Append("',"); }
            if (item_.Result3 != null) { sql.Append("result3 = '").Append(item_.Result3).Append("',"); }

            if (item_.Tzmoney4 != null) { sql.Append("tzmoney4 = '").Append(item_.Tzmoney4).Append("',"); }
            if (item_.Odds4 != null) { sql.Append("odds4 = '").Append(item_.Odds4).Append("',"); }
            if (item_.Result4 != null) { sql.Append("result4 = '").Append(item_.Result4).Append("',"); }

            if (item_.Tzmoney5 != null) { sql.Append("tzmoney5 = '").Append(item_.Tzmoney5).Append("',"); }
            if (item_.Odds5 != null) { sql.Append("odds5 = '").Append(item_.Odds5).Append("',"); }
            if (item_.Result5 != null) { sql.Append("result5 = '").Append(item_.Result5).Append("',"); }

            if (item_.Tzmoney6 != null) { sql.Append("tzmoney6 = '").Append(item_.Tzmoney6).Append("',"); }
            if (item_.Odds6 != null) { sql.Append("odds6 = '").Append(item_.Odds6).Append("',"); }
            if (item_.Result6 != null) { sql.Append("result6 = '").Append(item_.Result6).Append("',"); }

            if (item_.Tzmoney7 != null) { sql.Append("tzmoney7 = '").Append(item_.Tzmoney7).Append("',"); }
            if (item_.Odds7 != null) { sql.Append("odds7 = '").Append(item_.Odds7).Append("',"); }
            if (item_.Result7 != null) { sql.Append("result7 = '").Append(item_.Result7).Append("',"); }

            if (item_.Tzmoney8 != null) { sql.Append("tzmoney8 = '").Append(item_.Tzmoney8).Append("',"); }
            if (item_.Odds8 != null) { sql.Append("odds8 = '").Append(item_.Odds8).Append("',"); }
            if (item_.Result8 != null) { sql.Append("result8 = '").Append(item_.Result8).Append("',"); }

            if (item_.Tzmoney9 != null) { sql.Append("tzmoney9 = '").Append(item_.Tzmoney9).Append("',"); }
            if (item_.Odds9 != null) { sql.Append("odds9 = '").Append(item_.Odds9).Append("',"); }
            if (item_.Result9 != null) { sql.Append("result9 = '").Append(item_.Result9).Append("',"); }

            if (item_.Tzmoney10 != null) { sql.Append("tzmoney10 = '").Append(item_.Tzmoney10).Append("',"); }
            if (item_.Odds10 != null) { sql.Append("odds10 = '").Append(item_.Odds10).Append("',"); }
            if (item_.Result10 != null) { sql.Append("result10 = '").Append(item_.Result10).Append("',"); }

            if (item_.Tzmoney11 != null) { sql.Append("tzmoney11 = '").Append(item_.Tzmoney11).Append("',"); }
            if (item_.Odds11 != null) { sql.Append("odds11 = '").Append(item_.Odds11).Append("',"); }
            if (item_.Result11 != null) { sql.Append("result11 = '").Append(item_.Result11).Append("',"); }

            if (item_.Withdraw1 != null) { sql.Append("withdraw1 = '").Append(item_.Withdraw1).Append("',"); }

            //todo::
            sql.Append("wdresult1 = '").Append(item_.Wdresult1).Append("',");
            if (item_.Withdraw2 != null) { sql.Append("withdraw2 = '").Append(item_.Withdraw2).Append("',"); }

            if (item_.Nowmoney != null) { sql.Append("nowmoney = '").Append(item_.Nowmoney).Append("',"); }
            sql.Append("wdresult2 = '").Append(item_.Wdresult2).Append("';");


            //    string sql1 = $"insert into dailyaccount values('{item_.Websitetype}','{item_.Website}','{item_.Loginname}','{item_.Card_owner}','{item_.User}','{item_.Iplocation}','{item_.Initmoney})";


            return update(sql.ToString());
        }


        public int add(item item_, item item_db)
        {
            StringBuilder sql = new StringBuilder("insert into dailyaccount set  ");
            sql.Append("id = '").Append(item_.Id).Append("',")
.Append("websitetype = '").Append(item_.Websitetype).Append("',")
.Append("website = '").Append(item_.Website).Append("',")
.Append("loginname = '").Append(item_.Loginname).Append("',")
.Append("cardowner = '").Append(item_.Card_owner).Append("',")
.Append("user = '").Append(item_.User).Append("',")
.Append("iplocation = '").Append(item_.Iplocation).Append("',")
.Append("initmoney = '").Append(item_.Initmoney).Append("',");
            if (item_db.Avaimoney == null )
            {
                if (item_.Avaimoney != null) { sql.Append("avaimoney = '").Append(item_.Avaimoney).Append("',"); }
            }
            //new change: deposit 可以更改。往大处改
       //     if (item_db.Deposit == null) {
                if (item_.Deposit != null) { sql.Append("deposit = '").Append(item_.Deposit).Append("',"); }
       //     }
            if (item_db.Rebate == null) { 
            if (item_.Rebate != null) { sql.Append("rebate = '").Append(item_.Rebate).Append("',"); }
        }
            if(item_db.Tzmoney1 == null) { 
            if (item_.Tzmoney1 != null) { sql.Append("tzmoney1 = '").Append(item_.Tzmoney1).Append("',"); }
            }
            if (item_.Odds1 != null) { sql.Append("odds1 = '").Append(item_.Odds1).Append("',"); }
            if (item_.Result1 != null) { sql.Append("result1 = '").Append(item_.Result1).Append("',"); }

            if (item_.Tzmoney2 != null) { sql.Append("tzmoney2 = '").Append(item_.Tzmoney2).Append("',"); }
            if (item_.Odds2 != null) { sql.Append("odds2 = '").Append(item_.Odds2).Append("',"); }

            if (item_.Result2 != null) { sql.Append("result2 = '").Append(item_.Result2).Append("',"); }

            if (item_.Tzmoney3 != null) { sql.Append("tzmoney3 = '").Append(item_.Tzmoney3).Append("',"); }
            if (item_.Odds3 != null) { sql.Append("odds3 = '").Append(item_.Odds3).Append("',"); }
            if (item_.Result3 != null) { sql.Append("result3 = '").Append(item_.Result3).Append("',"); }

            if (item_.Tzmoney4 != null) { sql.Append("tzmoney4 = '").Append(item_.Tzmoney4).Append("',"); }
            if (item_.Odds4 != null) { sql.Append("odds4 = '").Append(item_.Odds4).Append("',"); }
            if (item_.Result4 != null) { sql.Append("result4 = '").Append(item_.Result4).Append("',"); }

            if (item_.Tzmoney5 != null) { sql.Append("tzmoney5 = '").Append(item_.Tzmoney5).Append("',"); }
            if (item_.Odds5 != null) { sql.Append("odds5 = '").Append(item_.Odds5).Append("',"); }
            if (item_.Result5 != null) { sql.Append("result5 = '").Append(item_.Result5).Append("',"); }

            if (item_.Tzmoney6 != null) { sql.Append("tzmoney6 = '").Append(item_.Tzmoney6).Append("',"); }
            if (item_.Odds6 != null) { sql.Append("odds6 = '").Append(item_.Odds6).Append("',"); }
            if (item_.Result6 != null) { sql.Append("result6 = '").Append(item_.Result6).Append("',"); }

            if (item_.Tzmoney7 != null) { sql.Append("tzmoney7 = '").Append(item_.Tzmoney7).Append("',"); }
            if (item_.Odds7 != null) { sql.Append("odds7 = '").Append(item_.Odds7).Append("',"); }
            if (item_.Result7 != null) { sql.Append("result7 = '").Append(item_.Result7).Append("',"); }

            if (item_.Tzmoney8 != null) { sql.Append("tzmoney8 = '").Append(item_.Tzmoney8).Append("',"); }
            if (item_.Odds8 != null) { sql.Append("odds8 = '").Append(item_.Odds8).Append("',"); }
            if (item_.Result8 != null) { sql.Append("result8 = '").Append(item_.Result8).Append("',"); }

            if (item_.Tzmoney9 != null) { sql.Append("tzmoney9 = '").Append(item_.Tzmoney9).Append("',"); }
            if (item_.Odds9 != null) { sql.Append("odds9 = '").Append(item_.Odds9).Append("',"); }
            if (item_.Result9 != null) { sql.Append("result9 = '").Append(item_.Result9).Append("',"); }

            if (item_.Tzmoney10 != null) { sql.Append("tzmoney10 = '").Append(item_.Tzmoney10).Append("',"); }
            if (item_.Odds10 != null) { sql.Append("odds10 = '").Append(item_.Odds10).Append("',"); }
            if (item_.Result10 != null) { sql.Append("result10 = '").Append(item_.Result10).Append("',"); }

            if (item_.Tzmoney11 != null) { sql.Append("tzmoney11 = '").Append(item_.Tzmoney11).Append("',"); }
            if (item_.Odds11 != null) { sql.Append("odds11 = '").Append(item_.Odds11).Append("',"); }
            if (item_.Result11 != null) { sql.Append("result11 = '").Append(item_.Result11).Append("',"); }

            if (item_.Withdraw1 != null) { sql.Append("withdraw1 = '").Append(item_.Withdraw1).Append("',"); }

            //todo::
            sql.Append("wdresult1 = '").Append(item_.Wdresult1).Append("',");
            if (item_.Withdraw2 != null) { sql.Append("withdraw2 = '").Append(item_.Withdraw2).Append("',"); }

            // if (item_.Nowmoney != null) { sql.Append("nowmoney = '").Append(item_.Nowmoney).Append("',"); }

            if (item_.Block != null) { sql.Append("block = '").Append(item_.Block).Append("',"); }
            if (item_.Status != null) { sql.Append("status = '").Append(item_.Status).Append("',"); }
            if (item_.Comment != null) { sql.Append("comment = '").Append(item_.Comment).Append("',"); }

            sql.Append("wdresult2 = '").Append(item_.Wdresult2).Append("'");
            
            sql.Append("ON DUPLICATE KEY UPDATE  ");

            // 网站套次和可打金额为可设置
            sql.Append("websitetype = '").Append(item_.Websitetype).Append("',");
         //   if (item_db.Avaimoney == null)
            {
                if (item_.Avaimoney != null) { sql.Append("avaimoney = '").Append(item_.Avaimoney).Append("',"); }
            }
            if (item_db.Deposit == null)
            {
                if (item_.Deposit != null) { sql.Append("deposit = '").Append(item_.Deposit).Append("',"); }
            }
            if (item_db.Rebate == null)
            {
                if (item_.Rebate != null) { sql.Append("rebate = '").Append(item_.Rebate).Append("',"); }
            }
            if (item_db.Tzmoney1 == null) { 
                if (item_.Tzmoney1 != null) { sql.Append("tzmoney1 = '").Append(item_.Tzmoney1).Append("',"); }
            }
            if (item_db.Odds1 == null)
            {
                if (item_.Odds1 != null) { sql.Append("odds1 = '").Append(item_.Odds1).Append("',"); }
            }
            if (item_db.Result1 == null)
            {
                if (item_.Result1 != null) { sql.Append("result1 = '").Append(item_.Result1).Append("',"); }
            }



            if (item_db.Tzmoney2 == null)
            {
                if (item_.Tzmoney2 != null) { sql.Append("tzmoney2 = '").Append(item_.Tzmoney2).Append("',"); }
            }
            if (item_db.Odds2 == null)
            {
                if (item_.Odds2 != null) { sql.Append("odds2 = '").Append(item_.Odds2).Append("',"); }
            }
            if (item_db.Result2 == null)
            {
                if (item_.Result2 != null) { sql.Append("result2 = '").Append(item_.Result2).Append("',"); }
            }

            if (item_db.Tzmoney3 == null)
            {
                if (item_.Tzmoney3 != null) { sql.Append("tzmoney3 = '").Append(item_.Tzmoney3).Append("',"); }
            }
            if (item_db.Odds3 == null)
            {
                if (item_.Odds3 != null) { sql.Append("odds3 = '").Append(item_.Odds3).Append("',"); }
            }
            if (item_db.Result3 == null)
            {
                if (item_.Result3 != null) { sql.Append("result3 = '").Append(item_.Result3).Append("',"); }
            }
            if (item_db.Tzmoney4 == null)
            {
                if (item_.Tzmoney4 != null) { sql.Append("tzmoney4 = '").Append(item_.Tzmoney4).Append("',"); }
            }
            if (item_db.Odds4 == null)
            {
                if (item_.Odds4 != null) { sql.Append("odds4 = '").Append(item_.Odds4).Append("',"); }
            }
            if (item_db.Result4 == null)
            {
                if (item_.Result4 != null) { sql.Append("result4 = '").Append(item_.Result4).Append("',"); }
            }
            if (item_db.Tzmoney5 == null)
            {
                if (item_.Tzmoney5 != null) { sql.Append("tzmoney5 = '").Append(item_.Tzmoney5).Append("',"); }
            }
            if (item_db.Odds5 == null)
            {
                if (item_.Odds5 != null) { sql.Append("odds5 = '").Append(item_.Odds5).Append("',"); }
            }
            if (item_db.Result5 == null)
            {
                if (item_.Result5 != null) { sql.Append("result5 = '").Append(item_.Result5).Append("',"); }
            }

            if (item_db.Tzmoney6 == null)
            {
                if (item_.Tzmoney6 != null) { sql.Append("tzmoney6 = '").Append(item_.Tzmoney6).Append("',"); }
            }
            if (item_db.Odds6 == null)
            {
                if (item_.Odds6 != null) { sql.Append("odds6 = '").Append(item_.Odds6).Append("',"); }
            }
            if (item_db.Result6 == null)
            {
                if (item_.Result6 != null) { sql.Append("result6 = '").Append(item_.Result6).Append("',"); }
            }

            if (item_db.Tzmoney7 == null)
            {
                if (item_.Tzmoney7 != null) { sql.Append("tzmoney7 = '").Append(item_.Tzmoney7).Append("',"); }
            }
            if (item_db.Odds7 == null)
            {
                if (item_.Odds7 != null) { sql.Append("odds7 = '").Append(item_.Odds7).Append("',"); }
            }
            if (item_db.Result7 == null)
            {
                if (item_.Result7 != null) { sql.Append("result7 = '").Append(item_.Result7).Append("',"); }
            }

            if (item_db.Tzmoney8 == null)
            {
                if (item_.Tzmoney8 != null) { sql.Append("tzmoney8 = '").Append(item_.Tzmoney8).Append("',"); }
            }
            if (item_db.Odds8 == null)
            {
                if (item_.Odds8 != null) { sql.Append("odds8 = '").Append(item_.Odds8).Append("',"); }
            }
            if (item_db.Result8 == null)
            {
                if (item_.Result8 != null) { sql.Append("result8 = '").Append(item_.Result8).Append("',"); }
            }

            if (item_db.Tzmoney9 == null)
            {
                if (item_.Tzmoney9 != null) { sql.Append("tzmoney9 = '").Append(item_.Tzmoney9).Append("',"); }
            }
            if (item_db.Odds9 == null)
            {
                if (item_.Odds9 != null) { sql.Append("odds9 = '").Append(item_.Odds9).Append("',"); }
            }
            if (item_db.Result9 == null)
            {
                if (item_.Result9 != null) { sql.Append("result9 = '").Append(item_.Result9).Append("',"); }
            }

            if (item_db.Tzmoney10 == null)
            {
                if (item_.Tzmoney10 != null) { sql.Append("tzmoney10 = '").Append(item_.Tzmoney10).Append("',"); }
            }
            if (item_db.Odds10 == null)
            {
                if (item_.Odds10 != null) { sql.Append("odds10 = '").Append(item_.Odds10).Append("',"); }
            }
            if (item_db.Result10 == null)
            {
                if (item_.Result10 != null) { sql.Append("result10 = '").Append(item_.Result10).Append("',"); }
            }

            if (item_db.Tzmoney11 == null)
            {
                if (item_.Tzmoney11 != null) { sql.Append("tzmoney11 = '").Append(item_.Tzmoney11).Append("',"); }
            }
            if (item_db.Odds11 == null)
            {
                if (item_.Odds11 != null) { sql.Append("odds11 = '").Append(item_.Odds11).Append("',"); }
            }
            if (item_db.Result11 == null)
            {
                if (item_.Result11 != null) { sql.Append("result11 = '").Append(item_.Result11).Append("',"); }
            }
            if (item_db.Withdraw1 == null)
            {
             
                if (item_.Withdraw1 != null)
                {
                    if (false == moneyAvailable(item_.Withdraw1, item_.Card_owner))
                    {
                        System.Windows.Forms.MessageBox.Show("提款" + item_.Card_owner + "金额" + item_.Withdraw1 + "不可用，请调整金额");

                    }
                    else
                    {
                        sql.Append("withdraw1 = '").Append(item_.Withdraw1).Append("',");
                    }
                }
            }

            //todo::
            sql.Append("wdresult1 = '").Append(item_.Wdresult1).Append("',");

            if (item_db.Nowmoney == null)
            {
                if (item_.Nowmoney != null) { sql.Append("nowmoney = '").Append(item_.Nowmoney).Append("',"); }
            }
            if (item_db.Withdraw2 == null)
            {

                if (item_.Withdraw2 != null)
                {
                    if (false == moneyAvailable(item_.Withdraw2, item_.Card_owner))
                    {
                        System.Windows.Forms.MessageBox.Show("提款"+ item_.Card_owner + "金额" + item_.Withdraw2 + "不可用，请调整金额");

                    }
                    else
                    {
                        sql.Append("withdraw2 = '").Append(item_.Withdraw2).Append("',");
                    }
                }
            }

       //     if (item_.Nowmoney != null) { sql.Append("nowmoney = '").Append(item_.Nowmoney).Append("',"); }

            if (item_.Block != null) { sql.Append("block = '").Append(item_.Block).Append("',"); }
            if (item_.Status != null) { sql.Append("status = '").Append(item_.Status).Append("',"); }
            if (item_.Comment != null) { sql.Append("comment = '").Append(item_.Comment).Append("',"); }
            sql.Append("wdresult2 = '").Append(item_.Wdresult2).Append("';");


            //    string sql1 = $"insert into dailyaccount values('{item_.Websitetype}','{item_.Website}','{item_.Loginname}','{item_.Card_owner}','{item_.User}','{item_.Iplocation}','{item_.Initmoney})";


            return update(sql.ToString());
        }





        public int add(item item_, item item_db, int userClass, int []tzstatus)
        {

   
            StringBuilder sql = new StringBuilder($"insert into dailyaccount set  ");
            sql.Append("id = '").Append(item_.Id).Append("',")
.Append("websitetype = '").Append(item_.Websitetype).Append("',")
.Append("website = '").Append(item_.Website).Append("',")
.Append("loginname = '").Append(item_.Loginname).Append("',")
.Append("cardowner = '").Append(item_.Card_owner).Append("',")
.Append("user = '").Append(item_.User).Append("',")
.Append("iplocation = '").Append(item_.Iplocation).Append("',")
.Append("initmoney = '").Append(item_.Initmoney).Append("',");
            if (item_db.Avaimoney == null || userClass == 0)
            {
                if (item_.Avaimoney != null) { sql.Append("avaimoney = '").Append(item_.Avaimoney).Append("',"); }
            }
            //new change: deposit 可以更改。往大处改
            //     if (item_db.Deposit == null || userClass == 0) {
            if (item_.Deposit != null) { sql.Append("deposit = '").Append(item_.Deposit).Append("',"); }
            //     }
            if (item_db.Rebate == null || userClass == 0)
            {
                if (item_.Rebate != null) { sql.Append("rebate = '").Append(item_.Rebate).Append("',"); }
            }
            if (item_db.Tzmoney1 == null || userClass == 0)
            {
                if (item_.Tzmoney1 != null) { sql.Append("tzmoney1 = '").Append(item_.Tzmoney1).Append("',"); }
            }
            if (item_.Odds1 != null) { sql.Append("odds1 = '").Append(item_.Odds1).Append("',"); }
            if (item_.Result1 != null) { sql.Append("result1 = '").Append(item_.Result1).Append("',"); }

            if (item_.Tzmoney2 != null) { sql.Append("tzmoney2 = '").Append(item_.Tzmoney2).Append("',"); }
            if (item_.Odds2 != null) { sql.Append("odds2 = '").Append(item_.Odds2).Append("',"); }

            if (item_.Result2 != null) { sql.Append("result2 = '").Append(item_.Result2).Append("',"); }

            if (item_.Tzmoney3 != null) { sql.Append("tzmoney3 = '").Append(item_.Tzmoney3).Append("',"); }
            if (item_.Odds3 != null) { sql.Append("odds3 = '").Append(item_.Odds3).Append("',"); }
            if (item_.Result3 != null) { sql.Append("result3 = '").Append(item_.Result3).Append("',"); }

            if (item_.Tzmoney4 != null) { sql.Append("tzmoney4 = '").Append(item_.Tzmoney4).Append("',"); }
            if (item_.Odds4 != null) { sql.Append("odds4 = '").Append(item_.Odds4).Append("',"); }
            if (item_.Result4 != null) { sql.Append("result4 = '").Append(item_.Result4).Append("',"); }

            if (item_.Tzmoney5 != null) { sql.Append("tzmoney5 = '").Append(item_.Tzmoney5).Append("',"); }
            if (item_.Odds5 != null) { sql.Append("odds5 = '").Append(item_.Odds5).Append("',"); }
            if (item_.Result5 != null) { sql.Append("result5 = '").Append(item_.Result5).Append("',"); }

            if (item_.Tzmoney6 != null) { sql.Append("tzmoney6 = '").Append(item_.Tzmoney6).Append("',"); }
            if (item_.Odds6 != null) { sql.Append("odds6 = '").Append(item_.Odds6).Append("',"); }
            if (item_.Result6 != null) { sql.Append("result6 = '").Append(item_.Result6).Append("',"); }

            if (item_.Tzmoney7 != null) { sql.Append("tzmoney7 = '").Append(item_.Tzmoney7).Append("',"); }
            if (item_.Odds7 != null) { sql.Append("odds7 = '").Append(item_.Odds7).Append("',"); }
            if (item_.Result7 != null) { sql.Append("result7 = '").Append(item_.Result7).Append("',"); }

            if (item_.Tzmoney8 != null) { sql.Append("tzmoney8 = '").Append(item_.Tzmoney8).Append("',"); }
            if (item_.Odds8 != null) { sql.Append("odds8 = '").Append(item_.Odds8).Append("',"); }
            if (item_.Result8 != null) { sql.Append("result8 = '").Append(item_.Result8).Append("',"); }

            if (item_.Tzmoney9 != null) { sql.Append("tzmoney9 = '").Append(item_.Tzmoney9).Append("',"); }
            if (item_.Odds9 != null) { sql.Append("odds9 = '").Append(item_.Odds9).Append("',"); }
            if (item_.Result9 != null) { sql.Append("result9 = '").Append(item_.Result9).Append("',"); }

            if (item_.Tzmoney10 != null) { sql.Append("tzmoney10 = '").Append(item_.Tzmoney10).Append("',"); }
            if (item_.Odds10 != null) { sql.Append("odds10 = '").Append(item_.Odds10).Append("',"); }
            if (item_.Result10 != null) { sql.Append("result10 = '").Append(item_.Result10).Append("',"); }

            if (item_.Tzmoney11 != null) { sql.Append("tzmoney11 = '").Append(item_.Tzmoney11).Append("',"); }
            if (item_.Odds11 != null) { sql.Append("odds11 = '").Append(item_.Odds11).Append("',"); }
            if (item_.Result11 != null) { sql.Append("result11 = '").Append(item_.Result11).Append("',"); }

        //    if (item_.Withdraw1 != null) { sql.Append("withdraw1 = '").Append(item_.Withdraw1).Append("',"); }

            //todo::
       //     sql.Append("wdresult1 = '").Append(item_.Wdresult1).Append("',");
       //     if (item_.Withdraw2 != null) { sql.Append("withdraw2 = '").Append(item_.Withdraw2).Append("',"); }

            // if (item_.Nowmoney != null) { sql.Append("nowmoney = '").Append(item_.Nowmoney).Append("',"); }

            if (item_.Block != null) { sql.Append("block = '").Append(item_.Block).Append("',"); }
            if (item_.Status != null) { sql.Append("status = '").Append(item_.Status).Append("',"); }
         //   if (item_.Comment != null)
            { sql.Append("comment = '").Append(item_.Comment).Append("'"); }

      //      sql.Append("wdresult2 = '").Append(item_.Wdresult2).Append("'");

            sql.Append("ON DUPLICATE KEY UPDATE  ");

            // 网站套次和可打金额为可设置
            sql.Append("websitetype = '").Append(item_.Websitetype).Append("',");
            //   if (item_db.Avaimoney == null || userClass == 0)
            {
                if (item_.Avaimoney != null) { sql.Append("avaimoney = '").Append(item_.Avaimoney).Append("',"); }
            }
            //     if (item_db.Deposit == null || userClass == 0)
            {
                if (item_.Deposit != null) { sql.Append("deposit = '").Append(item_.Deposit).Append("',"); }
            }
            if (item_db.Rebate == null || userClass == 0)
            {
                if (item_.Rebate != null) { sql.Append("rebate = '").Append(item_.Rebate).Append("',"); }
            }

            if(tzstatus[0] == 0) { 
                if (item_db.Tzmoney1 == null || userClass == 0)
                {
                    if (item_.Tzmoney1 != null) { sql.Append("tzmoney1 = '").Append(item_.Tzmoney1).Append("',"); }
                }
                if (item_db.Odds1 == null || userClass == 0)
                {
                    if (item_.Odds1 != null) { sql.Append("odds1 = '").Append(item_.Odds1).Append("',"); }
                }
                if (item_db.Result1 == null || userClass == 0)
                {
                    if (item_.Result1 != null) { sql.Append("result1 = '").Append(item_.Result1).Append("',"); }
                }
            }

            if (tzstatus[1] == 0)
            {
                if (item_db.Tzmoney2 == null || userClass == 0)
                {
                    if (item_.Tzmoney2 != null) { sql.Append("tzmoney2 = '").Append(item_.Tzmoney2).Append("',"); }
                }
                if (item_db.Odds2 == null || userClass == 0)
                {
                    if (item_.Odds2 != null) { sql.Append("odds2 = '").Append(item_.Odds2).Append("',"); }
                }
                if (item_db.Result2 == null || userClass == 0)
                {
                    if (item_.Result2 != null) { sql.Append("result2 = '").Append(item_.Result2).Append("',"); }
                }
            }


            if (tzstatus[2] == 0)
            {
                if (item_db.Tzmoney3 == null || userClass == 0)
                {
                    if (item_.Tzmoney3 != null) { sql.Append("tzmoney3 = '").Append(item_.Tzmoney3).Append("',"); }
                }
                if (item_db.Odds3 == null || userClass == 0)
                {
                    if (item_.Odds3 != null) { sql.Append("odds3 = '").Append(item_.Odds3).Append("',"); }
                }
                if (item_db.Result3 == null || userClass == 0)
                {
                    if (item_.Result3 != null) { sql.Append("result3 = '").Append(item_.Result3).Append("',"); }
                }
            }
            if (tzstatus[3] == 0)
            
                {
                    if (item_db.Tzmoney4 == null || userClass == 0)
                    {
                        if (item_.Tzmoney4 != null) { sql.Append("tzmoney4 = '").Append(item_.Tzmoney4).Append("',"); }
                    }
                    if (item_db.Odds4 == null || userClass == 0)
                    {
                        if (item_.Odds4 != null) { sql.Append("odds4 = '").Append(item_.Odds4).Append("',"); }
                    }
                    if (item_db.Result4 == null || userClass == 0)
                    {
                        if (item_.Result4 != null) { sql.Append("result4 = '").Append(item_.Result4).Append("',"); }
                    }
                }


            if (tzstatus[4] == 0)
            {
                if (item_db.Tzmoney5 == null || userClass == 0)
                {
                    if (item_.Tzmoney5 != null) { sql.Append("tzmoney5 = '").Append(item_.Tzmoney5).Append("',"); }
                }
                if (item_db.Odds5 == null || userClass == 0)
                {
                    if (item_.Odds5 != null) { sql.Append("odds5 = '").Append(item_.Odds5).Append("',"); }
                }
                if (item_db.Result5 == null || userClass == 0)
                {
                    if (item_.Result5 != null) { sql.Append("result5 = '").Append(item_.Result5).Append("',"); }
                }
            }


            if (tzstatus[5] == 0)
            {
                if (item_db.Tzmoney6 == null || userClass == 0)
                {
                    if (item_.Tzmoney6 != null) { sql.Append("tzmoney6 = '").Append(item_.Tzmoney6).Append("',"); }
                }
                if (item_db.Odds6 == null || userClass == 0)
                {
                    if (item_.Odds6 != null) { sql.Append("odds6 = '").Append(item_.Odds6).Append("',"); }
                }
                if (item_db.Result6 == null || userClass == 0)
                {
                    if (item_.Result6 != null) { sql.Append("result6 = '").Append(item_.Result6).Append("',"); }
                }
            }

            if (tzstatus[6] == 0)
            {
                if (item_db.Tzmoney7 == null || userClass == 0)
                {
                    if (item_.Tzmoney7 != null) { sql.Append("tzmoney7 = '").Append(item_.Tzmoney7).Append("',"); }
                }
                if (item_db.Odds7 == null || userClass == 0)
                {
                    if (item_.Odds7 != null) { sql.Append("odds7 = '").Append(item_.Odds7).Append("',"); }
                }
                if (item_db.Result7 == null || userClass == 0)
                {
                    if (item_.Result7 != null) { sql.Append("result7 = '").Append(item_.Result7).Append("',"); }
                }
            }

            if (tzstatus[7] == 0)
            {
                if (item_db.Tzmoney8 == null || userClass == 0)
                {
                    if (item_.Tzmoney8 != null) { sql.Append("tzmoney8 = '").Append(item_.Tzmoney8).Append("',"); }
                }
                if (item_db.Odds8 == null || userClass == 0)
                {
                    if (item_.Odds8 != null) { sql.Append("odds8 = '").Append(item_.Odds8).Append("',"); }
                }
                if (item_db.Result8 == null || userClass == 0)
                {
                    if (item_.Result8 != null) { sql.Append("result8 = '").Append(item_.Result8).Append("',"); }
                }
            }

            if (tzstatus[8] == 0)
            {
                if (item_db.Tzmoney9 == null || userClass == 0)
                {
                    if (item_.Tzmoney9 != null) { sql.Append("tzmoney9 = '").Append(item_.Tzmoney9).Append("',"); }
                }
                if (item_db.Odds9 == null || userClass == 0)
                {
                    if (item_.Odds9 != null) { sql.Append("odds9 = '").Append(item_.Odds9).Append("',"); }
                }
                if (item_db.Result9 == null || userClass == 0)
                {
                    if (item_.Result9 != null) { sql.Append("result9 = '").Append(item_.Result9).Append("',"); }
                }
            }

            if (tzstatus[9] == 0)
            {
                if (item_db.Tzmoney10 == null || userClass == 0)
                {
                    if (item_.Tzmoney10 != null) { sql.Append("tzmoney10 = '").Append(item_.Tzmoney10).Append("',"); }
                }
                if (item_db.Odds10 == null || userClass == 0)
                {
                    if (item_.Odds10 != null) { sql.Append("odds10 = '").Append(item_.Odds10).Append("',"); }
                }
                if (item_db.Result10 == null || userClass == 0)
                {
                    if (item_.Result10 != null) { sql.Append("result10 = '").Append(item_.Result10).Append("',"); }
                }
            }


            if (tzstatus[10] == 0)
            {
                if (item_db.Tzmoney11 == null || userClass == 0)
                {
                    if (item_.Tzmoney11 != null) { sql.Append("tzmoney11 = '").Append(item_.Tzmoney11).Append("',"); }
                }
                if (item_db.Odds11 == null || userClass == 0)
                {
                    if (item_.Odds11 != null) { sql.Append("odds11 = '").Append(item_.Odds11).Append("',"); }
                }
                if (item_db.Result11 == null || userClass == 0)
                {
                    if (item_.Result11 != null) { sql.Append("result11 = '").Append(item_.Result11).Append("',"); }
                }
            }
            if (userClass != 0)
            {
                if (item_db.Wdresult1 == null || item_db.Wdresult1 == "")
                {

                    if (item_db.Withdraw1 != item_.Withdraw1)
                    {

                        if (item_.Withdraw1 != null)
                        {
                            if (false == moneyAvailable(item_.Withdraw1, item_.Card_owner))
                            {
                                if (userClass != 0)
                                    System.Windows.Forms.MessageBox.Show("提款" + item_.Card_owner + "金额" + item_.Withdraw1 + "不可用，请调整金额");

                            }
                            else
                            {
                                sql.Append("withdraw1 = '").Append(item_.Withdraw1).Append("',");
                            }
                        }
                        else
                        {
                            sql.Append("withdraw1 = null,");
                        }
                    }
                }
            }

            //todo::
            if (userClass == 0) { 
            sql.Append("wdresult1 = '").Append(item_.Wdresult1).Append("',");
        }

            if (item_db.Nowmoney == null || userClass == 0)
            {
                if (item_.Nowmoney != null) { sql.Append("nowmoney = '").Append(item_.Nowmoney).Append("',"); }
            }

            if (userClass != 0)
            {
                if (item_db.Wdresult2 == null || item_db.Wdresult2 == "")
                {

                    if (item_db.Withdraw2 != item_.Withdraw2)
                    {

                        if (item_.Withdraw2 != null)
                        {
                            if (false == moneyAvailable(item_.Withdraw2, item_.Card_owner))
                            {
                                if (userClass != 0)
                                    System.Windows.Forms.MessageBox.Show("提款" + item_.Card_owner + "金额" + item_.Withdraw2 + "不可用，请调整金额");

                            }
                            else
                            {
                                sql.Append("withdraw2 = '").Append(item_.Withdraw2).Append("',");
                            }
                        }
                        else
                        {
                            sql.Append("withdraw2 = null,");
                        }
                    }
                }
            }


            //     if (item_.Nowmoney != null) { sql.Append("nowmoney = '").Append(item_.Nowmoney).Append("',"); }

            if (item_.Block != null) { sql.Append("block = '").Append(item_.Block).Append("',"); }
            if (item_.Status != null) { sql.Append("status = '").Append(item_.Status).Append("',"); }
      //      if (item_.Comment != null) { sql.Append("comment = '").Append(item_.Comment).Append("'"); }

            if(userClass == 0) {
            sql.Append("wdresult2 = '").Append(item_.Wdresult2).Append("',");
            }
            sql.Append("comment = '").Append(item_.Comment).Append("';");

            //    string sql1 = $"insert into dailyaccount values('{item_.Websitetype}','{item_.Website}','{item_.Loginname}','{item_.Card_owner}','{item_.User}','{item_.Iplocation}','{item_.Initmoney})";


            return update(sql.ToString());
        }



        public Boolean moneyAvailable(int ? money, string owner)
        {

            StringBuilder sql = new StringBuilder($"select  count(*) from dailyaccount where (cardowner = '{owner}' and withdraw1 = '{money}'  and wdresult1 = '' ) or (cardowner = '{owner}' and withdraw2 = '{money}' and wdresult2 = '') ");
            int i = count(sql.ToString());
            if (0 == i || -1 == i)
                return true;
            else
                return false;
        }

        public DataTable selectAll(string sql)
        {

            var conn = new MySql.Data.MySqlClient.MySqlConnection(connectStr_);
            var adp = new MySql.Data.MySqlClient.MySqlDataAdapter(sql, conn);

            DataTable dt = new DataTable();
            adp.Fill(dt);

            return dt;

        }

        public void setUpNotice(string user)
        {
            string sql = $"insert into  notice ( id, up) values('{user}','1') ON DUPLICATE KEY UPDATE up='0'";
            update(sql);
        }

        public void setDownNotice()
        {
            string sql = $"update notice set down = '0'";
            update(sql);
        }

        public void serverSetStatus(string user, int status)
        {
            string sql = null;
            if ("".Equals(user)) {  sql = $"update notice set  up='{status}'"; }else
            {
                 sql = $"update notice set  up='{status}' where id = '{user}'";
            }
            
            update(sql);
        }
        public void clientSetStatus(string user , int status)
        {

            string sql = null;
            if ("".Equals(user))
            {
                sql = $"update notice set  down='{status}'";
            }
            else { sql = $"insert into  notice ( id, down) values('{user}','{status}') ON DUPLICATE KEY UPDATE down='{status}'"; }
           
            update(sql);
        }


        public int archive(item item_, string date)
        {
            StringBuilder sql = new StringBuilder("insert into dailyaccount_history set  ");
            sql.Append("id = '").Append(item_.Id).Append("',")
.Append("websitetype = '").Append(item_.Websitetype).Append("',")
.Append("website = '").Append(item_.Website).Append("',")
.Append("loginname = '").Append(item_.Loginname).Append("',")
.Append("cardowner = '").Append(item_.Card_owner).Append("',")
.Append("user = '").Append(item_.User).Append("',")
.Append("iplocation = '").Append(item_.Iplocation).Append("',")
.Append("initmoney = '").Append(item_.Initmoney).Append("',");
            if (item_.Avaimoney != null) { sql.Append("avaimoney = '").Append(item_.Avaimoney).Append("',"); }
            if (item_.Deposit != null) { sql.Append("deposit = '").Append(item_.Deposit).Append("',"); }
            if (item_.Rebate != null) { sql.Append("rebate = '").Append(item_.Rebate).Append("',"); }
            if (item_.Tzmoney1 != null) { sql.Append("tzmoney1 = '").Append(item_.Tzmoney1).Append("',"); }
            if (item_.Odds1 != null) { sql.Append("odds1 = '").Append(item_.Odds1).Append("',"); }
            if (item_.Result1 != null) { sql.Append("result1 = '").Append(item_.Result1).Append("',"); }

            if (item_.Tzmoney2 != null) { sql.Append("tzmoney2 = '").Append(item_.Tzmoney2).Append("',"); }
            if (item_.Odds2 != null) { sql.Append("odds2 = '").Append(item_.Odds2).Append("',"); }

            if (item_.Result2 != null) { sql.Append("result2 = '").Append(item_.Result2).Append("',"); }

            if (item_.Tzmoney3 != null) { sql.Append("tzmoney3 = '").Append(item_.Tzmoney3).Append("',"); }
            if (item_.Odds3 != null) { sql.Append("odds3 = '").Append(item_.Odds3).Append("',"); }
            if (item_.Result3 != null) { sql.Append("result3 = '").Append(item_.Result3).Append("',"); }

            if (item_.Tzmoney4 != null) { sql.Append("tzmoney4 = '").Append(item_.Tzmoney4).Append("',"); }
            if (item_.Odds4 != null) { sql.Append("odds4 = '").Append(item_.Odds4).Append("',"); }
            if (item_.Result4 != null) { sql.Append("result4 = '").Append(item_.Result4).Append("',"); }

            if (item_.Tzmoney5 != null) { sql.Append("tzmoney5 = '").Append(item_.Tzmoney5).Append("',"); }
            if (item_.Odds5 != null) { sql.Append("odds5 = '").Append(item_.Odds5).Append("',"); }
            if (item_.Result5 != null) { sql.Append("result5 = '").Append(item_.Result5).Append("',"); }

            if (item_.Tzmoney6 != null) { sql.Append("tzmoney6 = '").Append(item_.Tzmoney6).Append("',"); }
            if (item_.Odds6 != null) { sql.Append("odds6 = '").Append(item_.Odds6).Append("',"); }
            if (item_.Result6 != null) { sql.Append("result6 = '").Append(item_.Result6).Append("',"); }

            if (item_.Tzmoney7 != null) { sql.Append("tzmoney7 = '").Append(item_.Tzmoney7).Append("',"); }
            if (item_.Odds7 != null) { sql.Append("odds7 = '").Append(item_.Odds7).Append("',"); }
            if (item_.Result7 != null) { sql.Append("result7 = '").Append(item_.Result7).Append("',"); }

            if (item_.Tzmoney8 != null) { sql.Append("tzmoney8 = '").Append(item_.Tzmoney8).Append("',"); }
            if (item_.Odds8 != null) { sql.Append("odds8 = '").Append(item_.Odds8).Append("',"); }
            if (item_.Result8 != null) { sql.Append("result8 = '").Append(item_.Result8).Append("',"); }

            if (item_.Tzmoney9 != null) { sql.Append("tzmoney9 = '").Append(item_.Tzmoney9).Append("',"); }
            if (item_.Odds9 != null) { sql.Append("odds9 = '").Append(item_.Odds9).Append("',"); }
            if (item_.Result9 != null) { sql.Append("result9 = '").Append(item_.Result9).Append("',"); }

            if (item_.Tzmoney10 != null) { sql.Append("tzmoney10 = '").Append(item_.Tzmoney10).Append("',"); }
            if (item_.Odds10 != null) { sql.Append("odds10 = '").Append(item_.Odds10).Append("',"); }
            if (item_.Result10 != null) { sql.Append("result10 = '").Append(item_.Result10).Append("',"); }

            if (item_.Tzmoney11 != null) { sql.Append("tzmoney11 = '").Append(item_.Tzmoney11).Append("',"); }
            if (item_.Odds11 != null) { sql.Append("odds11 = '").Append(item_.Odds11).Append("',"); }
            if (item_.Result11 != null) { sql.Append("result11 = '").Append(item_.Result11).Append("',"); }

            if (item_.Withdraw1 != null) { sql.Append("withdraw1 = '").Append(item_.Withdraw1).Append("',"); }
            sql.Append("date = '").Append(date).Append("',");

            //todo::
            sql.Append("wdresult1 = '").Append(item_.Wdresult1).Append("',");
            if (item_.Withdraw2 != null) { sql.Append("withdraw2 = '").Append(item_.Withdraw2).Append("',"); }

            if (item_.Nowmoney != null) { sql.Append("nowmoney = '").Append(item_.Nowmoney).Append("',"); }
         

            if (item_.Balance != null) { sql.Append("balance = '").Append(item_.Balance).Append("',"); }

            if (item_.Block != null) { sql.Append("block = '").Append(item_.Block).Append("',"); }

            if (item_.Status != null) { sql.Append("status = '").Append(item_.Status).Append("',"); }

            if (item_.Comment != null) { sql.Append("comment = '").Append(item_.Comment).Append("',"); }
            sql.Append("wdresult2 = '").Append(item_.Wdresult2).Append("'");

            sql.Append("ON DUPLICATE KEY UPDATE  ");
           { sql.Append("avaimoney = '").Append(item_.Avaimoney).Append("',"); }
            { sql.Append("deposit = '").Append(item_.Deposit).Append("',"); }
            { sql.Append("rebate = '").Append(item_.Rebate).Append("',"); }
            { sql.Append("tzmoney1 = '").Append(item_.Tzmoney1).Append("',"); }
            { sql.Append("odds1 = '").Append(item_.Odds1).Append("',"); }
           { sql.Append("result1 = '").Append(item_.Result1).Append("',"); }

             { sql.Append("tzmoney2 = '").Append(item_.Tzmoney2).Append("',"); }
             { sql.Append("odds2 = '").Append(item_.Odds2).Append("',"); }

            { sql.Append("result2 = '").Append(item_.Result2).Append("',"); }

            { sql.Append("tzmoney3 = '").Append(item_.Tzmoney3).Append("',"); }
             { sql.Append("odds3 = '").Append(item_.Odds3).Append("',"); }
           { sql.Append("result3 = '").Append(item_.Result3).Append("',"); }

             { sql.Append("tzmoney4 = '").Append(item_.Tzmoney4).Append("',"); }
             { sql.Append("odds4 = '").Append(item_.Odds4).Append("',"); }
             { sql.Append("result4 = '").Append(item_.Result4).Append("',"); }

          { sql.Append("tzmoney5 = '").Append(item_.Tzmoney5).Append("',"); }
            { sql.Append("odds5 = '").Append(item_.Odds5).Append("',"); }
           { sql.Append("result5 = '").Append(item_.Result5).Append("',"); }

             { sql.Append("tzmoney6 = '").Append(item_.Tzmoney6).Append("',"); }
            { sql.Append("odds6 = '").Append(item_.Odds6).Append("',"); }
            { sql.Append("result6 = '").Append(item_.Result6).Append("',"); }

            { sql.Append("tzmoney7 = '").Append(item_.Tzmoney7).Append("',"); }
             { sql.Append("odds7 = '").Append(item_.Odds7).Append("',"); }
             { sql.Append("result7 = '").Append(item_.Result7).Append("',"); }

          { sql.Append("tzmoney8 = '").Append(item_.Tzmoney8).Append("',"); }
            { sql.Append("odds8 = '").Append(item_.Odds8).Append("',"); }
            { sql.Append("result8 = '").Append(item_.Result8).Append("',"); }

            { sql.Append("tzmoney9 = '").Append(item_.Tzmoney9).Append("',"); }
            { sql.Append("odds9 = '").Append(item_.Odds9).Append("',"); }
             { sql.Append("result9 = '").Append(item_.Result9).Append("',"); }

            { sql.Append("tzmoney10 = '").Append(item_.Tzmoney10).Append("',"); }
            { sql.Append("odds10 = '").Append(item_.Odds10).Append("',"); }
            { sql.Append("result10 = '").Append(item_.Result10).Append("',"); }

            { sql.Append("tzmoney11 = '").Append(item_.Tzmoney11).Append("',"); }
            { sql.Append("odds11 = '").Append(item_.Odds11).Append("',"); }
            { sql.Append("result11 = '").Append(item_.Result11).Append("',"); }

           { sql.Append("withdraw1 = '").Append(item_.Withdraw1).Append("',"); }

            //todo::
            sql.Append("wdresult1 = '").Append(item_.Wdresult1).Append("',");
        { sql.Append("withdraw2 = '").Append(item_.Withdraw2).Append("',"); }

          { sql.Append("nowmoney = '").Append(item_.Nowmoney).Append("',"); }

          { sql.Append("balance = '").Append(item_.Balance).Append("',"); }

           { sql.Append("block = '").Append(item_.Block).Append("',"); }

            { sql.Append("status = '").Append(item_.Status).Append("',"); }

           { sql.Append("comment = '").Append(item_.Comment).Append("',"); }
       
            sql.Append("date = '").Append(date).Append("',");
            sql.Append("wdresult2 = '").Append(item_.Wdresult2).Append("';");


            //    string sql1 = $"insert into dailyaccount values('{item_.Websitetype}','{item_.Website}','{item_.Loginname}','{item_.Card_owner}','{item_.User}','{item_.Iplocation}','{item_.Initmoney})";


            return update(sql.ToString());
        }





    }
}
