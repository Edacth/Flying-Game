using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;
    public GameObject bullet;
    public GameObject[] bullets = new GameObject[50];
    GameObject Player;
    GameManager GM;

    [Header("Variables")]
    public float moveSpeed;
    public float bulletSpeed;
    public float moveTime;
    public float timeToFireBullet;
    public float xBoundary;
    public float yBoundary;
    public int health;
    float bulletTimeStamp;
    float moveTimer;

    void Awake()
    {
        bulletTimeStamp = Time.time;
        for (int i = 0; i < bullets.Length; ++i)
        {
            bullets[i] = Instantiate(bullet) as GameObject;
        }
        Player = GameObject.FindGameObjectWithTag("Player");
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (health <= 0)
        {
            GM.kills++;
            for (int i = 0; i < bullets.Length; ++i)
            {
                bullets[i].GetComponent<Bullet>().hasParent = false;
            }
            Destroy(gameObject);
        }
        if (transform.position.z > 1 && Time.time - bulletTimeStamp > timeToFireBullet)
        {
            fireBullet();
            bulletTimeStamp = Time.time;
        }
    }

    public void takeDamage(int amount)
    {
        health -= amount;
    }

    void fireBullet()
    {
        foreach (GameObject bul in bullets)
        {
            if (!bul.activeSelf)
            {
                Vector3 targetPos = (Player.transform.position - transform.position).normalized;

                bul.SetActive(true);
                bul.transform.position = firePoint.position;
                bul.transform.LookAt(targetPos);
                bul.GetComponent<Rigidbody>().AddForce(targetPos * bulletSpeed, ForceMode.Impulse);
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            takeDamage(health);
        }
    }
}
