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
    /// Interaction logic for QuizSpelenUser.xaml
    /// </summary>
    public partial class QuizSpelenUser : Window
    {
        private string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        public QuizSpelenUser()
        {
            InitializeComponent();
            PointCollection polygonColl = new PointCollection();
            polygonColl.Add(new Point(0, 80));
            polygonColl.Add(new Point(40, 10));
            polygonColl.Add(new Point(80, 80));
            polygonTriangle.Points = polygonColl;
        }

        public void StartGame(string titel, string numberOfQuestions)
        {
            lblQuizTitel.Content = titel;
            lblNumberOfQuestions.Content = numberOfQuestions;
        }

        public void StartCheck(string titel)
        {
            lblQuizTitelCheck.Content = titel;
            gridQuizVragen.Visibility = Visibility.Collapsed;
            gridQuizVragenCheck.Visibility = Visibility.Collapsed;
            gridTussenScherm.Visibility = Visibility.Collapsed;
            gridTussenSchermCheck.Visibility = Visibility.Visible;
        }

        public void UpdateTimer(int getalTimer)
        {
            lblTimer.Content = getalTimer;
        }

        public void NextQuestion(string answer1, string answer2, string answer3, string answer4, string question, int getal, string image)
        {
            gridQuizVragen.Visibility = Visibility.Visible;
            gridTussenScherm.Visibility = Visibility.Collapsed;
            lblAnswer1.Content = answer1;
            lblAnswer2.Content = answer2;
            lblAnswer3.Content = answer3;
            lblAnswer4.Content = answer4;
            lblVraag.Content = question;

            if (image != null)
            {
                baseDir = baseDir.Replace(@"bin\Debug\Images", "");
                baseDir = baseDir + @"Images\" + image;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(baseDir);
                bitmap.EndInit();
                imgQuestion.Source = bitmap;
                borderGridImage.BorderThickness = new Thickness(0);
                baseDir = baseDir.Replace(string.Format(@"Images\{0}", image), "");
            } else
            {
                imgQuestion.Source = null;
            }
        }

        public void NextQuestionAnswer(string answer1, string answer2, string answer3, string answer4, string question, int getal, string image)
        {
           
            lblAnswer1Check.Content = answer1;
            lblAnswer2Check.Content = answer2;
            lblAnswer3Check.Content = answer3;
            lblAnswer4Check.Content = answer4;
            lblVraagCheck.Content = question;

            lblAnswer1Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#E21B3C");
            lblAnswer2Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#1368CE");
            lblAnswer3Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#D89E00");
            lblAnswer4Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#26890C");

            if (image != null)
            {
                baseDir = baseDir.Replace(@"bin\Debug\Images", "");
                baseDir = baseDir + @"Images\" + image;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(baseDir);
                bitmap.EndInit();
                imgQuestionCheck.Source = bitmap;
                borderGridImageCheck.BorderThickness = new Thickness(0);
                baseDir = baseDir.Replace(string.Format(@"Images\{0}", image), "");
            }
            else
            {
                imgQuestionCheck.Source = null;
            }
        }

        public void ShowAnswer(int rightAnswer)
        {
            switch (rightAnswer)
            {
                case 1:
                    gridAnswer2Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    gridAnswer3Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    gridAnswer4Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    break;
                case 2:
                    gridAnswer1Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    gridAnswer3Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    gridAnswer4Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    break;
                case 3:
                    gridAnswer1Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    gridAnswer2Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    gridAnswer4Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    break;
                case 4:
                    gridAnswer1Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    gridAnswer2Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    gridAnswer3Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    break;
            }
        }

        public void NextDivider(string numberOfQuestion)
        {
            gridQuizVragen.Visibility = Visibility.Collapsed;
            gridTussenScherm.Visibility = Visibility.Visible;
            lblQuestionNumber.Content = numberOfQuestion;
        }
    }
}
