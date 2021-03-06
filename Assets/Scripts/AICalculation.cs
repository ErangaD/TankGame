﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;

public class AICalculation : MonoBehaviour
{

    // Use this for initialization
    public static List<Player> players;
    public volatile static List<int[]> walldet;
    public static List<Vector3> rockList;
    public static List<Vector3> waterList;
    public static List<GameObject> coins=new List<GameObject>();
    private static List<int[]> array=new List<int[]>();
    private static List<int[]> rockaDetails=new List<int[]>();
    private static List<int[]> waterDetails = new List<int[]>();
    public static volatile bool changed;
    public static string myTank;
    private int myLocationX;
    private int myLocationY;
    private int mydirection;
    private int mydamageLevel;
    public static int numbOfRows, i;
    public static int nmbOfColumns;
    bool second;
    public static string messa;
    Thread oThread;
    // Update is called once per frame
    /*private void Start()
    {
        oThread= new Thread(sendMessage);
        oThread.IsBackground = true;
        second = false;
        oThread.Start();
    }
    private void OnDestroy()
    {
        oThread.Abort();
    }*/
    void sendMessage()
    {
       
           
                if (second)
                {
                    RequestSender.Message = messa;
                    //second = false;
                }
                
                
            
            
        
        
    }
    public static void updateRockList()
    {
        //This has to be called after updating the rockList vaector 
        int length = rockList.Count;
        rockaDetails.Clear();
        waterDetails.Clear();
        for (int i = 0; i < length; i++)
        {
            int[] r = { (int)rockList[i].x,-(int)rockList[i].y };
            rockaDetails.Add(r);
            
        }
        //created waterlist for checkings
        int length1 = waterList.Count;
        for (int i = 0; i < length1; i++)
        {
            int[] r = { (int)waterList[i].x, -(int)waterList[i].y };
            waterDetails.Add(r);
            //Debug.Log(r[0]+"    "+r[1]);
        }
    }
    void Update()
    {
        if (changed)
        {
            array.Clear();
            foreach (Player c in players)
            {

                if (c.Player_name.Equals(myTank))
                {
                    //getting our position for calculations
                    myLocationX = c.locationX;
                    mydirection = c.direction;
                    myLocationY = c.locationY;
                    mydamageLevel = c.health;
                }
                else
                {
                    int[] enemyLocation = { c.locationX, c.locationY, c.direction,c.health };
                    array.Add( enemyLocation);
                }
            }

            int checker = 0;
            foreach (int[] di in array)
            {
               
                if (myLocationX == di[0] && di[3]>0)
                {
                    checker = 1;
                    //checking there are rocks in the way
                    int p = 0;
                    foreach (int[] m in rockaDetails)
                    {
                        if (m[0] == myLocationX)
                        {
                            if ((myLocationY + 1 < m[1] && m[1] < di[1]) || (di[1] + 1 < m[1] && m[1] < myLocationY))
                            {
                                p = 1;
                                break;
                            }
                        }
                    }
                    if (p == 0)
                    {
                        //There are no rocks in the middle

                        if (myLocationY < di[1])
                        {
                            //I am under the opponent
                            if (mydirection == 2)
                            {
                                RequestSender.Message = "SHOOT#";
                            }
                            else
                            {
                                RequestSender.Message = "DOWN#";
                            }
                        }
                        else
                        {
                            //I am above the opponent
                            if (mydirection == 0)
                            {
                                RequestSender.Message = "SHOOT#";
                            }
                            else
                            {
                                RequestSender.Message = "UP#";
                            }
                        }
                    }
                }
                else if (myLocationY == di[1] && di[3] > 0)
                {
                    //In the same row
                    checker = 1;
                    int p = 0;
                    foreach (int[] m in rockaDetails)
                    {
                        if (m[1] == myLocationY)
                        {
                            if ((myLocationX + 1 < m[0] && m[0] < di[0]) || (di[0] + 1 < m[0] && m[0] < myLocationX))
                            {
                                p = 1;
                                break;
                            }
                        }
                    }
                    if (p == 0)
                    {
                        if (myLocationX < di[0])
                        {
                            //I am left to the opponent
                            if (mydirection == 1)
                            {
                                RequestSender.Message = "SHOOT#";
                            }
                            else
                            {
                                RequestSender.Message = "RIGHT#";
                            }
                        }
                        else
                        {
                            if (mydirection == 3)
                            {
                                RequestSender.Message = "SHOOT#";
                            }
                            else
                            {
                                RequestSender.Message = "LEFT#";
                            }
                        }
                    }
                }
                

            }

            //put a condition
            if (checker == 0)
            {
                //There are no one in the sorizontal and vertical lines
                //Debug.Log("In the Checker WOOOO hhhaaaa");
                if (!pathToCoin())
                {

                    if (!second)
                    {
                        if (mydamageLevel <= 2)
                        {
                            //this condition has to be set cosidering the health

                        }
                        else
                        {
                            int x;
                            while (true)
                            {
                                x = UnityEngine.Random.Range(0, 4);
                                if (x == 0)
                                {
                                    if ((myLocationY - 1) >= 0)
                                    {
                                        //search for an enemy in the mylocationy+1 
                                        if (goodForRocks(myLocationY - 1, true))
                                        {
                                            //good to go without rocks
                                            if (readyToGo(myLocationY - 1, true))
                                            {
                                                //No impact of an enemy
                                                //have to check for walls and water
                                                if (checkForWater(myLocationY - 1, true))
                                                {
                                                    //no water no rock no danger
                                                    int r = checkForWalls(myLocationY - 1, true);
                                                    if (r == 0)
                                                    {

                                                        RequestSender.Message = "UP#";
                                                        if (mydirection != 0)
                                                        {

                                                            second = true;
                                                        }
                                                        break;
                                                    }
                                                    else if (r == 1)
                                                    {
                                                        RequestSender.Message = "SHOOT#";
                                                        break;
                                                    }
                                                }
                                            }

                                        }

                                    }
                                }
                                else if (x == 2)
                                {
                                    if (myLocationY + 1 < numbOfRows)
                                    {
                                        //search for an enemy in the mylocationy+1 
                                        if (goodForRocks(myLocationY + 1, true))
                                        {
                                            //good to go without rocks
                                            if (readyToGo(myLocationY + 1, true))
                                            {
                                                //No impact of an enemy
                                                //have to check for walls and water
                                                if (checkForWater(myLocationY + 1, true))
                                                {
                                                    //no water no rock no danger
                                                    int r = checkForWalls(myLocationY + 1, true);
                                                    if (r == 0)
                                                    {
                                                        RequestSender.Message = "DOWN#";

                                                        if (mydirection != 2)
                                                        {

                                                            second = true;
                                                        }

                                                        break;
                                                    }
                                                    else if (r == 1)
                                                    {
                                                        RequestSender.Message = "SHOOT#";
                                                        break;
                                                    }
                                                }
                                            }

                                        }

                                    }
                                }
                                else if (x == 1)
                                {
                                    //going towards right
                                    if (myLocationX + 1 < nmbOfColumns)
                                    {
                                        //search for an enemy in the mylocationy+1 
                                        if (goodForRocks(myLocationX + 1, false))
                                        {
                                            //good to go without rocks
                                            if (readyToGo(myLocationX + 1, false))
                                            {
                                                //No impact of an enemy
                                                //have to check for walls and water
                                                if (checkForWater(myLocationX + 1, false))
                                                {
                                                    //no water no rock no danger
                                                    int r = checkForWalls(myLocationX + 1, false);
                                                    if (r == 0)
                                                    {
                                                        RequestSender.Message = "RIGHT#";
                                                        if (mydirection != 1)
                                                        {
                                                            second = true;
                                                        }
                                                        break;
                                                    }
                                                    else if (r == 1)
                                                    {
                                                        RequestSender.Message = "SHOOT#";
                                                        break;
                                                    }
                                                }
                                            }

                                        }

                                    }
                                }
                                else
                                {
                                    if (myLocationX - 1 >= 0)
                                    {
                                        //search for an enemy in the mylocationy+1 
                                        if (goodForRocks(myLocationX - 1, false))
                                        {
                                            //good to go without rocks
                                            if (readyToGo(myLocationX - 1, false))
                                            {
                                                //No impact of an enemy
                                                //have to check for walls and water
                                                if (checkForWater(myLocationX - 1, false))
                                                {
                                                    //no water no rock no danger
                                                    int r = checkForWalls(myLocationX - 1, false);
                                                    if (r == 0)
                                                    {
                                                        RequestSender.Message = "LEFT#";
                                                        if (mydirection != 3)
                                                        {

                                                            second = true;
                                                        }
                                                        break;
                                                    }
                                                    else if (r == 1)
                                                    {
                                                        RequestSender.Message = "SHOOT#";
                                                        break;
                                                    }
                                                }
                                            }

                                        }

                                    }
                                }
                            }
                        }

                        RequestSender.sendRequest();
                    }
                    else
                    {
                        RequestSender.sendRequest();
                        second = false;
                    }
                    changed = false;

                }

                //Debug.Log("Change was Assessed in the AICALCULATIONS");

                else
                {
                    RequestSender.sendRequest();
                }
                    changed = false;
                }
                else
                {
                    RequestSender.sendRequest();
                    changed = false;
                }
            }
        }

    

    bool pathToCoin()
    {
        foreach(GameObject cv in coins)
        {
            if (cv != null)
            {
                int posx = (int)(cv.transform.position.x);
                int posy = -(int)(cv.transform.position.y);
                //remember that y position is minus
                if ( posx== myLocationX)
                {
                    //there is a coin in x
                    Debug.Log("In the same X");
                    if (myLocationY < posy)
                    {
                        if (goodForRocks(myLocationY + 1, true))
                        {
                            if (checkForWater(myLocationY + 1, true))
                            {
                                if (checkForWalls(myLocationY + 1, true)!=0)
                                {
                                    Debug.Log("In x and checkForWalls not 0");
                                    if (mydirection == 2)
                                    {
                                        //send shoot
                                        RequestSender.Message = "SHOOT#";
                                        return true;
                                    }
                                   
                                }
                                //send down
                                RequestSender.Message = "DOWN#";
                                return true;
                                //return from this
                            }
                        }
                        //coin is in the south
                    }else
                    {
                        //coin is in the upper
                        if (myLocationY - 1 >= 0 && goodForRocks(myLocationY - 1, true) )
                        {
                            if (checkForWater(myLocationY - 1, true))
                            {
                                if (checkForWalls(myLocationY - 1, true) != 0)
                                {
                                    if (mydirection ==0)
                                    {
                                        //send shoot
                                        RequestSender.Message = "SHOOT#";
                                        return true;
                                    }

                                }
                                //send up
                                RequestSender.Message = "UP#";
                                return true;
                                //return from this
                            }
                        }
                    }
                }
                else if(posy==myLocationY)
                {
                    Debug.Log("In the same Y");
                    if (myLocationX < posx)
                    {
                        //coin is in the write
                        if (goodForRocks(myLocationX + 1, false))
                        {
                            if (checkForWater(myLocationX + 1, false))
                            {
                                if (checkForWalls(myLocationX + 1, false) != 0)
                                {
                                    if (mydirection == 1)
                                    {
                                        //send shoot
                                        RequestSender.Message = "SHOOT#";
                                        return true;
                                    }

                                }
                                //send Right
                                RequestSender.Message = "RIGHT#";
                                return true;
                                //return from this
                            }
                        }
                    }
                    else
                    {
                        //coin is in the left
                        if (goodForRocks(myLocationX - 1, false))
                        {
                            if (checkForWater(myLocationX - 1, false))
                            {
                                if (checkForWalls(myLocationX - 1, false) != 0)
                                {
                                    if (mydirection == 3)
                                    {
                                        //send shoot
                                        RequestSender.Message = "SHOOT#";
                                        return true;
                                    }

                                }
                                //send Left
                                RequestSender.Message = "LEFT#";
                                //return from this
                                return true;
                            }
                        }
                    }
                }
            }
        }
        //  return statement to know that know coins in either direction
        return false;
    }

    bool goodForRocks(int position, bool changeForY)
    {
        if (changeForY)
        {
            foreach(int[] df in rockaDetails)
            {
                if(df[0]==myLocationX && df[1] == position)
                {

                    return false;
                }
                
            }
            return true;
        }
        else
        {
            foreach (int[] df in rockaDetails)
            {
                if (df[1] == myLocationY && df[0] == position)
                {
                    return false;
                }
            }
            return true;
        }
    }
    bool readyToGo(int position,bool changeForY)
    {
        if (changeForY)
        {
            //the position will be a y
            foreach (int[] di in array)
            {
                if (position == di[1])
                {
                    //has a opponent in that row
                    //check there are rocks in between
                    foreach (int[] m in rockaDetails)
                    {
                        if (m[1] == position)
                        {
                            if (!((myLocationX + 1 < m[0] && m[0] < di[0]) ||
                                (di[0] + 1 < m[0] && m[0] < myLocationX)))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        else
        {
            foreach (int[] di in array)
            {
                if (position == di[0])
                {
                    //has a opponent in that column
                    //check there are rocks in between
                    foreach (int[] m in rockaDetails)
                    {
                        if (m[0] == position)
                        {
                            if (!((myLocationY + 1 < m[1] && m[1] < di[1]) || (di[1] + 1 < m[1] && m[1] < myLocationY)))
                            {
                                return false;
          
                            }
                        }
                    }
                }
            }
            return true;
        }
        
    }
    bool checkForWater(int position,bool changeForY)
    {
        if (changeForY)
        {
            foreach(int[] dk in waterDetails)
            {
                if (position == dk[1] && myLocationX==dk[0])
                {
                    //we can't make the move 
                    return false;
                }
                
            }
            return true;
        }
        else
        {
            foreach (int[] dk in waterDetails)
            {
                if (position == dk[0] && myLocationY== dk[1])
                {
                    //we can't make the move 
                    return false;
                }
            }
            return true;
        }
        
    }
    int checkForWalls(int position,bool changeForY)
    {
        if (changeForY)
        {
            foreach(int[] fd in walldet)
            {
                
                if ((fd[1]==position && fd[0] == myLocationX )&& fd[2]>0)
                {
                    Debug.Log("change for y" + fd[2]);

                    //there is a wall in the position
                    if (fd[2] <= 2)
                    {
                        return 1;
                    }
                    else
                    {
                        return 2;
                    }
                }
            }
            return 0;
        }
        else
        {
            foreach (int[] fd in walldet)
            {
                
                if ((fd[0] == position && fd[1] == myLocationY )&& fd[2] > 0)
                {
                    Debug.Log("change for X" + fd[2]);
                    //there is a wall in the position
                    if (fd[2] <= 2)
                    {
                        return 1;
                    }
                    else
                    {
                        return 2;
                    }
                }
            }
            return 0;
        }
    }
}
