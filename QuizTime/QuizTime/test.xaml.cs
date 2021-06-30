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

namespace QuizTime
{
    /// <summary>
    /// Interaction logic for test.xaml
    /// </summary>
    public partial class test : Window
    {
        private List<string> listSelectedImages = new List<string>();
        private int number;
        private int numberOfQuestions;
        private int getal = 10;
        private DispatcherTimer timer = new DispatcherTimer();
        public test()
        {
            InitializeComponent();
            btnTest.Click += BtnTest_Click;
            numberOfQuestions = 6;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            getal--;
            lblGetal.Content = getal.ToString();

            if (getal == 0)
            {
                MessageBox.Show("De tijd is om");
            }
        }

        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {

            timer.Start();
            /*number++;


            if (listSelectedImages.Count < numberOfQuestions)
            {
                getal = numberOfQuestions - listSelectedImages.Count;
                for (int i = 0; i < getal; i++) // Loop through List with for
                {
                    listSelectedImages.Add($"");
                }
            }

            if (number == 1)
            {
                listSelectedImages[0] = "test0";
            }
            else if (number == 3)
            {
                listSelectedImages[2] = "test2";
            }
            else if (number == 5)
            {
                listSelectedImages[4] = "test4";
            }

            for (int i = 0; i < listSelectedImages.Count; i++) // Loop through List with for
            {
                MessageBox.Show(listSelectedImages[i]);
            }
            MessageBox.Show(listSelectedImages.Count.ToString());*/
        }
    }
}
