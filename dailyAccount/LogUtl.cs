using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace dailyAccount
{
    /// <summary>
    /// level: 0: 不写log, 1: 只打印错误, 2: 打印错误和指示信息, 3：打印错误、指示信息、调试信息
    /// </summary>
    static class LogUtl
    {
        public static bool DebugOn
        {
            get
            {
                return level > 2;
            }
        }

        public static void info(string str)
        {
            if (level > 1)
            {
                WriteToStream("INFO", str);
                WriteToCtrl(str, null, 2);
            }
        }

        public static void err(string str, Exception ex = null)
        {
            if (level > 0)
            {
                WriteToStream("ERROR", str, ex);
                WriteToCtrl(str, ex?.Message, 1);
            }
        }

        public static void debug(string str)
        {
            if (level > 2)
            {
                WriteToStream("DEBUG", str);
                WriteToCtrl(str, null, 3);
            }
        }

        public static void WriteToStream(string type, string msg, Exception ex = null)
        {
            try
            {
                //var tw = Console.Out;
                StreamWriter tw = new StreamWriter(logFileName_, true);
                tw.Write(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"));
                tw.Write(" [");
                tw.Write(type);
                tw.Write("] ");
                tw.Write(msg);
                tw.WriteLine();
                if (ex != null)
                {
                    tw.WriteLine(ex.Message);
                    tw.WriteLine(ex.StackTrace);
                }
                //tw.Flush();
                tw.Close();
            }
            catch { }
        }

        public static void WriteToCtrl(string str, string exMessage, int level)
        {
            if (logCtrl_ != null)
            {
                var act = new Action(() =>
                {
                    logCtrl_.AppendText(DateTime.Now.ToString("mm:ss"));
                    logCtrl_.AppendText(" ");
                    var originColor = logCtrl_.SelectionColor;
                    logCtrl_.SelectionColor = level == 1 ? System.Drawing.Color.Red : (level == 2 ? System.Drawing.Color.Black : System.Drawing.Color.DarkSlateGray);
                    logCtrl_.AppendText(str);
                    logCtrl_.SelectionColor = originColor; // 恢复原来的颜色
                    if (!string.IsNullOrWhiteSpace(exMessage))
                    {
                        logCtrl_.AppendText(":");
                        logCtrl_.AppendText(exMessage);
                    }
                    logCtrl_.AppendText("\n");

                    logCtrl_.Select(logCtrl_.TextLength, 0);
                    logCtrl_.ScrollToCaret();
                });
                if (logCtrl_.InvokeRequired)
                {
                    logCtrl_.Invoke(act);
                }
                else
                {
                    act();
                }
            }
        }

        public static void initLogger(string fileName, System.Windows.Forms.RichTextBox logCtrl)
        {
            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            }
            logFileName_ = fileName;
            //StreamWriter sw = new StreamWriter(fileName, true);
            //Console.SetOut(sw);

            logCtrl_ = logCtrl;
        }

        public static void uninitLogger()
        {
            // 恢复标准输出
            StreamWriter sw = new StreamWriter(Console.OpenStandardOutput());
            sw.AutoFlush = true;
            Console.SetOut(sw);

            // 取消控件输出
            logCtrl_ = null;
        }

        public static int level = 3;
        private static System.Windows.Forms.RichTextBox logCtrl_ = null;
        private static string logFileName_ = null;
    }

    static class WebLog
    {
        public static void log(string url, NameValueCollection args, string result, Exception ex = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")); // 日期
            sb.Append('\t');
            sb.Append("/"); // 接口，如sfAllHM
            sb.Append(url);
            sb.Append('?'); // 参数
            foreach (string key in args.AllKeys)
            {
                sb.Append(key);
                sb.Append('=');
                sb.Append(args[key]);
                sb.Append('&');
            }
            if (sb[sb.Length - 1] == '&') sb.Length--; // 移除末尾 '&' 字符
            sb.Append('\t');//结果
            if (result == null) sb.Append("null");
            else sb.Append(result);
            if (ex != null) // 异常
            {
                sb.Append('\t');
                sb.Append("异常: ");
                sb.Append(ex.Message);
            }
            sw.WriteLine(sb.ToString());
            sw.Flush();
        }

        public static void initLogger(string workDir)
        {
            string logDir = workDir + "\\api.log";
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }
            string path = logDir + "\\api.log_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            path_ = path;

            sw = new StreamWriter(path_, true);
        }

        private static StreamWriter sw = null;
        private static string path_ = null;
        public static bool LogRequest = false;
    }
}
