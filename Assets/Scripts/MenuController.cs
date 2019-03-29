using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
    public string gameScene;

    GameManager GM;
    PlayerController playerController;
    GameObject pauseScreen;
    Button pauseButton;
    Button resumeButton;
    GameObject endScreen;
    Button restartButton;
    Button pauseExitButton;
    Button endExitButton;

    GameObject mainScreen;
    Button startButton;
    Button optionsButton;
    GameObject optionsScreen;
    Button backButton;
    Toggle yAxisToggle;
    Slider sensitivitySlider;
    Text sensitivityNumber;
    float sensitivity = 0.5f;
    bool yAxisFlipped = false;


    void Start () {
        
    }

    public void Initalize()
    {
        GM = gameObject.GetComponent<GameManager>();
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            mainScreen = GameObject.Find("/Main Camera/Canvas/MainScreen");
            startButton = GameObject.Find("/Main Camera/Canvas/MainScreen/StartButton").GetComponent<Button>();
            optionsButton = GameObject.Find("/Main Camera/Canvas/MainScreen/OptionsButton").GetComponent<Button>();

            optionsScreen = GameObject.Find("/Main Camera/Canvas/OptionsScreen");
            backButton = GameObject.Find("/Main Camera/Canvas/OptionsScreen/BackButton").GetComponent<Button>();
            sensitivitySlider = GameObject.Find("/Main Camera/Canvas/OptionsScreen/SensitivitySlider").GetComponent<Slider>();
            yAxisToggle = GameObject.Find("/Main Camera/Canvas/OptionsScreen/YAxisToggle").GetComponent<Toggle>();
            sensitivityNumber = GameObject.Find("/Main Camera/Canvas/OptionsScreen/SensitivitySlider/Number").GetComponent<Text>();

            startButton.onClick.AddListener(delegate { SceneManager.LoadScene(gameScene); });
            optionsButton.onClick.AddListener(delegate {
                mainScreen.SetActive(false);
                optionsScreen.SetActive(true);
            });

            backButton.onClick.AddListener(delegate {
                optionsScreen.SetActive(false);
                mainScreen.SetActive(true);
            });
            sensitivitySlider.onValueChanged.AddListener(delegate {
                sensitivity = sensitivitySlider.value;
                sensitivityNumber.text = (sensitivitySlider.value * 100).ToString("f0");
            });
            yAxisToggle.onValueChanged.AddListener(delegate {
                yAxisFlipped = yAxisToggle.isOn; });
        }
        else
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            playerController.sensitivity = sensitivity;
            playerController.yAxisFlipped = yAxisFlipped;

            pauseScreen = GameObject.Find("/Main Camera/Canvas/PauseScreen");
            endScreen = GameObject.Find("/Main Camera/Canvas/EndScreen");
            pauseButton = GameObject.Find("/Main Camera/Canvas/PauseButton").GetComponent<Button>();
            resumeButton = GameObject.Find("/Main Camera/Canvas/PauseScreen/ResumeButton").GetComponent<Button>();
            restartButton = GameObject.Find("/Main Camera/Canvas/EndScreen/RestartButton").GetComponent<Button>();
            pauseExitButton = GameObject.Find("/Main Camera/Canvas/PauseScreen/PauseExitButton").GetComponent<Button>();
            endExitButton = GameObject.Find("/Main Camera/Canvas/EndScreen/EndExitButton").GetComponent<Button>();

            pauseButton.onClick.AddListener(delegate { SetPauseState(true); });
            resumeButton.onClick.AddListener(delegate { SetPauseState(false); });
            restartButton.onClick.AddListener(delegate {
                GM.resetScore();
                GM.reloadScene();
            });
            pauseExitButton.onClick.AddListener(delegate {
                GM.resetScore();
                Time.timeScale = 1;
                SceneManager.LoadScene("MainMenu");
            });
            endExitButton.onClick.AddListener(delegate {
                GM.resetScore();
                SceneManager.LoadScene("MainMenu");
            });
        }  
    }
    public void SetPauseState(bool state)
    {
        pauseScreen.SetActive(state);
        Time.timeScale = state ? 0 : 1;
        pauseButton.gameObject.SetActive(!state);
    }

    public void SetEndState(bool state)
    {
        endScreen.SetActive(state);
        pauseButton.gameObject.SetActive(!state);
    }
}
