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
using OpenPop.Pop3;
using OpenPop.Mime;
using System.Net.Mail;
using System.Net;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Fitness
{
    /// <summary>
    /// Логика взаимодействия для LoginOrRegistration.xaml
    /// </summary>
    public partial class LoginOrRegistration : Window
    {
        BD bd = new BD();
        public LoginOrRegistration()
        {
            InitializeComponent();
            LoginButton.Visibility = Visibility.Visible;
            RegistrationButton.Visibility = Visibility.Visible;
            Registration.Visibility = Visibility.Hidden;
            Login.Visibility = Visibility.Hidden;
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            LoginButton.Visibility = Visibility.Hidden;
            RegistrationButton.Visibility = Visibility.Hidden;
            Registration.Visibility = Visibility.Visible;
        }

        private void Back_Click(object sender, RoutedEventArgs e)// вернуться на главную
        {
            LoginButton.Visibility = Visibility.Visible;
            RegistrationButton.Visibility = Visibility.Visible;
            Registration.Visibility = Visibility.Hidden;
            Login.Visibility = Visibility.Hidden;
            EMail.Text = "Введите E-Mail";
            Mail.Text = "Введите E-Mail";
            PersonName.Text = "Введите nickname";
        }

        private void EMail_GotFocus(object sender, RoutedEventArgs e)
        {
            EMail.Text = "";
        }

        private void EMail_LostFocus(object sender, RoutedEventArgs e)
        {
            if(EMail.Text=="")
                EMail.Text = "Введите E-Mail";
        }
        private void Mail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Mail.Text == "")
                Mail.Text = "Введите E-Mail";
        }
        private void Mail_GotFocus(object sender, RoutedEventArgs e)
        {
            Mail.Text = "";
        }

        private void PersonName_LostFocus(object sender, RoutedEventArgs e)
        {
            if(PersonName.Text=="")
                PersonName.Text = "Введите nickname";
        }

        private void PersonName_GotFocus(object sender, RoutedEventArgs e)
        {
            PersonName.Text = "";
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginButton.Visibility = Visibility.Hidden;
            RegistrationButton.Visibility = Visibility.Hidden;
            Login.Visibility = Visibility.Visible;
        }
        private bool CheckForCreating()//Проверка на наличие подобного юзера в бд
        {
            try
            {
                List<object> list = bd.Get("mail");
                foreach (var a in list)
                {
                    if ((string)a == EMail.Text)
                    {
                        Error.Content = "Пользователь с такой почтой уже существет";
                        EMail.BorderBrush = Brushes.Red;
                        throw new Exception();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }

        }
        private Boolean Enter(String adress, String password, String POP3, Int32 port)
        {
            try
            {
                using (Pop3Client client = new Pop3Client())
                {
                    client.Connect(POP3, port, true);
                    client.Authenticate(adress, password);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        private bool CheckMailAndPassword(string Mail, string Password)//Проверка почты
        {
            String POP3_server = "";
            Int32 POP3_port = 0;
            try
            {
                switch (Mail.Split('@')[1])
                {
                    case "mail.ru":
                        {
                            POP3_server = "pop.mail.ru";
                            POP3_port = 995;
                            break;
                        }
                    case "yandex.ru":
                        {
                            POP3_server = "pop.yandex.ru";
                            POP3_port = 995;
                            break;
                        }
                    case "gmail.com":
                        {
                            POP3_server = "pop.gmail.com";
                            POP3_port = 995;
                            break;
                        }
                        
                    default:
                        {
                            Error.Content = "Данная почта не поддерживается.";
                            EMail.BorderBrush = Brushes.Red;
                            break;
                        }
                }
            }
            catch
            {
                Error.Content = "Неправильная почта.";
                EMail.BorderBrush = Brushes.Red;
                return false;
            }
            if (!Enter(Mail, Password, POP3_server, POP3_port))
            {
                Error.Content = "Неправильный пароль.";
                PasswordBox2.BorderBrush = Brushes.Red;
                return false;
            }

            return true;
        }

        private void ButtonForRegistration_Click(object sender, RoutedEventArgs e)
        {
            Error.Content = "";
            PasswordBox2.BorderBrush = Brushes.Black;
            EMail.BorderBrush = Brushes.Black;
            if (CheckMailAndPassword(EMail.Text, PasswordBox2.Password))
            {
                if (CheckForCreating())//Добавление usera
                {
                    try
                    {
                        if (String.Compare(PersonName.Text, "Введите nickname") == 0)
                            new Exception("Введите nickname");
                        if (PersonName.Text.IndexOf(' ') >= 0)
                            new Exception("Nickname не должен содержать пробелов");
                        string connectionString = @"Data Source=.;Initial Catalog=Fitness;Integrated Security=True";
                        string sqlAdd;
                        SqlConnection connection = new SqlConnection(connectionString);
                        connection.Open();
                        sqlAdd = String.Format("INSERT INTO Users (Mail, Password, Name) VALUES ('{0}', '{1}','{2}')", EMail.Text, PasswordBox2.Password, PersonName.Text);
                        SqlCommand command = new SqlCommand(sqlAdd, connection);
                        int number = command.ExecuteNonQuery();
                        connection.Close();
                        SqlConnection connection1 = new SqlConnection(connectionString);
                        connection1.Open();
                        sqlAdd = String.Format("INSERT INTO Stat (Mail,[Ккал],[Упражнений на пресс выполнено],[Упражнений на бицепс выполнено],[Упражнений на грудь выполнено],[Упражнений на плечи выполнено],[Упражнений на ноги выполнено],[Упражнений на спину выполнено]) VALUES ('{0}',0,0,0,0,0,0,0)", EMail.Text);
                        SqlCommand command1 = new SqlCommand(sqlAdd, connection1);
                        int number1 = command1.ExecuteNonQuery();
                        connection1.Close();
                        BufferClass.Mail = EMail.Text;
                        BufferClass.Password = PasswordBox2.Password;
                        BufferClass.Name = PersonName.Text;
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        Close();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    

                }
            }
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            Err.Content = "";
            PasswordBox1.BorderBrush = Brushes.Black;
            Mail.BorderBrush = Brushes.Black;
            if (String.Compare(Mail.Text, "Введите E-Mail" ) != 0)
            {
                
                if (PasswordBox1.Password != "")
                {
                    List<object> bdMail = bd.Get("mail");
                    List<object> bdPassword = bd.Get("password");
                    for (int a = 0; a < bdMail.Count; a++)
                    {
                        if ((string)bdMail[a] == Mail.Text)
                        {
                            if ((string)bdPassword[a] == PasswordBox1.Password)
                            {
                                try
                                {
                                    BufferClass.Mail = Mail.Text;
                                    BufferClass.Password = PasswordBox1.Password;
                                    List<object> person = bd.GetSomthPerson(BufferClass.Mail);
                                    if (person.Count > 2)
                                        BufferClass.Name = (string)person[2];
                                    if (person.Count > 3)
                                        BufferClass.Photo = (string)person[3];
                                    MainWindow mainWindow = new MainWindow();
                                    mainWindow.Show();
                                    Close();
                                }
                                catch(Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            else
                            {
                                Err.Content = "Неверный пароль.";
                                PasswordBox1.BorderBrush = Brushes.Red;
                            }
                        }
                    }
                    Err.Content = "Неверная почта.";
                    Mail.BorderBrush = Brushes.Red;
                }
                else
                {
                    Err.Content = "Введите пароль.";
                    PasswordBox1.BorderBrush = Brushes.Red;
                }
            }
            else
            {
                Err.Content = "Введите почту.";
                Mail.BorderBrush = Brushes.Red;
            }
        }
    }
}
