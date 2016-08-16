
using CsvHelper.Configuration;
namespace RIC.CLI.Models
{
    /// <summary>
    /// SBL.xlsm の CSV Model
    /// </summary>
    public class SprintBacklog
    {
        public string StoryName { get; set; }
        public string TaskName { get; set; }
        public string Done { get; set; }
        public string Hours { get; set; }
    }

    /// <summary>
    /// CSV カラム名とModelプロパティのマッピング
    /// </summary>
    public class CsvMap : CsvClassMap<SprintBacklog>
    {
        public CsvMap()
        {
            Map(m => m.StoryName).Name("ストーリー");
            Map(m => m.TaskName).Name("タスク名");
            Map(m => m.Done).Name("DONEの定義");
            Map(m => m.Hours).Name("時間");
        }
    }
}
