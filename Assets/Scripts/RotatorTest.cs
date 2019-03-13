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
        Vector3 rot = new Vector3(Input.gyro.gravity.y, Input.gyro.gravity.x, 0) * 45;
        gyroText.text = (rot).ToString();
        arrow.transform.eulerAngles = rot;
    }
}