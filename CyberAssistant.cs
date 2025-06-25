using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10440733_PROG6221_POE
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class CyberAssistant
    {
        private List<TaskItem> tasks = new List<TaskItem>();
        private List<string> activityLog = new List<string>();
        private bool reminderAsked = false;

        // === QUIZ ===
        private List<QuizQuestion> quizQuestions = new List<QuizQuestion>();
        private bool isQuizActive = false;
        private int currentQuestionIndex = 0;
        private int score = 0;

        public bool IsQuizActive() => isQuizActive;

        public string StartQuiz()
        {
            if (quizQuestions == null || !quizQuestions.Any())
                LoadQuizQuestions();

            isQuizActive = true;
            currentQuestionIndex = 0;
            score = 0;
            LogActivity("Started cybersecurity quiz");

            return GetCurrentQuestion();
        }

        public void LoadQuizQuestions()
        {
            quizQuestions = new List<QuizQuestion>
        {
            new QuizQuestion(
                "Which of the following is the strongest password?",
                "3",
                new string[] { "John1989", "1234", "Gr8$B3@k!92", "Password123" }
            ),
            new QuizQuestion(
                "Is using your birthday in a password secure?",
                "2",
                new string[] { "Yes", "No" }
            ),
            new QuizQuestion(
                "What is a common sign of a phishing email?",
                "3",
                new string[] { "Proper grammar", "Personalized greeting", "Urgent language and sketchy links", "Sent from a known contact" }
            ),
            new QuizQuestion(
                "Phishing emails usually come from trusted sources. True or False?",
                "2",
                new string[] { "True", "False" }
            ),
            new QuizQuestion(
                "What does the lock icon in the browser address bar mean?",
                "3",
                new string[] { "The site is safe to shop", "You've visited this site before", "The site uses HTTPS and encrypts data", "It's a paid site" }
            ),
            new QuizQuestion(
                "Which is the best response to someone urgently asking for private info?",
                "3",
                new string[] { "Give it immediately to avoid issues", "Ignore and delete the message", "Pause and verify their identity through another method", "Forward it to your friends" }
            ),
            new QuizQuestion(
                "What should you avoid when using public Wi-Fi?",
                "2",
                new string[] { "Watching videos", "Logging into banking or sensitive accounts", "Browsing news sites", "Reading emails" }
            ),
            new QuizQuestion(
                "Why should you only download apps from official app stores?",
                "3",
                new string[] { "They are cheaper", "They update automatically", "To avoid malware and ensure app security", "They run faster" }
            ),
            new QuizQuestion(
                "True or False: Keeping your device updated helps protect against new security threats.",
                "1",
                new string[] { "True", "False" }
            ),
            new QuizQuestion(
                "What is the best way to keep your software secure?",
                "2",
                new string[] { "Ignore updates", "Regularly update your software", "Use pirated versions", "Disable antivirus" }
            )
        };
        }

        private string GetCurrentQuestion()
        {
            if (currentQuestionIndex >= quizQuestions.Count)
                return "No more questions.";

            var question = quizQuestions[currentQuestionIndex];
            var options = new StringBuilder();

            for (int i = 0; i < question.Options.Length; i++)
            {
                options.AppendLine($"{i + 1}. {question.Options[i]}");
            }

            return $"{question.Question}\n\n{options}";
        }

        public string HandleQuizAnswer(string userAnswer)
        {
            // Check if quiz is active and question index is valid
            if (isQuizActive)
                return "No active quiz. Click 'start quiz' to begin.";

            if (currentQuestionIndex >= quizQuestions.Count)
            {
                isQuizActive = false;
                return $"Quiz complete! Final score: {score}/{quizQuestions.Count}";
            }

            var currentQuestion = quizQuestions[currentQuestionIndex];
            bool isCorrect = currentQuestion.IsCorrectAnswer(userAnswer);

            string feedback;
            if (isCorrect)
            {
                score++;
                feedback = "Correct";
            }
            else
            {
                int correctIndex = int.Parse(currentQuestion.CorrectAnswer) - 1;
                feedback = $"Incorrect. The correct answer was option {currentQuestion.CorrectAnswer}: {currentQuestion.Options[correctIndex]}";
            }

            currentQuestionIndex++;

            // If there are more questions, show next one; otherwise, finish quiz
            if (currentQuestionIndex < quizQuestions.Count)
            {
                return $"{feedback}\n\n{GetCurrentQuestion()}";
            }
            else
            {
                isQuizActive = false;
                LogActivity("Completed quiz");
                return $"{feedback}\n\nQuiz complete! Final score: {score}/{quizQuestions.Count}";
            }
        }

        public int QuizLength => quizQuestions.Count;

        public string GetQuizQuestion(int index)
        {
            if (index >= 0 && index < quizQuestions.Count)
            {
                var q = quizQuestions[index];
                // Use numbers instead of letters for options
                var formattedOptions = string.Join("\n", q.Options.Select((opt, i) => $"{i + 1}. {opt}"));
                return $"{q.Question}\n{formattedOptions}";
            }
            return "Invalid question index.";
        }

        public int GetFinalScore() => score;

        // === TASK MANAGEMENT ===
        public void AddTask(string title, string description, DateTime? reminder = null)
        {
            tasks.Add(new TaskItem { Title = title, Description = description, Reminder = reminder, IsCompleted = false });
            LogActivity($"Task added: '{title}'{(reminder.HasValue ? $" with reminder for {reminder.Value:g}" : "")}");
        }

        public void DeleteTask(string title)
        {
            var task = tasks.FirstOrDefault(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (task != null)
            {
                tasks.Remove(task);
                LogActivity($"Task deleted: '{title}'");
            }
        }

        public void CompleteTask(string title)
        {
            var task = tasks.FirstOrDefault(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (task != null)
            {
                task.IsCompleted = true;
                LogActivity($"Task completed: '{title}'");
            }
        }

        public string GetFormattedTaskList()
        {
            if (!tasks.Any()) return "You have no tasks.";

            var builder = new StringBuilder();
            foreach (var task in tasks)
            {
                string status = task.IsCompleted ? "Completed" : "Pending";
                string reminder = task.Reminder.HasValue ? $"Reminder: {task.Reminder.Value:g}" : "No reminder";
                builder.AppendLine($"• {task.Title}\n  Description: {task.Description}\n  {status} | {reminder}\n");
            }
            return builder.ToString().Trim();
        }

        public void LogActivity(string activity)
        {
            if (activityLog.Count >= 10) activityLog.RemoveAt(0);
            activityLog.Add($"{DateTime.Now:HH:mm} - {activity}");
        }

        public List<string> GetActivityLog(int count = 5) => activityLog.TakeLast(count).ToList();

        public DateTime? ParseNaturalReminderTime(string input)
        {
            input = input.ToLower();

            DateTime now = DateTime.Now;
            DateTime targetDate = now;
            TimeSpan defaultTime = new TimeSpan(9, 0, 0); // 9:00 AM by default
            TimeSpan? userTime = null;

            // === Time parser ===
            var timeMatch = Regex.Match(input, @"at\s+(\d{1,2})(:(\d{2}))?\s*(am|pm)?", RegexOptions.IgnoreCase);
            if (timeMatch.Success)
            {
                int hour = int.Parse(timeMatch.Groups[1].Value);
                int minute = timeMatch.Groups[3].Success ? int.Parse(timeMatch.Groups[3].Value) : 0;
                string ampm = timeMatch.Groups[4].Value.ToLower();

                if (ampm == "pm" && hour < 12) hour += 12;
                if (ampm == "am" && hour == 12) hour = 0;

                userTime = new TimeSpan(hour, minute, 0);
            }

            // === Date parser ===
            var dayMatch = Regex.Match(input, @"in (\d+) day");
            var weekMatch = Regex.Match(input, @"in (\d+) week");
            var monthMatch = Regex.Match(input, @"in (\d+) month");

            if (dayMatch.Success)
            {
                int days = int.Parse(dayMatch.Groups[1].Value);
                targetDate = now.AddDays(days);
            }
            else if (weekMatch.Success)
            {
                int weeks = int.Parse(weekMatch.Groups[1].Value);
                targetDate = now.AddDays(weeks * 7);
            }
            else if (monthMatch.Success)
            {
                int months = int.Parse(monthMatch.Groups[1].Value);
                targetDate = now.AddMonths(months);
            }
            else if (input.Contains("tomorrow"))
            {
                targetDate = now.AddDays(1);
            }
            else if (input.Contains("next week"))
            {
                targetDate = now.AddDays(7);
            }
            else if (input.Contains("next month"))
            {
                targetDate = now.AddMonths(1);
            }
            else
            {
                return null; // could not understand the date part
            }
            //1
            // Apply parsed time or default
            return targetDate.Date + (userTime ?? defaultTime);
        }
    }
}