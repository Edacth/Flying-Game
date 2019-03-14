using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    float timer = 0;

    void OnEnable()
    {

    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
        if (timer >= 1 && gameObject.activeSelf)
        {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.SetActive(false);
            timer = 0;
        }
    }
}
