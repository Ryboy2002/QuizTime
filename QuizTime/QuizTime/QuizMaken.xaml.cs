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

namespace QuizTime
{
    /// <summary>
    /// Interaction logic for QuizMaken.xaml
    /// </summary>
    public partial class QuizMaken : Window
    {
        public QuizMaken()
        {
            InitializeComponent();
            btnMaken.Click += BtnMaken_Click;         
        }

        private void BtnMaken_Click(object sender, RoutedEventArgs e)
        {
            QuizAanmaken QuizAanmaken = new QuizAanmaken();
            QuizAanmaken.Show();
            this.Close();
            
            
        }
    }
}
