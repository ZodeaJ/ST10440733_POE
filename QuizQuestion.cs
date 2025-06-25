using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ST10440733_PROG6221_POE
{
    public class QuizQuestion
    {
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }
        public string[] Options { get; set; }

        public QuizQuestion(string question, string correctAnswer, string[] options)
        {
            Question = question;
            CorrectAnswer = correctAnswer;
            Options = options;
        }
        public bool IsCorrectAnswer(string userAnswer)
        {
            if (string.IsNullOrWhiteSpace(userAnswer))
                return false;

            userAnswer = userAnswer.Trim();

            // Check if user entered a number corresponding to the option
            if (int.TryParse(userAnswer, out int selectedOption))
            {
                int correctOption = int.Parse(CorrectAnswer);

                // Make sure selectedOption is within valid range
                if (selectedOption >= 1 && selectedOption <= Options.Length)
                {
                    return selectedOption == correctOption;
                }
            }

            // Otherwise, check full text match (optional)
            return string.Equals(userAnswer, CorrectAnswer, StringComparison.OrdinalIgnoreCase);
        }

    }
}