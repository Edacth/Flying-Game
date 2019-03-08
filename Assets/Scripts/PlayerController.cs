using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rBody;
    public float moveSpeed;
    void Start()
    {

    }

    void FixedUpdate()
    {
        rBody.AddForce(new Vector3(Input.acceleration.x, Input.acceleration.y, 0) * moveSpeed);
        rBody.AddForce(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * moveSpeed);
    }
}