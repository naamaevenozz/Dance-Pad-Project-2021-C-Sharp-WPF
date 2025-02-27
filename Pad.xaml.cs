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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace WPF_DancePad_Client
{
    /// <summary>
    /// Interaction logic for Pad.xaml
    /// </summary>
    public partial class Pad : Window
    {       
        private Joystick joystick; 
        private bool[] joystickButtons; 
        private DispatcherTimer joystickTimer;
        private Client client;
        DispatcherTimer timer;
        int ticks = 0;
        public char button;
        private int i = 0;
        private string[] allStr; 
        private string txtFile; 
        private int time; 
        public int counter = 0;
        private MediaPlayer mPlayer;
        private int lenOfSound;
        private BitmapImage imgNewX;
        private BitmapImage imgNewO;
        private BitmapImage imgNewT;
        private BitmapImage imgNewS;
        private BitmapImage imgNew_Down;
        private BitmapImage imgNew_Up;
        private BitmapImage imgNew_Left;
        private BitmapImage imgNew_Right;
        private BitmapImage imgX;
        private BitmapImage imgO;
        private BitmapImage imgT;
        private BitmapImage imgS;
        private BitmapImage img_Up;
        private BitmapImage img_Right;
        private BitmapImage img_Left;
        private BitmapImage img_Down;       
        private int time_of_song;


      
        public Pad(Client client)
        {
            Uri iconUri = new Uri(@"..\..\Images\Disco.jpg", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(iconUri);
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            InitializeImg();           
            timer = new DispatcherTimer();
            this.client = client;
            txtFile = client.GetTextFile();
            allStr = txtFile.Split('\n');
            lenOfSound = allStr.Length;
            time_of_song = client.GetLenOfSong();            
        }

        
        private void InitializeImg()
        {
            imgNewX = new BitmapImage(new Uri("pack://application:,,,/Images/NewX.jpg"));
            imgNewO = new BitmapImage(new Uri("pack://application:,,,/Images/NewO.jpg"));
            imgNewS = new BitmapImage(new Uri("pack://application:,,,/Images/NewS.jpg"));
            imgNewT = new BitmapImage(new Uri("pack://application:,,,/Images/NewT.jpg"));
            imgNew_Up = new BitmapImage(new Uri("pack://application:,,,/Images/NewUpArrow.jpg"));
            imgNew_Down = new BitmapImage(new Uri("pack://application:,,,/Images/NewDownArrow.jpg"));
            imgNew_Right = new BitmapImage(new Uri("pack://application:,,,/Images/NewRightArrow.jpg"));
            imgNew_Left = new BitmapImage(new Uri("pack://application:,,,/Images/NewLeftArrow.jpg"));


            imgX = new BitmapImage(new Uri("pack://application:,,,/Images/X.jpg"));
            imgO = new BitmapImage(new Uri("pack://application:,,,/Images/O.jpg"));
            img_Right = new BitmapImage(new Uri("pack://application:,,,/Images/RightArrow.jpg"));
            img_Left = new BitmapImage(new Uri("pack://application:,,,/Images/LeftArrow.jpg"));
            img_Up = new BitmapImage(new Uri("pack://application:,,,/Images/UpArrow.jpg"));
            img_Down = new BitmapImage(new Uri("pack://application:,,,/Images/DownArrow.jpg"));
            imgS = new BitmapImage(new Uri("pack://application:,,,/Images/S.jpg"));
            imgT = new BitmapImage(new Uri("pack://application:,,,/Images/T.jpg"));
        }

   
        private void enableTimer()
        {
            joystickTimer.Start();
        }

        private void JoystickTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                joystick.UpdateStatus();
                joystickButtons = joystick.buttons;
                Title = counter.ToString();
                if (joystickButtons[0] == true)
                {
                    if (this.button == '1')
                    {
                        counter++;
                    }
                }

                if (joystickButtons[1] == true)
                {
                    if (this.button == '2')
                    {
                        counter++;
                    }
                }

                if (joystickButtons[2] == true)
                {
                    if (this.button == '3')
                    {
                        counter++;
                    }
                }

                if (joystickButtons[3] == true)
                {
                    if (this.button == '4')
                    {
                        counter++;
                    }
                }

                if (joystickButtons[4] == true)
                {
                    if (this.button == '5')
                    {
                        counter++;
                    }
                }

                if (joystickButtons[5] == true)
                {
                    if (this.button == '6')
                    {
                        counter++;
                    }
                }

                if (joystickButtons[6] == true)
                {
                    if (this.button == '7')
                    {
                        counter++;
                    }
                }

                if (joystickButtons[7] == true)
                {
                    if (this.button == '8')
                    {
                        counter++;
                    }
                }
            }
            catch
            {

                joystickTimer.Stop();
                connectToJoystick(joystick);
            }
        }     

        public void Game(object sender, EventArgs e)
        {
            btnStart.IsEnabled = false;

            joystickTimer = new DispatcherTimer();
            joystickTimer.Interval = TimeSpan.FromMilliseconds(1);
            joystickTimer.Tick += JoystickTimer_Tick;

            IntPtr windowHandle = (new WindowInteropHelper(this)).Handle; //new WindowInteropHelper(this).Handle;
            if (windowHandle != IntPtr.Zero)
            {
                joystick = new Joystick(windowHandle);
                 connectToJoystick(joystick);
            }
            else
            {
                MessageBox.Show("Handle of window not found");
                Environment.Exit(0);
            }
            while (client.GetTextFile() == "")  
            {

            }
            string txtFile = client.GetTextFile();

            mPlayer = new MediaPlayer();
            string path = Environment.CurrentDirectory;
            path += @"\song.mp3";
            mPlayer.Open(new Uri(path));
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += Timer_Tick;
            mPlayer.Open(new Uri("song.mp3", UriKind.Relative));
            mPlayer.Play();
            timer.Start();
        }

        
        private void connectToJoystick(Joystick joystick)
        {
            string sticks = joystick.FindJoysticks();
            if (sticks == null)
            {
                MessageBox.Show("Joystick not found! \n connect him");
                Environment.Exit(0);
            }
            while (true)
            {
                if (sticks != null)
                {
                    if (joystick.AcquireJoystick(sticks))
                    {
                        enableTimer();
                        break;
                    }
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (ticks < time_of_song) //Checking if we can continue to the next line
            {
                string line = this.allStr[i];
                this.button = line[0];
                this.time = (int)(double.Parse(line.Substring(2)));
                this.time /= 10;
                ticks++;                
                Title = counter.ToString();
                if (ticks == this.time)
                {
                    i++;
                    X.Source = imgX;
                    O.Source = imgO;
                    Up.Source = img_Up;
                    Left.Source = img_Left;
                    Right.Source = img_Right;
                    Triangular.Source = imgT;
                    Down.Source = img_Down;
                    Square.Source = imgS;

                    switch (this.button)
                    {
                        case '1':
                            X.Source = imgNewX;
                            break;
                        case '2':
                            Up.Source = imgNew_Up;
                            break;
                        case '3':
                            O.Source = imgNewO;
                            break;
                        case '4':
                            Left.Source = imgNew_Left;
                            break;
                        case '5':
                            Right.Source = imgNew_Right;
                            break;
                        case '6':
                            Triangular.Source = imgNewT;
                            break;
                        case '7':
                            Down.Source = imgNew_Down;
                            break;
                        case '8':
                            Square.Source = imgNewS;
                            break;
                    }
                }
            }
            else
            {
                timer.Stop();                
                string msg2 = "!" + counter;
                client.SendMessage(msg2);
                int calories = (time_of_song / 250)-40;
                string msg = "Great! you earn " + counter + "points\n" + "You burn " + calories + " calories!\n" + "Bye bye";
                string title = "End of game";
                MessageBox.Show(msg, title);
                this.Close();                
                Environment.Exit(0);
            }
        }

        
    }

}
