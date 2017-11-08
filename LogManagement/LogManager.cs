using System;
using System.IO;
using System.Text;

namespace BeerProcessingManager.LogManagement
{
    abstract public class LogManager
    {
        abstract public void InfoLog(string s_text);
        abstract public void ErrorLog(string s_text);
    }
    public sealed class ErrInfLogger : LogManager
    {
        private static ErrInfLogger Instance = null;
        private static readonly object Lock = new object();

        private string s_FormatOfDatetime;
        public static string s_FilePath;

        public ErrInfLogger(bool b_append = false)
        {
            s_FormatOfDatetime = "yyyy-MM-dd HH:mm:ss.fff";
            s_FilePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\file.log";

            string s_text = s_FilePath + " is created.";
            if (!File.Exists(s_FilePath))
                WritePreparedLine(DateTime.Now.ToString(s_FormatOfDatetime) + " " + s_text, false);
            else
            {
                if (b_append == false)
                    WritePreparedLine(DateTime.Now.ToString(s_FormatOfDatetime) + " " + s_text, false);
            }
        }
        override public void InfoLog(string s_text)
        {
            WriteLog(LogType.INFO_LOG, s_text);
        }
        override public void ErrorLog(string s_text)
        {
            WriteLog(LogType.ERROR_LOG, s_text);
        }
        private void WriteLog(LogType level, string s_text)
        {
            string s_pretext;
            switch (level)
            {
                case LogType.INFO_LOG: s_pretext = DateTime.Now.ToString(s_FormatOfDatetime) + " (INFO_LOG)    "; break;
                case LogType.ERROR_LOG: s_pretext = DateTime.Now.ToString(s_FormatOfDatetime) + " (ERROR_LOG)    "; break;
                default: s_pretext = string.Empty; break;
            }
            WritePreparedLine(s_pretext + s_text);
        }
        private void WritePreparedLine(string s_text, bool b_append = true)
        {
            try
            {
                using (StreamWriter sw_stream = new StreamWriter(s_FilePath, b_append, Encoding.UTF8))
                {
                    if (s_text != "") sw_stream.WriteLine(s_text);
                }
            }
            catch
            {
                throw;
            }
        }
        [Flags]
        private enum LogType
        {
            INFO_LOG,
            ERROR_LOG
        }
        public static ErrInfLogger LockInstance
        {
            get
            {
                lock (Lock)
                {
                    if (Instance == null)
                        Instance = new ErrInfLogger();
                    return Instance;
                }
            }
        }
    }
}