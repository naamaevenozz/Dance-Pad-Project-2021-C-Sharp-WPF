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

namespace WPF_DancePad_Client
{
    /// <summary>
    /// Interaction logic for NewUser.xaml
    /// </summary>
    public partial class NewUser : Window
    {
        private Client client;
        private string userName;
        private string passWord;

        public NewUser()
        {
            Uri iconUri = new Uri(@"..\..\Images\Disco.jpg", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(iconUri);
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();

        }
        /// <summary>
        /// פונקצייה זו מכניסה לתוך משתנה את מה שהמשתמש מקליד לתיבת הטקסט
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            userName = TextUser.Text;
        }

        /// <summary>
        /// פונקצייה זו מכניסה לתוך משתנה את מה שהמשתמש מקליד לתיבת הטקסט
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            passWord = TextPass.Text;
        }
        /// <summary>
        /// פונקצייה המעדכנת את ערכי העצם client 
        /// בשם משתמש וסיסמא,לאחר מכן היא פותחת את חלון פתיחת השירים
        /// בעדכון ערכי העצם מתבצעת שליחת הפרטים לשרת
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Enter(object sender, RoutedEventArgs e)
        {

            client = new Client(userName, passWord);
            Chose_song chose_Song = new Chose_song(client);
            this.Close();
            chose_Song.Show();
        }
    }
}
