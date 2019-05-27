using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Fitness
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BD bd = new BD();
        delegate void sendMail();
        public MainWindow()
        {
            InitializeComponent();
            PersonNameLabel.Content = BufferClass.Name;
            if (BufferClass.Photo != null && System.IO.File.Exists(BufferClass.Photo))
            {
                PersonPhoto.Stretch = Stretch.UniformToFill;
                PersonPhoto.Source = new BitmapImage(new Uri(BufferClass.Photo));
                PersonAccountPhoto.Stretch = Stretch.UniformToFill;
                PersonAccountPhoto.Source = new BitmapImage(new Uri(BufferClass.Photo));
            }
            else
            {
                PersonPhoto.Stretch = Stretch.UniformToFill;
                PersonPhoto.Source = new BitmapImage(new Uri("E:/Kirill/Code/blablabla/Fitness/Fitness/Image/StandartPhotoProfile.PNG"));
                PersonAccountPhoto.Stretch = Stretch.UniformToFill;
                PersonAccountPhoto.Source = new BitmapImage(new Uri("E:/Kirill/Code/blablabla/Fitness/Fitness/Image/StandartPhotoProfile.PNG"));
            }
        }
        private void SetName_GotFocus(object sender, RoutedEventArgs e)
        {
            SetName.Text = "";
        }
        private void SetName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SetName.Text == "")
                SetName.Text = "Введите имя";
        }

        private void MoveMenu(double x1,double x2)
        {
            var animation = new ThicknessAnimation();
            animation.From = new Thickness(x1, 0, 0, 0);
            animation.To = new Thickness(x2, 0, 0, 0);
            animation.SpeedRatio = 2;
            Menu.BeginAnimation(MarginProperty, animation);
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)//Анимация
        {
            if (Menu.Margin.Left != -105)
            {
                MoveMenu(Menu.Margin.Left, -105);
            }
            else
            {
                MoveMenu(-105, 0);
            }
            
        }

        private void PersonAcc_Click(object sender, RoutedEventArgs e)
        {
            PersonAccount.Visibility = Visibility.Visible;
            Lessons.Visibility = Visibility.Hidden;
            Eat.Visibility = Visibility.Hidden;
            Stat.Visibility = Visibility.Hidden;
            MoveMenu(0, -105);
        }

        private void ChangePhoto_Click(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog();
            openDialog.Filter ="Image files (*.BMP, *.JPG, *.GIF, *.TIF, *.PNG, *.ICO, *.EMF, *.WMF)|*.bmp;*.jpg;*.gif; *.tif; *.png; *.ico; *.emf; *.wmf";

            if (openDialog.ShowDialog() == true)
            {
                PersonPhoto.Source = new BitmapImage(new Uri(openDialog.FileName));
                PersonAccountPhoto.Source = new BitmapImage(new Uri(openDialog.FileName));
                bd.AddSomthPerson(BufferClass.Mail, "Photo", openDialog.FileName);
                BufferClass.Photo = openDialog.FileName;
                MessageBox.Show("Ваше фото изменено");
            }
        }

        private void ChangeName_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SetName.Text.IndexOf(' ') >= 0)
                    new Exception("Nickname не должен содержать пробелов");
                if (String.Compare(SetName.Text, "Введите nickname") != 0)
                {
                    BufferClass.Name = SetName.Text;
                    PersonNameLabel.Content = BufferClass.Name;
                    bd.AddSomthPerson(BufferClass.Mail, "Name", BufferClass.Name);
                    MessageBox.Show("Ваш nickname изменен.");
                }
                else
                {
                    MessageBox.Show("Введите nickname");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            
        }

        private void ChangeMail_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("На вашу почту отправлено письмо с кодом для смены почты.");
            ChangeMailWindow changeMailWindow = new ChangeMailWindow();
            changeMailWindow.ShowDialog();
        }
        public void AddButton(List<object> list, int q)
        {
            wrapPanel.Children.Clear();
            for (int a = 0; a <= list.Count; a++)
            {
                if (q == 1 && a == 0)
                    a++;
                Button button = new Button();
                button.Name = "Button" + Convert.ToString(a);
                if (a == 0 && q != 1)
                    button.Content = "Назад";
                else
                    button.Content = (string)list[a - 1];
                button.Width = 800;
                button.Height = 100;
                button.Background = Brushes.White;
                button.Foreground = Brushes.Red;
                button.FontFamily = new FontFamily("Segoe UI Semibold");
                button.FontSize = 18;

                if (q == 1 && a != 0)
                    button.Click += Button_Click;
                else if (q == 2 && a != 0)
                    button.Click += Button1_Click;
                else if (q != 1 && a == 0)
                    button.Click += ButtonBack_Click;
                wrapPanel.Children.Add(button);

            }
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            List<object> list = bd.Get("Группы мышц", "Trainer");
            AddButton(list, 1);
        }

        string ourBd = "";
        string source = "";
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            ok.Visibility = Visibility.Visible;
            Great.Visibility = Visibility.Hidden;
            Button button = (Button)sender;
            List<object> PersonStat = bd.GetSomthPersonStat(BufferClass.Mail);
            List<object> ThisLess1 = bd.GetSomthLess((string)button.Content, ourBd);
            bd.UpdateStat(BufferClass.Mail, "Ккал", Convert.ToString(Convert.ToInt32(PersonStat[1]) - Convert.ToInt32(ThisLess1[2])));
            int percent = 0;
            switch (ourBd)
            {
                case "Press":
                    {
                        percent = (int)PersonStat[2];
                        break;
                    }
                case "Biceps":
                    {
                        percent = (int)PersonStat[3];
                        break;
                    }
                case "ChestMuscles":
                    {
                        percent = (int)PersonStat[4];
                        break;
                    }
                case "Shoulder":
                    {
                        percent = (int)PersonStat[5];
                        break;
                    }
                case "Legs":
                    {
                        percent = (int)PersonStat[6];
                        break;
                    }
                case "Rear":
                    {
                        percent = (int)PersonStat[7];
                        break;
                    }
            }
            int numberOfPovt = 12 + (12 * 5 * percent / 100);
            int numberOfPodx = 4;
            LabelForLess.Content = "Количетсво подходов: " + Convert.ToString(numberOfPodx) + ". Количество повторений: " + Convert.ToString(numberOfPovt);
            List1.Visibility = Visibility.Hidden;
            ThisLess.Visibility = Visibility.Visible;
            NameLesson.Content = button.Content;
            List<object> list = bd.Get("Упражнение", ourBd);
            List<object> list1 = bd.Get("Видео", ourBd);
           
            for (int a = 0; a < list.Count; a++)
            {
                if ((string)button.Content == (string)list[a])
                {
                    source = (string)list1[a];
                    break;
                }
            }
            MyVideo.Source = new Uri(source);
            MyVideo.Play();
        }

        private void MyVideo_MediaEnded_1(object sender, RoutedEventArgs e)
        {
            MyVideo.Source = new Uri(source);
            MyVideo.Play();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {            
            Button button = (Button)sender;
            List<object> list = bd.Get("Группы мышц", "Trainer");
            List<object> list1 = bd.Get("Таблица", "Trainer");
            
            for(int a = 0; a < list.Count; a++)
            {
                if ((string)button.Content == (string)list[a])
                {
                    ourBd = (string)list1[a];
                    break;
                }
            }
            List<object> list2 = bd.Get("Упражнение", ourBd);
            AddButton(list2, 2);
        }

        private void less_Click(object sender, RoutedEventArgs e)
        {
            
            PersonAccount.Visibility = Visibility.Hidden;
            Lessons.Visibility = Visibility.Visible;
            List1.Visibility = Visibility.Visible;
            ThisLess.Visibility = Visibility.Hidden;
            Eat.Visibility = Visibility.Hidden;
            Stat.Visibility = Visibility.Hidden;
            MoveMenu(0, -105);
            List<object> list = bd.Get("Группы мышц", "Trainer");
            AddButton(list, 1);
        }

        private void BackLess_Click(object sender, RoutedEventArgs e)
        {
            List1.Visibility = Visibility.Visible;
            ThisLess.Visibility = Visibility.Hidden;
            List<object> list2 = bd.Get("Упражнение", ourBd);
            AddButton(list2, 2);
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ok.Visibility = Visibility.Hidden;
            Great.Visibility = Visibility.Visible;
            List<object> PersonStat = bd.GetSomthPersonStat(BufferClass.Mail);
            switch (ourBd)
            {
                case "Press":
                    {
                        int data = (int)PersonStat[2] + 1;
                        bd.UpdateStat(BufferClass.Mail, "[Упражнений на пресс выполнено]", Convert.ToString(data));
                        break;
                    }
                case "Biceps":
                    {
                        int data = (int)PersonStat[3] + 1;
                        bd.UpdateStat(BufferClass.Mail, "[Упражнений на бицепс выполнено]", Convert.ToString(data));
                        break;
                    }
                case "ChestMuscles":
                    {
                        int data = (int)PersonStat[4] + 1;
                        bd.UpdateStat(BufferClass.Mail, "[Упражнений на грудь выполнено]", Convert.ToString(data));
                        break;
                    }
                case "Shoulder":
                    {
                        int data = (int)PersonStat[5] + 1;
                        bd.UpdateStat(BufferClass.Mail, "[Упражнений на плечи выполнено]", Convert.ToString(data));
                        break;
                    }
                case "Legs":
                    {
                        int data = (int)PersonStat[6] + 1;
                        bd.UpdateStat(BufferClass.Mail, "[Упражнений на ноги выполнено]", Convert.ToString(data));
                        break;
                    }
                case "Rear":
                    {
                        int data = (int)PersonStat[7] + 1;
                        bd.UpdateStat(BufferClass.Mail, "[Упражнений на спину выполнено]", Convert.ToString(data));
                        break;
                    }
            }
        }

        private void eatt_Click(object sender, RoutedEventArgs e)
        {
            PersonAccount.Visibility = Visibility.Hidden;
            Lessons.Visibility = Visibility.Hidden;
            Eat.Visibility = Visibility.Visible;
            Stat.Visibility = Visibility.Hidden;
            WhatNeed.Visibility = Visibility.Visible;
            EatWhatWant.Visibility = Visibility.Hidden;
            Dish.Visibility = Visibility.Hidden;
            MoveMenu(0, -105);

        }

        void eatMenu(List<object> list, StackPanel stack, List<object> photo)
        {
            stack.Children.Clear();
            for (int a = 0; a < list.Count; a++)
            {

                Button button = new Button();
                ImageBrush imBr = new ImageBrush();
                imBr.ImageSource = new System.Windows.Media.Imaging.BitmapImage(new Uri((string)photo[a], UriKind.Relative));
                imBr.Stretch = Stretch.UniformToFill;
                button.Name = "Button" + Convert.ToString(a);
                button.Background = imBr;
                button.Width = 150;
                button.Height = 145;
                button.Foreground = Brushes.Red;
                button.FontFamily = new FontFamily("Segoe UI Semibold");
                button.FontSize = 18;
                button.Click += EatButton_Click;
                stack.Children.Add(button);

            }
        }
        string fullName = "";

        private void EatButton_Click(object sender, RoutedEventArgs e)
        {
            EatWhatWant.Visibility = Visibility.Hidden;
            Dish.Visibility = Visibility.Visible;
            Button button = (Button)sender;
            char[] number = button.Name.ToCharArray();
            int indx = (int)Char.GetNumericValue(number[number.Length - 1]);
            StackPanel stack = (StackPanel)button.Parent;
            fullName = nameBd + "_" + stack.Name;
            List<object> list = bd.Get("Упражнение", nameBd + "_" + stack.Name);
            List<object> listPhoto = bd.Get("Видео", nameBd + "_" + stack.Name);
            NameEat.Content = (string)list[indx];
            PhotoEat.Stretch = Stretch.UniformToFill;
            PhotoEat.Source = new BitmapImage(new Uri((string)listPhoto[indx]));
            List<object> IngridientsList = bd.Get("name", nameBd + "_" + stack.Name);
            List<object> ReceptList = bd.Get("photo", nameBd + "_" + stack.Name);
            string text = System.IO.File.ReadAllText((string)IngridientsList[indx], Encoding.Default).Replace("\n", " ");
            ingridients.Text = text;
            text = System.IO.File.ReadAllText((string)ReceptList[indx], Encoding.Default).Replace("\n", " ");
            Recepts.Text = text;
        }

        string nameBd = "";


        private void Muscule_Click(object sender, RoutedEventArgs e)// вывод блюд
        {
            Button button = (Button)sender;
            WhatNeed.Visibility = Visibility.Hidden;
            EatWhatWant.Visibility = Visibility.Visible;
            nameBd = button.Name;
            List<StackPanel> stackPanels = new List<StackPanel>();
            stackPanels.Add(day1);
            stackPanels.Add(day2);
            stackPanels.Add(day3);
            stackPanels.Add(day4);
            stackPanels.Add(day5);
            stackPanels.Add(day6);
            stackPanels.Add(day7);
            for (int a = 0; a < stackPanels.Count; a++)
            {
                List<object> ListDay1 = bd.Get("Упражнение", nameBd + "_day" + (a + 1));
                List<object> listPhoto = bd.Get("Видео", nameBd + "_day" + (a + 1));
                eatMenu(ListDay1, stackPanels[a], listPhoto);
            }
            
        }

        private void EatBack_Click(object sender, RoutedEventArgs e)
        {
            WhatNeed.Visibility = Visibility.Hidden;
            EatWhatWant.Visibility = Visibility.Visible;
            cooking.Visibility = Visibility.Visible;
            GreatCooking.Visibility = Visibility.Hidden;
            Dish.Visibility = Visibility.Hidden;
        }
        
        private void cooking_Click(object sender, RoutedEventArgs e)//добавление в статистику
        {
            cooking.Visibility = Visibility.Hidden;
            GreatCooking.Visibility = Visibility.Visible;
            List<object> ThisStat = bd.GetSomthPersonStat(BufferClass.Mail);
            List<object> thisDish = bd.GetSomthDish((string)NameEat.Content, fullName);
            bd.UpdateStat(BufferClass.Mail, "Ккал", Convert.ToString(Convert.ToInt32(ThisStat[1]) + Convert.ToInt32(thisDish[4])));
            if((Convert.ToInt32(ThisStat[1]) + Convert.ToInt32(thisDish[4])) > 6000)
            {
                sendMail sendMail1 = () =>
                {
                    try
                    {
                        MailAddress from = new MailAddress("fitnesstrainerbstu2019@mail.ru", "FitnessTrainer");
                        MailAddress to = new MailAddress(BufferClass.Mail);
                        MailMessage m = new MailMessage(from, to);
                        m.Subject = "Напоминание о тренировках";
                        m.Body = "<h2>" + "Вы слишком много набрали, пора позаниматься." + "</h2>";
                        m.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
                        smtp.Credentials = new NetworkCredential("fitnesstrainerbstu2019@mail.ru", "23892389k");
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

        }

        private void EatBack2_Click(object sender, RoutedEventArgs e)
        {
            WhatNeed.Visibility = Visibility.Visible;
            EatWhatWant.Visibility = Visibility.Hidden;
            cooking.Visibility = Visibility.Visible;
            GreatCooking.Visibility = Visibility.Hidden;
            Dish.Visibility = Visibility.Hidden;
        }

        private void statist_Click(object sender, RoutedEventArgs e)
        {
            PersonAccount.Visibility = Visibility.Hidden;
            Lessons.Visibility = Visibility.Hidden;
            Eat.Visibility = Visibility.Hidden;
            Stat.Visibility = Visibility.Visible;
            MoveMenu(0, -105);
            List<object> StatThisPerson = bd.GetSomthPersonStat(BufferClass.Mail);
            StatForAcc.Content = "Статистика аккаунта " + StatThisPerson[0];
            Kkal.Content = "Количество ккал: " + StatThisPerson[1];
            PressStat.Content = "Упражнений на пресс выполнено: " + StatThisPerson[2];
            BicepsStat.Content = "Упражнений на бицепс выполнено: " + StatThisPerson[3];
            GrydStat.Content = "Упражнений на грудь выполнено: " + StatThisPerson[4];
            PlechiStat.Content = "Упражнений на плечи выполнено: " + StatThisPerson[5];
            LegsStat.Content = "Упражнений на ноги выполнено: " + StatThisPerson[6];
            SpinaStat.Content = "Упражнений на спину выполнено: " + StatThisPerson[7];
        }

        private void getStat_Click(object sender, RoutedEventArgs e)
        {
            sendMail sendMail1 = () =>
            {
                try
                {
                    /////Отправка сообщения
                    MailAddress from = new MailAddress("fitnesstrainerbstu2019@mail.ru", "FitnessTrainer");
                    MailAddress to = new MailAddress(BufferClass.Mail);
                    MailMessage m = new MailMessage(from, to);
                    m.Subject = "Статистика аккаунта";
                    m.Body = "<h2>" + StatForAcc.Content + "<br/>" + Kkal.Content + "<br/>" + PressStat.Content + "<br/>" + BicepsStat.Content + "<br/>" + GrydStat.Content + "<br/>" + PlechiStat.Content + "<br/>" + LegsStat.Content + "<br/>" + SpinaStat.Content + "<br/>" + "</h2>";
                    m.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
                    smtp.Credentials = new NetworkCredential("fitnesstrainerbstu2019@mail.ru", "23892389k");
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

        private void ClearStat_Click(object sender, RoutedEventArgs e)
        {
            bd.UpdateStat(BufferClass.Mail, "Ккал", "0");
            bd.UpdateStat(BufferClass.Mail, "[Упражнений на пресс выполнено]", "0");
            bd.UpdateStat(BufferClass.Mail, "[Упражнений на бицепс выполнено]", "0");
            bd.UpdateStat(BufferClass.Mail, "[Упражнений на грудь выполнено]", "0");
            bd.UpdateStat(BufferClass.Mail, "[Упражнений на плечи выполнено]", "0");
            bd.UpdateStat(BufferClass.Mail, "[Упражнений на ноги выполнено]", "0");
            bd.UpdateStat(BufferClass.Mail, "[Упражнений на спину выполнено]", "0");
            List<object> StatThisPerson = bd.GetSomthPersonStat(BufferClass.Mail);
            StatForAcc.Content = "Статистика аккаунта " + StatThisPerson[0];
            Kkal.Content = "Количество ккал: " + StatThisPerson[1];
            PressStat.Content = "Упражнений на пресс выполнено: " + StatThisPerson[2];
            BicepsStat.Content = "Упражнений на бицепс выполнено: " + StatThisPerson[3];
            GrydStat.Content = "Упражнений на грудь выполнено: " + StatThisPerson[4];
            PlechiStat.Content = "Упражнений на плечи выполнено: " + StatThisPerson[5];
            LegsStat.Content = "Упражнений на ноги выполнено: " + StatThisPerson[6];
            SpinaStat.Content = "Упражнений на спину выполнено: " + StatThisPerson[7];
        }

        private void ShowTimer_Click(object sender, RoutedEventArgs e)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.ShowDialog();
        }
    }
}
