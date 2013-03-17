using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SocketService;

namespace ClientSocketExample
{
    class UCS : UDPClientService
    {
        public UCS(IPEndPoint iep)
            : base(iep)
        { }
        public UCS(int port)
            : base(port)
        { }
        // Really dumb example of processing the recieved buffer. 
        // We just print it, send back another message. Obviously
        // you would want to implement some sort of state machine
        // or intelligence in here.
        public override void ProcessBuffer(StateObject state)
        {
            Console.WriteLine("In ProcessBuffer");
            Console.WriteLine("Recv'd: " + state.sb.ToString());
            StateObject newstate = new StateObject();
            newstate.endPoint = state.endPoint;
            newstate.workSocket = state.workSocket;
            newstate.buffer = Encoding.ASCII.GetBytes("asdfadf");
            this.SendData(newstate, new AsyncCallback(this.OnSent));
        }
    }

    class Program
    {   
        // our endpoint
        public static IPEndPoint iep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4466);
        // our udp client
        public static UCS client = new UCS(iep);

        public static ManualResetEvent allDone = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            StateObject so = client.Connect();
            so.buffer = Encoding.ASCII.GetBytes("oh hai.\n");

            // dumb way of making sure we don't exit before our OnSent 
            // async call back is called. You would implement this 
            // in a more realistic event loop.
            while (true)
            {
                allDone.Reset();
                client.SendData(so, new AsyncCallback(client.OnSent));
                allDone.WaitOne();
            }
        }
    }
}
