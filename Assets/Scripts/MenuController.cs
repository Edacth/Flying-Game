using System.Collections;
using System.Collections.Generic;
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
    Button exitButton;

    Button startButton;

    void Start () {
        
    }

    public void Initalize()
    {
        GM = gameObject.GetComponent<GameManager>();
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            startButton = GameObject.Find("/Main Camera/Canvas/StartButton").GetComponent<Button>();

            startButton.onClick.AddListener(delegate { SceneManager.LoadScene(gameScene); });
        }
        else
        {
            pauseScreen = GameObject.Find("/Main Camera/Canvas/PauseScreen");
            endScreen = GameObject.Find("/Main Camera/Canvas/EndScreen");
            pauseButton = GameObject.Find("/Main Camera/Canvas/PauseButton").GetComponent<Button>();
            resumeButton = GameObject.Find("/Main Camera/Canvas/PauseScreen/ResumeButton").GetComponent<Button>();
            restartButton = GameObject.Find("/Main Camera/Canvas/EndScreen/RestartButton").GetComponent<Button>();
            exitButton = GameObject.Find("/Main Camera/Canvas/EndScreen/ExitButton").GetComponent<Button>();

            pauseButton.onClick.AddListener(delegate { SetPauseState(true); });
            resumeButton.onClick.AddListener(delegate { SetPauseState(false); });
            restartButton.onClick.AddListener(delegate { GM.reloadScene(); });
            exitButton.onClick.AddListener(delegate { SceneManager.LoadScene("MainMenu"); });
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
