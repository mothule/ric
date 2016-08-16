using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using NLog;
using RIC.Models.Redmine;

namespace RIC.CLI.Models.Redmine
{
    /// <summary>
    /// Redmine API の Body Model
    /// </summary>
    public class TicketPostRequestBody
    {
        [JsonProperty(PropertyName = "issue")]
        public Issue Issue { get; set; }
    };

    /// <summary>
    /// Issue Default Value Setter
    /// </summary>
    public class IssueDefaultSetter
    {
        public static TicketPostRequestBody SetupDefault(TicketPostRequestBody body)
        {
            body.Issue.ProjectId = Consts.ProjectName;
            body.Issue.WatcherUserIds = "204,543";
            body.Issue.AssignedToId = "204";
            body.Issue.CustomFieldValues = new Dictionary<string, string> { 
                {"30", "管理ツール"}, // 対応チーム                
            };
            return body;
        }
    }

    /// <summary>
    /// Issue 取得リクエスト用パラメータ
    /// </summary>
    public class IssueGetRequestParams
    {
        public string SprintNo { get; set; }
        public string StatusId { get; set; }
        public string TrackerName { get; set; }
    }


    /// <summary>
    /// Redmine's Rest API.
    /// </summary>
    class RedmineApi
    {
        private static readonly string BaseURL = Consts.BaseURL;
        private static readonly string Uri = "projects/" + Consts.ProjectName + "/issues.json";
        private static readonly string IssueGetUri = "projects/" + Consts.ProjectName + "/issues.json";
        private static readonly string VersionsGetUri = "projects/" + Consts.ProjectName + "/versions.json";
        private static readonly string TrackerGetUri = "trackers.json";


        public static async Task<HttpResponseMessage> GetVersionsAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var request = new HttpRequestMessage(HttpMethod.Get, VersionsGetUri);
                request.Headers.Add("X-Redmine-API-Key", Consts.Key);
                var response = await client.SendAsync(request);
                return response;
            }
        }

        public static async Task<HttpResponseMessage> GetTrackerAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //var kv = new Dictionary<string, string> { { "project_id", Consts.ProjectName } };
                var query = HttpUtility.ParseQueryString(string.Empty);
                var request = new HttpRequestMessage(HttpMethod.Get, TrackerGetUri + (string.IsNullOrWhiteSpace(query.ToString()) ? string.Empty : "?" + query.ToString()));
                request.Headers.Add("X-Redmine-API-Key", Consts.Key);
                return await client.SendAsync(request);
            }
        }

        public static async Task<HttpResponseMessage> GetIssueAsync(IssueGetRequestParams requestParams)
        {

            var trackersRes = await GetTrackerAsync();
            var trackers = JsonConvert.DeserializeObject<TrackerRoot>(trackersRes.Content.ReadAsStringAsync().Result);
            var trackerId = trackers.FindIdByName(requestParams.TrackerName);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var kv = new Dictionary<string, string> { { "project_id", Consts.ProjectName } };
                var query = HttpUtility.ParseQueryString(string.Empty);
                query["project_id"] = Consts.ProjectName;
                query["tracker_id"] = trackerId.ToString();
                query["status_id"] = requestParams.StatusId;// "open";//closed or *
                query["fixed_version_id"] = requestParams.SprintNo;
                query["limit"] = "100";
                var request = new HttpRequestMessage(HttpMethod.Get, IssueGetUri + "?" + query.ToString());
                request.Headers.Add("X-Redmine-API-Key", Consts.Key);
                var response = await client.SendAsync(request);
                return response;
            }
        }


        /// <summary>
        /// スプリントバックログのチケットを１件登録
        /// </summary>
        /// <param name="task">SBLの１タスク</param>
        /// <returns></returns>
        public static async Task PostIssueAsync(TicketPostRequestBody requestBody)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var request = new HttpRequestMessage(HttpMethod.Post, Uri);
                request.Headers.Add("X-Redmine-API-Key", Consts.Key);

                var bodyJson = JsonConvert.SerializeObject(requestBody);
                request.Content = new StringContent(bodyJson, Encoding.UTF8, "application/json");
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(string.Format("チケットを登録. {0}", bodyJson));
                    Console.WriteLine(string.Format("Register ticket. {0}", bodyJson));
                    LogManager.GetCurrentClassLogger().Info(string.Format("チケットを登録. {0}", bodyJson));
                }
                else
                {
                    Debug.WriteLine(string.Format("チケットの登録失敗. {0}", bodyJson));
                    Console.WriteLine(string.Format("Failed register ticket. {0}", bodyJson));
                }
            }
        }
    }
}
