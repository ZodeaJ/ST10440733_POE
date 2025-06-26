YouTube Link: https://youtu.be/Xv4HWNgWFzo
Project Classes 

ChatHandler.cs 

Supports the main chatbot logic including the different mode like Cyber Advice, Task Management and Quiz mode. 
It interprets input provided by a user, keeps conversation state, understands natural language to complete tasks and reminders, and regulates the quiz flow.  

CyberAssistant.cs  

Serves as the back end service that deals with the tasks (add, complete, delete), natural language parsing of reminder times 
and user activity logging. It gives the core processes that ChatHandler utilizes in order to undertake task related activities. 

ChatPage.xaml.cs 

Handles the graphical interface control with the user. It controls the button clicks, shows conversation history, 
alternates the modes (Cyber Advice and Task Management) and transfers the inputs made by the user to ChatHandler (giving the responses of the chatbot). 

MainWindow.xaml.cs 

The first window in which the user launched the application. When loaded it presents ASCII art and plays a voice greeting.
The users provide their name, as well as choose a favorite cybersecurity subject. When the start button is clicked, a verification process is carried out 
after which the chatbot retrieves the user information and renders a welcome message followed by the opening of the ChatPage window to start the chat session, alongside closing the startup program. 

Overall Project Achivement 

The present project provides a chatbot cybersecurity application that has a number of user modes: Cyber Advice, Task Management, and a Cybersecurity Quiz. 
It has a graphene, convenient and logical system of planning the tasks and remembering them, switching modes without any hindrance, and exciting quiz features. 
This design focuses on user friendliness, using a clear prompt and logging of activity, but since it is kept highly modular and clean separation of concern, class to class. 
What you get is an efficient, adaptable helper who can guide his users on the subject of cybersecurity, assist them in keeping their schedules straight, and give them their wits and knowledge under test in an user friendly, 
responsive interface. 
