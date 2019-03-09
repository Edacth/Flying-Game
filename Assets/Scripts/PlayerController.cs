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
    public Vector3 origin;
    void Awake()
    {
        Input.gyro.enabled = true;
        SetCalibration();
    }

    void Start()
    {

    }

    void FixedUpdate()
    {
        //rBody.AddForce(transform.forward * moveSpeed);
        //rBody.rotation = Quaternion.Lerp(rBody.rotation, Quaternion.Euler(Input.gyro.rotationRateUnbiased * 10), rotLerp);
        Vector3 newForward = Input.gyro.attitude * Vector3.forward;
        Vector3 difference = newForward - origin;
        transform.rotation = Quaternion.Euler(new Vector3(-(Mathf.Rad2Deg * difference.y), (Mathf.Rad2Deg * difference.x), 0));
        rBody.AddForce(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * moveSpeed);
        //rBody.AddForce(transform.forward * moveSpeed);
        //transform.eulerAngles = new Vector3(difference.x , difference.y, transform.eulerAngles.z);
        //transform.position = new Vector3(Mathf.Clamp(transform.position.x, -xBoundary, xBoundary), Mathf.Clamp(transform.position.y, -yBoundary, yBoundary), transform.position.z);
        for (int i = 0; i < floatingUI.Length - 1; i++) floatingUI[i].forward = Camera.main.transform.forward;
    }
    public void SetCalibration()
    {
        origin = Input.gyro.attitude * Vector3.forward; //Phone's forward
        //transform.position = Vector3.zero;
    }
}