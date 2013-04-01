using System;
using System.ServiceModel;

namespace InstantRiceMessenger
{
	class Client
	{
		private ChannelFactory<IServer> channelFactory;

		public Client(string host, int port)
		{
			initialize(host, port);
		}
		public void Restart(string host, int port)
		{
			initialize(host, port);
		}
		private void initialize(string host, int port)
		{
			channelFactory = new ChannelFactory<IServer>(
				new BasicHttpBinding(),
				new EndpointAddress("http://" + host + ":" + port)
			);
		}

		public void Send(string msg)
		{
			IServer proxy = channelFactory.CreateChannel();
			using (proxy as IDisposable)
			{
				proxy.ProcessMsg(msg);
			}
		}
	}
}
