using System;
using System.Net;

namespace InstantRiceMessenger
{
	class Connection : ViewModel
	{
		private string _host;
		public string Host
		{
			get { return _host; }
			set
			{
				_host = value;
				if (Client != null)
				{
					Client.Restart(Host, Port);
				}
				NotifyPropertyChanged("Host");
			}
		}

		private int _port;
		public int Port
		{
			get { return _port; }
			set
			{
				_port = value;
				if (Server != null)
				{
					Server.Restart(Port);
				}
				if (Client != null)
				{
					Client.Restart(Host, Port);
				}
				NotifyPropertyChanged("Port");
			}
		}

		public Server Server;
		public Client Client;

		public Connection(string host, int port, Action<string> printer)
		{
			Host = host;
			Port = port;
			Client = new Client(host, port);
			Server = new Server(port, printer);
		}
	}
}
