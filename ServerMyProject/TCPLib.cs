using System;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ServerMyProject
{
	public class TCPLib
	{
		const int TCP_PORT_NUMBER = 15010;
		TcpListener server=new TcpListener(IPAddress.Any,15010);
		public Socket socket;

		[DllImport("user32.dll")]
		static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

		Dictionary<string,Action<int>> commands = new Dictionary<string, Action<int>>();

		public TCPLib ()
		{
			server.Start ();
			Console.WriteLine("[TCP] Server started listening");
			server.BeginAcceptSocket (new AsyncCallback(ThreadRun), server);
			commands.Add ("vol+", _=> {
				keybd_event((byte)175, 0, 0, 0); // increase volume
			});
			commands.Add ("vol-", _=> {
				keybd_event((byte)174, 0, 0, 0); // increase volume
			});
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
			if(commands.ContainsKey(command)){
				commands [command].Invoke (0);
			}
			startListening ();
		}

		public void startListening(){
			byte[] buffer = new byte[30000];
			try{
				socket.BeginReceive (buffer, 0, 30000, SocketFlags.None, onReceive, buffer);
			}catch{
				Console.WriteLine ("Exception!");
			}
		}
	}
}

