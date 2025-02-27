using System;
using System.Collections.Generic;
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

namespace WPF_DancePad_Client
{
    /// <summary>
    /// Interaction logic for Chose_song.xaml
    /// </summary>
    public partial class Chose_song : Window
    {
        private Client client;
        private string filesName;

        /// <summary>
        /// פונקצייה שממתינה עד לקבלת קובץ הטקסט מהשרת ללקוח
        /// כאשר קובץ הטקסט מומר למחרוזת  היא הופכת 
        /// היא מקבלת רשימה ומכניסה אותה למשתנה מסוג
        /// ListView שיסייע בהצגה מובנת לשחקן את השירים האפשריים
        /// </summary>
        /// <param name="client"></param>
        public Chose_song(Client client)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Uri iconUri = new Uri(@"..\..\Images\Disco.jpg", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(iconUri);
            InitializeComponent();
            this.client = client;
            while (!this.client.GetSoundsName().StartsWith("*")) ///כל עוד הלקוח עדיין לא קיבל את רשימת השירים התוכנית צריכה להמתין
            {

            }
            this.filesName = this.client.GetSoundsName();
            var sounds = GetList(); ///קבלת הרשימה לתוך משתנה שקשור לlistView
            if (sounds.Count > 0)
            {
                listOfSounds.ItemsSource = sounds;
            }
        }

        /// <summary>
        /// פעולה המחזירה רשימה-היא יוצרת רשימה מתוך מחרוזת,כל חוליה מהווה שם של שיר
        /// </summary>
        /// <returns></returns>
        private List<string> GetList()
        {
            string song_name = "";
            List<string> listSounds = new List<string> { };
            //List<string> temp = listSounds;
            while (this.filesName.IndexOf("#") != -1)
            {
                int index = this.filesName.IndexOf("#");
                if (index == 13)   //אם זאת ריצה ראשונה אז צריך להוריד את *
                {
                    song_name = filesName.Substring(1, index - 1);
                }
                else
                {
                    song_name = filesName.Substring(0, index);
                }

                this.filesName = filesName.Substring(index + 1);
                listSounds.Add(song_name);
            }
            listSounds.Add(this.filesName);
            return listSounds;
        }

        /// <summary>
        /// פונקצייה ששולחת לשרת את שם השיר הנבחר 
        /// לאחר ונוצר קובץ בינארי היא פותחת את חלון המשחק המרכזי,Pad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            string fileName = string.Empty;
            if (Title != "The dance game !")
            {
                fileName = Title;
                client.SendMessage(fileName);
                while (!File.Exists(@"..\..\bin\Debug\song.mp3"))
                {

                }
                try
                {
                    Pad pad = new Pad(client);
                    pad.Show();
                    this.Close();
                }
                catch (Exception ex)
                {

                }
            }
        }
        /// <summary>
        /// פונקצייה שמשנה את כותרת החלון לשם השיר הנבחר מאחר ובפעולה BtnStart_Click
        /// יש בדיקה כל עוד הכותרת אינה שם של שיר אין לשלוח אותו לשרת.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listOfSounds_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {
                Title = System.IO.Path.GetFileNameWithoutExtension(item.ToString());
                btnStart.IsEnabled = true;
            }
        }

    }
}
