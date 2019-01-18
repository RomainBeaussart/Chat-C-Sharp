using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace Client
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Client client = new Client("127.0.0.1", 64000);
            client.start();
        }
    }
}
