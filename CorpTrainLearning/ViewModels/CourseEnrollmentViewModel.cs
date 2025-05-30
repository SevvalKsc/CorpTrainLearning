using System.Collections.Generic;
using CorpTrainLearning.Models;

namespace CorpTrainLearning.ViewModels
{
    public class CourseEnrollmentViewModel
    {
        public User CurrentUser { get; set; }
        public List<Course> EnrolledCourses { get; set; }
        public List<Course> AvailableCourses { get; set; }
    }
}