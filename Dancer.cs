using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace DancePadServer
{                
    /// <summary>
    /// פעולה בונה של מחלקת Dancer
    /// </summary>
    class Dancer
    { 
        public NetworkStream stream { get; private set; } 
        private string userName;
        private string password;
        private string filenames = "*GottaFeeling#Parparim#Problam#LaLaLa#Worth_It#Lush_Life";        
        DT dt;        
        TcpClient client;
        Server server;

        /// <summary>
        /// פעולה הבונה של המחלקה,מתחילה את התקשורת וניגשת לבסיס הנתונים.
        /// </summary>
        /// <param name="tcpClient"></param>
        /// <param name="server"></param>
        public Dancer(TcpClient tcpClient, Server server)
        {

            client = tcpClient;
            stream = client.GetStream();
            this.server = server;   
            dt = new DT();
           
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
            //char kav = str[1];
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
        /// פונקציה שמטרתה לנתח את המידע המתקבל מהלקוח ולשלוח לו מידע בהתאם.
        /// פונקציה זו מזמנת את פונקציית GetMessage       
        /// </summary>
        public void Process()
        {
            try
            {
                byte[] bytes = new byte[1024];
                string data;          
                string message = GetMessage();            
                int index = message.IndexOf("#");
                userName = message.Substring(0, index);
                password = message.Substring(index + 1);
                userName = StrWithout(userName);
                password = StrWithout(password);

                if (dt.IsExist(userName, password))
                {
                    Console.WriteLine("Exist user!");
                    Console.WriteLine(userName + " " + password);
                    byte[] msg = System.Text.Encoding.Unicode.GetBytes(filenames);
                    //  Send files list.
                    stream.Write(msg, 0, msg.Length);                    
                }
                else
                {
                    Console.WriteLine("Its a new player");
                    dt.AddDancer(userName, password);
                    byte[] msg = System.Text.Encoding.Unicode.GetBytes(filenames);                    
                    try
                    {                       
                        stream.Write(msg, 0, msg.Length);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }                

                while (true)
                {
                    byte[] msg;
                    message = GetMessage();
                    if (message != "" && !message.StartsWith("!"))
                    {
                        switch (message)
                        {
                            case "Parparim":                               
                                SendFileT("Parparim.txt");
                                SendFileB("Parparim.mp3");
                                break;
                            case "GottaFeeling":                             
                                SendFileT("GottaFeeling.txt");
                                SendFileB("GottaFeeling.mp3");
                                break;
                            case "Problam":                               
                                SendFileT("Problam.txt");
                                SendFileB("Problam.mp3");
                                break;
                            case "Worth_It":                               
                                SendFileT("Worth_It.txt");
                                SendFileB("Worth_It.mp3");
                                break;
                            case "Lush_Life":                           
                                SendFileT("Lush_Life.txt");
                                SendFileB("Lush_Life.mp3");
                                break;
                            case "LaLaLa":                             
                                SendFileT("LaLaLa.txt");
                                SendFileB("LaLaLa.mp3");
                                break;
                        }                                         
                    }
                    else ///נקודות השחקן שנשלח ממנו
                    {
                        int points = int.Parse(message.Substring(1)); ///ממקום 1 עד הסוף כדי לחתוך את ! ששימש כדי לזהות שמדובר בשיא
                        dt.SetRecord(userName, points);                     
                    }
                }
            }
            finally
            {              
                Close();
            }
        }
        /// <summary>
        /// פונקציה שממירה את קובץ הטקסט הנדרש למחרוזת ושולחת אותו ללקוח
        /// </summary>
        /// <param name="fileName"></param>
        public void SendFileT(string fileName)
        {
            string original_name = fileName; //שמירת שם הקובץ לפני שמשרשרים אליו את אורך זמן השיר
            switch (fileName)
            {
                case "Parparim.txt":
                    fileName = "17980";
                    break;
                case "GottaFeeling.txt":
                    fileName ="18000";
                    break;
                case "Problam.txt":
                    fileName = "12300";
                    break;
                case "Worth_It.txt":
                    fileName = "14165";
                    break;
                case "Lush_Life.txt":
                    fileName = "12680";
                    break;
                case "LaLaLa.txt":
                    fileName = "12730";
                    break;
            }
            string[] lines = File.ReadAllLines(original_name); ///קבלת הקובץ על ידי חיפוש בתקיית bin
            //Get the string from the file
            foreach (string line in lines)   ///לקיחת כל שורת טקסט מהקובץ ושרשורה לתוך משתנה
            {
                fileName = fileName + line + "\n";
            }
            //Translate the string to byte
            byte[] bytes = Encoding.Unicode.GetBytes(fileName);
            //TcpClient tcpClient = tcpListener.AcceptTcpClient();
            //NetworkStream stream = tcpClient.GetStream();
            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// פונקציה ששולחת קובץ בינארי-קובץ השיר
        /// </summary>
        /// <param name="filename"></param>
        public void SendFileB(string filename)
        {
            string fileBin = "";
            switch (filename)
            {
                case "Parparim.mp3":
                    fileBin = @"..\..\Sounds\Parparim.mp3";
                    break;
                case "GottaFeeling.mp3":
                    fileBin = @"..\..\Sounds\GottaFeeling.mp3";
                    break;
                case "Problam.mp3":
                    fileBin = @"..\..\Sounds\Problam.mp3";
                    break;
                case "Worth_It.mp3":
                    fileBin = @"..\..\Sounds\Worth_It.mp3";
                    break;
                case "Lush_Life.mp3":
                    fileBin = @"..\..\Sounds\Lush_Life.mp3";
                    break;
                case "LaLaLa.mp3":
                    fileBin = @"..\..\Sounds\LaLaLa.mp3";
                    break;
            }        
                  
            byte[] allBytes = File.ReadAllBytes(fileBin); ///קבלת הקובץ על פי השם בתקיית bin

            byte[] buf = Encoding.Unicode.GetBytes(allBytes.Length.ToString()); ///המרת המערך למערך בייטים לשליחה

            stream.Write(buf, 0, buf.Length);
            while (GetMessage() !="con") 
            {

            }
            stream.Write(allBytes, 0, allBytes.Length);
        }
        /// <summary>
        /// פונקציה שמקבלת את המידע שמגיע מהלקוח, היא ממירה את מערך הבייטים שמקבלת למחרוזת
        /// </summary>
        /// <returns></returns>
        private string GetMessage()
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
           

            return builder.ToString();
        } 
        /// <summary>
        /// פונקציה שמנתקת את התקשורת בין השרת ללקוח
        /// </summary>
        public void Close()
        {
            if (stream != null)
                stream.Close();
            if (client != null)
                client.Close();
        }
    }
}
