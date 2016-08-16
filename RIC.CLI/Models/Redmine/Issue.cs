using System.Collections.Generic;
using Newtonsoft.Json;

namespace RIC.CLI.Models
{
    /// <summary>
    /// Redmine API の Issue Model
    /// </summary>
    public class Issue
    {
        [JsonProperty(PropertyName = "parent_issue_id")]
        public int ParentIssueId { get; set; }
        [JsonProperty(PropertyName = "project_id")]
        public string ProjectId { get; set; }
        [JsonProperty(PropertyName = "version_id")]
        public string VersionId { get; set; }
        [JsonProperty(PropertyName = "category_id")]
        public int CategoryId { get; set; }
        [JsonProperty(PropertyName = "subject")]
        public string Subject { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "estimated_hours")]
        public float EstimatedHours { get; set; }
        [JsonProperty(PropertyName = "watcher_user_id")]
        public string WatcherUserIds { get; set; }
        [JsonProperty(PropertyName = "assigned_to_id")]
        public string AssignedToId { get; set; }
        //[JsonProperty(PropertyName = "fixed_version")]
        //public string FixedVersion { get; set; }
        [JsonProperty("custom_field_values")]
        public Dictionary<string, string> CustomFieldValues;
    };
}
