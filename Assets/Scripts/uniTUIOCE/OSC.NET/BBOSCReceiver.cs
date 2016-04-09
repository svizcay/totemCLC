using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class BBOSCReceiver {} // bogus declaration to meet with unity's strict name = filename scheme


namespace OSC.NET
{
	/// <summary>
	/// OSCReceiver
	/// </summary>
	public class OSCReceiver
	{
		protected UdpClient udpClient;
		protected int localPort;

		public OSCReceiver(int localPort)
		{
			this.localPort = localPort;
			Connect();
		}

		public void Connect()
		{
			if(this.udpClient != null) Close();
			this.udpClient = new UdpClient(this.localPort);
		}

		public void Close()
		{
			if (this.udpClient != null) {
//				Debug.Log("ESTA SE TIENE QUE EJECUTAR SI O SI LA PRIMERA VEZ!!!");
//				Debug.Log ("upd client not null...calling close");
				this.udpClient.Close ();
			} else {
//				Debug.Log("udp client null (when calling close");
			}
			this.udpClient = null;
		}

		public OSCPacket Receive()
		{
            try
            {
                IPEndPoint ip = null;
                byte[] bytes = this.udpClient.Receive(ref ip);
                if (bytes != null && bytes.Length > 0)
                    return OSCPacket.Unpack(bytes);

            } catch (Exception e) { 
                Console.WriteLine(e.Message);
                return null;
            }

			return null;
		}
	}
}
