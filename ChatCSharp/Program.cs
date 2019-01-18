using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Net.Sockets;


namespace ChatCSharp
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Server server = new Server(64000);
            server.start();

        }

    }
}
