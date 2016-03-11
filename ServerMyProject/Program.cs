using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;
using System.Runtime.InteropServices;

namespace ServerMyProject
{
	class MainClass
	{
		public static List<Client> clients = new List<Client> ();
		public static List<string> clientsEndPoints = new List<string> ();
		const int TCP_IN_PORT_NUMBER = 15020;
		public static TCPLib server;
		static PerformanceCounter cpuCounter;
	

		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello Server!");
			UDPLib udp = new UDPLib();
			cpuCounter = new PerformanceCounter();
			cpuCounter.CategoryName = "Processor";
			cpuCounter.CounterName = "% Processor Time";
			cpuCounter.InstanceName = "_Total";
			//udp.Start();
			server = new TCPLib ();

			System.Timers.Timer timer = new System.Timers.Timer(3000);
			timer.Elapsed += (source,e) => udp.Send("Hello World!");
			timer.AutoReset=true;
			timer.Enabled=true;

			ConsoleKeyInfo cki;
			do
			{
				if (Console.KeyAvailable)
				{
					cki = Console.ReadKey(true);
					switch (cki.KeyChar)
					{
					case 's':
						udp.Send("Hello World!");
						break;
					case 'x':
						udp.Stop();
						return;
					case 'a':
						System.Timers.Timer timera = new System.Timers.Timer(4000);
						timera.Elapsed += (source,e) => BroadcastMessage(getCurrentCpuUsage());
						timera.AutoReset=true;
						timera.Enabled=true;
						break;
					}
				}
				Thread.Sleep(10);
			} while (true);
		}

		public static void BroadcastMessage(string message){
			Console.WriteLine ("Adept di send");
			Socket socket = server.socket;
			if (socket != null && socket.Connected) {
				Console.WriteLine ("Sending {0} to client {1}", message, socket.LocalEndPoint.ToString());
				socket.Send (Encoding.ASCII.GetBytes (message));
			}
		}

		public static string getCurrentCpuUsage(){
			return Math.Round((decimal)cpuCounter.NextValue(),3).ToString()+"%";
		}
	}
}
