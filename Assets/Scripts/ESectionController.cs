using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESectionController : MonoBehaviour {
    enum Structure { Tower, Arch};
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
            if (gameObject.transform.GetChild(i).name == "Tower(Clone)" || gameObject.transform.GetChild(i).name == "Arch(Clone)")
            {
                Destroy(gameObject.transform.GetChild(i).gameObject);
            }
        }
        pos = gameObject.transform.position;

        
        WeightedEntry test = new WeightedEntry(1, 1);
        WeightedEntry[] genOptions = new WeightedEntry[] { new WeightedEntry(0, 10), new WeightedEntry(1, 3) };
        int structureType = WeightedRandom.WeightedSelect(genOptions);
        Vector3 structurePos;
        if (structureType == (int)Structure.Tower)
        {
            structurePos = new Vector3(pos.x + Random.Range(-25, 26), pos.y + 25, pos.z);
            Instantiate(tower, structurePos, Quaternion.identity, gameObject.transform);
        }
        else if (structureType == (int)Structure.Arch)
        {
            structurePos = new Vector3(pos.x + Random.Range(-15, 16), pos.y + 18.5f, pos.z);
            Instantiate(arch, structurePos, Quaternion.identity, gameObject.transform);
        }
    }
}