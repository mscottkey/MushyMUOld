using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.IO;


public class MushyNet
{
    private TcpClient _client;

    private Thread _thread;

    private NetworkStream _stream;

    private Action<string> _updateMethod;

    public void Connect(string host, int port, Action<string> updateTextBox)
    {
        _updateMethod = updateTextBox;
        _client = new TcpClient();
        _client.Connect(host, port);
        _stream = _client.GetStream();
        _thread = new Thread(CommThread) { IsBackground = true };
        _thread.Start();
    }

    public void Send(string data)
    {
        data = data + "\n";
        _stream.Write(Encoding.ASCII.GetBytes(data), 0, data.Length);
    }

    private void CommThread()
    {
        string line;
        string newLine;
        StreamReader lineRead = new StreamReader(_stream);
        while ((line = lineRead.ReadLine()) != null)
        {
            newLine = line.TrimEnd('\n', '\r');
            _updateMethod.Invoke(newLine);
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