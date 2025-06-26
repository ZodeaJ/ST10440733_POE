using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ST10440733_PROG6221_POE
{
    public class ChatHandler
    {
        private readonly CyberAssistant cyberAssistant = new CyberAssistant();

        private enum TaskCreationStep
        {
            None,
            AwaitingTitle,
            AwaitingDescription,
            AwaitingReminder
        }

        private TaskCreationStep currentStep = TaskCreationStep.None;
        private string pendingTitle;
        private string pendingDescription;
        private bool waitingForCompletionTitle = false;
        // Track if quiz mode is active
        private bool isQuizMode = false;
        // Track current quiz question index
        private int currentQuizQuestionIndex = -1;
        // Track current quiz score
        private int score = 0;
        private bool isTaskMode = false;

        //ChatHandler constructor
        public ChatHandler(CyberAssistant assistant)
        {
            cyberAssistant = assistant;
        }
        public ChatHandler()
        {
            cyberAssistant = new CyberAssistant();
        }
        public void LogUserAction(string actionDescription)
        {
            // You can add timestamps or any formatting here
            cyberAssistant.LogActivity(actionDescription);
        }
        public void ResetQuizState()
        {
            isQuizMode = false;
            currentQuizQuestionIndex = -1; 
            score = 0;                      
        }

        public string HandleUserInput(string input)
        {
            string originalInput = input.Trim();
            input = originalInput.ToLower();

            
            if (currentStep != TaskCreationStep.None)
            {
                return HandleStepByStepTask(originalInput);
            }

            // Handles waiting for task title to complete
            if (waitingForCompletionTitle)
            {
                waitingForCompletionTitle = false;
                cyberAssistant.CompleteTask(originalInput);
                return $"Task '{originalInput}' marked as completed.";
            }

            // if statement for when use whats to add a task
            if (input == "add task" || input == "create task" || input == "add new task" || input == "create new task")
            {
                currentStep = TaskCreationStep.AwaitingTitle;
                return "Sure! What is the title of the task?";
            }

            // === QUIZ RELATED ===
            if (input.Contains("start quiz"))
            {
                LogUserAction("User started quiz");
                return cyberAssistant.StartQuiz();
            }
            else if (cyberAssistant.IsQuizActive())
            {
                return cyberAssistant.HandleQuizAnswer(input);
            }

            // === ACTIVITY LOG ===
            if (input.Contains("show activity log") || input.Contains("what have you done for me"))
            {
                var log = cyberAssistant.GetActivityLog();
                LogUserAction("User requested to view activity log");
                return "Recent Activity:\n" + string.Join("\n", log);
            }

            // === VIEW TASKS ===
            if (input.Contains("show tasks") || input.Contains("list tasks") || input.Contains("view tasks"))
            {
                LogUserAction("User requested to view tasks");
                return cyberAssistant.GetFormattedTaskList();
            }

            // === DELETE TASK ===
            if (input.StartsWith("delete task"))
            {
                string title = input.Replace("delete task", "").Trim();
                if (string.IsNullOrWhiteSpace(title))
                    return "Please specify the task title to delete.";

                cyberAssistant.DeleteTask(title);
                return $"Task '{title}' has been deleted.";
            }

            // === COMPLETE TASK ===
            if (input.StartsWith("complete task") || input.StartsWith("mark task as complete"))
            {
                string title = input.Replace("complete task", "").Replace("mark task as complete", "").Trim();
                if (string.IsNullOrWhiteSpace(title))
                    return "Please specify the task title to mark as complete.";

                cyberAssistant.CompleteTask(title);
                return $"Task '{title}' marked as completed.";
            }

            // === ADD TASK / REMINDER NLP DETECTION ===
            if (input.Contains("remind me") || input.Contains("add task") || input.Contains("remember to") || input.Contains("reminder"))
            {
                string title = ExtractTaskTitle(input);
                string description = title;
                DateTime? reminder = cyberAssistant.ParseNaturalReminderTime(input);

                cyberAssistant.AddTask(title, description, reminder);

                string response = $"Added task: '{title}'";
                if (reminder.HasValue)
                    response += $" with reminder set for {reminder.Value:g}.";

                return response;
            }

            return "I'm not sure how to help with that. Try asking to 'start quiz', 'add a task', 'delete task', 'complete task' or 'show activity log'.";
        }
        private string HandleStepByStepTask(string input)
        {
            string originalInput = input.Trim();
            input = originalInput.ToLower();

            switch (currentStep)
            {
                case TaskCreationStep.AwaitingTitle:
                    pendingTitle = originalInput;
                    currentStep = TaskCreationStep.AwaitingDescription;
                    return "Got it. What's the description?";

                case TaskCreationStep.AwaitingDescription:
                    pendingDescription = originalInput;
                    currentStep = TaskCreationStep.AwaitingReminder;
                    return "Would you like to set a reminder? If yes, say something like 'Remind me tomorrow at 5pm'. Otherwise, say 'no'.";

                case TaskCreationStep.AwaitingReminder:
                    DateTime? reminder = cyberAssistant.ParseNaturalReminderTime(originalInput);

                    // Preserve values before resetting
                    string confirmedTitle = pendingTitle;
                    string confirmedDescription = pendingDescription;

                    ResetStepFlow();

                    if (input == "no" || !reminder.HasValue)
                    {
                        cyberAssistant.AddTask(confirmedTitle, confirmedDescription, null);
                        return $"Added task: '{confirmedTitle}' with no reminder.";
                    }
                    else
                    {
                        cyberAssistant.AddTask(confirmedTitle, confirmedDescription, reminder);
                        return $"Added task: '{confirmedTitle}' with reminder set for {reminder.Value:g}.";
                    }
            }

            return "Something went wrong in task creation.";
        }
        // Resets the current task creation step and clears pending inputs
        private void ResetStepFlow()
        {
            currentStep = TaskCreationStep.None;
            pendingTitle = null;
            pendingDescription = null;
        }

        // Extracts a task title from user input based on known prefixes
        private string ExtractTaskTitle(string input)
        {
            string[] prefixes = { "remind me to", "add task to", "add a task to", "remember to", "reminder to", "add reminder to", "task to" };

            foreach (var prefix in prefixes)
            {
                if (input.Contains(prefix))
                {
                    int index = input.IndexOf(prefix);
                    return input.Substring(index + prefix.Length).Trim();
                }
            }
            return input;
        }


    }
}