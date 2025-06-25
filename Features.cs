using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace ST10440733_PROG6221_POE
{
    internal class Features
    {
        public static string GetGreetingMessage()
        {
            return "Hi there! The Cyber Security Awareness Bot welcomes you! Helping you stay secure online is my goal!";
        }

        // Plays the voice greeting audio
        public static void PlayVoiceGreeting()
        {
            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "VoicePlayer.wav");

                if (File.Exists(path))
                {
                    using (SoundPlayer player = new SoundPlayer(path))
                    {
                        player.Play(); 
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Audio file not found.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error playing audio: " + ex.Message);
            }
        }

        public static string GetAsciiArt()
        {
            return @"
             ________________________________________________
            /                                                \
           |    _________________________________________     |
           |   |                                         |    |
           |   |           Cyber Security                |    |
           |   |              ChatBot                    |    |
           |   |_________________________________________|    |
            \_________________________________________________/
                   \___________________________________/
                ___________________________________________
             _-'    .-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.  --- `-_
          _-'.-.-. .---.-.-.-.-.-.-.-.-.-.-.-.-.-.-.--.  .-.-.`-_
       _-'.-.-.-. .---.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-`__`. .-.-.-.`-_
    _-'.-.-.-.-. .-----.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-----. .-.-.-.-.`-_
 _-'.-.-.-.-.-. .---.-. .-------------------------. .-.---. .---.-.-.-.`-_
:-------------------------------------------------------------------------:
`---._.-------------------------------------------------------------._.---'
";
        }
    }
}