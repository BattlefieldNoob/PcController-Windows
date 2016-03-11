using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ServerMyProject
{
	public class UDPer
	{
		const int PORT_NUMBER = 15000;
		const int TCP_PORT_NUMBER = 15010;
		private readonly UdpClient udp = new UdpClient();
		public UDPer ()
		{			
		}

		public void Start()
		{
			Console.WriteLine("Started listening");
			StartListening();
		}
		public void Stop()
		{
			try
			{
				udp.Close();
				Console.WriteLine("Stopped listening");
			}
			catch { /* don't care */ }
		}
			
		private void StartListening()
		{
			udp.BeginReceive(Receive, new object());
		}

		private void Receive(IAsyncResult ar)
		{
			IPEndPoint ip = new IPEndPoint(IPAddress.Any, PORT_NUMBER);
			byte[] bytes = udp.EndReceive(ar, ref ip);
			string message = Encoding.ASCII.GetString(bytes);
			Console.WriteLine("[UDP] From {0} received: {1} ", ip.Address.ToString(), message);
			//dovrei creare un nuovo oggetto Client con parametro l'indirizzo del server
			Console.WriteLine("[UDP] Salvo l'indirizzo ip del server, apro una connessione tcp per dirgli chi sono");
			MainClass.clientsEndPoints.Add (ip.Address.ToString ());
			MainClass.clients.Add (new Client (ip.Address.ToString ()));

		}
		public void Send(string message)
		{
			UdpClient client = new UdpClient();
			IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, PORT_NUMBER);
			byte[] bytes = Encoding.ASCII.GetBytes(message);
			client.Send(bytes, bytes.Length, ip);
			client.Close();
			Console.WriteLine("[UDP] Sent: {0} ", message);
		}
	}
}

