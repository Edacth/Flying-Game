using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform LeadObject;
	// Use this for initialization
	void Start ()
    {
        LeadObject = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = new Vector3(LeadObject.position.x, LeadObject.position.y, LeadObject.position.z - 20) / 2;
	}
}