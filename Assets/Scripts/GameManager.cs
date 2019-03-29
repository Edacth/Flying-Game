using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour {

    class saveData
    {
        public saveData(float _sensitivity, bool _yAxisFlipped, int _highScore)
        {
            sensitivity = _sensitivity;
            yAxisFlipped = _yAxisFlipped;
            highScore = _highScore;
        }
        public float sensitivity;
        public bool yAxisFlipped;
        public int highScore;
    }

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
    public int kills;
    public float gunAmmo;
    public float missileAmmo;
    public bool yAxisFlipped { get; set; }
    public float sensitivity { get; set; }

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
        //Load preferences
        Load();

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
                recursiveTransparency(ESectionPool[i].gameObject, false);
            }

            //Move sections back if they have passed the player
            if (ESectionPool[i].transform.position.z < -80)
            {
                ESectionPool[i].transform.position = new Vector3(0, percievedElevation, ESectionPool[i].transform.position.z + (sectionSpeed * Time.deltaTime) + (sectionLength * numOfSections));
                ESectionPool[i].GenerateTower();
                recursiveTransparency(ESectionPool[i].gameObject, true);
            }

        }
       if (kills == 5)
       {
            gunAmmo += 3;
            missileAmmo += 10;
            kills = 0;
       }
        gunAmmo = Mathf.Clamp(gunAmmo, 0, 100);
        missileAmmo = Mathf.Clamp(missileAmmo, 0, 100);

    }

    void recursiveTransparency(GameObject _object, bool fadingIn)
    {
        ITransparancy objectTransScript = _object.GetComponent<ITransparancy>();
        if (objectTransScript != null)// && _object.tag == "Building")
        {
            //objectRenderer.material = transparentMaterial;
            if (!fadingIn) objectTransScript.Fade(0.2f);
            else objectTransScript.startFadingIn();
            //Debug.Break();
        }
        
        for (int j = 0; j < _object.transform.childCount; j++)
        {
            recursiveTransparency(_object.transform.GetChild(j).gameObject, fadingIn);
        }
    }

    void Initialize()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            ESectionPool.Clear();
            gunAmmo = 100;
            missileAmmo = 100;
            for (int i = 0; i < numOfSections; i++)
            {
                Vector3 pos = new Vector3(0, percievedElevation, sectionLength * i + 80);
                GameObject section = Instantiate(environmentSection, pos, Quaternion.identity);
                ESectionPool.Add(section.GetComponent<ESectionController>());

                //Set the scale of the floor
                for (int j = 0; j < section.transform.childCount; j++)
                {
                    if (section.transform.GetChild(j).name == "Floor" || section.transform.GetChild(j).name == "Grass")
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

    public void resetScore()
    {
        if (score > highScore)
        {
            highScore = score;
        }
        score = 0;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        Initialize();
    }

    public void Save()
    {
        saveData data = new saveData(sensitivity, yAxisFlipped, highScore);
        string path = Path.Combine(Application.persistentDataPath, "save.txt");
        string jsonString = JsonUtility.ToJson(data);

        using (StreamWriter streamWriter = File.CreateText(path))
        {
            streamWriter.Write(jsonString);
        }
        Debug.Log("I'M SAVING " + path);
    }

    public void Load()
    {
        string path = Path.Combine(Application.persistentDataPath, "save.txt");
        using (StreamReader streamReader = File.OpenText(path))
        {
            string jsonString = streamReader.ReadToEnd();
            saveData data = JsonUtility.FromJson<saveData>(jsonString);
            sensitivity = data.sensitivity;
            yAxisFlipped = data.yAxisFlipped;
            highScore = data.highScore;
        }
        
    }
}


