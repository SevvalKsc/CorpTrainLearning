using System.Collections.Generic;
using CorpTrainLearning.Models; 

namespace CorpTrainLearning.ViewModels
{
    public class ModuleDashboardViewModel
    {
        public User CurrentUser { get; set; }
        public List<CourseWithModulesViewModel> EnrolledCourses { get; set; }
    }

    public class CourseWithModulesViewModel
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public List<ModuleViewModel> Modules { get; set; }
    }

    public class ModuleViewModel
    {
        public int ModuleId { get; set; }
        public string ModuleType { get; set; }
        public string ContentSnippet { get; set; } 
    }

    public class FullModuleContentViewModel
    {
        public int ModuleId { get; set; }
        public string ModuleType { get; set; }
        public string CourseTitle { get; set; }
        public string FullContent { get; set; }
    }
}