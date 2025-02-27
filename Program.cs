using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DancePadServer
{
    class Program
    {
        static Server server;
        static Thread listenThread;
        /// <summary>
        /// מחלקה ראשית לשרת,היא זאת שמריצה אותו כשהיא מגדירה עצם מסוג Server
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.Title = "Server for Life Dancer";

            try
            {
                server = new Server();
                listenThread = new Thread(new ThreadStart(server.Listen));
                listenThread.Start();
            }
            catch (Exception ex)
            {
                server.Disconnect();
                Console.WriteLine(ex.Message);
            }
        }
    }
}
