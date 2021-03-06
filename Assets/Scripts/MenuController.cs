﻿using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
    public string gameScene;

    //Gameplay UI
    GameManager GM;
    //Different screens
    GameObject pauseScreen, endScreen;
    //Gameplay buttons
    Button pauseButton, pauseExitButton, resumeButton, endExitButton, calibrateButton, restartButton;
    //Results screen
    TextMeshProUGUI enemyText, timeText;

    //MenuUI
    GameObject mainScreen;
    //Main Menu
    Button startButton, optionsButton;
    //Options Menu
    Button optionBackButton, htpButton, clearButton, creditsButton, creditsBackButton;
    //How To Play screen
    Button htpBackButton, continueButton;
    //Menu screens
    GameObject optionsScreen, htpScreen, creditsScreen, planeSelectScreen;
    //Options
    Toggle yAxisToggle;
    Slider sensitivitySlider;
    TextMeshProUGUI sensitivityNumber;
    //Plane Selections
    Button F16Button, A10Button, GyroButton;

    public PlayerController playerController;


    public void Initalize()
    {
        GM = gameObject.GetComponent<GameManager>();
        
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
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
            creditsScreen = GameObject.Find("/Main Camera/Canvas/CreditsScreen");
            creditsButton = GameObject.Find("/Main Camera/Canvas/OptionsScreen/CreditsButton").GetComponent<Button>();
            creditsBackButton = GameObject.Find("/Main Camera/Canvas/CreditsScreen/CreditsBackButton").GetComponent<Button>();

            htpScreen = GameObject.Find("/Main Camera/Canvas/HTPScreen");
            htpBackButton = GameObject.Find("/Main Camera/Canvas/HTPScreen/HTPBackButton").GetComponent<Button>();
            continueButton = GameObject.Find("/Main Camera/Canvas/HTPScreen/ContinueButton").GetComponent<Button>();

            planeSelectScreen = GameObject.Find("/Main Camera/Canvas/PlaneSelectScreen");
            F16Button = GameObject.Find("/Main Camera/Canvas/PlaneSelectScreen/F16Button").GetComponent<Button>();
            A10Button = GameObject.Find("/Main Camera/Canvas/PlaneSelectScreen/A10Button").GetComponent<Button>();
            GyroButton = GameObject.Find("/Main Camera/Canvas/PlaneSelectScreen/GyroButton").GetComponent<Button>();

            startButton.onClick.AddListener(delegate {
                mainScreen.SetActive(false);
                planeSelectScreen.SetActive(true);
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
            });

            continueButton.onClick.AddListener(delegate { SceneManager.LoadScene(gameScene); });
            clearButton.onClick.AddListener(delegate { GM.ClearSave(); });

            creditsButton.onClick.AddListener(delegate {
                optionsScreen.SetActive(false);
                creditsScreen.SetActive(true);
                GM.Save();
            });

            creditsBackButton.onClick.AddListener(delegate {
                creditsScreen.SetActive(false);
                mainScreen.SetActive(true);
            });

            F16Button.onClick.AddListener(delegate {
                GM.planeType = GameManager.PLANETYPE.F16;
                LoadGame();
            });

            A10Button.onClick.AddListener(delegate {
                GM.planeType = GameManager.PLANETYPE.A10;
                LoadGame();
            });

            GyroButton.onClick.AddListener(delegate {
                GM.planeType = GameManager.PLANETYPE.GYRO;
                LoadGame();
            });
        }
        else
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            pauseScreen = GameObject.Find("/Main Camera/Canvas/PauseScreen");
            endScreen = GameObject.Find("/Main Camera/Canvas/EndScreen");
            pauseButton = GameObject.Find("/Main Camera/Canvas/PauseButton").GetComponent<Button>();
            calibrateButton = GameObject.Find("/Main Camera/Canvas/PauseScreen/CalibrateButton").GetComponent<Button>();
            resumeButton = GameObject.Find("/Main Camera/Canvas/PauseScreen/ResumeButton").GetComponent<Button>();
            restartButton = GameObject.Find("/Main Camera/Canvas/EndScreen/RestartButton").GetComponent<Button>();
            pauseExitButton = GameObject.Find("/Main Camera/Canvas/PauseScreen/PauseExitButton").GetComponent<Button>();
            endExitButton = GameObject.Find("/Main Camera/Canvas/EndScreen/EndExitButton").GetComponent<Button>();
            enemyText = GameObject.Find("/Main Camera/Canvas/EndScreen/EnemyText").GetComponent<TextMeshProUGUI>();
            timeText = GameObject.Find("/Main Camera/Canvas/EndScreen/TimeText").GetComponent<TextMeshProUGUI>();


            pauseButton.onClick.AddListener(delegate { SetPauseState(true); });
            calibrateButton.onClick.AddListener(delegate { playerController.SetCalibration(); });
            resumeButton.onClick.AddListener(delegate { SetPauseState(false); });
            restartButton.onClick.AddListener(delegate {
                GM.resetScore();
                StopCoroutine("EndScreenDelay");
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
        enemyText.text = "ENEMIES\nDESTROYED:\n" + GM.totalKills;
        timeText.text = "TIME\nSURVIVED:\n" + GM.totalGameTime.ToString("F2");
    }

    public void resetOptionsMenu()
    {
        sensitivitySlider.value = 0.5f;
        yAxisToggle.isOn = false;
    }

    public IEnumerator EndScreenDelay()
    {
        yield return new WaitForSeconds(1.5f);
        SetEndState(true);
    }

    public void LoadGame()
    {
        if (GM.firstPlay)
        {
            htpScreen.SetActive(true);
            //mainScreen.SetActive(false);
            planeSelectScreen.SetActive(false);
            continueButton.gameObject.SetActive(true);
            GM.firstPlay = false;
        }
        else
        {
            SceneManager.LoadScene(gameScene);
        }
    }
}