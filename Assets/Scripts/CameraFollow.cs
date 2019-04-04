using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform LeadObject;
    public float distanceFactor;
    public float zOffset;

    public float shakeFactor;
    public float shakeDecrement;

    private float shakeDuration;
    

    // Use this for initialization
    void Start ()
    {
        LeadObject = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (shakeDuration > 0)
        {
            shakeDuration -= shakeDecrement;
        }
        else
        {
            shakeDuration = 0;
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Shake(shakeFactor);
        }
        transform.position = new Vector3(LeadObject.position.x + Random.Range(-shakeDuration, shakeDuration), LeadObject.position.y + 1 + Random.Range(-shakeDuration, shakeDuration), LeadObject.position.z + zOffset) / distanceFactor;
	}
    public void Shake(float factor)
    {
        shakeDuration = factor;
    }
}