using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseScript : MonoBehaviour
{
    public void setTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }
}
