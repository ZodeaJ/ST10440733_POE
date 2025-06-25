using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10440733_PROG6221_POE
{
    public class ChatbotLogic : CyberAdvice
    {
        private static string currentTopic = string.Empty;
        private static string userNameMemory = "";
        private static string favouriteTopicMemory = "";

        private CyberAdvice advice = new CyberAdvice();

        public void SetUserInfo(string userName, string favouriteTopic)
        {
            userNameMemory = userName;
            favouriteTopicMemory = favouriteTopic;
        }

        public string GetGreeting()
        {
            return "Ask me about cybersecurity topics like:\n" +
                   " - How are you?\n - What's your purpose?\n - What can I ask you about?\n - Type 'exit' to quit.\n";
        }

        public string ProcessInput(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                return "I didn't quite understand that. Please make sure you entered correctly.";

            userInput = userInput.ToLower();

            if (userInput == "exit")
                return "Goodbye! Stay safe online.";
            if (userInput.Contains("password"))
            {
                currentTopic = "password";
            }
            else if (userInput.Contains("phishing"))
            {
                currentTopic = "phishing";
            }
            else if (userInput.Contains("browsing"))
            {
                currentTopic = "browsing";
            }
            else if (userInput.Contains("device safety"))
            {
                currentTopic = "device safety";
            }
            else if (userInput.Contains("social engineering"))
            {
                currentTopic = "social engineering";
            }
            else if (userInput.Contains("wifi"))
            {
                currentTopic = "wifi";
            }

            if (EmotionHandling.ContainsNegativeKeyword(userInput))
            {
                string response = EmotionHandling.EmotionResponse(userInput);
                if (!string.IsNullOrEmpty(currentTopic))
                    response += "\n" + advice.DisplayRandomTip(currentTopic);
                else
                    response += "\nIf you want advice, please ask me about a cybersecurity topic.";

                return response;
            }

            //Displaying user memory with favorite topic and name
            if (userInput.Contains("favourite topic"))
            {
                return $"Your favourite topic is {favouriteTopicMemory}, {userNameMemory}!";
            }

            switch (userInput)
            {
                case "how are you":
                case "how are you?":
                    return "I'm doing well, thanks for asking!";

                case "what's your purpose?":
                case "what's your purpose":
                case "whats your purpose":
                    return "My purpose is to educate you on online threats and help you avoid them.";

                case "what can i ask you about":
                case "what can i ask you about?":
                    return "You can ask me about:\n - Password Safety\n - Phishing\n - Safe Browsing\n - Device Safety\n - Social Engineering\n - Public Wifi Safety";
            }

            // Topic detection
            if (userInput.Contains("password"))
            {
                currentTopic = "password";
                return IsFavouriteTopic("password") + advice.GetPasswordAdvice();
            }
            else if (userInput.Contains("phishing"))
            {
                currentTopic = "phishing";
                return IsFavouriteTopic("phishing") + advice.GetPhishingAdvice();
            }
            else if (userInput.Contains("browsing"))
            {
                currentTopic = "browsing";
                return IsFavouriteTopic("browsing") + advice.GetSafeBrowsingAdvice();
            }
            else if (userInput.Contains("device safety"))
            {
                currentTopic = "device safety";
                return IsFavouriteTopic("device safety") + advice.GetDeviceSafetyAdvice();
            }
            else if (userInput.Contains("social engineering"))
            {
                currentTopic = "social engineering";
                return IsFavouriteTopic("social engineering") + advice.GetSocialEngineeringAdvice();
            }
            else if (userInput.Contains("wifi"))
            {
                currentTopic = "wifi";
                return IsFavouriteTopic("wifi") + advice.GetPublicWifiSafetyAdvice();
            }
            else if (userInput.Contains("tell me more") || userInput.Contains("explain") || userInput.Contains("know more about"))
            {
                if (!string.IsNullOrEmpty(currentTopic))
                    return $"Let me tell you more about {currentTopic}:\n" + DisplayRandomTip(currentTopic);
                else
                    return "I haven't given you any advice yet. Please ask me about a topic first.";
            }

            return "Sorry, I didn't quite get that. Can you ask me about one of the topics?";
        }

        private string IsFavouriteTopic(string topic)
        {
            return favouriteTopicMemory == topic ? $"Ah yes, {userNameMemory}, your favourite topic! Here's some advice:\n" : "";
        }
    }
}
   