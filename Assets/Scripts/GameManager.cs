using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null; //Instance of this script
    public GameObject environmentSection;
    public Material transparentMaterial;
    public int numOfSections;
    public float sectionSpeed;
    public float sectionLength;
    public float percievedElevation;
    public float zFadePoint;
    public int score;
    public int highScore;
    public float gunAmmo;
    public float missileAmmo;

    
    List<ESectionController> ESectionPool = new List<ESectionController>();
    bool isReloading = true;
    MenuController menuController;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Start ()
    {
        menuController = gameObject.GetComponent<MenuController>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        Initialize();

        //Seed the random
        Random.InitState(System.DateTime.Now.Millisecond);
    }
	
	void Update ()
    {
        if (isReloading || SceneManager.GetActiveScene().name == "MainMenu") return;
        
            if (score >= highScore)
            {
                highScore = score;
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                for (int i = 0; i < numOfSections; i++)
                {
                    ESectionPool[i].GenerateTower();
                }
            }

            for (int i = 0; i < numOfSections; i++)
            {
                if (ESectionPool[i] == null)
                {
                    Debug.Log("Hi");
                }
                //Translate the sections along
                ESectionPool[i].transform.Translate(0f, 0f, sectionSpeed * Time.deltaTime);
                
                if (ESectionPool[i].transform.position.z < zFadePoint)
                {

                    recursiveTransparency(ESectionPool[i].gameObject);
                }

                //Move sections back if they have passed the player
                if (ESectionPool[i].transform.position.z < -80)
                {
                    ESectionPool[i].transform.position = new Vector3(0, percievedElevation, ESectionPool[i].transform.position.z + sectionLength * numOfSections);
                    ESectionPool[i].GenerateTower();
                }

            }
       
    }

    void recursiveTransparency(GameObject _object)
    {
        ITransparancy objectTransScript = _object.GetComponent<ITransparancy>();
        if (objectTransScript != null && _object.tag == "Building")
        {
            //objectRenderer.material = transparentMaterial;
            objectTransScript.Fade(0.2f);
        }
        
        for (int j = 0; j < _object.transform.childCount; j++)
        {
            recursiveTransparency(_object.transform.GetChild(j).gameObject);
        }
    }

    void Initialize()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            ESectionPool.Clear();
            for (int i = 0; i < numOfSections; i++)
            {
                Vector3 pos = new Vector3(0, percievedElevation, sectionLength * i + 80);
                GameObject section = Instantiate(environmentSection, pos, Quaternion.identity);
                ESectionPool.Add(section.GetComponent<ESectionController>());

                //Set the scale of the floor
                for (int j = 0; j < section.transform.childCount; j++)
                {
                    if (section.transform.GetChild(j).name == "Floor")
                    {
                        section.transform.GetChild(j).transform.localScale = new Vector3(70, 1, sectionLength);
                    }
                }
                ESectionPool[i].GenerateTower();
            }
        }
        menuController.Initalize();
        isReloading = false;
    }

    public void reloadScene()
    {
        isReloading = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        Initialize();
    }

}


