
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
namespace RIC.CLI.Models
{
    /// <summary>
    /// 外部ファイル
    /// </summary>
    public class Configuration
    {
        private static readonly string FilePath = Directory.GetCurrentDirectory() + "\\" + "config.json";

        /// <summary>
        /// xml保存パラメータ Model
        /// </summary>
        public class Parameters
        {
            /// <summary>
            /// AccessKey
            /// </summary>
            public string Key { get; set; }

            /// <summary>
            /// ProjectName
            /// </summary>
            public string ProjectName { get; set; }

            /// <summary>
            /// RedmineのURLトップ
            /// </summary>
            public string BaseURL { get; set; }

            /// <summary>
            /// バージョンID
            /// </summary>
            public string VersionId { get; set; }

            /// <summary>
            /// カテゴリID
            /// </summary>
            public int CategoryId { get; set; }

            /// <summary>
            /// ウォッチID一覧 [,]区切り
            /// </summary>
            public string WatcherUserIds { get; set; }

            /// <summary>
            /// 担当者ID
            /// </summary>
            public string AssignedToId { get; set; }

            /// <summary>
            /// トラッカー名
            /// </summary>
            public string TrackerName { get; set; }

            /// <summary>
            /// カスタムフィールド
            /// </summary>
            public Dictionary<string, string> CustomFields { get; set; }
        }

        public static Parameters Params { get; set; }

        /// <summary>
        /// xmlファイルの存在確認.
        /// </summary>
        /// <returns></returns>
        public static bool IsExist()
        {
            return System.IO.File.Exists(FilePath);
        }

        /// <summary>
        /// xmlファイルに保存
        /// </summary>
        public static void Store()
        {
            var w = new StreamWriter(FilePath, false, Encoding.UTF8);
            w.Write(JsonConvert.SerializeObject(Params));
            w.Close();
        }

        /// <summary>
        /// xmlファイルから読み込み
        /// </summary>
        public static void Load()
        {
            var r = new StreamReader(FilePath, Encoding.UTF8);
            Params = JsonConvert.DeserializeObject<Parameters>(r.ReadToEnd());
            r.Close();
        }

        /// <summary>
        /// 設定ファイルの読み込み.なければデフォルト作成.
        /// </summary>
        /// <returns></returns>
        public static bool LoadOrDefault()
        {
            if (IsExist())
            {
                Configuration.Load();
                return true;
            }
            else
            {
                Params = new Parameters
                {
                    Key = "個人設定のAPIアクセスキーをいれること",
                    ProjectName = "登録先プロジェクト名を入れること",
                    BaseURL = "RedmineサイトのURLトップを入れること",
                    VersionId = "バージョンID",
                    TrackerName = "スプリントタスク"

                };
                Configuration.Store();
                return false;
            }
        }


    }
}
