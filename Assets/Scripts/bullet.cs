using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {

    volatile int rotation;
    //public Transform bulletPrefab;
    // Update is called once per frame
    void Update () {
      
       
            Vector3 position = transform.position;
        //Debug.Log("Position of the current Bullet" + position.y);
            float time = Time.deltaTime*3;
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
            if (position.x > 19 || position.x < 0 || position.y > 0 || position.y < -19)
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

        

        if (col.gameObject.name == "coin")
        {
            Debug.Log("Collision Happened with a bullet and Coin");
            //Transform bullet3 = Instantiate(bulletPrefab) as Transform;
            //col.collider.enabled = false;
            Physics2D.IgnoreLayerCollision(8,9);

        }else
        {
            Destroy(this.gameObject);
        }



    }
}
