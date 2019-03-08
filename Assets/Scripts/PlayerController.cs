using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rBody;
    public Transform gameAnchor;
    public Vector2 axisInput;
    public Vector3 defaultPos;
    public float rotLerp;
    public float moveSpeed;
    public float xBoundary;
    public float yBoundary;
    public Transform[] floatingUI;
    void Awake()
    {
        Input.gyro.enabled = true;
    }

    void Start()
    {

    }

    void FixedUpdate()
    {
        rBody.AddForce(transform.forward * moveSpeed);
        rBody.rotation = Quaternion.Lerp(rBody.rotation, Quaternion.Euler(Input.gyro.rotationRateUnbiased * 10), rotLerp);
        //transform.position = new Vector3(Mathf.Clamp(transform.position.x, -xBoundary, xBoundary), Mathf.Clamp(transform.position.y, -yBoundary, yBoundary), transform.position.z);
        for (int i = 0; i < floatingUI.Length - 1; i++) floatingUI[i].forward = Camera.main.transform.forward;
    }
    public void SetCalibration()
    {
        transform.position = Vector3.zero;
    }
}