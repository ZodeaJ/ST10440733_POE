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

        public ChatHandler(CyberAssistant assistant)
        {
            cyberAssistant = assistant;
        }
        public ChatHandler()
        {
            cyberAssistant = new CyberAssistant();
        }


        public string HandleUserInput(string input)
        {
            string originalInput = input.Trim();
            input = originalInput.ToLower();

            // === Handle step-by-step task creation ===
            if (currentStep != TaskCreationStep.None)
            {
                return HandleStepByStepTask(originalInput);
            }

            // === Handle waiting for task title to complete ===
            if (waitingForCompletionTitle)
            {
                waitingForCompletionTitle = false;
                cyberAssistant.CompleteTask(originalInput);
                return $"Task '{originalInput}' marked as completed.";
            }

            // === Step-by-step triggers ===
            if (input == "add task" || input == "create task" || input == "add new task" || input == "create new task")
            {
                currentStep = TaskCreationStep.AwaitingTitle;
                return "Sure! What is the title of the task?";
            }

            // === QUIZ RELATED ===
            if (input.Contains("start quiz"))
            {
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
                return "Recent Activity:\n" + string.Join("\n", log);
            }

            // === VIEW TASKS ===
            if (input.Contains("show tasks") || input.Contains("list tasks"))
            {
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
            switch (currentStep)
            {
                case TaskCreationStep.AwaitingTitle:
                    pendingTitle = input.Trim();
                    currentStep = TaskCreationStep.AwaitingDescription;
                    return "Got it. What's the description?";

                case TaskCreationStep.AwaitingDescription:
                    pendingDescription = input.Trim();
                    currentStep = TaskCreationStep.AwaitingReminder;
                    return "Would you like to set a reminder? If yes, say something like 'Remind me tomorrow at 5pm'. Otherwise, say 'no'.";

                case TaskCreationStep.AwaitingReminder:
                    DateTime? reminder = cyberAssistant.ParseNaturalReminderTime(input);

                    if (input.Trim().ToLower() == "no" || !reminder.HasValue)
                    {
                        cyberAssistant.AddTask(pendingTitle, pendingDescription, null);
                        ResetStepFlow();
                        return $"Added task: '{pendingTitle}' with no reminder.";
                    }
                    else
                    {
                        cyberAssistant.AddTask(pendingTitle, pendingDescription, reminder);
                        ResetStepFlow();
                        return $"Added task: '{pendingTitle}' with reminder set for {reminder.Value:g}.";
                    }
            }

            return "Something went wrong in task creation.";
        }

        private void ResetStepFlow()
        {
            currentStep = TaskCreationStep.None;
            pendingTitle = null;
            pendingDescription = null;
        }

        private string ExtractTaskTitle(string input)
        {
            // Try to remove common prefixes
            string[] prefixes = { "remind me to", "add task to", "add a task to", "remember to", "reminder to", "add reminder to", "task to" };

            foreach (var prefix in prefixes)
            {
                if (input.Contains(prefix))
                {
                    int index = input.IndexOf(prefix);
                    return input.Substring(index + prefix.Length).Trim();
                }
            }

            // Fallback if no prefix match
            return input;
        }

    }
}