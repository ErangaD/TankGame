using UnityEngine;
using System.Collections;
using System.Text;
using System;

public class RequestSender : MonoBehaviour {

    public static void sendRequests(string message)
    {
        try
        {
            using (var client = new System.Net.Sockets.TcpClient("127.0.0.1", 6000))
            {
                var byteData = Encoding.ASCII.GetBytes(message);
                client.GetStream().Write(byteData, 0, byteData.Length);
            }
        }
        catch (Exception ex)
        {

            Debug.Break();
        }
    }
}
