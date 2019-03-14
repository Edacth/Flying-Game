using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null; //Instance of this script
    public GameObject environmentSection; 
    public int numOfSections;
    List<ESectionController> ESectionPool = new List<ESectionController>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Start () {
        for (int i = 0; i < numOfSections; i++)
        {
            Vector3 pos = new Vector3(0, -5, 40 * i + 40);
            GameObject section = Instantiate(environmentSection, pos, Quaternion.identity);
            ESectionPool.Add(section.GetComponent<ESectionController>());

            ESectionPool[i].GenerateTower();
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Z))
        {
            for (int i = 0; i < numOfSections; i++)
            {
                ESectionPool[i].GenerateTower();
            }
        }

        for (int i = 0; i < numOfSections; i++)
        {
            if (ESectionPool[i].transform.position.z < -80)
            {
                ESectionPool[i].transform.position = new Vector3(0, -5, ESectionPool[i].transform.position.z + 40 * numOfSections);
            }
            
        }
    }
}
