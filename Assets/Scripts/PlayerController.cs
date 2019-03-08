using System.Collections;
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

    }

    void FixedUpdate()
    {
        if(testingOnPC)
            axisInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        else
            axisInput = new Vector2(Input.acceleration.x, Input.acceleration.y);

        rBody.AddForce(new Vector3(axisInput.x, axisInput.y, 0) * moveSpeed);
        //if (transform.position.x > xBoundary)
        //    transform.position = new Vector3(xBoundary, transform.position.y, transform.position.z);
    }
}