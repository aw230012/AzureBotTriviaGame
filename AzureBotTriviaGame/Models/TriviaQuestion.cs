using System;

namespace AzureBotTriviaGame.Models
{
    [Serializable]
    public class TriviaQuestion
    {
        public int Index { get; set; }

        public int Answer { get; set; }

        public string Question { get; set; }

        public string[] Choices { get; set; }
    }
}