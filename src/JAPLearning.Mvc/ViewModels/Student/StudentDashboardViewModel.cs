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

        public int TotalWatchedHours   => TotalWatchedSeconds / 3600;
        public int TotalWatchedMinutes => (TotalWatchedSeconds % 3600) / 60;

        /// <summary>Texto formatado: "2h 15m" ou "45m" se menos de 1 hora.</summary>
        public string TotalWatchedLabel =>
            TotalWatchedHours > 0
                ? (TotalWatchedMinutes > 0 ? $"{TotalWatchedHours}h {TotalWatchedMinutes}m" : $"{TotalWatchedHours}h")
                : (TotalWatchedMinutes > 0 ? $"{TotalWatchedMinutes}m" : "0m");

        // Continue watching
        public List<CourseProgressViewModel> InProgressCourses  { get; set; } = new();

        // Completed courses
        public List<CourseProgressViewModel> CompletedCourses   { get; set; } = new();
    }
}
