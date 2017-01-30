using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class scoreUpdater : MonoBehaviour {
    public static bool changed;
    public GameObject entry;
    public static List<Player> players;
    // Use this for initialization
    ScoreManager manager;
    void Start () {
        manager = GameObject.FindObjectOfType<ScoreManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (changed)
        {
            //if (manager != null)
            //{
                while (this.transform.childCount > 0)
                {
                    Transform c = this.transform.GetChild(0);
                    c.SetParent(null);
                    Destroy(c.gameObject);
                }
                foreach(Player x in players)
                {
                    GameObject go = (GameObject)Instantiate(entry);
                    go.transform.SetParent(this.transform);
                    go.transform.Find("player").GetComponent<Text>().text = x.Player_name;
                    go.transform.Find("coins").GetComponent<Text>().text = x.coins.ToString();
                    go.transform.Find("points").GetComponent<Text>().text = x.point.ToString();
                    go.transform.Find("health").GetComponent<Text>().text = x.health.ToString()+"%";
                }
           // }
            changed = false;
        }
	}
}
