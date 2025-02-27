using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DancePadServer
{
    class Server
    {
        static TcpListener tcpListener;
        private Dancer dancer;
        DT dt;

       
        /// <summary>
        /// פעולה בונה של המחלקה,ניגשת לבסיס הנתונים. 
        /// </summary>
        public Server()
        {
            dt = new DT();
            Listen();            
        }

        /// <summary>
        /// פעולה שמוסיפה שחקן חדש לטבלה
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public void AddConnection(string userName, string password)
        {
            dt.AddDancer(userName, password);
        }
       
          /// <summary>
          /// פעולה שמאזינה ראשונית ללקוחות שמעוניינים להתחבר.
          /// ברגע שלקוח מתחבר למשחק היא יוצרת עבור האזנה לו thread 
          /// שמשם יאזין לכל בקשות הלקוח 
          /// </summary>
        public void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, 11000);
                tcpListener.Start();
                Console.WriteLine("The server is running. Waiting for connections...");
                byte[] bytes = new byte[1024];
                //string data;
                while (true)
                {
                    try
                    {
                        TcpClient tcpClient = tcpListener.AcceptTcpClient();
                        dancer = new Dancer(tcpClient, this);
                        Thread dancerThread = new Thread(new ThreadStart(dancer.Process));
                        dancerThread.Start();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    #region
                    //NetworkStream stream = tcpClient.GetStream();
                    //int i;
                    //// Loop to receive all the data sent by the client.
                    //i = stream.Read(bytes, 0, bytes.Length);
                    //while (i != 0)
                    //{
                    //    // Translate data bytes to a ASCII string.
                    //    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    //    Console.WriteLine(String.Format("Received: {0}", data));
                    //    // Process the data sent by the client.
                    //    //data = data.ToUpper();
                    //    byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                    //    // Send back a response.
                    //    stream.Write(msg, 0, msg.Length);
                    //    Console.WriteLine(String.Format("Sent: {0}", data));
                    //    Console.WriteLine((data));
                    //    //string strData = String.Format(data);
                    //    int indexOf = data.IndexOf('#');
                    //    Console.WriteLine("index of is " + indexOf);

                    //    if (indexOf != -1)
                    //    {

                    //        string userName = data.Substring(0, indexOf);
                    //        string password = data.Substring(indexOf + 1);
                    //        string userName2 = StrWithout(userName);
                    //        string password2 = StrWithout(password);
                    //        Console.WriteLine("User " + userName2 + " Password " + password2);
                    //        if (dt.IsExist(userName2, password2))
                    //        {
                    //            Console.WriteLine("Exist user!");
                    //            Console.WriteLine( userName + " "+ password);
                    //            msg = System.Text.Encoding.Unicode.GetBytes("Start");
                    //            //  Send back a response.
                    //            stream.Write(msg, 0, msg.Length);
                    //            Console.WriteLine(String.Format("Sent: {0}", data));

                    //        }
                    //        else if (data.StartsWith("-"))
                    //        {
                    //            //int BufferSize = tcpClient.ReceiveBufferSize;
                    //            //FileStream fs;
                    //            if (data == "-Parparim.mp3")
                    //            {
                    //                SendFile("Parparim.mp3");
                    //            }

                    //            else if (data == "-GottaFeeling.mp3")
                    //            {
                    //                SendFile("GottaFeeling.mp3");
                    //            }

                    //        }
                    //        else 
                    //        {
                    //            Console.WriteLine("Its a new player");
                    //            dt.AddDancer(userName, password);
                    //            msg = System.Text.Encoding.Unicode.GetBytes(filenames);
                    //            // Send back a response.
                    //            try
                    //            {
                    //                stream.Write(msg, 0, msg.Length);
                    //            }
                    //            catch(Exception ex)
                    //            {
                    //                Console.WriteLine(ex.Message);
                    //            }
                    //            Console.WriteLine(String.Format("Sent: {0}", data));
                    //        }

                    //        i = stream.Read(bytes, 0, bytes.Length);
                    //    }
                    //}
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }
        //protected internal void SendMsg(string message)
        //{
        //    byte[] data = Encoding.Unicode.GetBytes(message);
        //    dancer.stream.Write(data, 0, data.Length);
        //}
        public void Disconnect()
        {
            tcpListener.Stop();
            Environment.Exit(0);
        }
        public void RemoveConnection(string id)
        {

        }
    }
}
