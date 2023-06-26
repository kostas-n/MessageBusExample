using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassTransit.Abstractions
{
	public class BusConfig
	{
		public string Host { get; set; }
		public ushort Port { get; set; }
		public string VirtualHost { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }

		public Uri AsUri()
		{
			return
				new UriBuilder()
				{
					Scheme = "amqp",
					Host = Host,
					Port = Port,
					UserName = Username,
					Password = Password,
					Path = VirtualHost.Replace("/", "%2f")
				}.Uri;
		}
	}
}
