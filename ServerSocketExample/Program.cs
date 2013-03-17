using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SocketService;

// author @_wirepair : github.com/wirepair
// date: 03172013 
// copyright: ME AND MINE but i guess you can use it :D.

namespace ServerSocketExample
{
    class USS : UDPServerService
    {
        public USS(IPEndPoint iep)
            : base(iep)
        { }
        public USS(int port)
            : base(port)
        { }

        public override void ProcessBuffer(StateObject state)
        {
            StateObject sendState = new StateObject();
            // Need this for sending data backsies.
            sendState.endPoint = state.endPoint;
            sendState.workSocket = state.workSocket;

            Console.WriteLine("We recv'd: " + state.sb.ToString());

            sendState.buffer = Encoding.ASCII.GetBytes("oh haizzzz....");
            SendData(sendState, new AsyncCallback(this.OnSent));
        }
    }

    class Program
    {
        // create server listening on loopback:4466
        static USS uss = new USS(4466);
        static void Main(string[] args)
        {
            // Just call startListening to begin recv'ing data.
            uss.StartListening();
        }
    }
}
