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
using System.Windows.Threading;

namespace Fitness
{
    /// <summary>
    /// Логика взаимодействия для MyTimer.xaml
    /// </summary>
    public partial class MyTimer : Window
    {
        DateTime date;
        DispatcherTimer timer = new DispatcherTimer();
        public MyTimer()
        {
            InitializeComponent();
        }

        private void StartTimer_Click(object sender, RoutedEventArgs e)
        {

            date = DateTime.Now;


            timer.Tick += new EventHandler(time_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }
        private void time_Tick(object sender, EventArgs e)
        {
            // Updating the Label which displays the current second 
            long tick = DateTime.Now.Ticks - date.Ticks;
            DateTime stopWatch = new DateTime();

            stopWatch = stopWatch.AddTicks(tick);
            lblSec.Content = String.Format("{0:HH:mm:ss}", stopWatch);

            // Forcing the CommandManager to raise the RequerySuggested event 

        }

        private void StopTimer_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            lblSec.Content = "00:00:00";
        }
    }
}
