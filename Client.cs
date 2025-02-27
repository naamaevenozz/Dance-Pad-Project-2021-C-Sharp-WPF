using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_DancePad_Client
{
    public class Client
    {
        private string userName;
        private string password;
        private const string host = "127.0.0.1";
        private const int port = 11000;
        static TcpClient client;
        static NetworkStream stream;
        private string filesList;
        private string current_song_txt;
        private int countToMedia = 0;
        private static byte[] buffer = new byte[1024];
        private int len_of_song;


        /// <summary>
        /// פעולה בונה של המחלקה המאתחלת את ערכי שם המשתמש והססמא בערכים שהיא מקבלת
        /// את המשתנים שאינה מקבלת היא מאתחלת כמחרוזת ריקה
        /// היא מזמנת את פעולת MakeClient
        /// בכדי להתחיל ביצירת התקשורת בין השרת ללקוח
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        public Client(string user, string pass)
        {
            client = new TcpClient();
            this.userName = user;
            this.password = pass;
            this.filesList = "";
            this.current_song_txt = "";
            MakeClient();
        }

        /// <summary>
        /// פעולה המתחילה את התקשורת בין השרת ללקוח על ידי התחברות אליו בעזרת כתובת IP ופורט
        /// לאחר מכן היא שולחת לו את שם המשתמש והססמא של השחקן שהתחבר,
        /// היא יוצרת thread 
        /// הוא ירוץ במקביל לתוכנית,הוא יריץ את פונקציית SendMessage מאחר ויש בה לולאה אינסופית הוא ירוץ לאורך כל המשחק       
        /// </summary>
        private void MakeClient()
        {
            try
            {
                client.Connect(host, port);
                stream = client.GetStream();
                string message = this.userName.Trim() + "#" + this.password.Trim();
                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start();
                SendMessage(message);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

            }

        }

        /// <summary>
        /// פעולה המעדכנת את ערך הuserName
        /// </summary>
        /// <param name="userName1"></param>
        public void SetUser(string userName1)
        {
            this.userName = userName1;
        }

        /// <summary>
        /// פעולה המטפלת במחרוזות הנשלחות מהשרת. 
        /// מאחר ואנו ממירים מbyte לstring את המידע שהיה בבסיס הקסהדצימלי מתווספים למחרוזת 
        /// תווים מיותרים כגון-\0 .פונקצייה זו מחזירה את המחרוזת המקורית ללא התווים המיותרים.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string StrWithout(string str)
        {

            string newstring = "";
            char kav = str[1];
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != '\0')
                {
                    newstring = newstring + str[i];
                }
            }
            return newstring;
        }

        /// <summary>
        /// פעולה המעדכנת את ערך הpassword
        /// </summary>
        /// <param name="userName1"></param>
        public void SetPass(string password1)
        {
            this.password = password1;
        }


        /// <summary>
        /// פעולה המחזירה את שרשור קובץ הטקסט עבור השיר שנבחר
        /// </summary>
        /// <returns></returns>
        public string GetSoundsName()
        {
            return this.filesList;
        }

        /// <summary>
        /// פעולה המחזירה משתמש שמסייע בסכנרון התוכנית
        /// </summary>
        /// <returns></returns>
        public string GetTextFile()
        {
            return this.current_song_txt;
        }

        /// <summary>
        /// פעולה הממירה מחרוזת למערך בייטים ושולחת את המערך לשרת
        /// </summary>
        /// <param name="msg"></param>
        public void SendMessage(string msg)
        {
            try
            {
                byte[] data = Encoding.Unicode.GetBytes(msg);
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        /// <summary>
        /// פעולה שבעזרת thread רצה במקביל לתוכנית 
        /// בעזרת שיש בה לולאה אינסופית
        /// הפעולה מאזינה לכל המידע שמתקבל מהשרת ומנתחת אותו במידת הצורך
        /// </summary>
        public void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[64];
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    //byte[] buf = new byte[7];
                    //int count = stream.Read(buf, 0, buf.Length);
                    try
                    {
                        do
                        {
                            bytes = stream.Read(data, 0, data.Length);
                            builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                        } while (stream.DataAvailable);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);

                    }
                    string message = builder.ToString();
                    if (message != "")
                    {
                        message = StrWithout(message);
                    }
                    Console.WriteLine(message);

                    if (message.StartsWith("*")) //המידע שהתקבל הוא שם השיר שהלקוח מעוניין בו
                    {
                        this.filesList = message;
                        this.countToMedia = 1; //כעת ניתן להיות פנויים לקבלת קובץ הטקסט
                    }
                    else if (this.countToMedia == 1)
                    {
                        this.len_of_song = int.Parse(message.Substring(0, 5));
                        message = message.Substring(5);
                        this.current_song_txt = message;       //קבלת קובץ הטקסט כשרשור              
                        this.countToMedia = 0; //איפוס המשתנה שמכריע האם יש להתייחס לקבלת שרשור קובץ טקסט שןב
                    }
                    else // קבלת קובץ בינארי-השיר
                    {
                        string fileName = @"..\..\bin\Debug\song.mp3";
                        string str = Encoding.Unicode.GetString(data);
                        data = new Byte[int.Parse(str)];
                        string continue1 = "con";
                        byte[] data2 = Encoding.Unicode.GetBytes(continue1);
                        stream.Write(data2, 0, data2.Length);

                        bytes = stream.Read(data, 0, data.Length);
                        File.WriteAllBytes(fileName, data);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Connection aborted!");
                    Console.ReadLine();
                    Disconnect();
                }
            }
        }

        /// <summary>
        /// פעולה שמחזירה את כמות המילי שניות שמצריך השיר
        /// </summary>
        /// <returns></returns>
        public int GetLenOfSong()
        {
            return this.len_of_song;
        }
        /// <summary>
        /// פעולה שמקבלת קובץ מהשרת ומשרשרת אותו למשתנה מסוג מחרוזת
        /// </summary>
        /// <returns></returns>
        public string ReceiveFile()
        {
            string fileInString = "";
            while (true)
            {
                try
                {
                    byte[] data = new byte[64];
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();
                    fileInString = message + "\n";
                    Console.WriteLine(fileInString);

                }

                catch
                {
                    Console.WriteLine("Connection aborted!");
                    Console.ReadLine();
                    Disconnect();
                }
            }
            return fileInString;

        }

        /// <summary>
        /// פעולה המנתקת את הלקוח מן השרת ומנתקת את "צינור" התקשורת בינהם
        /// </summary>
        public void Disconnect()
        {
            if (stream != null)
                stream.Close();
            if (client != null)
                client.Close();
            Environment.Exit(0);
        }
    }
}
