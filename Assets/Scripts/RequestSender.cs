﻿using UnityEngine;
using System.Collections;
using System.Text;
using System;
using System.Threading;

public class RequestSender : MonoBehaviour {

    private volatile static string message = "JOIN#";
    Thread oThread;

    public static string Message
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
    /*private void Update()
    {

        Thread.Sleep(1000);
        message = "SHOOT#";
        sendRequest();
        Thread.Sleep(1000);
        message = "DOWN#";
        sendRequest();
    }*/

    public static void sendRequest()
    {
            try
            {
                using (var client = new System.Net.Sockets.TcpClient("127.0.0.1", 6000))
                {
                    //Thread.Sleep(2000);
                    //Debug.Log("Join Sent");
                    /*if (!message.Equals(""))
                    {*/
                        var byteData = Encoding.ASCII.GetBytes(Message);
                        client.GetStream().Write(byteData, 0, byteData.Length);
                        Debug.Log(message);
                        
                        
                //}
                }
                
            }
            catch (Exception ex)
            {
                
                Debug.Log("Error");
                //break;
            }
    }
    
}
