using MundoDev.Mvc.ViewModels.Entities;

namespace MundoDev.Mvc.ViewModels.Student
{
    public class PlayerViewModel
    {
        public CourseViewModel Course       { get; set; } = null!;
        public LessonViewModel CurrentLesson { get; set; } = null!;
        public string EmbedUrl              { get; set; } = string.Empty;

        // Sidebar nav: topics with their lessons
        public List<TopicWithLessonsViewModel> Topics { get; set; } = new();

        // Lesson IDs the user has already completed
        public HashSet<Guid> CompletedLessonIds { get; set; } = new();

        public int ProgressPercent { get; set; }
        public bool IsCurrentLessonCompleted => CompletedLessonIds.Contains(CurrentLesson?.Id ?? Guid.Empty);
    }

    public class TopicWithLessonsViewModel
    {
        public TopicViewModel Topic       { get; set; } = null!;
        public List<LessonViewModel> Lessons { get; set; } = new();
    }
}
