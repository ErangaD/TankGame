using UnityEngine;
using System.Collections;
using System;

public class CheckDestroy : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Tank")
        {
            
            Destroy(this.gameObject);
        }
    }
    void Update()
    {

    }
}
