using UnityEngine;
using System.Collections;

public class Tank1 : MonoBehaviour {
    volatile Player x;
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
            if (x.health > 0)
            {
                int xPosition = x.locationX;
                int yPosition = -x.locationY;
                Quaternion localRotation = Quaternion.Euler(0, 0, 0); ;
                Vector3 d = new Vector3(xPosition, yPosition, 0);
                int bulletX = xPosition;
                int bulletY = yPosition + 1;
                int rotation = x.direction;
                //Debug.Log(rotation+""+xPosition+""+yPosition);
                switch (rotation)
                {
                    case 1:
                        localRotation = Quaternion.Euler(0, 0, -90);
                        bulletX = xPosition + 1;
                        bulletY = yPosition;
                        //Debug.Log("Direction changed");
                        break;
                    case 2:
                        localRotation = Quaternion.Euler(0, 0, 180);
                        bulletX = xPosition;
                        bulletY = yPosition - 1;
                        break;
                    case 3:
                        localRotation = Quaternion.Euler(0, 0, 90);
                        bulletX = xPosition - 1;
                        bulletY = yPosition;
                        break;
                }
                transform.rotation = localRotation;
                transform.position = d;
                Vector3 fh = new Vector3(bulletX, bulletY, 0);
                if (x.shot == 1)
                {
                    if ((bulletX >= 0 && bulletX < 10) && (bulletY <= 0 && bulletY > -10))
                    {

                        GameObject bull = Instantiate(bullet, fh, localRotation) as GameObject;
                        bull.SendMessage("setPose", rotation);
                        //Physics.IgnoreCollision(bull.GetComponent<Collider>(),this.GetComponent<Collider>());
                    }

                }
            }
            else
            {
                Destroy(this.gameObject);
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
