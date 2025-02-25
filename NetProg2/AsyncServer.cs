using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetProg2
{
    internal class AsyncServer
    {
        private IPAddress _iPAsress;
        private IPEndPoint _iPEndPoint;
        private Socket _socketServer;

        public AsyncServer(string ip, int port)
        {
            if (port <= 0) throw new ArgumentOutOfRangeException("Error -> Port number incorrect value.");

            if (IPAddress.TryParse(ip, out _iPAsress))
            {
                _iPEndPoint = new IPEndPoint(_iPAsress, port);
                _socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            else
            {
                throw new ArgumentException("Error-> IP adress incorrect format.");
            }
        }

        public void Start()
        {
            _socketServer.Bind(_iPEndPoint);
            _socketServer.Listen(1000);
            Console.WriteLine("Server started");

            while (true)
            {
                _socketServer.BeginAccept(new AsyncCallback(BeginAcceptCallback), _socketServer);
            }
        }

        private void BeginAcceptCallback(IAsyncResult ar)
        {
            Socket socketSvr = (Socket)ar.AsyncState;
            Socket socketClient = socketSvr.EndAccept(ar);

            SO stateObject = new SO();
            stateObject.WorkSocket = socketClient;

            socketClient.BeginReceive(stateObject.Buffer, 0, SO.BufferSize, SocketFlags.None, new AsyncCallback(EndReciveCallback), stateObject);
        }

        private void EndReciveCallback(IAsyncResult ar)
        {
            SO stateObject = (SO)ar.AsyncState;
            Socket handler = stateObject.WorkSocket;

            int len = handler.EndReceive(ar);

            if (len > 0)
            {
                stateObject.Message += Encoding.UTF8.GetString(stateObject.Buffer, 0, len);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Client message:");
                Console.WriteLine(stateObject.Message);
                Console.ResetColor();

                SendMessage(handler, "Привет клиент. Запрос анализируется.");
            }
        }

        private void SendMessage(Socket worlSocket, string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);

            worlSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendMessageCallback), worlSocket);
        }

        private void SendMessageCallback(IAsyncResult ar)
        {
            Socket clientWorkSocket = (Socket)ar.AsyncState;

            try
            {
                clientWorkSocket.EndSend(ar);
                clientWorkSocket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while sending data: " + ex.Message);
            }
            finally
            {
                clientWorkSocket.Close();
            }
        }

    }
}
