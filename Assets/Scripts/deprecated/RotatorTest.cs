using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorTest : MonoBehaviour
{
    public TextMesh gyroText;
    public GameObject arrow;

    Quaternion origin;
    Matrix4x4 baseMat = Matrix4x4.identity;

    public Vector3 adjustedGravity
    {
        get
        {
            return baseMat.MultiplyVector(Input.gyro.gravity);
        }
    }

    // Use this for initialization
    void Start()
    {
        Input.gyro.enabled = true;
        SetCalibration();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rot = new Vector3(adjustedGravity.y + adjustedGravity.z, adjustedGravity.x, 0).normalized;
        gyroText.text = (rot).ToString();
        arrow.transform.eulerAngles = rot;
    }

    public void SetCalibration()
    {
        Quaternion rot = Quaternion.FromToRotation(new Vector3(0,0,-1), Input.gyro.gravity);

        Matrix4x4 mat = Matrix4x4.TRS(Vector3.zero, rot, new Vector3(1.0f, 1.0f, 1.0f));

        baseMat = mat.inverse;
    }
}