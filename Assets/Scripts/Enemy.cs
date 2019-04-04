using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;
    public GameObject bullet;
    public GameObject[] bullets = new GameObject[50];
    public Rigidbody rBody;
    [SerializeField]
    GameObject explosion;
    GameObject Player;
    GameManager GM;

    [Header("Variables")]
    public float bulletSpeed;
    public float timeToFireBullet;
    public float xBoundary;
    public float yBoundary;
    public int health;
    public int pointWorth;

    public float moveSpeed;
    public Vector2 moveTarget;
    public Vector3 moveDirection;
    public float moveTimeStamp;
    public float moveDelay;
    public Vector3 velocity;
    float bulletTimeStamp;
    Vector2 areaClamp;

    void Awake()
    {
        bulletTimeStamp = Time.time;
        // instantiate object pool
        moveTimeStamp = Time.time;
        for (int i = 0; i < bullets.Length; ++i)
        {
            bullets[i] = Instantiate(bullet) as GameObject;
        }
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerController playerScript = Player.GetComponent<PlayerController>();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        areaClamp = new Vector2(playerScript.xBoundary, playerScript.yBoundary);
        moveTarget = new Vector2(Random.Range(-areaClamp.x, areaClamp.x), Random.Range(-areaClamp.y, areaClamp.y));
        moveDirection = new Vector3(transform.position.x - moveTarget.x, transform.position.y - moveTarget.y);
    }
	
	// Update is called once per frame
	void Update ()
    {
        // if enemy dies
		if (health <= 0)
        {
            GM.kills++;
            GM.score += pointWorth;
            for (int i = 0; i < bullets.Length; ++i)
            {
                //De-parents all the bullets from the enemy so they can exist on their own
                bullets[i].GetComponent<Bullet>().hasParent = false;
            }
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        // if enemy is far enough away from the player
        if (transform.position.z > 1 && Time.time - bulletTimeStamp > timeToFireBullet)
        {
            //Fires a bullet and resets the cooldown timer
            fireBullet();
            bulletTimeStamp = Time.time;
        }
        if(Time.time - moveTimeStamp > moveDelay)
        {
            //Calculates a random point within the bounds to fly to
            moveTarget = new Vector2(Random.Range(-areaClamp.x, areaClamp.x), Random.Range(-areaClamp.y, areaClamp.y));
            //Sets the direction to move towards that point
            moveDirection = new Vector2(moveTarget.x - transform.position.x, moveTarget.y - transform.position.y);
            //Resets the timer for selecting new target
            moveTimeStamp = Time.time;
        }
        //Add speed to velocity (additive)
        velocity += (moveDirection * moveSpeed * Time.deltaTime);
        //velocity = new Vector3(velocity.x * Time.deltaTime, velocity.y * Time.deltaTime, velocity.z * Time.deltaTime);
        //Changing rotation according to its movement
        transform.eulerAngles = new Vector3(velocity.y * 120, velocity.x * 60, velocity.x * -70);
        //Change the enemy's position
        if (Time.timeScale != 0) transform.Translate(velocity);
        //Clamp the enemy within a bounding box
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -areaClamp.x, areaClamp.x),
                                         Mathf.Clamp(transform.position.y, -areaClamp.y, areaClamp.y),
                                         transform.position.z);
    }

    public void takeDamage(int amount)
    {
        health -= amount;
    }

    void fireBullet()
    {
        foreach (GameObject bul in bullets)
        {
            // search for first available bullet
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
        // deal damage to player on collision
        if (other.tag == "Player")
        {
            takeDamage(health);
        }
    }
}
