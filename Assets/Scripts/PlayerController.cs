using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public TextMeshProUGUI inputText, originText, outputText;

    void Awake()
    {
        Input.gyro.enabled = true;
    }

    void Start()
    {
        SetCalibration();
    }

    void FixedUpdate()
    {
        //rBody.AddForce(transform.forward * moveSpeed);
        //rBody.rotation = Quaternion.Lerp(rBody.rotation, Quaternion.Euler(Input.gyro.rotationRateUnbiased * 10), rotLerp);
        //Vector3 newForward = Input.gyro.attitude * Vector3.forward;
        //Vector3 difference = new Vector3(newForward.x - origin.x, newForward.y - origin.y, 0);
        //Vector3 difference = new Vector3(origin.x - newForward.x, origin.y - newForward.y, 0);
        Vector3 rot = new Vector3(Input.gyro.gravity.y * 90, Input.gyro.gravity.x * 90, 0);
        inputText.text = (rot).ToString();
        outputText.text = rot.ToString();
        transform.eulerAngles = rot;
        rBody.AddForce(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * moveSpeed);
        rBody.AddForce(transform.forward * moveSpeed);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -xBoundary, xBoundary), Mathf.Clamp(transform.position.y, -yBoundary, yBoundary), transform.position.z);
        for (int i = 0; i < floatingUI.Length - 1; i++) floatingUI[i].forward = Camera.main.transform.forward;
    }
    public void SetCalibration()
    {
        origin = Input.gyro.gravity.normalized; //Phone's forward
        originText.text = origin.ToString();
        transform.rotation = Quaternion.identity;
    }
}