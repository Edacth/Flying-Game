using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorTest : MonoBehaviour
{
    public TextMesh gyroText;
    public GameObject arrow;
    // Use this for initialization
    void Start()
    {
        Input.gyro.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        gyroText.text = Input.gyro.attitude.eulerAngles.ToString();
        arrow.transform.rotation = Input.gyro.attitude;
    }
}