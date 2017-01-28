using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {

    int rotation;
    bool b;
	// Update is called once per frame
	void Update () {
        if (b)
        {
            Vector3 position = transform.position;

            if (rotation == 0)
            {
                position.y -= Time.deltaTime * 3;
            }
            else if (rotation == 2)
            {
                position.y += Time.deltaTime * 3;
            }
            else if (rotation == 1)
            {
                position.x += Time.deltaTime * 3;
            }
            else
            {
                position.x -= Time.deltaTime * 3;
            }
            transform.position = position;
            if (position.x > 9 || position.x < 0 || position.y < 0 || position.y > 9)
            {
                Destroy(gameObject);
            }
        }
    }
    void setPose(int rotation)
    {
        this.rotation = rotation;
    }
    void OnCollisionEnter2D(Collision2D col)
    {

        Destroy(this.gameObject);

    }
}
