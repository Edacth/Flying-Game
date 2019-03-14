using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESectionController : MonoBehaviour {

    Vector3 pos;
    
    public GameObject tower;
    public GameObject arch;

    void Start () {
        
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(0f, 0f, -0.35f);
	}

    public void GenerateTower()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).name == "Tower(Clone)")
            {
                Destroy(gameObject.transform.GetChild(i).gameObject);
            }
        }
        pos = gameObject.transform.position;

        int scructureType = Random.Range(0, 2);
        Vector3 structurePos;

        switch (scructureType)
        {
            case 0:
                structurePos = new Vector3(pos.x + Random.Range(-25, 26), pos.y + 25, pos.z);
                Instantiate(tower, structurePos, Quaternion.identity, gameObject.transform);
                break;

            case 1:
                structurePos = new Vector3(pos.x + Random.Range(-25, 26), pos.y + 25, pos.z);
                Instantiate(arch, structurePos, Quaternion.identity, gameObject.transform);
                break;

        }

        
    }
}
