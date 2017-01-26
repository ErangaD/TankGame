using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class AICalculation : MonoBehaviour
{

    // Use this for initialization
    public static List<Player> players;
    public static List<Vector3> wallList;
    public static List<Vector3> rockList;
    public static List<Vector3> waterList;
    private static int[][] array;
    private static int[][] rockaDetails;
    public static volatile bool changed;
    public static string myTank;
    private int myLocationX;
    private int myLocationY;
    private int mydirection;
    public static int levels, i;
    public static int cubes;
    // Update is called once per frame
    public static void createTheFrame()
    {
        Thread.Sleep(1000);
        //levels = 10;
        //cubes = 10;
    }
    void updateRockList()
    {
        //This has to be called after updating the rockList vaector 
        int length = rockList.Count;
        for (int i = 0; i < length; i++)
        {
            int[] r = { (int)rockList[i].x, (int)rockList[i].y };
            rockaDetails[i] = r;
        }
    }
    void Update()
    {
        if (changed)
        {
            i = 0;
            foreach (Player c in players)
            {

                if (c.Player_name.Equals(myTank))
                {
                    //getting our position for calculations
                    myLocationX = c.locationX;
                    mydirection = c.direction;
                    myLocationY = c.locationY;
                }
                else
                {
                    int[] enemyLocation = { c.locationX, c.locationY, c.direction };
                    array[i] = enemyLocation;
                    i++;
                }
            }
            foreach (int[] di in array)
            {
                if (myLocationX == di[0])
                {
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
                            if (mydirection == 0)
                            {
                                //send shoot
                            }
                            else
                            {
                                //send up
                            }
                        }
                        else
                        {
                            //I am above the opponent
                            if (mydirection == 2)
                            {
                                //send shoot
                            }
                            else
                            {
                                //send Down
                            }
                        }
                    }

                    if (myLocationY == di[1])
                    {
                        //In the same row
                        if (mydirection == di[2])
                        {
                            //send shoot
                        }
                    }

                }

            }
        }

    }
    void checkYDir()
    {

    }
}
