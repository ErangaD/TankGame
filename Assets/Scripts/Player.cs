using UnityEngine;
using System.Collections;

public class Player {

    private string name;
    private int locationX, locationY,direction;
    private int health, coins, point, damage;

    public string Name
    {
        get
        {
            return name;
        }

       
    }

    public int LocationX
    {
        get
        {
            return locationX;
        }
        
    }

    public int LocationY
    {
        get
        {
            return locationY;
        }

    }

    public int Direction
    {
        get
        {
            return direction;
        }

    }

    public int Health
    {
        get
        {
            return health;
        }
        
    }

    public int Point
    {
        get
        {
            return point;
        }

    }

    public int Coins
    {
        get
        {
            return coins;
        }
        
    }

    public int Damage
    {
        get
        {
            return damage;
        }

    }

    public Player(string name,int locationX,int locationY,int direction,int health,int coins,int point,
        string damage) 
    {
        this.name = name;
        this.locationX = locationX;
        this.locationY = locationY;
        this.direction = direction;
        this.health = health;
        this.coins = coins;
        this.point = point;
        this.damage = damage;
    }
	
}
