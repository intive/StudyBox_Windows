using Newtonsoft.Json;

namespace StudyBox.Core.Models
{
    public class TestResult
    {
        [JsonProperty(PropertyName = "flashcardId")]
        public string FlashcardId { get; set; }

        [JsonProperty(PropertyName = "isCorrectAnswer")]
        public string IsCorrectAnswer { get; set; }

        public TestResult() { }
    }
}
