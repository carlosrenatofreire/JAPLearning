using JAPLearning.Mvc.ViewModels.Entities;

namespace JAPLearning.Mvc.ViewModels.Student
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
        public int PassingScore    { get; set; } = 60;
        public bool IsCurrentLessonCompleted => CompletedLessonIds.Contains(CurrentLesson?.Id ?? Guid.Empty);

        // Quiz questions for the current lesson (empty = no quiz)
        public List<QuestionViewModel> Questions { get; set; } = new();

        // Quiz attempt history for the current lesson
        public bool IsQuizLesson      => Questions.Any(q => q.Options.Any());
        public bool HasAttemptedQuiz  { get; set; }
        public int? BestQuizScore     { get; set; }
        public bool QuizPassed        => HasAttemptedQuiz && BestQuizScore.HasValue && BestQuizScore.Value >= PassingScore;
    }

    public class TopicWithLessonsViewModel
    {
        public TopicViewModel Topic       { get; set; } = null!;
        public List<LessonViewModel> Lessons { get; set; } = new();
    }
}
