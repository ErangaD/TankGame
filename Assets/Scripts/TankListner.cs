using UnityEngine;
using System.Collections;

public class TankListner : MonoBehaviour
{

    // Use this for initialization
    float speed = 10;
    Vector3 position;
    Vector3 prePosition;
    bool canMove = false;
    int maxMove = 10;
    Quaternion originalRotation;
    float addY = 0;
    float addX = 0;
    bool colided = false;
    string direction = "up";
    void Start()
    {
        originalRotation = transform.rotation;
        position = transform.position;
        prePosition = position;
    }

    // Update is called once per frame
    void Update()
    {

        if (canMove)
        {
            maxMove -= 1;
            if (maxMove == 9)
            {
                prePosition.x = position.x;
                prePosition.y = position.y;
            }
            if (!colided)
            {
                position.x = transform.position.x + addX;
                position.y = transform.position.y + addY;
                transform.position = position;
            }
            if (maxMove <= 0)
            {
                canMove = false;
                maxMove = 10;
            }
        }
        else
        {
            position.x = Mathf.Round(transform.position.x);
            position.y = Mathf.Round(transform.position.y);
            transform.position = position;
            if (Input.GetKey(KeyCode.RightArrow))
            {
                Debug.logger.Log("dfg" + Mathf.Round(transform.position.x) + "  " + (Mathf.Round(transform.position.y)));
                transform.rotation = originalRotation * Quaternion.Euler(0, 0, -90);
                if (Mathf.Round(transform.position.x) < 9)
                {
                    if (direction == "right")
                    {
                        addX = 0.1f;
                    }
                    else { addX = 0f; }
                    direction = "right";

                    addY = 0f;
                    canMove = true;
                    colided = false;
                }
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                Debug.logger.Log("dfg" + Mathf.Round(transform.position.x) + "  " + (Mathf.Round(transform.position.y)));
                transform.rotation = originalRotation * Quaternion.Euler(0, 0, 90);
                if (Mathf.Round(transform.position.x) >= 1)
                {
                    if(direction == "left")
                    {
                        addX = -0.1f;
                    }
                    else { addX = 0f; }
                    direction = "left";                   
                    addY = 0f;
                    canMove = true;
                    colided = false;
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                Debug.logger.Log("dfg" + Mathf.Round(transform.position.x) + "  " + (Mathf.Round(transform.position.y)));
                transform.rotation = originalRotation * Quaternion.Euler(0, 0, 180);
                if (Mathf.Round(transform.position.y) > -9)
                {
                    if(direction=="down")
                        addY = -0.1f;
                    direction = "down";
                    addX = 0f;
                    canMove = true;
                    colided = false;
                }
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                Debug.logger.Log("dfg" + Mathf.Round(transform.position.x) + "  " + (Mathf.Round(transform.position.y)));
                transform.rotation = originalRotation * Quaternion.Euler(0, 0, 0);
                if (Mathf.Round(transform.position.y) <= -1)
                {
                    
                    addX = 0f;
                    
                    if (direction == "up")
                        addY = 0.1f;
                    direction = "up";
                    canMove = true;
                    colided = false;
                }
            }


        }

    }
    public void OnCollisionEnter2D(Collision2D col)
    {
        colided = true;
        transform.position = position;
    }
}
