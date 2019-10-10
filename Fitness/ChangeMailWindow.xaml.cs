using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
using System.Data.SqlClient;
using System.Threading;

namespace Fitness
{
    /// <summary>
    /// Логика взаимодействия для ChangeMailWindow.xaml
    /// </summary>
    public partial class ChangeMailWindow : Window
    {
        BD bd = new BD();
        string CodeForChangeMail;
        delegate void sendMail();
        public ChangeMailWindow()
        {
            InitializeComponent();
            Random random = new Random();
            CodeForChangeMail = Convert.ToString(random.Next(1000, 9999));
            sendMail sendMail1 = () =>
            {
                try
                {
                    /////Отправка сообщения
                    MailAddress from = new MailAddress(".............", "FitnessTrainer");// тут с какой почты будет отправлятся сообщение, и типа псевдоним
                    MailAddress to = new MailAddress(BufferClass.Mail); // кому будет отправлятся сообщение
                    MailMessage m = new MailMessage(from, to);
                    m.Subject = "Код для смены почты"; // тема сообщения
                    m.Body = "<h2>Код для смены почты " + CodeForChangeMail + "</h2>"; // тута сообщение
                    m.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587); // это смтп сервер и смтп порт для отправки с mail.ru, если с другой почты, то это надо будет поменять
                    smtp.Credentials = new NetworkCredential(".................", "............"); // здесь почта и пароль для отправки с нее
                    smtp.EnableSsl = true;
                    smtp.Send(m);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            };
            Thread thread = new Thread(new ThreadStart(sendMail1));
            thread.Start();

        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            if (CodeForChangeMail == TextBox1.Text)
            {
                Code.Visibility = Visibility.Hidden;
                ChangeMailAndPassword.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Код введен неверно. Повторите попытку.");
            }
        }
        private bool CheckForCreating()//Проверка на наличие подобного юзера в бд
        {
            try
            {
                List<object> list = bd.Get("mail");
                foreach (var a in list)
                {
                    if ((string)a == TextBox2.Text)
                    {
                        MessageBox.Show("Пользователь с такой почтой уже существет");
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
                            MessageBox.Show( "Данная почта не поддерживается.");
                            break;
                        }
                }
            }
            catch
            {
                MessageBox.Show("Неправильная почта.");
                return false;
            }
            if (!Enter(Mail, Password, POP3_server, POP3_port))
            {
                MessageBox.Show("Неправильный пароль.");
                return false;
            }

            return true;
        }

        private void ChangeMail_Click(object sender, RoutedEventArgs e)
        {
            if (String.Compare(TextBox2.Text, "") != 0)
            {
                if (CheckMailAndPassword(TextBox2.Text, PasswordBox1.Password))
                {
                    if (CheckForCreating())
                    {
                        try
                        {
                            bd.AddSomthPerson(BufferClass.Mail, "Mail", TextBox2.Text);
                            bd.AddSomthPerson(BufferClass.Mail, "Mail", TextBox2.Text, "Stat");
                            bd.AddSomthPerson(TextBox2.Text, "Password", PasswordBox1.Password);
                            BufferClass.Mail = TextBox2.Text;
                            BufferClass.Password = PasswordBox1.Password;
                            Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }


                    }


                }

            }
            else
                MessageBox.Show("Введите почту");
        }
    }
}
