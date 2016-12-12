using UnityEngine;
using System.Collections;
using System.Text;
using System;

public class RequestSender : MonoBehaviour {

    private string message = "JOIN#";
    private void Start()
    {
        try
        {
            using (var client = new System.Net.Sockets.TcpClient("127.0.0.1", 6000))
            {
                Debug.Log("Join Sent");
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
