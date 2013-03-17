using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


namespace SocketService
{
    public abstract class UDPServerService : UDPServices
    {
        public UDPServerService(int port)
            : base(port)
        { }

        public UDPServerService(IPEndPoint iep)
            : base(iep)
        { }

        #region Server Code
        public void StartListening()
        {
            EndPoint localEp = (EndPoint)_iep;
            Socket listener = new Socket(AddressFamily.InterNetwork,
                                          SocketType.Dgram,
                                          ProtocolType.Udp);
            try
            {
                listener.Bind(localEp);
                Console.WriteLine("Waiting for a connection...");
                while (true)
                {
                    allDone.Reset();
                    StateObject so = new StateObject();
                    so.workSocket = listener;
                    
                    IAsyncResult result =
                    listener.BeginReceiveFrom(so.buffer,
                                              0,
                                              StateObject.BUFFER_SIZE,
                                              SocketFlags.None,
                                              ref localEp,
                                              new AsyncCallback(this.OnReceive), so);
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public override void OnSent(IAsyncResult asyncResult)
        {
            StateObject so = (StateObject)asyncResult.AsyncState;
            if (so != null)
            {
                // sent
                Socket s = so.workSocket;
                int sent = s.EndSendTo(asyncResult);
                return;
            }

        }
        #endregion
    }
}