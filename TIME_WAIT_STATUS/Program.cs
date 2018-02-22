using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TIME_WAIT_STATUS
{
    class Program
    {
        static System.Diagnostics.ProcessStartInfo ProInfo;
        static System.Diagnostics.Process Pro;
        static int Max_Time_WAIT;
        static int Max_CLOSE_WAIT;


        static void Main(string[] args)
        {
            Max_Time_WAIT = 0;
            Max_CLOSE_WAIT = 0;

            ProInfo = new System.Diagnostics.ProcessStartInfo();
            ProInfo.FileName = @"cmd";

            ProInfo.CreateNoWindow = false;
            ProInfo.UseShellExecute = false;

            ProInfo.RedirectStandardInput = true;
            ProInfo.RedirectStandardError = true;
            ProInfo.RedirectStandardOutput = true;

            Pro = new System.Diagnostics.Process();

            while(true)
            {
                int TimeWait = Command(@"netstat -an | find ""TIME_WAIT"" /c" + Environment.NewLine);
                int CloseWait = Command(@"netstat -an | find ""CLOSE_WAIT"" /c" + Environment.NewLine);

                if (TimeWait > Max_Time_WAIT)
                    Max_Time_WAIT = TimeWait;

                if (CloseWait > Max_CLOSE_WAIT)
                    Max_CLOSE_WAIT = CloseWait;

                Console.WriteLine("TIME_WAIT : " + TimeWait + " MAX TIME_WAIT : " + Max_Time_WAIT);
                Console.WriteLine("CLOSE_WAIT : " + CloseWait + " MAX CLOSE_WAIT : " + Max_CLOSE_WAIT);

                Thread.Sleep(1000);
            }
        }

        static int Command(String Commands)
        {
            Pro.StartInfo = ProInfo;
            Pro.Start();
            Pro.StandardInput.Write(Commands);
            Pro.StandardInput.Close();

            String ResultValue = Pro.StandardOutput.ReadToEnd();
            String[] Split = Regex.Split(ResultValue, "\r\n");

            int Result = Int32.Parse(Split[Split.Length - 3]);

            Pro.WaitForExit();
            Pro.Close();

            return Result;
        }
       
    }


}
