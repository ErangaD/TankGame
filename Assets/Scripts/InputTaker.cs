using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.IO;

public class InputTaker : MonoBehaviour {

    public static void Connect()
    {
        System.Net.Sockets.TcpListener clientServer = null;
        try
        {
            var port = 7000;
            var address = System.Net.IPAddress.Parse("127.0.0.1");

            clientServer = new System.Net.Sockets.TcpListener(address, port);

            clientServer.Start();

            while (true)
            {
                string inputStr;

                using (var stream = clientServer.AcceptTcpClient().GetStream())
                {
                    byte[] data = new byte[1024];
                    using (MemoryStream ms = new MemoryStream())
                    {

                        int numBytesRead;
                        while ((numBytesRead = stream.Read(data, 0, data.Length)) > 0)
                        {
                            ms.Write(data, 0, numBytesRead);


                        }
                        inputStr = Encoding.ASCII.GetString(ms.ToArray(), 0, (int)ms.Length);
                    }
                }

                Debug.Log(inputStr);

            }


        }
        catch (Exception e)
        {

        }
    }
}
