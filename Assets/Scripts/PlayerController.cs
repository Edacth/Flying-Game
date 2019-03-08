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
    void Start()
    {
        Input.gyro.enabled = true;
    }

    void FixedUpdate()
    {
        rBody.AddForce(new Vector3(defaultPos.x - Input.gyro.gravity.x, defaultPos.y + Input.gyro.gravity.y, 0).normalized * moveSpeed);
        rBody.rotation = Input.gyro.attitude;
        rBody.AddForce(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * moveSpeed);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -xBoundary, xBoundary), Mathf.Clamp(transform.position.y, -yBoundary, yBoundary), transform.position.z);
    }
    public void SetCalibration()
    {
        defaultPos = Input.gyro.gravity;
    }
}