using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountDataTransform.App
{
    internal class Logger
    {
        private static string LogFile
        {
            get
            {
                DateTime now = DateTime.Now;
                return "log_" + now.Year + "_" + now.Month + "_" + now.Day + "" + ".log";
            }
        }
        public static void WriteLog(Exception ex)
        {
            string logFile = LogFile;
            if(!File.Exists(logFile))
            {
                try
                {
                    File.Create(logFile);
                }
                catch(Exception)
                {

                }
            }
            if(File.Exists(logFile))
            {
                IList<string> lines = new List<string>();
               
                lines.Add(DateTime.Now.ToString());
                lines.Add(ex.Message);
                lines.Add(ex.StackTrace);
                lines.Add("****************************************");
                File.WriteAllLines(logFile, lines.ToArray());
              
            }
        }

    }
}
