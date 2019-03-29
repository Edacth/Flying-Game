using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;
    public Transform[] missilePoint = new Transform[2];
    public GameObject bullet;
    public GameObject[] bullets = new GameObject[50];
    public GameObject missile;
    public GameObject[] missiles = new GameObject[20];
    public GameObject[] missileDummies = new GameObject[2];
    public GameManager GM;
    public float gunCost;
    public float missileCost;

    [Header("Variables")]
    public float bulletSpeed;
    public float timeToFireBullet;
    public float timeToFireMissile;
    float bulletTimeStamp;
    float missileTimeStamp;

	// Use this for initialization
	void Awake ()
    {
        bulletTimeStamp = missileTimeStamp = Time.time;
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
        // --Untested on phone as of yet
        //-----------------------------------------------------------------------
        if ((Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Stationary) || Input.GetMouseButton(0))
        {
            if (Time.time - bulletTimeStamp > timeToFireBullet)
            {
                fireBullet();
                bulletTimeStamp = Time.time;
            }
        }
        if ((Input.touchCount > 1 && Input.GetTouch(1).phase == TouchPhase.Began) || Input.GetMouseButtonDown(1)) 
            {
            if (Time.time - missileTimeStamp > timeToFireMissile) {
                fireMissile();
                missileTimeStamp = Time.time;
            }
        }
        //-----------------------------------------------------------------------

        //Working and tested on phone, revert back to this if necessary
        //-----------------------------------------------------------------------
        //if (Input.touchCount == 1)
        //{
        //    Touch touch = Input.GetTouch(0);

        //    if (touch.phase == TouchPhase.Stationary)
        //    {
        //        if (Time.time - timeStamp > timeToFire)
        //        {
        //            fireBullet();
        //            timeStamp = Time.time;
        //        }

        //    }
        //}
        //if (Input.touchCount > 1) {
        //    Touch touch = Input.GetTouch(1);

        //    if (touch.phase == TouchPhase.Began) {
        //        fireMissile();
        //    }
        //}
        //-----------------------------------------------------------------------
        for (int i = 0; i < missiles.Length; ++i)
        {
            if (missiles[i].activeSelf)
                missileDummies[i].SetActive(false);
            else
                missileDummies[i].SetActive(true);
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
                    bul.transform.position = firePoint.position;
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
            for (int i = 0; i < missiles.Length; ++i)
            {
                var msl = missiles[i];
                if (!msl.activeSelf)
                {
                    missileDummies[i].SetActive(false);
                    msl.transform.position = missilePoint[i].position;
                    msl.transform.rotation = missilePoint[i].rotation;
                    msl.SetActive(true);
                    GM.missileAmmo -= missileCost;
                    Vector3 launchDirection = (-transform.up * 2);
                    msl.GetComponent<Rigidbody>().AddForce(launchDirection, ForceMode.Impulse);
                    break;
                }
            }
        }
    }
}
