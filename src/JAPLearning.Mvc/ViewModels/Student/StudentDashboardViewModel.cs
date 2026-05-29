namespace JAPLearning.Mvc.ViewModels.Student
{
    public class StudentDashboardViewModel
    {
        public string FirstName { get; set; } = string.Empty;

        // KPIs
        public int CoursesInProgress    { get; set; }
        public int CoursesCompleted     { get; set; }
        public int CertificatesCount    { get; set; }
        public int TotalWatchedSeconds  { get; set; }

        public int TotalWatchedHours => TotalWatchedSeconds / 3600;

        // Continue watching
        public List<CourseProgressViewModel> InProgressCourses  { get; set; } = new();

        // Completed courses
        public List<CourseProgressViewModel> CompletedCourses   { get; set; } = new();
    }
}
