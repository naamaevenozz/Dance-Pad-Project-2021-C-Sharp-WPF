using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WPF_DancePad_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        /// <summary>
        /// הפעולה הבונה של הפונקצייה.היא מגדירה את האייקון שיהיה לחלון וקובעת שהחלון יהיה במרכז המסך.
        /// </summary>
        public MainWindow()
        {          
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Uri iconUri = new Uri(@"..\..\Images\Disco.jpg", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(iconUri);
            InitializeComponent();
        }

        /// <summary>
        /// פונקצייה שפותחת את חלון השחקן הקיים במידה ונלחץ כפתור השחקן הקיים.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exist_user(object sender, RoutedEventArgs e)
        {
            ExistUser existUser = new ExistUser();
            this.Close();
            existUser.Show();


        }
        /// <summary>
        /// פונקצייה שפותחת את חלון ההסבר במידה ונלחץ כפתור ההסבר.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Help(object sender, RoutedEventArgs e)
        {
            Help help = new Help();
            this.Close();
            help.Show();
        }
        /// <summary>
        /// פונקציה שסוגרת את כל החלונות ומסיימת המשחק.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// פונקצייה שפותחת את חלו החדש הקיים במידה ונלחץ כפתור השחקן החדש.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void New_user(object sender, RoutedEventArgs e)
        {
            NewUser newUser = new NewUser();
            this.Close();
            newUser.Show();
        }
    }
}

