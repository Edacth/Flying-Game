using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    public Transform firePoint;
    public GameObject bullet;
    public GameObject[] bullets = new GameObject[100];

    public float fireRate;
    float timeStamp;

	// Use this for initialization
	void Awake ()
    {
        timeStamp = Time.time;
		for(int i = 0; i < bullets.Length - 1; i++)
        {
            bullets[i] = Instantiate(bullet) as GameObject;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Stationary)
            {
                if (Time.time - timeStamp > fireRate)
                {
                    fireBullet();
                    timeStamp = Time.time;
                }

            }
        }
	}

    void fireBullet()
    {
        foreach (GameObject bul in bullets)
        {
            if (bul.activeSelf == false)
            {
                bul.SetActive(true);
                bul.transform.position = new Vector3(firePoint.position.x, firePoint.position.y, firePoint.position.z);
                bul.transform.rotation = firePoint.rotation;
                bul.GetComponent<Rigidbody>().AddForce(bul.transform.forward * 30, ForceMode.Impulse);
                break;
            }
        }
    }
}
