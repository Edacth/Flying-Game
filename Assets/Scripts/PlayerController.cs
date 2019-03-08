﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rBody;
    public Transform gameAnchor;
    public Vector2 axisInput;
    public bool testingOnPC;
    public float moveSpeed;
    public float xBoundary;
    public float yBoundary;
    void Start()
    {
        Input.gyro.enabled = true;
    }

    void FixedUpdate()
    {
        rBody.AddForce(new Vector3(Input.gyro.gravity.x, -Input.gyro.gravity.y, 0) * moveSpeed);
        rBody.rotation = Input.gyro.attitude;
        rBody.AddForce(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * moveSpeed);
    }
}