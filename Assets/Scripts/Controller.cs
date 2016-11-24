using UnityEngine;
using System.Collections;
using System.Threading;
using System;

public class Controller : MonoBehaviour {

    void Start()
    {
        
        RequestSender.sendRequests("JOIN#");
        Thread oThread = new Thread(new ThreadStart(InputTaker.Connect));
        oThread.Start();
        
        //InputTaker.Connect();

    }

   
    /*static Controller()
{
    Debug.Log("Updating");

    RequestSender.sendRequests("JOIN#");
    InputTaker.Connect("127.0.0.1");
}*/
}
