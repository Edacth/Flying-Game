﻿using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
    public string gameScene;

    GameManager GM;
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
    Button optionBackButton;
    Toggle yAxisToggle;
    Slider sensitivitySlider;
    TextMeshProUGUI sensitivityNumber;
    Button htpButton;
    Button clearButton;
    GameObject htpScreen;
    Button htpBackButton;
    Button continueButton;


    public void Initalize()
    {
        GM = gameObject.GetComponent<GameManager>();
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            mainScreen = GameObject.Find("/Main Camera/Canvas/MainScreen");
            startButton = GameObject.Find("/Main Camera/Canvas/MainScreen/StartButton").GetComponent<Button>();
            optionsButton = GameObject.Find("/Main Camera/Canvas/MainScreen/OptionsButton").GetComponent<Button>();

            optionsScreen = GameObject.Find("/Main Camera/Canvas/OptionsScreen");
            optionBackButton = GameObject.Find("/Main Camera/Canvas/OptionsScreen/OptionBackButton").GetComponent<Button>();
            sensitivitySlider = GameObject.Find("/Main Camera/Canvas/OptionsScreen/SensitivitySlider").GetComponent<Slider>();
            yAxisToggle = GameObject.Find("/Main Camera/Canvas/OptionsScreen/YAxisToggle").GetComponent<Toggle>();
            sensitivityNumber = GameObject.Find("/Main Camera/Canvas/OptionsScreen/SensitivitySlider/Number").GetComponent<TextMeshProUGUI>();
            htpButton = GameObject.Find("/Main Camera/Canvas/OptionsScreen/HTPButton").GetComponent<Button>();
            clearButton = GameObject.Find("/Main Camera/Canvas/OptionsScreen/ClearButton").GetComponent<Button>();

            htpScreen = GameObject.Find("/Main Camera/Canvas/HTPScreen");
            htpBackButton = GameObject.Find("/Main Camera/Canvas/HTPScreen/HTPBackButton").GetComponent<Button>();
            continueButton = GameObject.Find("/Main Camera/Canvas/HTPScreen/ContinueButton").GetComponent<Button>();

            startButton.onClick.AddListener(delegate {
                if (GM.firstPlay)
                {
                    htpScreen.SetActive(true);
                    mainScreen.SetActive(false);
                    continueButton.gameObject.SetActive(true);
                    GM.firstPlay = false;
                }
                else
                {
                    SceneManager.LoadScene(gameScene);
                }
            });
            optionsButton.onClick.AddListener(delegate {
                mainScreen.SetActive(false);
                optionsScreen.SetActive(true);
                sensitivitySlider.value = GM.sensitivity;
                yAxisToggle.isOn = GM.yAxisFlipped;
            });

            optionBackButton.onClick.AddListener(delegate {
                optionsScreen.SetActive(false);
                mainScreen.SetActive(true);
                GM.Save();
            });
            sensitivitySlider.onValueChanged.AddListener(delegate {
                GM.sensitivity = sensitivitySlider.value;
                sensitivityNumber.text = (sensitivitySlider.value * 100).ToString("f0");
            });
            yAxisToggle.onValueChanged.AddListener(delegate {
                GM.yAxisFlipped = yAxisToggle.isOn; });

            htpButton.onClick.AddListener(delegate {
                optionsScreen.SetActive(false);
                htpScreen.SetActive(true);
                htpBackButton.gameObject.SetActive(true);
                GM.Save();
            });

            htpBackButton.onClick.AddListener(delegate {
                htpScreen.SetActive(false);
                mainScreen.SetActive(true);
                htpBackButton.gameObject.SetActive(false);
                GM.Save();
            });
            continueButton.onClick.AddListener(delegate { SceneManager.LoadScene(gameScene); });
            clearButton.onClick.AddListener(delegate { GM.ClearSave(); });
        }
        else
        {
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
                GM.Save();
            });
            endExitButton.onClick.AddListener(delegate {
                GM.resetScore();
                optionBackButton.gameObject.SetActive(false);
                continueButton.gameObject.SetActive(false);
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

    public void resetOptionsMenu()
    {
        sensitivitySlider.value = 0.5f;
        yAxisToggle.isOn = false;
    }
}