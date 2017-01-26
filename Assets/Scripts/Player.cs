using UnityEngine;
using System.Collections;

public class Player {
    
    public string Player_name;
    public int locationX, locationY,direction;
    public int health, coins, point;
    public string damage;

    

    public Player(string name,int locationX,int locationY,int direction,int health,int coins,int point,
        string damage) 
    {
        
        this.Player_name = name;
        this.locationX = locationX;
        this.locationY = locationY;
        this.direction = direction;
        this.health = health;
        this.coins = coins;
        this.point = point;
        this.damage = damage;
    }
	
}
