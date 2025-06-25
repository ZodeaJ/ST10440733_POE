using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ST10440733_PROG6221_POE;

namespace ST10440733_PROG6221_POE
{
    /// <summary>
    /// Interaction logic for ChatPage.xaml
    /// </summary>
    public partial class ChatPage : Window
    {
        private CyberAssistant assistant;
        private ChatHandler chatHandler;
        private ChatbotLogic chatbot;

        private bool isTaskMode = false;
        private bool isQuizMode = false;
        private int currentQuizQuestionIndex = -1;



        public ChatPage(ChatbotLogic chatbot)
        {
            InitializeComponent();

            this.chatbot = chatbot;

            assistant = new CyberAssistant();
            assistant.LoadQuizQuestions();

            chatHandler = new ChatHandler(assistant);

            txtConversationHistory.Text = chatbot.GetGreeting() + "\n\n";
        }

        private void btnSwitchMode_Click(object sender, RoutedEventArgs e)
        {

            isTaskMode = !isTaskMode;
            if (isTaskMode)
            {
                chatHandler.LogUserAction("User switched to task management mode");
                btnSwitchMode.Content = "Switch to Cyber Advice";
                AppendToConversation("\n--- Switched to Task Management mode ---\n");
            }
            else
            {
                chatHandler.LogUserAction("User switched to cyber advice mode");
                btnSwitchMode.Content = "Switch to Task Management";
                AppendToConversation("\n--- Switched to Cyber Advice mode ---\n");

            }
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            string userInput = txtUserInput.Text.Trim();
            string response;

            if (isQuizMode)
            {
                response = assistant.HandleQuizAnswer(userInput);
                AppendToConversation($"\nYou: {userInput}\nChatBot: {response}\n");
            }
            else if (isTaskMode)
            {
                response = chatHandler.HandleUserInput(userInput);
                AppendToConversation($"\nYou: {userInput}\nChatBot: {response}\n");
            }
            else
            {
                response = chatbot.ProcessInput(userInput);
                AppendToConversation($"\nYou: {userInput}\nChatBot: {response}\n");
            }

            txtUserInput.Clear();
        }

        private void AppendToConversation(string message)
        {
            txtConversationHistory.AppendText(message + "\n");
            txtConversationHistory.ScrollToEnd();
        }

        private void btnStartQuiz_Click(object sender, RoutedEventArgs e)
        {
            if (isQuizMode)
            {
                AppendToConversation("You're already in quiz mode.");
                return;
            }

            isQuizMode = true;
            currentQuizQuestionIndex = 0;
            chatHandler.LogUserAction("User started quiz via button");
            AppendToConversation("\n--- Quiz started. Please answer the questions. ---\n");
            ShowCurrentQuizQuestion();
        }

        private void ShowCurrentQuizQuestion()
        {
            if (currentQuizQuestionIndex >= 0 && currentQuizQuestionIndex < assistant.QuizLength)
            {
                string questionText = assistant.GetQuizQuestion(currentQuizQuestionIndex);
                AppendToConversation($"Q{currentQuizQuestionIndex + 1}: {questionText}");
            }
            else
            {
                AppendToConversation($"Quiz completed! {assistant.GetFinalScore()}");
                isQuizMode = false;
                currentQuizQuestionIndex = -1;
            }
        }
    }
}