using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;
    public Transform missilePoint;
    public GameObject bullet;
    public GameObject[] bullets = new GameObject[50];
    public GameObject missile;
    public GameObject[] missiles = new GameObject[20];
    public GameManager GM;
    public float gunCost;
    public float missileCost;

    [Header("Variables")]
    public float bulletSpeed;
    public float timeToFire;
    float timeStamp;

	// Use this for initialization
	void Awake ()
    {
        timeStamp = Time.time;
		for (int i = 0; i < bullets.Length; ++i)
        {
            bullets[i] = Instantiate(bullet) as GameObject;
        }
        for (int i = 0; i < missiles.Length; ++i)
        {
            missiles[i] = Instantiate(missile) as GameObject;
        }
	}

    private void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Stationary)
            {
                if (Time.time - timeStamp > timeToFire)
                {
                    fireBullet();
                    timeStamp = Time.time;
                }

            }
        } else if (Input.touchCount > 1)
        {
            Touch touch = Input.GetTouch(1);

            if (touch.phase == TouchPhase.Began)
            {
                fireMissile();
            }
        }
	}

    void fireBullet()
    {
        if (GM.gunAmmo > 0)
        {
            foreach (GameObject bul in bullets)
            {
                if (!bul.activeSelf)
                {
                    bul.SetActive(true);
                    GM.gunAmmo -= gunCost;
                    bul.transform.position = new Vector3(firePoint.position.x, firePoint.position.y, firePoint.position.z);
                    bul.transform.rotation = firePoint.rotation;
                    bul.GetComponent<Rigidbody>().AddForce(bul.transform.forward * bulletSpeed, ForceMode.Impulse);
                    break;
                }
            }
        }
    }

    void fireMissile()
    {
        if (GM.missileAmmo > 0)
        {
            foreach (GameObject msl in missiles)
            {
                if (!msl.activeSelf)
                {
                    msl.transform.position = new Vector3(missilePoint.position.x, missilePoint.position.y, firePoint.position.z);
                    //msl.transform.rotation = Quaternion.Euler(missilePoint.rotation.x + 90, missilePoint.rotation.y, missilePoint.rotation.z);
                    msl.transform.rotation = missilePoint.rotation;
                    msl.SetActive(true);
                    GM.missileAmmo -= missileCost;
                    Vector3 launchDirection = -transform.up * 5;
                    msl.GetComponent<Rigidbody>().AddForce(launchDirection, ForceMode.Impulse);
                    break;
                }
            }
        }
    }
}
