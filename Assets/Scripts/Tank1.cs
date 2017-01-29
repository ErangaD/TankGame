using UnityEngine;
using System.Collections;

public class Tank1 : MonoBehaviour {
    volatile Player x;
    public GameObject coin;
    public GameObject bullet;
    // Use this for initialization
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "health" || collision.gameObject.name == "coin")
        {
            //Debug.Log("Collision Happened");
            Destroy(collision.gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
        if (x != null)
        {
            int xPosition = x.locationX;
            int yPosition = -x.locationY;
            Quaternion localRotation = Quaternion.Euler(0, 0, 0);
            Vector3 d = new Vector3(xPosition, yPosition, 0);
            if (x.health > 0)
            {
                int bulletX = xPosition;
                int bulletY = yPosition + 1;
                int rotation = x.direction;
                Vector2 c = Vector2.up;
                //Debug.Log(rotation+""+xPosition+""+yPosition);
                switch (rotation)
                {
                    case 1:
                        localRotation = Quaternion.Euler(0, 0, -90);
                        bulletX = xPosition + 1;
                        bulletY = yPosition;
                        c = Vector2.right;
                        //Debug.Log("Direction changed");
                        break;
                    case 2:
                        localRotation = Quaternion.Euler(0, 0, 180);
                        bulletX = xPosition;
                        bulletY = yPosition - 1;
                        c = Vector2.down;
                        break;
                    case 3:
                        localRotation = Quaternion.Euler(0, 0, 90);
                        bulletX = xPosition - 1;
                        bulletY = yPosition;
                        c = Vector2.left;
                        break;
                }
                transform.rotation = localRotation;
                transform.position = d;
                Vector3 fh = new Vector3(bulletX, bulletY, 0);
                if (x.shot == 1)
                {
                    if ((bulletX >= 0 && bulletX < 20) && (bulletY <= 0 && bulletY > -20))
                    {
                        //RaycastHit2D hit = Physics2D.Raycast(fh,c,.1f);
                        //Debug.DrawLine(transform.position, hit.point, Color.red);
                        //if (hit.collider == null)
                        //{
                            GameObject bull = Instantiate(bullet, fh, localRotation) as GameObject;
                            bull.SendMessage("setPose", rotation);
                        //}    
                    }

                }
            }
            else
            {
                GameObject coin1 = Instantiate(coin, d, localRotation) as GameObject;
                coin1.name = "coin";
                DestroyImmediate(this.gameObject);
            }
        }
        this.x = null;
    }
    void doUpdate(Player x)
    {
        //Debug.Log("In do upadte");
        this.x = x;
    }
}
