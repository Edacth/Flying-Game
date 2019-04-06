using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour {

    class saveData
    {
        public saveData(float _sensitivity, bool _yAxisFlipped, bool _firstPlay, int _highScore)
        {
            sensitivity = _sensitivity;
            yAxisFlipped = _yAxisFlipped;
            firstPlay = _firstPlay;
            highScore = _highScore;
        }
        public float sensitivity;
        public bool yAxisFlipped;
        public bool firstPlay;
        public int highScore;
    }

    public static GameManager instance = null; //Instance of this script
    public GameObject environmentSection;
    public GameObject AmmoReplenText;
    public Material transparentMaterial;
    public int numOfSections;
    public float startingSectionSpeed;
    public float sectionLength;
    public float percievedElevation;
    public float zFadePoint;
    public int score;
    public int highScore;
    public int milestoneCount;
    public int totalKills;
    public float gunAmmo;
    public float missileAmmo;
    public bool debugOptions = false;
    public bool yAxisFlipped { get; set; }
    public bool firstPlay { get; set; }
    public float sensitivity;
    public float gameTime;
    public float totalGameTime;
    public float fadeIncrement;

    [SerializeField] float sectionSpeed;
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
        if (debugOptions) sensitivity = 0.5f;

    }

    void Start ()
    {
        //Load preferences
        sectionSpeed = startingSectionSpeed;
        sensitivity = 0.5f;
        firstPlay = true;
        Load();

        menuController = gameObject.GetComponent<MenuController>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        Initialize();

        //Seed the random
        Random.InitState(System.DateTime.Now.Millisecond);
    }
	
	void Update ()
    {

        gameTime += Time.deltaTime;
        totalGameTime += Time.deltaTime;
        if (gameTime >= 15)
        {
            for (int i = 0; i < numOfSections; i++)
            {
                if (ESectionPool[i].genOptions[3].weight < 25)
                    ESectionPool[i].genOptions[3].weight++;
                if (ESectionPool[i].genOptions[4].weight < 30)
                    ESectionPool[i].genOptions[4].weight++;
            }

            sectionSpeed = Mathf.Clamp(sectionSpeed - 2, -50, 0);

            gameTime = 0;
        }

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
                ITransparancy[] transparancyScripts = ESectionPool[i].gameObject.GetComponentsInChildren<ITransparancy>();
                foreach (ITransparancy script in transparancyScripts)
                {
                    if (script != null)
                    {
                        script.Fade(1);
                    }
                }
            }

            //Move sections back if they have passed the player
            if (ESectionPool[i].transform.position.z < -80)
            {
                ESectionPool[i].transform.position = new Vector3(0, percievedElevation, ESectionPool[i].transform.position.z + (sectionSpeed * Time.deltaTime) + (sectionLength * numOfSections));
                ESectionPool[i].GenerateTower();
            }

        }
       if (milestoneCount == 5)
       {
            gunAmmo += 10;
            missileAmmo += 15;
            milestoneCount = 0;
            AmmoReplenText.GetComponent<Animator>().Play("Fade");
       }
        gunAmmo = Mathf.Clamp(gunAmmo, 0, 100);
        missileAmmo = Mathf.Clamp(missileAmmo, 0, 100);
    }

    
    void recursiveTransparency(GameObject _object, bool fadingIn)
    {
        
        ITransparancy objectTransScript = _object.GetComponent<ITransparancy>();
        if (objectTransScript != null)
        {
            objectTransScript.Fade(0.2f);
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
            AmmoReplenText = GameObject.FindGameObjectWithTag("ReplenishText");
            // reset parameters
            gunAmmo = 100;
            missileAmmo = 100;
            milestoneCount = 0;
            sectionSpeed = -25;

            for (int i = 0; i < numOfSections; i++)
            {
                Vector3 pos = new Vector3(0, percievedElevation, sectionLength * i);
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
                if (i > 2)
                {
                    ESectionPool[i].GenerateTower();
                }
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
        sectionSpeed = startingSectionSpeed;
        milestoneCount = 0;
        totalKills = 0;
        gameTime = 0;
        totalGameTime = 0;

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        Initialize();
    }

    public void Save()
    {
        saveData data = new saveData(sensitivity, yAxisFlipped, firstPlay, highScore);
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
        if (!File.Exists(path)) return;

        using (StreamReader streamReader = File.OpenText(path))
        {
            string jsonString = streamReader.ReadToEnd();
            saveData data = JsonUtility.FromJson<saveData>(jsonString);
            sensitivity = data.sensitivity;
            yAxisFlipped = data.yAxisFlipped;
            firstPlay = data.firstPlay;
            highScore = data.highScore;
        }
        
    }

    public void ClearSave()
    {
        string path = Path.Combine(Application.persistentDataPath, "save.txt");
        if (!File.Exists(path)) return;
        File.Delete(path);

        sensitivity = 0.5f;
        yAxisFlipped = false;
        firstPlay = true;
        highScore = 0;
        Save();
        menuController.resetOptionsMenu();
    }
}


