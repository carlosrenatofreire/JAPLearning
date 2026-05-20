using JAPLearning.Mvc.ViewModels.Entities;
using JAPLearning.Mvc.ViewModels.Parameters;

namespace JAPLearning.Mvc.ViewModels.Student
{
    public class CourseCatalogViewModel
    {
        public List<CourseCatalogItemViewModel> Courses     { get; set; } = new();
        public List<CategoryViewModel>          Categories  { get; set; } = new();
    }

    public class CourseCatalogItemViewModel
    {
        public CourseViewModel Course          { get; set; } = null!;
        public int             TotalLessons    { get; set; }
        public int             TotalTopics     { get; set; }
        public int             CompletedLessons { get; set; }
        public int             ProgressPercent => TotalLessons == 0 ? 0
            : (int)Math.Round((double)CompletedLessons / TotalLessons * 100);

        // "not-started" | "in-progress" | "completed"
        public string ProgressStatus => CompletedLessons == 0 ? "not-started"
            : (ProgressPercent >= 100 ? "completed" : "in-progress");

        public Guid? LastLessonId { get; set; }

        // Total duration from lessons
        public TimeSpan TotalDuration { get; set; }
        public string DurationDisplay => TotalDuration.TotalMinutes < 1 ? "—"
            : TotalDuration.TotalHours >= 1
                ? $"{(int)TotalDuration.TotalHours}h {TotalDuration.Minutes:D2}min"
                : $"{(int)TotalDuration.TotalMinutes}min";
    }
}
