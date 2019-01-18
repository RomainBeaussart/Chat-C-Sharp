using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Communication;

namespace ChatCSharp
{
    public class Server
    {
        private int port;
        private List<Thread> ThreadsClients = new List<Thread>();

        public Server(int port)
        {
            this.port = port;
        }

        public void start()
        {

            //----- Création des comptes ------

            TcpListener l = new TcpListener(new IPAddress(new byte[] { 127, 0, 0, 1 }), port);
            l.Start();
            Console.WriteLine("Server Start !");

            //----- Recherche de nouvelle connexion -----

            while (true)
            {
                TcpClient tcp = l.AcceptTcpClient();
                Console.WriteLine("Connection établie @" + tcp);

                new Thread(new Receiver(tcp).sending).Start();
            }
        }
    }

    class Receiver
    {
        private static List<Account> Accounts = new List<Account>();
        private static List<TcpClient> tcpClients = new List<TcpClient>();
        private TcpClient comm;

        public Receiver(TcpClient comm)
        {
            this.comm = comm;
            tcpClients.Add(comm);
            Account account = new Account("Clnt1", "azerty");
            Account account2 = new Account("Clnt2", "azerty");

            Accounts.Add(account);
            Accounts.Add(account2);
        }

        public void sending()
        {
            //Connexion
            bool connect = false;
            while (!connect)
            {
                Account a = (Account)Net.rcvMsg(comm.GetStream());

                foreach(Account account in Accounts)
                {
                    if(account.Pseudo == a.Pseudo)
                    {
                        if(account.Password == a.Password)
                        {
                            connect = true;
                            break;
                        }
                    }
                }
                if (connect)
                {
                    Net.sendMsg(this.comm.GetStream(), new VerifAccount(true));
                }
                else
                {
                    Net.sendMsg(this.comm.GetStream(), new VerifAccount(false));
                }
            }


            //Envoi des messages

            while (true)
            { 
                MsgChat msg = (MsgChat)Net.rcvMsg(comm.GetStream());
                Console.WriteLine(msg);
                Net.sendMsg(tcpClients, msg);
                
            }
        }
    }
}
