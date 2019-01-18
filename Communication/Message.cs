using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Communication
{

    public interface Message
    {
        string ToString();
    }

    [Serializable]
    public class MsgChat : Message
    {
        private string message;
        private string pseudo;
        private string topic;

        public MsgChat(string message, string pseudo, string topic)
        {
            this.message = message;
            this.pseudo = pseudo;
            this.topic = topic;
        }

        public string Message
        {
            get { return message; }
        }

        public string Pseudo{
            get { return pseudo; }
        }

        public string Topic{
            get { return topic; }
        }

        public override string ToString()
        {
            return "[" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + "] <" + this.pseudo + "> " + message;
        }

    }

    [Serializable]
    public class Account : Message
    {
        private string pseudo;
        private string password;

        public Account(string pseudo, string password)
        {
            this.pseudo = pseudo;
            this.password = password;
        }

        public string Pseudo
        {
            get { return pseudo; }
        }

        public string Password
        {
            get { return password; }
        }

        public override string ToString()
        {
            return this.pseudo + " [" + this.password + "]";
        }

    }



    [Serializable]
    public class VerifAccount : Message
    {
        private bool verif;

        public VerifAccount(bool verif){
            this.verif = verif;
        }

        public bool Verif
        {
            get { return verif; }
        }

        public override string ToString()
        {
            if(this.verif)
            {
                return "Ce compte est ok !";

            }else {
                return "Ce compte n'est pas bon";
            }
        }

    }
}
