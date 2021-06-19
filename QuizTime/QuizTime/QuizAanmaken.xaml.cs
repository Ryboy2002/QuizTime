using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using Newtonsoft.Json;

namespace QuizTime
{
    /// <summary>
    /// Interaction logic for QuizAanmaken.xaml
    /// </summary>
    public partial class QuizAanmaken : Window
    {
        database database = new database();
        private List<List<SaveQuestions>> listQuestions = new List<List<SaveQuestions>>();
        private Microsoft.Win32.OpenFileDialog _image = new Microsoft.Win32.OpenFileDialog();
        private string _imgNaam;
        private string _selectedFileName;
        private int _questionNumber = 1;
        private int _listNumber;
        private int _rightAnswer;
        private int _numberOfQuestions;

        //private string _imgfile;
        public QuizAanmaken()
        {
            InitializeComponent();

            #region
            // Functies toevoegen

            txbQuizTitel.GotFocus += TxbQuizTitel_GotFocus;
            txbTimer.GotFocus += TxbTimer_GotFocus;
            txbQuizVraag.GotFocus += TxbQuizVraag_GotFocus;
            txbAnswer1.GotFocus += TxbAnswer1_GotFocus;
            txbAnswer2.GotFocus += TxbAnswer2_GotFocus;
            txbAnswer3.GotFocus += TxbAnswer3_GotFocus;
            txbAnswer4.GotFocus += TxbAnswer4_GotFocus;
            btnQuit.Click += BtnQuit_Click;
            btnSave.Click += BtnSave_Click;
            btnVolgende.Click += BtnVolgende_Click;
            btnImage.Click += BtnImage_Click;
            btnVorige.Click += BtnVorige_Click;

            #endregion

            PointCollection polygonColl = new PointCollection();
            polygonColl.Add(new Point(0, 80));
            polygonColl.Add(new Point(40, 10));
            polygonColl.Add(new Point(80, 80));
            polygonTriangle.Points = polygonColl;
        }

        private static String GetDestinationPath(string filename, string foldername)
        {
            String appStartPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            appStartPath = String.Format(appStartPath + "\\{0}\\" + filename, foldername);
            return appStartPath;
        }

        private void BtnImage_Click(object sender, RoutedEventArgs e)
        {
            _image.Title = "Voeg een afbeelding toe";
            _image.DefaultExt = ".png";
            _image.Filter = "PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg";
            Nullable<bool> result = _image.ShowDialog();
            if (result == true)
            {
                _selectedFileName = _image.FileName;
                _imgNaam = System.IO.Path.GetFileName(_image.FileName);
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(_selectedFileName);
                bitmap.EndInit();
                imgQuestion.Source = bitmap;              
                gridImage.Background = null;
                borderGridImage.BorderThickness = new Thickness(0);

                MessageBox.Show(_imgNaam);
                MessageBox.Show(_selectedFileName);
                MessageBox.Show(_image.FileName);
            }
        }

        private void BtnVolgende_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)cboxCorrectAnswer1.IsChecked)
            {
                _rightAnswer = 1;
            } else if ((bool)cboxCorrectAnswer2.IsChecked)
            {
                _rightAnswer = 2;
            } else if ((bool)cboxCorrectAnswer3.IsChecked)
            {
                _rightAnswer = 3;
            } else if ((bool)cboxCorrectAnswer4.IsChecked)
            {
                _rightAnswer = 4;
            } else
            {
                _rightAnswer = 0;
            }
                  
            if (_numberOfQuestions < _questionNumber)
            {
                List<SaveQuestions> question = new List<SaveQuestions>
            {
                new SaveQuestions
                {
                    question = txbQuizVraag.Text,
                    answer1 = txbAnswer1.Text,
                    answer2 = txbAnswer2.Text,
                    answer3 = txbAnswer3.Text,
                    answer4 = txbAnswer4.Text,
                    rightAnswer = _rightAnswer,
                    image = _imgNaam,
                    timer = Convert.ToInt32(txbTimer.Text)
                },
            };
                listQuestions.Add(question);
                _numberOfQuestions++;
               
                txbAnswer1.Text = "";
                txbAnswer2.Text = "";
                txbAnswer3.Text = "";
                txbAnswer4.Text = "";
                txbQuizVraag.Text = "";
                txbTimer.Text = "30";
                cboxCorrectAnswer1.IsChecked = false;
                cboxCorrectAnswer2.IsChecked = false;
                cboxCorrectAnswer3.IsChecked = false;
                cboxCorrectAnswer4.IsChecked = false;

                gridImage.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#B2FFFFFF");
                borderGridImage.BorderThickness = new Thickness(3);
                imgQuestion.Source = null;
            } else
            {
                listQuestions[_listNumber][0].answer1 = txbAnswer1.Text;
                listQuestions[_listNumber][0].answer2 = txbAnswer2.Text;
                listQuestions[_listNumber][0].answer3 = txbAnswer3.Text;
                listQuestions[_listNumber][0].answer4 = txbAnswer4.Text;
                listQuestions[_listNumber][0].rightAnswer = _rightAnswer;
                listQuestions[_listNumber][0].question = txbQuizVraag.Text;
                listQuestions[_listNumber][0].timer = Convert.ToInt32(txbTimer.Text);
                listQuestions[_listNumber][0].image = _imgNaam;
            }

            _questionNumber++;
            lblQuestionNumber.Content = $"Vraag {_questionNumber}";
            _listNumber = _questionNumber - 1;
            
            if (_selectedFileName == _image.FileName && _image.FileName != "")
            {
                string destinationPath = GetDestinationPath(_imgNaam, "Images");
                File.Copy(_selectedFileName, destinationPath, true);

                MessageBox.Show(_imgNaam);
                MessageBox.Show(_selectedFileName);
                _imgNaam = "";
                _selectedFileName = "";
            }
            else
            {
                MessageBox.Show(_imgNaam);
                MessageBox.Show(_selectedFileName);
                MessageBox.Show(_image.FileName);
                MessageBox.Show("Niet gelukt");
            }

        }

        private void BtnVorige_Click(object sender, RoutedEventArgs e)
        {
           

            if (_questionNumber != 1)
            {
                _questionNumber--;
            }

            _listNumber = _questionNumber - 1;

            txbAnswer1.Text = listQuestions[_listNumber][0].answer1.ToString();
            txbAnswer2.Text = listQuestions[_listNumber][0].answer2.ToString();
            txbAnswer3.Text = listQuestions[_listNumber][0].answer3.ToString();
            txbAnswer4.Text = listQuestions[_listNumber][0].answer4.ToString();
            txbQuizVraag.Text = listQuestions[_listNumber][0].question.ToString();
            txbTimer.Text = listQuestions[_listNumber][0].timer.ToString();
            
            if (listQuestions[_listNumber][0].image != "")
            {

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(listQuestions[_listNumber][0].image);
                bitmap.EndInit();
                imgQuestion.Source = bitmap;
                gridImage.Background = null;
                borderGridImage.BorderThickness = new Thickness(0);
            }

            if (_rightAnswer == 1)
            {
                cboxCorrectAnswer1.IsChecked  = true;
            }
            else if (_rightAnswer == 2)
            {
                cboxCorrectAnswer2.IsChecked = true;
            }
            else if (_rightAnswer == 3)
            {
                cboxCorrectAnswer3.IsChecked = true;
            }
            else if (_rightAnswer == 4)
            {
                cboxCorrectAnswer4.IsChecked = true;
            }


            lblQuestionNumber.Content = $"Vraag {_questionNumber}";
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string json = JsonConvert.SerializeObject(listQuestions);
            database.SaveQuiz(txbQuizTitel.Text, json);
        }

        private void BtnQuit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #region 
        // Remove placeholders
        private void TxbQuizVraag_GotFocus(object sender, RoutedEventArgs e)
        {
            txbQuizVraag.Text = "";
        }

        private void TxbTimer_GotFocus(object sender, RoutedEventArgs e)
        {
            txbTimer.Text = "";
        }

        private void TxbQuizTitel_GotFocus(object sender, RoutedEventArgs e)
        {
            txbQuizTitel.Text = "";
        }

        private void TxbAnswer4_GotFocus(object sender, RoutedEventArgs e)
        {
            txbAnswer4.Text = "";
        }

        private void TxbAnswer3_GotFocus(object sender, RoutedEventArgs e)
        {
            txbAnswer3.Text = "";
        }

        private void TxbAnswer2_GotFocus(object sender, RoutedEventArgs e)
        {
            txbAnswer2.Text = "";
        }

        private void TxbAnswer1_GotFocus(object sender, RoutedEventArgs e)
        {
            txbAnswer1.Text = "";
        }

        #endregion
    }
}
