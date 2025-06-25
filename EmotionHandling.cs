using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10440733_PROG6221_POE
{
    internal class EmotionHandling
    {
        //Array list for all negative words.
        private static List<string> NegativeKeywords = new List<string>
        {
            "worried", "scared", "concerned", "frustrated", "anxious", "nervous", "overwhelmed", "unsure", "afraid"
        };

        public static bool ContainsNegativeKeyword(string userInput)
        {
            if (string.IsNullOrEmpty(userInput))
                return false;

            foreach (var word in NegativeKeywords)
            {
                if (userInput.Contains(word))
                    return true;
            }
            return false;
        }

        //Method to provide a response regarding the negative words.
        public static string EmotionResponse(string userInput)
        {
            foreach (var word in NegativeKeywords)
            {
                if (userInput.Contains(word))
                {
                    return "\nChatbot: I understand this can feel a bit overwhelming.\n" +
                           "But you're taking the right steps by learning. I'm here to help, one step at a time! Here's a tip:";
                }
            }

            return string.Empty;
        }
    }
}