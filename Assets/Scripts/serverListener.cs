using UnityEngine;
using System.Collections;
using System;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;


public class serverListener : MonoBehaviour
{

    public bool isconnection;
    public GameObject Wall;
    public GameObject Stone;
    public GameObject Water;
    public GameObject HealthPack;
    public GameObject Coins;

    Quaternion originalRot;
    private System.Collections.Generic.List<Vector3> walls = new System.Collections.Generic.List<Vector3>();
    private System.Collections.Generic.List<Vector3> stones = new System.Collections.Generic.List<Vector3>();
    private System.Collections.Generic.List<Vector3> waters = new System.Collections.Generic.List<Vector3>();
    private bool bordInitialized = true;

    bool b, c = true;
    
    void Start()
    {
        //Console.WriteLine("Thread is comming on the way");
        originalRot = transform.rotation;
        
        var thread = new System.Threading.Thread(listen);
        thread.Start();

    }
    void Update()
    {

        if (!bordInitialized)
        {
            foreach (Vector3 vec in walls)
            {
                Instantiate(Wall, vec, originalRot);
            }
            foreach (Vector3 vec in stones)
            {
                Instantiate(Stone, vec, originalRot);
            }
            foreach (Vector3 vec in waters)
            {
                Instantiate(Water, vec, originalRot);
            }
            bordInitialized = true;
        }
    }
    void listen()
    {
        try
        {
            UnityEngine.Debug.logger.Log("Thread started");
            
            var listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7000);
            listener.Start();

             
            while (true)
            {
                
                string value;

                    
                using (var networkStream = listener.AcceptTcpClient().GetStream())
                {
                    

                    var bytes = new System.Collections.Generic.List<byte>();

                    int asw = 0;
                    while (asw != -1)
                    {
                        asw = networkStream.ReadByte();
                        bytes.Add((Byte)asw);
                    }
                    if (b && c)
                    {

                        b = false;
                    }
                    // Convert bytes to text. 
                    value = Encoding.UTF8.GetString(bytes.ToArray());
                    UnityEngine.Debug.Log(value);

                }
                String[] datas = value.Split(':');
                
                if (datas[0].Equals("I"))
                {
                    
                    String walls = datas[2];
                    this.walls = getVecotors(walls);
                    
                    String stone = datas[3];
                    stones = getVecotors(stone);
                    
                    String water = datas[4].Trim();
                    water = water.Substring(0, water.Length - 2);
                    waters = getVecotors(water);
                    
                    bordInitialized = false;

                }
                // Call an external function (void) given. 

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Occurred");
        }
    }
    System.Collections.Generic.List<Vector3> getVecotors(String data)
    {
        String[] cod = data.Split(';');
        System.Collections.Generic.List<Vector3> vectors = new System.Collections.Generic.List<Vector3>();

        foreach (String e in cod)
        {
            String[] d = e.Split(',');
            Vector3 v = new Vector3(Int32.Parse(d[0]), -Int32.Parse(d[1]), 0);
            vectors.Add(v);
        }
        return vectors;

    }

    // Update is called once per frame

}

