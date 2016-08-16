using System.Collections.Generic;
using Newtonsoft.Json;

namespace RIC.CLI.Models.Redmine.Response
{

    public class Project
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Tracker
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Status
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Priority
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Author
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class AssignedTo
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class FixedVersion
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Parent
    {
        public int id { get; set; }
    }

    public class CustomField
    {
        public int id { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Issue
    {
        public int id { get; set; }
        public Project project { get; set; }
        public Tracker tracker { get; set; }
        public Status status { get; set; }
        public Priority priority { get; set; }
        public Author author { get; set; }
        public AssignedTo assigned_to { get; set; }
        public Category category { get; set; }
        public FixedVersion fixed_version { get; set; }
        public Parent parent { get; set; }
        public string subject { get; set; }
        public string description { get; set; }
        public string start_date { get; set; }
        public int done_ratio { get; set; }
        public float estimated_hours { get; set; }
        public List<CustomField> custom_fields { get; set; }
        public string created_on { get; set; }
        public string updated_on { get; set; }
    }

    public class IssueGetResponse
    {
        public List<Issue> issues { get; set; }
        public int total_count { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }
    }



    public class VersionsGetResponse
    {
        public List<Version> versions { get; set; }
        public int total_count { get; set; }
    }

    public class Version
    {
        public int id { get; set; }
        public Project project { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public string due_date { get; set; }
        public string sharing { get; set; }
        public string created_on { get; set; }
        public string updated_on { get; set; }

        public class Repository
        {
            public static Version GetByName(string name)
            {
                var versions = JsonConvert.DeserializeObject<VersionsGetResponse>(RedmineApi.GetVersionsAsync().Result.Content.ReadAsStringAsync().Result);
                var version = versions.versions.Find(v => v.name == name);
                return version;
            }
        }
    }

}
