using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;

namespace MushyExtensionMethods
{
   public class GameModel
    {
        private TcpClient _client;

        private Thread _thread;

        public NetworkStream _stream;

        private Action<string> _updateMethod;

        public ConnectionInfo _connectInfo;
 
        public GameModel(ConnectionInfo connectInfo)
        {
           _connectInfo = connectInfo;
        }

        public GameController Controller { get; set; }

        public void Connect()
        {
            
            _updateMethod = Controller.HandleDataReceived;
            _client = new TcpClient();
            try
            {
                _client.Connect(_connectInfo.Host, _connectInfo.Port);
                _stream = _client.GetStream();
                _thread = new Thread(CommThread) { IsBackground = true };
                _thread.Start();
            }
            catch
            {
                _updateMethod("Unable to connect to:" + _connectInfo.Name + " @ " + _connectInfo.Host + ":" + _connectInfo.Port);
            }
            
        }

        public void Disconnect()
        {
            // stop the poll thread and close out your connection and stuff
        }

        public void Send(string input)
        {
            string data;
            data = input + "\n";
            _stream.Write(Encoding.ASCII.GetBytes(data), 0, data.Length);
        }
        
        private void CommThread()
        {
            string line;
            StreamReader lineRead = new StreamReader(_stream);
            while ((line = lineRead.ReadLine()) != null)
            {
                Controller.HandleDataReceived(line);
            }

            //var buffer = new byte[1024];
            //while (true)
            //{
            //    var recv = _stream.Read(buffer, 0, buffer.Length);
            //    var data = Encoding.ASCII.GetString(buffer, 0, recv);
            //    _updateMethod.Invoke(data);
            //}
        }
    }
}
