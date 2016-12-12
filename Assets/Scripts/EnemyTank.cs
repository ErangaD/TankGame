using UnityEngine;
using System.Collections;

public class EnemyTank : MonoBehaviour {

    public Transform enemyObj;
    

    // Use this for initialization
    void Start () {
        
            
        Instantiate(enemyObj, new Vector3(0.5f, 0.5f+1, 0), Quaternion.identity);
        Instantiate(enemyObj, new Vector3(0.5f+5, 0.5f + 3, 0), Quaternion.identity);
        //Instantiate(enemyObj, new Vector3(0.5f + 5, 0.5f + 3, 0), transform.rotation);



    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
