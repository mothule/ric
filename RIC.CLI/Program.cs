using System;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using RIC.CLI.Models;
using RIC.CLI.Models.Redmine;
using RIC.CLI.Models.Redmine.Response;

namespace RIC.CLI
{
    public class Consts
    {
        public static readonly string Key = Configuration.Params.Key;
        public static readonly string ProjectName = Configuration.Params.ProjectName;
        public static readonly string BaseURL = Configuration.Params.BaseURL;
    }

    /// <summary>
    /// エントリーポイント
    /// </summary>
    class Program
    {
        static void PrintHelp()
        {
            /*
             *  .exe /register-sbl <path of SprintBacklog.csv>
             *      <path of SprintBacklog.csv> : e.g. ./sbl.csv
             *  
             *  .exe /get-remain-hours <sprintNo>
             *      <sprintNo> : e.g. "sprint3"
             *      
            */
            var sb = new StringBuilder();
            sb.Append("Assign a Sprint backlog file formated csv.\n");
            sb.Append("\n");
            sb.Append("usage:\n");
            sb.Append("\t1. Edit settings.xml.\n");
            sb.Append("\t\tInput a API Access Key to Key Field.\n");
            sb.Append("\t\tInput a Project Name to ProjectName Field.\n");
            sb.Append("\n");
            sb.Append("Press Enter to continue.\n");
            Console.WriteLine(sb.ToString());
            Console.ReadLine();
            Environment.Exit(0);
        }

        static float GetSumHours(string sprintNo)
        {
            Debug.WriteLine("タスクの合計時間を取得します.");
            Console.WriteLine("Get sum hours of tasks...");

            var versionId = RIC.CLI.Models.Redmine.Response.Version.Repository.GetByName(sprintNo).id.ToString();
            var res = RedmineApi.GetIssueAsync(new IssueGetRequestParams { TrackerName = "スプリントタスク", SprintNo = versionId.ToString(), StatusId = "*" }).Result;
            var resJson = JsonConvert.DeserializeObject<IssueGetResponse>(res.Content.ReadAsStringAsync().Result);

            float totalHours = 0.0F;
            resJson.issues.ForEach(i =>
                Debug.WriteLine(string.Format("{0}\t[{1}]\t{2}", (i.fixed_version != null ? i.fixed_version.name : "none"), i.status.name, i.subject))
            );
            resJson.issues.ForEach(i => totalHours += i.estimated_hours);
            return totalHours;
        }

        static float GetRemainTaskHours(string sprintNo)
        {
            Debug.WriteLine("残タスクの残り時間を取得します.");
            Console.WriteLine("Get remain hours of tasks...");

            var versionId = RIC.CLI.Models.Redmine.Response.Version.Repository.GetByName(sprintNo).id.ToString();
            var res = RedmineApi.GetIssueAsync(new IssueGetRequestParams { TrackerName = "スプリントタスク", SprintNo = versionId.ToString(), StatusId = "open" }).Result;
            var resJson = JsonConvert.DeserializeObject<IssueGetResponse>(res.Content.ReadAsStringAsync().Result);

            float totalHours = 0.0F;
            resJson.issues.ForEach(i =>
                Debug.WriteLine(string.Format("{0}\t[{1}]\t{2}", (i.fixed_version != null ? i.fixed_version.name : "none"), i.status.name, i.subject))
            );
            resJson.issues.ForEach(i => totalHours += i.estimated_hours);
            return totalHours;
        }

        [Conditional("DEBUG")]
        static void PauseIfDebug()
        {
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
#if !DEBUG
            System.AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
#endif
            Console.OutputEncoding = Encoding.UTF8;
            if (Configuration.LoadOrDefault() == false)
            {
                Console.WriteLine("config.jsonに情報を記載して再起動してください.");
                Environment.Exit(0);
            }

            if (args.Length <= 0)
            {
                PrintHelp();
            }


            var cmd = args[0];
            if (cmd == "/get-remain-hours")
            {
                if (args.Length <= 1)
                {
                    Console.WriteLine("Need 2nd arg. Input sprint no. i.g. sprint3");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
                var sprintNo = args[1];
                if (string.IsNullOrWhiteSpace(sprintNo) || !System.Text.RegularExpressions.Regex.IsMatch(sprintNo, "[0-9]*"))//  "sprint[0-9]"))
                {
                    Console.WriteLine("Need 2nd arg. Input sprint no. i.g. sprint3");
                    Console.ReadLine();
                    Environment.Exit(0);
                }

                var res = GetRemainTaskHours(sprintNo);
                Console.WriteLine("Remain hours is " + res.ToString() + " h");
                PauseIfDebug();
                Environment.Exit(0);
            }
            else if (cmd == "/get-sum-hours")
            {
                if (args.Length <= 1)
                {
                    Console.WriteLine("Need 2nd arg. Input sprint no. i.g. sprint3");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
                var sprintNo = args[1];
                if (string.IsNullOrWhiteSpace(sprintNo) || !System.Text.RegularExpressions.Regex.IsMatch(sprintNo, "[0-9]*"))// "sprint[0-9]"))
                {
                    Console.WriteLine("Need 2nd arg. Input sprint no. i.g. sprint3");
                    Console.ReadLine();
                    Environment.Exit(0);
                }

                var res = GetSumHours(sprintNo);
                Console.WriteLine("Sum hours is " + res.ToString() + " h");
                PauseIfDebug();
                Environment.Exit(0);
            }
        }

        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Debug.WriteLine(e.ExceptionObject.ToString());
            Console.WriteLine(e.ExceptionObject.ToString());
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
            Environment.Exit(1);
        }
    }
}
