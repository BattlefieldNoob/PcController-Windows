using System;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ServerMyProject
{
	public class Server
	{
		const int TCP_PORT_NUMBER = 15010;
		TcpListener server=new TcpListener(IPAddress.Any,15010);
		public Socket socket;

		[DllImport("user32.dll")]
		static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

		public Server ()
		{
			server.Start ();
			Console.WriteLine("TCP Server started listening");
			server.BeginAcceptSocket (new AsyncCallback(ThreadRun), server);
		}

		public void ThreadRun(IAsyncResult ar){
			socket = ((TcpListener)ar.AsyncState).EndAcceptSocket (ar);
			Console.WriteLine ("[TCP] connection with client {0}", socket.LocalEndPoint.ToString());
			startListening ();
		}

		public void onReceive(IAsyncResult ar){
			byte[] bytes = ar.AsyncState as byte[];
			int num=socket.EndReceive(ar);
			byte[] message = new byte[num];
			Buffer.BlockCopy (bytes, 0, message, 0, num);
			string command = Encoding.ASCII.GetString (message);
			Console.WriteLine("Client Say "+command);
			if(command.Contains("vol+")){
				keybd_event((byte)175, 0, 0, 0); // increase volume
			}
			else if(command.Contains("vol-")){
				keybd_event((byte)174, 0, 0, 0); // increase volume
			}
			startListening ();
		}

		public void startListening(){
			byte[] buffer = new byte[30000];
			socket.BeginReceive (buffer, 0, 30000, SocketFlags.None, onReceive, buffer);
		}
	}
}

