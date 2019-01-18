using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Sockets;

namespace Communication
{

    public class Net
    {
        public static void sendMsg(List<TcpClient> tcpClients, Message msg)
        {
            BinaryFormatter bf = new BinaryFormatter();
            foreach(TcpClient tcp in tcpClients)
                bf.Serialize(tcp.GetStream(), msg);
        }

        public static void sendMsg(Stream s, Message msg)
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(s, msg);
        }

        public static Message rcvMsg(Stream s)
        {
            BinaryFormatter bf = new BinaryFormatter();
            return (Message)bf.Deserialize(s);
        }
    }
}
