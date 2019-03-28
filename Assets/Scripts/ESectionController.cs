using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESectionController : MonoBehaviour {
    enum Structure { Tower, Arch, Windmill};
    Vector3 pos;
    
    public GameObject[] structures;
    public WeightedEntry[] genOptions;

    public void GenerateTower()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).tag == "Building")
            {
                Destroy(gameObject.transform.GetChild(i).gameObject);
            }
        }
        pos = gameObject.transform.position;

         //genOptions = new WeightedEntry[] { new WeightedEntry(0, 0), new WeightedEntry(1, 0), new WeightedEntry(2, 3) };
        int structureType = WeightedRandom.WeightedSelect(genOptions);
        Vector3 structurePos;

        structurePos = new Vector3(pos.x + Random.Range(-15, 16), pos.y + 25, pos.z);
        Instantiate(structures[structureType], structurePos, Quaternion.identity, gameObject.transform);

    }
}