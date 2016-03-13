using StudyBox.Core.Models;

namespace StudyBox.Messages
{
    public class DataMessageToSummary
    {
        public Exam ExamInstance { get; set; }

        public DataMessageToSummary(Exam exam)
        {
            this.ExamInstance = exam;
        }
    }
}