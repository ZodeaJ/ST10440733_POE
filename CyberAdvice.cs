using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10440733_PROG6221_POE
{
    public class CyberAdvice : ICyberAdvice
    {
        private static readonly Random rdm = new Random();

        private static readonly Dictionary<string, List<string>> tips = new Dictionary<string, List<string>>
    {
        { "password", new List<string> {
            "Use a combination of uppercase and lowercase letters, numbers, and special symbols to make your password harder to guess.",
            "Avoid using things like your name, birthdate, or simple patterns like 1234, they’re the first things hackers try.",
            "The longer and more random your password is, the better. Using a password manager can really help."
        }},
        { "phishing", new List<string> {
            "Legit companies won’t ask for your passwords or credit card details through email or text, so never send that stuff unless you’re 100% sure.",
            "If you get an unexpected email or message with a link, don’t click right away, double check who it's from first.",
            "Look out for weird spelling, urgent language, or sketchy-looking links,these are common signs of phishing."
        }},
        { "browsing", new List<string> {
            "Make sure the site uses HTTPS (you’ll see a little lock icon in the address bar) before entering any personal info.",
            "Don’t download files or apps from sketchy sites, malware often hides in those sneaky pop-ups and free offers.",
            "If a website looks off or the URL seems weird, it’s best to avoid it. Fake sites can look surprisingly real."
        }},
        { "social engineering", new List<string> {
            " If something feels off in a conversation—like too much pressure or urgency, it’s okay to pause and not respond right away.",
            " If someone asks for sensitive info or access, even if they seem legit, double check through another method first.",
            "Be careful about what you post, details like your job, location, or family can be used to trick you."
        }},
        { "wifi", new List<string> {
            " A virtual private network encrypts your connection, making it way harder for anyone nearby to steal your info.",
            "Try not to log into banking or important accounts when you’re on public Wi-Fi—it’s easier for hackers to snoop.",
            "Once you’re done, disconnect and remove the public Wi-Fi from your device so it doesn’t connect automatically next time."
        }},
        { "device safety", new List<string> {
            "Regular updates patch security holes and keep your device protected from the latest threats.",
            "Lock your devices with passwords, PINs, or fingerprint/face recognition to stop others from accessing them.",
            "Only download apps from official stores and check permissions so you don’t accidentally give apps access to more than they need."
        }},
    };

        public string DisplayRandomTip(string topic)
        {
            if (tips.ContainsKey(topic))
            {
                string randomTip = tips[topic][rdm.Next(tips[topic].Count)];
                return $"{ToTitleCase(topic)} Tip: {randomTip}";
            }
            return "Sorry, I don't have advice for that topic.";
        }

        public string GetPasswordAdvice()
        {
           return DisplayRandomTip("password");
        }

        public string GetPhishingAdvice()
        {
            return DisplayRandomTip("phishing");
        }

        public string GetSafeBrowsingAdvice()
        {
           return DisplayRandomTip("browsing");
        }

        public string GetSocialEngineeringAdvice()
        {
           return DisplayRandomTip("social engineering");
        }

        public string GetPublicWifiSafetyAdvice()
        {
            return DisplayRandomTip("wifi");
        }

        public string GetDeviceSafetyAdvice()
        {
           return DisplayRandomTip("device safety");
        }

        private string ToTitleCase(string input)
        {
            return string.Join(" ", input.Split(' ').Select(word =>
                char.ToUpper(word[0]) + word.Substring(1)));
        }
    }
}
