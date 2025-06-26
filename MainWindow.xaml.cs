using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ST10440733_PROG6221_POE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Display ASCII Art in a TextBox or TextBlock named txtAsciiArt
            txtAsciiArt.Text = Features.GetAsciiArt();

            // Play audio greeting
            Features.PlayVoiceGreeting();

        }
        //Button start chat which takes the user to chatpage window
        private void StartChat_Click(object sender, RoutedEventArgs e)
        {
            string userName = txtName.Text.Trim();
            string topic = cmbFavouriteTopic.Text.Trim();

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(topic))
            {
                MessageBox.Show("Please enter your name and select a topic.");
                return;
            }

            // Initializes the chatbot and pass user infomation
            ChatbotLogic chatbot = new ChatbotLogic();
            chatbot.SetUserInfo(userName, topic);

            MessageBox.Show($"Welcome {userName}! Your Favourite topic {topic} is noted!", "Cyber Advice", MessageBoxButton.OK, MessageBoxImage.Information);

            // Passes the chatbot object to the ChatPage
            ChatPage chatPage = new ChatPage(chatbot);
            chatPage.Show();
            this.Close(); 
        }
    }
}