using UnityEngine;
using System;

using System.Text;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Collections;

public class InputTaker : MonoBehaviour {

    public GameObject map;
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

    //for the view update purpose
    private List<string> damageDetails = new List<string>();
    
    private List<Player> players = new List<Player>();
    private List<GameObject> playersInst = new List<GameObject>();
    private bool boardSet , locatedTank,isCreated,changeHappened;
    Quaternion originalRot;
    Thread oThread,oThread1;
    Vector3 tankLocation;
    string myTankName;
    private System.Object thisLock = new System.Object();

    

    void Start()
    {
        originalRot = transform.rotation;
        oThread = new Thread(takeInput);
        oThread.IsBackground = true;    
        oThread.Start();
        var instantiatedPrefab =Instantiate(map, new Vector3( 4.5f,-4.5f,0), originalRot) as GameObject;
        //for 20*20 9.5 and -9.5
        instantiatedPrefab.transform.localScale = new Vector3(4.05f, 4.9f,0);
        //for 20*20 8.06 and 9.70
        AICalculation.levels = 10;
        AICalculation.cubes = 10;
        Debug.Log("Thread started");
        this.boardSet = true;
        this.locatedTank = true;

    }
    void addEnemies()
    {
        lock (thisLock)
        {

            players.Clear();
        }
    }

    void enemyCreations()
    {
       
            if (changeHappened)
            {
                lock (thisLock)
                {
                    foreach (GameObject x in playersInst)
                    {
                        Destroy(x);
                    }
                    
                    playersInst.Clear();
                    Debug.Log("Cleared the palyerInst");
                    foreach (Player x in players)
                    {

                        int xPosition = x.locationX;
                        int yPosition = -x.locationY;
                        Quaternion localRotation= Quaternion.Euler(0, 90, 0); ;
                        Vector3 d = new Vector3(xPosition, yPosition, 0);
                        int rotation = x.direction;
                        Debug.Log(rotation+""+xPosition+""+yPosition);
                        switch (rotation)
                        {
                            case 1:
                                localRotation = Quaternion.Euler(0, 0, -90);
                                Debug.Log("Direction changed");
                                break;
                            case 2:
                                localRotation = Quaternion.Euler(0, 0, 180);
                                break;
                            case 3:
                                localRotation = Quaternion.Euler(0, 0, 90);
                                break;
                        }
                        string objName = x.Player_name;
                        if (objName.Equals(myTankName))
                        {
                            Debug.Log("In my TankName");
                            playersInst.Add((GameObject)Instantiate(myTank, d, localRotation));
                        }
                        else
                        {
                            playersInst.Add((GameObject)Instantiate(enemy1, d, localRotation));
                        }
                    }
                    //can also update score inside this method
                    changeHappened = false;
                }

            }else
            {
                Thread.Sleep(300);
            }
        
        
        
    }
    void createGridForCalc()
    {
        AICalculation.createTheFrame();
    }
    void takeInput()
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

                String[] dataArray = inputStr.Split(':');
                String dType = dataArray[0];

                if (dType.Equals("G"))
                {
                    int lengthOfArray = dataArray.Length;
                    string[] damages = dataArray[lengthOfArray - 1].Split(';');
                    int damagesLength = damages.Length;
                    string lastDamage = damages[damagesLength - 1];
                    addEnemies();
                    damages[damagesLength - 1] = lastDamage.Substring(0, lastDamage.Length - 1);
                    for (int i = 1; i < lengthOfArray - 1; i++)
                    {
                        string[] playerDetails = dataArray[i].Split(';');
                        string playerName = playerDetails[0];
                        string[] locations = playerDetails[1].Split(',');
                        int xCordintes = int.Parse(locations[0]);
                        int yCordinates = int.Parse(locations[1]);
                        int direction = int.Parse(playerDetails[2]);
                        string shotedStatus = playerDetails[3];
                        int health = int.Parse(playerDetails[4]);
                        int coins = int.Parse(playerDetails[5]);
                        int points = int.Parse(playerDetails[6]);
                        Player c1 = new Player(playerName, xCordintes, yCordinates, direction, health,
                            coins, points, damages[i - 1]);
                        players.Add(c1);
                    }
                    AICalculation.players = new List<Player>(this.players);
                    AICalculation.changed = true;
                    changeHappened = true;


                }
                else if (dType.Equals("I"))
                {
                    
                    Debug.Log("Map Data is received");
                    //myTankName = dataArray[1];
                    String brickCordinates = dataArray[2];
                    wallList = getVecotors(brickCordinates);
                    AICalculation.wallList = this.wallList;
                    String stoneCordinates = dataArray[3];
                    rockList = getVecotors(stoneCordinates);
                    AICalculation.rockList = this.rockList;
                    String waterCordinates = dataArray[4].Trim();                    
                    waterCordinates = waterCordinates.Substring(0, waterCordinates.Length - 1);                   
                    waterList = getVecotors(waterCordinates);
                    AICalculation.waterList = this.waterList;                
                    this.boardSet = false;
                    //no clarification yet for using a seperate thread
                    Thread r = new Thread(createGridForCalc);
                    r.Start();

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
                    tankLocation= new Vector3(locationX, - (locationY), 0);
                    locatedTank = false;
                    Debug.Log("Set the position of Tank");
                    AICalculation.myTank = myTankName;
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

        enemyCreations();
        //Debug.Log(boardSet);
        if (boardSet==false)
        {

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
            //Debug.Log("Map Update finished");
        }
        if (locatedTank == false)
        {
            if (!isCreated)
            {
                
                playersInst.Add((GameObject)Instantiate(myTank, tankLocation, originalRot));
                
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
            
            Destroy((GameObject)Instantiate(coins, coinList[0], originalRot), coinTime[0]);
            coinList.RemoveAt(0);
            coinTime.RemoveAt(0);
            Debug.Log("Coins were initialized");
        }
        if (healthList.Count != 0)
        {
            
            Destroy((GameObject)Instantiate(health, healthList[0], originalRot), healtTime[0]);
            healthList.RemoveAt(0);
            healtTime.RemoveAt(0);
            Debug.Log("Life Objects initialized");
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
