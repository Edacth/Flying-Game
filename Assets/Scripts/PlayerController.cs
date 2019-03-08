using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rBody;
    public Transform gameAnchor;
    public Vector2 axisInput;
    public Vector3 defaultPos;
    public float moveSpeed;
    public float xBoundary;
    public float yBoundary;
    public Transform[] floatingUI;
    void Start()
    {
        Input.gyro.enabled = true;
    }

    void FixedUpdate()
    {
        rBody.AddForce(new Vector3(defaultPos.x - Input.gyro.gravity.x, defaultPos.y + Input.gyro.gravity.y, 0).normalized * moveSpeed);
        rBody.rotation = Input.gyro.attitude;
        transform.eulerAngles = new Vector3(-rBody.velocity.y, rBody.velocity.x, 0);
        rBody.AddForce(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * moveSpeed);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -xBoundary, xBoundary), Mathf.Clamp(transform.position.y, -yBoundary, yBoundary), transform.position.z);
        for (int i = 0; i < floatingUI.Length - 1; i++) floatingUI[i].forward = Camera.main.transform.forward;
    }
    public void SetCalibration()
    {
        defaultPos = Input.gyro.gravity;
    }
}