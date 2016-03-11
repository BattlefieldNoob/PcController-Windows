using System;
using System.Net.Sockets;
using System.Net;

namespace ServerMyProject
{
	public class Client
	{
		TcpClient client;
		TcpListener miniServer;
		const int TCP_PORT_NUMBER = 15010;
		const int TCP_IN_PORT_NUMBER = 15020;
		string centralserver;

		public Client (string serverIP)
		{
			centralserver = serverIP;
			//informo il server della mia esitenza, cosi che possa prendere il mio IP
			client = new TcpClient (serverIP, TCP_PORT_NUMBER);
			Console.WriteLine(client.Client.LocalEndPoint.ToString());
			client.Close ();//chiudo la connessione, per ora non serve
			miniServer=new TcpListener(IPAddress.Any,TCP_IN_PORT_NUMBER);
			miniServer.Start ();
			miniServer.BeginAcceptSocket (Receive, miniServer);//mi metto in ascolto per eventuali messaggi
		}

		public void Receive(IAsyncResult ar){
			Console.WriteLine ("[TCP] ricevuto qualche comando");
		}

	}
}

