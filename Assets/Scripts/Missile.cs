using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    Rigidbody rBody;

    public Vector3 targetVec;
    public Transform target;
    public float acceleration;
    public float startDelay;

    float timer = 0;
    float delayTimer = 0;

    // Use this for initialization
    void Start ()
    {
        rBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
        if (timer >= 3 && gameObject.activeSelf)
        {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.SetActive(false);
            timer = 0;
            delayTimer = 0;
        }
        if (delayTimer >= startDelay)
        {
            rBody.AddForce(targetVec * acceleration, ForceMode.Force);
        } else
        {
            delayTimer += Time.deltaTime;
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
