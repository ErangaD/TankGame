using UnityEngine;
using System.Collections;
using System.Text;
using System;
using System.Threading;

public class RequestSender : MonoBehaviour {

    private volatile string message = "JOIN#";
    Thread oThread;

    public string Message
    {
        get
        {
            return message;
        }

        set
        {
            message = value;
        }
    }

    private void Start()
    {
        
        oThread = new Thread(sendRequest);
        oThread.IsBackground = true;
        oThread.Start();
    }
    public void sendRequest()
    {
        while (true)
        {
            try
            {
                using (var client = new System.Net.Sockets.TcpClient("127.0.0.1", 6000))
                {
                    Thread.Sleep(1000);
                    //Debug.Log("Join Sent");

                    var byteData = Encoding.ASCII.GetBytes(Message);
                    client.GetStream().Write(byteData, 0, byteData.Length);
                }
                message = "RIGHT#";
                /*Thread.Sleep(500);
                message = "SHOOT#";*/
            }
            catch (Exception ex)
            {
                
                Debug.Log("Error");
                break;
            }
        }
    }
    
}
