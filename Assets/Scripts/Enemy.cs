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

    [Header("Variables")]
    public float bulletSpeed;
    public float timeToFireBullet;
    public float health;
    public float xBoundary;
    public float yBoundary;
    float bulletTimeStamp;

    void Awake()
    {
        bulletTimeStamp = Time.time;
        for (int i = 0; i < bullets.Length; ++i)
        {
            bullets[i] = Instantiate(bullet) as GameObject;
        }
        Player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (health <= 0)
        {
            for (int i = 0; i < bullets.Length; ++i)
            {
                Destroy(bullets[i]);
            }
            Destroy(gameObject);
        }
        if (Time.time - bulletTimeStamp > timeToFireBullet)
        {
            fireBullet();
            bulletTimeStamp = Time.time;
        }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -xBoundary, xBoundary), Mathf.Clamp(transform.position.y, -yBoundary, yBoundary), transform.position.z);
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
}
