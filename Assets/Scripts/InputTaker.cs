using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.IO;
using System.Threading;
using System.Collections.Generic;

public class InputTaker : MonoBehaviour {


    public GameObject enemy1;
    public GameObject rocks;
    public GameObject wall;
    public GameObject water;
    public GameObject health;
    public GameObject myTank;
    public GameObject coins;

    private List<Vector3> wallList = new List<Vector3>();
    private List<Vector3> rockList = new List<Vector3>();
    private List<Vector3> waterList = new List<Vector3>();
    private List<Vector3> coinList = new List<Vector3>();
    private List<float> coinTime = new List<float>();
    private List<Vector3> healthList = new List<Vector3>();
    private List<float> healtTime = new List<float>();
    private List<string> damageDetails = new List<string>();
    private bool boardSet , locatedTank,isCreated;
    Quaternion originalRot;
    Thread oThread;
    Vector3 tankLocation;
    string myTankName;
    void Start()
    {
        originalRot = transform.rotation;
        oThread = new Thread(takeInput);
        oThread.Start();
        Debug.Log("Thread started");
        this.boardSet = true;
        this.locatedTank = true;
    }
    void takeInput()
    {
        System.Net.Sockets.TcpListener clientServer = null;
        try
        {
            var port = 7000;
            var address = System.Net.IPAddress.Parse("127.0.0.1");
            //Debug.Log("Threadswd in");
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

                String[] dataArray = inputStr.Split(':');
                String dType = dataArray[0];
                if (dType.Equals("I"))
                {
                    
                    Debug.Log("Map Data is received");

                    String brickCordinates = dataArray[2];
                    wallList = getVecotors(brickCordinates);
                    
                    String stoneCordinates = dataArray[3];
                    rockList = getVecotors(stoneCordinates);

                    String waterCordinates = dataArray[4].Trim();                    
                    waterCordinates = waterCordinates.Substring(0, waterCordinates.Length - 1);                   
                    waterList = getVecotors(waterCordinates);                    
                    this.boardSet = false;
                    

                }else if (dType.Equals("S")){
                    //Game has started
                    //set the location of the tank
                    string[] withSemiColone = dataArray[1].Split(';');
                    //to identify the tank
                    myTankName = withSemiColone[0];

                    string[] locations = withSemiColone[1].Split(',');
                    Debug.Log("Setting the Tank posit");
                    int locationX = Int32.Parse(locations[0]);
                    int locationY = Int32.Parse(locations[1]);
                    string direction = withSemiColone[2];
                    int directionInt= int.Parse(direction.Substring(0, direction.Length - 1));
                    Debug.Log("before vector");
                    tankLocation= new Vector3(locationX, -(locationY), 0);
                    locatedTank = false;
                    Debug.Log("Set the position of Tank");
                }
                else if (dType.Equals("C"))
                {
                    Debug.Log("Added to Array");
                    string[] positions = dataArray[1].Split(',');
                    int xPosition = Int32.Parse(positions[0]);
                    int yPosition = Int32.Parse(positions[1]);
                    float timeToDisplay = (Int32.Parse(dataArray[2]))/1000;
                    string valueOfCoin = (dataArray[3]);
                    int valuetrue = Int32.Parse(valueOfCoin.Substring(0, valueOfCoin.Length - 1));
                    coinList.Add(new Vector3(xPosition, -(yPosition), 0));
                    coinTime.Add(timeToDisplay);
                }else if (dType.Equals("L"))
                {
                    Debug.Log("Adding life packs"); 
                    string[] lifePacks = dataArray[1].Split(',');
                    string timeOfMedi = dataArray[2];
                    float timeIntMedi = (int.Parse(timeOfMedi.Substring(0, timeOfMedi.Length - 1)))/1000;
                    int xPosition = Int32.Parse(lifePacks[0]);
                    int yPosition = Int32.Parse(lifePacks[1]);
                    healthList.Add(new Vector3(xPosition, -(yPosition), 0));
                    healtTime.Add(timeIntMedi);
                }else if (dType.Equals("G"))
                {
                    int lengthOfArray = dataArray.Length;
                    
                    for(int i = 1; i < lengthOfArray - 1; i++)
                    {
                        string[] playerDetails = dataArray[i].Split(';');
                        string playerName = playerDetails[0];
                        string[] locations = playerDetails[1].Split(',');
                        int xCordintes = int.Parse(locations[0]);
                        int yCordinates = int.Parse(locations[1]);
                        int direction = int.Parse(playerDetails[2]);
                        string shotedStatus = playerDetails[3];
                        int health = int.Parse(playerDetails[4]);
                    }
                    string[] damages = dataArray[lengthOfArray - 1].Split(';');
                    
                }
           }
        }
        catch (Exception e)
        {
            Debug.Log("Crashed");
            Debug.Log(e.Source);
        }
    }
    void Update()
    {
        
        Debug.Log(boardSet);

        if (boardSet==false)
        {
            Debug.Log("In Update finished");

            foreach (Vector3 vec in wallList)
            {
                Instantiate(wall, vec, originalRot);
            }
            foreach (Vector3 vec in rockList)
            {
                Instantiate(rocks, vec, originalRot);
            }
            foreach (Vector3 vec in waterList)
            {
                Instantiate(water, vec, originalRot);
            }
            boardSet = true;
            Debug.Log("Map Update finished");
        }
        if (locatedTank == false)
        {
            if (!isCreated)
            {
                Instantiate(myTank, tankLocation, originalRot);
                isCreated = true;
                Debug.Log("Tank is initiated");
            }
            else
            {
                myTank.transform.position = tankLocation;
                locatedTank = true;
                Debug.Log("Tank Location set");
            }
            
            
        }
        if (coinList.Count != 0)
        {
            Debug.Log("Going to inite");
            GameObject instantiated = (GameObject)Instantiate(coins, coinList[0], originalRot);
            Destroy(instantiated,coinTime[0]);
            coinList.RemoveAt(0);
            coinTime.RemoveAt(0);
        }
        if (healthList.Count != 0)
        {
            Debug.Log("Creating Life Objects");
            GameObject instantiated = (GameObject)Instantiate(health, healthList[0], originalRot);
            Destroy(instantiated, healtTime[0]);
            healthList.RemoveAt(0);
            healtTime.RemoveAt(0);
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
}
