using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {

    volatile int rotation;
    
	// Update is called once per frame
	void Update () {
      
       
            Vector3 position = transform.position;
        //Debug.Log("Position of the current Bullet" + position.y);
            float time = Time.deltaTime*5;
            if (rotation == 2)
            {
                position.y -= time;
            }
            else if (rotation == 0)
            {
                position.y +=time;
            }
            else if (rotation == 1)
            {
                position.x += time ;
            }
            else
            {
                position.x -= time;
            }
            transform.position = position;
            if (position.x > 9 || position.x < 0 || position.y > 0 || position.y < -9)
            {
                Destroy(gameObject);
            }
        
    }
    void setPose(int rotation)
    {
        //Debug.Log("Position Set in the bullet");
        this.rotation = rotation;
    }
    void OnCollisionEnter2D(Collision2D col)
    {

        Debug.Log("Collision Happened with a bullet");
        if(col.gameObject.name=="Tank" || col.gameObject.name == "wall" || col.gameObject.name == "rock")
        {
            //Destroy(this.gameObject);
        }
        

    }
}
