using UnityEngine;
using System.Collections;
using System.Threading;
using System;

public class Controller : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "health")
        {
            Destroy(collision.gameObject);
        }
    }
    
}
