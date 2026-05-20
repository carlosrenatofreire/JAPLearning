using JAPLearning.Mvc.ViewModels.Entities;

namespace JAPLearning.Mvc.ViewModels.Student
{
    public class CourseProgressViewModel
    {
        public CourseViewModel Course { get; set; } = null!;
        public int CompletedLessons   { get; set; }
        public int TotalLessons       { get; set; }
        public int ProgressPercent    => TotalLessons == 0 ? 0 : (int)Math.Round((double)CompletedLessons / TotalLessons * 100);
        public bool IsCompleted       => TotalLessons > 0 && CompletedLessons >= TotalLessons;

        // Last lesson visited (for "continue watching")
        public Guid? LastLessonId   { get; set; }
        public string? LastTopicName { get; set; }
    }
}
