using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Communication;
using System.Threading;

namespace Client
{
    public class Client
    {
        private readonly string hostname;
        private readonly int port;
        private string pseudo, topic;
        private TcpClient comm;

        public Client(string h, int p)
        {
            this.hostname = h;
            this.port = p;
            this.topic = "main";
        }

        public void start()
        {
            comm = new TcpClient(hostname, port);
            bool verif = false;
            while(!verif)
            {
                Console.WriteLine("Pseudo");
                this.pseudo = Console.ReadLine();
                Console.WriteLine("Password");
                string pwd = Console.ReadLine();
                Net.sendMsg(comm.GetStream(), new Account(this.pseudo,pwd));
                VerifAccount verifAccount = (VerifAccount)Net.rcvMsg(comm.GetStream());
                verif = verifAccount.Verif;
                if (!verif)
                {
                    Console.WriteLine("Incorrect login or password. Please try again ! ");
                }
            }

            connexion();

        }

        private void connexion()
        {
            Console.WriteLine("Vous êtes connécté");

            Console.WriteLine("====== HELP ======");
            Console.WriteLine("/psdo <Nickname> : Change nickname");
            Console.WriteLine("/tpic <Topic> : Change topic");
            Console.WriteLine("/mesg <Pseudo> : Send private messages");
            Console.WriteLine("/help: All commands");
            Console.WriteLine("==================");

            Console.WriteLine("You are in the topic [main]");

            //Thread ecriture des messages
            Thread thWriteMessage = new Thread(new ThreadStart(writeMessage));
            thWriteMessage.Start();

            //Thread lecturedes messages
            Thread thReadMessage = new Thread(new ThreadStart(readMessage));
            thReadMessage.Start();

        }

        public void readMessage()
        {
            while (true)
            { 
                MsgChat reciv = (MsgChat)Net.rcvMsg(comm.GetStream());
                if (reciv.Topic == this.topic)
                {
                    Console.WriteLine(reciv);
                } else if(reciv.Topic == this.pseudo)
                {
                    Console.WriteLine("[" + reciv.Pseudo + "->" + reciv.Topic + "] : " + reciv);
                }
            }
        }

        public void writeMessage()
        {
            while (true)
            {
                string msg = Console.ReadLine();
                char[] arr = msg.ToCharArray();
                if (arr[0].Equals('/'))
                {
                    string cmd = new string(msg.ToCharArray(1, 4));
                    if (cmd == "psdo")
                    {
                        this.pseudo = new string(msg.ToCharArray(6, msg.Length-6));
                        Console.WriteLine("Your new username is "+this.pseudo);
                    }
                    else if (cmd == "tpic")
                    {
                        this.topic = new string(msg.ToCharArray(6, msg.Length-6));
                        Console.WriteLine("You are in the topic [" + this.topic+"]");
                    }
                    else if (cmd == "mesg")
                    {
                        string toPseudo = new string (msg.ToCharArray(6, msg.Length - 6));
                        Console.WriteLine("Write your message to " + toPseudo);
                        Net.sendMsg(comm.GetStream(), new MsgChat(Console.ReadLine(), this.pseudo, toPseudo));
                    }
                    else if (cmd== "help")
                    {
                        Console.WriteLine("====== HELP ======");
                        Console.WriteLine("/psdo <Nickname> : Change nickname");
                        Console.WriteLine("/tpic <Topic> : Change topic");
                        Console.WriteLine("/mesg <Pseudo> : Send private messages");
                        Console.WriteLine("/help: All commands");
                        Console.WriteLine("==================");
                    } else {
                        Console.WriteLine("Command not found !");
                    }
                }
                else
                {
                    Net.sendMsg(comm.GetStream(), new MsgChat(msg, this.pseudo, this.topic));
                }
            }
        }

    }
}

