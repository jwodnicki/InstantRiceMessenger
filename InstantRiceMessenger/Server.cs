using System;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Windows;

namespace InstantRiceMessenger
{
	public class Server : IServer
	{
		private ServiceHost host;
		private static Speak speak;
		private static Action<string> printer;

		public Server() { }
		public Server(int port, Action<string> _printer)
		{
			initializeConnection(port);
			speak = new Speak();
			printer = _printer;
		}
		public void Start()
		{
			host.Open();
		}
		public void Restart(int port)
		{
			host.Close();
			initializeConnection(port);
			Start();
		}
		private void initializeConnection(int port)
		{
			host = new ServiceHost(typeof(Server));
			host.AddServiceEndpoint(typeof(IServer), new BasicHttpBinding(), "http://127.0.0.1:" + port);

			ServiceMetadataBehavior metadataBehavior = host.Description.Behaviors.Find<ServiceMetadataBehavior>();
			metadataBehavior.HttpGetEnabled = false;
			metadataBehavior.HttpsGetEnabled = false;
			//metadataBehavior.HttpGetUrl = new Uri("http://127.0.0.1:8888");
		}

		public void ProcessMsg(string msg)
		{
			speak.Say(msg);
			printer(((RemoteEndpointMessageProperty)OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name]).Address + ": " + msg);
		}
	}

	[ServiceContract]
	public interface IServer
	{
		[OperationContract(IsOneWay = true)]
		void ProcessMsg(string msg);
	}
}
