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

             
            }
        }

        private void BtnVolgende_Click(object sender, RoutedEventArgs e)
        {
            List<SaveQuestions> question = new List<SaveQuestions>
            {
                new SaveQuestions
                {
                    question = "Is Ryan lekker",
                    answer1 = "Perhaps",
                    answer2 = "Hmm yes",
                    answer3 = "Neen",
                    answer4 = "JAAAAAAAAAAHAAAAA",
                    rightAnswer = 1,
                    image = "ryan.jpg",
                    timer = 30
                },
            };
            listQuestions.Add(question);

            MessageBox.Show(_selectedFileName);

            // string name = System.IO.Path.GetFileName(_selectedFileName);
            string destinationPath = GetDestinationPath(_imgNaam, "Images");

            File.Copy(_selectedFileName, destinationPath, true);

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
