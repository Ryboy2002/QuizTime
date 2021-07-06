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
            polygonTriangleCheck.Points = polygonColl;
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

            if (lblAnswer3.Content == "" && lblAnswer4.Content == "")
            {
                gridAnswer3.Visibility = Visibility.Collapsed;
                gridAnswer4.Visibility = Visibility.Collapsed;
                gridAnswer1.Margin = new Thickness(0, 761, 0, 0);
                gridAnswer2.Margin = new Thickness(0, 898, 0, 0);
                gridAnswer1.Width = 1882;
                gridAnswer2.Width = 1882;
                
            } else if (lblAnswer4.Content == "")
            {
                gridAnswer4.Visibility = Visibility.Collapsed;
                gridAnswer3.Visibility = Visibility.Visible;
                gridAnswer3.Margin = new Thickness(0, 898, 0, 0);
                gridAnswer1.Margin = new Thickness(-945, 761, 0, 0);
                gridAnswer2.Margin = new Thickness(949, 761, 0, 0);
                gridAnswer3.Width = 1882;
                gridAnswer1.Width = 935;
                gridAnswer2.Width = 935;
            } else
            {
                gridAnswer1.Visibility = Visibility.Visible;
                gridAnswer2.Visibility = Visibility.Visible;
                gridAnswer3.Visibility = Visibility.Visible;
                gridAnswer4.Visibility = Visibility.Visible;
                gridAnswer1.Margin = new Thickness(-945, 761, 0, 0);
                gridAnswer2.Margin = new Thickness(949, 761, 0, 0);
                gridAnswer3.Margin = new Thickness(-945, 898, 0, 0);
                gridAnswer4.Margin = new Thickness(949, 898, 0, 0);
                gridAnswer1.Width = 935;
                gridAnswer2.Width = 935;
                gridAnswer3.Width = 935;
                gridAnswer4.Width = 935;
            }

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
            gridQuizVragen.Visibility = Visibility.Collapsed;
            gridQuizVragenCheck.Visibility = Visibility.Visible;
            gridTussenScherm.Visibility = Visibility.Collapsed;
            gridTussenSchermCheck.Visibility = Visibility.Collapsed;
            lblAnswer1Check.Content = answer1;
            lblAnswer2Check.Content = answer2;
            lblAnswer3Check.Content = answer3;
            lblAnswer4Check.Content = answer4;
            lblVraagCheck.Content = question;

            gridAnswer1Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#E21B3C");
            gridAnswer2Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#1368CE");
            gridAnswer3Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#D89E00");
            gridAnswer4Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#26890C");
            lblAnswer1Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#E21B3C");
            lblAnswer2Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#1368CE");
            lblAnswer3Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#D89E00");
            lblAnswer4Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#26890C");

            if (lblAnswer3Check.Content == "" && lblAnswer4.Content == "")
            {
                gridAnswer3Check.Visibility = Visibility.Collapsed;
                gridAnswer4Check.Visibility = Visibility.Collapsed;
                gridAnswer1Check.Margin = new Thickness(0, 761, 0, 0);
                gridAnswer2Check.Margin = new Thickness(0, 898, 0, 0);
                gridAnswer1Check.Width = 1882;
                gridAnswer2Check.Width = 1882;

            }
            else if (lblAnswer4Check.Content == "")
            {
                gridAnswer4Check.Visibility = Visibility.Collapsed;
                gridAnswer3Check.Visibility = Visibility.Visible;
                gridAnswer3Check.Margin = new Thickness(0, 898, 0, 0);
                gridAnswer1Check.Margin = new Thickness(-945, 761, 0, 0);
                gridAnswer2Check.Margin = new Thickness(949, 761, 0, 0);
                gridAnswer3Check.Width = 1882;
                gridAnswer1Check.Width = 935;
                gridAnswer2Check.Width = 935;
            }
            else
            {
                gridAnswer1Check.Visibility = Visibility.Visible;
                gridAnswer2Check.Visibility = Visibility.Visible;
                gridAnswer3Check.Visibility = Visibility.Visible;
                gridAnswer4Check.Visibility = Visibility.Visible;
                gridAnswer1Check.Margin = new Thickness(-945, 761, 0, 0);
                gridAnswer2Check.Margin = new Thickness(949, 761, 0, 0);
                gridAnswer3Check.Margin = new Thickness(-945, 898, 0, 0);
                gridAnswer4Check.Margin = new Thickness(949, 898, 0, 0);
                gridAnswer1Check.Width = 935;
                gridAnswer2Check.Width = 935;
                gridAnswer3Check.Width = 935;
                gridAnswer4Check.Width = 935;
            }

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
                    lblAnswer2Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    lblAnswer3Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    lblAnswer4Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    break;
                case 2:
                    gridAnswer1Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    gridAnswer3Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    gridAnswer4Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    lblAnswer1Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    lblAnswer3Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    lblAnswer4Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    break;
                case 3:
                    gridAnswer1Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    gridAnswer2Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    gridAnswer4Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    lblAnswer1Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    lblAnswer2Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    lblAnswer4Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    break;
                case 4:
                    gridAnswer1Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    gridAnswer2Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    gridAnswer3Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    lblAnswer1Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    lblAnswer2Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
                    lblAnswer3Check.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#707070");
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
