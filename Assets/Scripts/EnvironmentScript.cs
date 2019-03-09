using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentScript : MonoBehaviour {

    Vector3 pos;
    Vector3 towerPos;
    public GameObject tower;

    void Start () {
        pos = gameObject.transform.position;
        GenerateTower();
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GenerateTower();
        }	
	}

    void GenerateTower()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).name == "Tower(Clone)")
            {
                Destroy(gameObject.transform.GetChild(i).gameObject);
            }
        }
        
        towerPos = new Vector3(pos.x + Random.Range(-10, 10), pos.y + 25, pos.z);
        Instantiate(tower, towerPos, Quaternion.identity, gameObject.transform);
    }
}
