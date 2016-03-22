using Newtonsoft.Json;

namespace StudyBox.Core.Models
{
    public class TestResult
    {
        [JsonProperty(PropertyName = "flashcardId")]
        public string FlashcardId { get; set; }

        [JsonProperty(PropertyName = "isCorrectAnswer")]
        public bool IsCorrectAnswer { get; set; }

        public TestResult() { }

        public TestResult(string flashcardId, bool isCorrectAnswer)
        {
            FlashcardId = flashcardId;
            IsCorrectAnswer = isCorrectAnswer;
        }
    }
}
