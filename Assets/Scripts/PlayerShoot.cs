using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;
    public Transform[] missilePoint = new Transform[2];
    private GameObject bullet;
    public GameObject weakBullet;
    public GameObject strongBullet;
    public GameObject[] bullets = new GameObject[50];
    public GameObject missile;
    public GameObject[] missiles = new GameObject[20];
    public GameObject[] missileDummies = new GameObject[2];
    public GameManager GM;
    public float gunCost;
    public float missileCost;
    private bool variablesAssigned = false;

    [Header("Variables")]
    public float bulletSpeed;
    public float timeToFireBullet;
    public float timeToFireMissile;
    float bulletTimeStamp;
    float missileTimeStamp;

    [HideInInspector] public bool dead = false;

    // Use this for initialization
    void Awake()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if (GM.planeType == GameManager.PLANETYPE.F16)
        {
            bullet = weakBullet;
            missileCost = 2.5f; //40
            gunCost = 0.2f; //500
        }
        if (GM.planeType == GameManager.PLANETYPE.A10)
        {
            bullet = strongBullet;
            missileCost = 5; //20
            gunCost = 0.1f; //1000
        }
        if (GM.planeType == GameManager.PLANETYPE.GYRO)
        {
            bullet = strongBullet;
            missileCost = 1; //100
            gunCost = 0.1f; //1000
        }
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
    
    void Update()
    {
        if (!variablesAssigned)
        {
            AssignReferences();
            variablesAssigned = true;
        }
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
            if (Time.time - missileTimeStamp > timeToFireMissile)
            {
                fireMissile();
                missileTimeStamp = Time.time;
            }
        }

        for (int i = 0; i < missiles.Length; ++i)
        {
            if (missiles[i].activeSelf || dead)
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
                    bul.transform.position = firePoint.position;
                    bul.transform.rotation = firePoint.rotation;
                    bul.GetComponent<Rigidbody>().AddForce(bul.transform.forward * bulletSpeed, ForceMode.Impulse);
                    GM.gunAmmo -= gunCost;
                    break;
                }
            }
        }
    }

    void fireMissile()
    {
        if (!dead)
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
                    Vector3 launchDirection = (-transform.up * 2);
                    msl.GetComponent<Rigidbody>().AddForce(launchDirection, ForceMode.Impulse);
                    break;
                }
            }
        }
    }
    void AssignReferences()
    {
        firePoint = GameObject.Find("/Player/" + GM.planeType.ToString() + "/FirePoint").transform;
        missilePoint[0] = GameObject.Find("/Player/" + GM.planeType.ToString() + "/Missile Positions/position left").transform;
        missilePoint[1] = GameObject.Find("/Player/" + GM.planeType.ToString() + "/Missile Positions/position right").transform;
        missileDummies[0] = GameObject.Find("/Player/" + GM.planeType.ToString() + "/Missile Positions/position left/missileModel");
        missileDummies[1] = GameObject.Find("/Player/" + GM.planeType.ToString() + "/Missile Positions/position right/missileModel");
        
    }
}
