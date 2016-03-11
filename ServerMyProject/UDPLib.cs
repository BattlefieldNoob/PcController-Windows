using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ServerMyProject
{
	public class UDPLib
	{
		const int PORT_NUMBER = 15000;
		private readonly UdpClient udp;
		public UDPLib ()
		{			
			udp = new UdpClient(PORT_NUMBER);
		}

		public void Start()
		{
			Console.WriteLine("[UDP] Started listening");
			StartListening();
		}
		public void Stop()
		{
			try
			{
				udp.Close();
				Console.WriteLine("[UDP] Stopped listening");
			}
			catch { /* don't care */ }
		}
			
		private void StartListening()
		{
			udp.BeginReceive((ar)=>Receive(ar),udp);
		}

		private void Receive(IAsyncResult ar)
		{
			IPEndPoint ip = new IPEndPoint(IPAddress.Any, PORT_NUMBER);
			byte[] bytes = udp.EndReceive(ar, ref ip);
			string message = Encoding.ASCII.GetString(bytes);
			Console.WriteLine("[UDP] Device {0} found me", ip.Address.ToString());
			StartListening ();
			//dovrei creare un nuovo oggetto Client con parametro l'indirizzo del server
		/*	Console.WriteLine("[UDP] Salvo l'indirizzo ip del server, apro una connessione tcp per dirgli chi sono");
			MainClass.clientsEndPoints.Add (ip.Address.ToString ());
			MainClass.clients.Add (new Client (ip.Address.ToString ()));*/
		}

		public void Send(string message)
		{
			UdpClient client = new UdpClient();
			client.Ttl = 1;
			IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, PORT_NUMBER);
			byte[] bytes = Encoding.ASCII.GetBytes(message);
			client.Send(bytes, bytes.Length, ip);
			Console.WriteLine("[UDP] Sent: {0} TTl="+client.Ttl, message);
			client.Close();
		}
	}
}

