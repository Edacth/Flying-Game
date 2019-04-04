﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Gameplay Variables")]
    public int damage;

    public bool invinc;
    public float invincDuration;
    private float invincTime;

    public bool dead;
    public bool godMode;

    [Header("Physics/Input")]
    public Rigidbody rBody;
    public MeshRenderer mr;
    public Vector2 axisInput;
    public Vector3 defaultPos;
    public float rotLerp;
    public float defaultMoveSpeed;
    public float xBoundary;
    public float yBoundary;
    public Vector3 origin;
    public GameManager GM;
    public MenuController menuController;
    private float scoreTimer;
    public float scoreInterval;
    private float moveSpeed;
    private PlayerShoot playerShootScript;
    public GameObject exhaustParticle;
    public GameObject crossHairs;

    [Header("Misc")]
    public GameObject explosion;

    void Awake()
    {
        Input.gyro.enabled = true;
    }

    void Start()
    {
        SetCalibration();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        menuController = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MenuController>();
        moveSpeed = defaultMoveSpeed * GM.sensitivity * 2;
        playerShootScript = gameObject.GetComponent<PlayerShoot>();
    }
    

    void FixedUpdate()
    {
        if (godMode) damage = 0;
        if (!dead)
        {
            //Physics/Movement
            Vector3 rot = new Vector3(Input.gyro.gravity.y * (GM.yAxisFlipped ? -40 : 40), Input.gyro.gravity.x * 30, -Input.gyro.gravity.x * 45);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rot), rotLerp);
            rBody.AddForce(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical") * (GM.yAxisFlipped ? -1 : 1), 0) * moveSpeed); //Keyboard
            rBody.AddForce(transform.forward * moveSpeed); //Gyroscope (Mobile)
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -xBoundary, xBoundary), Mathf.Clamp(transform.position.y, -yBoundary, yBoundary), transform.position.z);
        
            if (Time.time - invincTime > invincDuration)
                invinc = false;
            if (invinc)
                mr.enabled = !mr.enabled;
            else mr.enabled = true;
        }
        else
        {
            //Death Sequence
            mr.enabled = false;
            rBody.velocity = Vector3.zero;
            GM.gunAmmo = 0;
            GM.missileAmmo = 0;
            playerShootScript.dead = true;
            //playerShootScript.missileDummies[0].SetActive(false);
            //playerShootScript.missileDummies[1].SetActive(false);
            exhaustParticle.SetActive(false);
            crossHairs.SetActive(false);
        }
        if(damage >= 100)
        {
            damage = 100;
            dead = true;
        }
    }
    private void Update()
    {
        if (!dead)
        {
            scoreTimer += Time.deltaTime;
            if (scoreTimer >= scoreInterval)
            {
                scoreTimer = 0;
                GM.score++;
            }
        }
    }
    public void SetCalibration()
    {
        origin = Input.gyro.gravity.normalized; //Phone's forward
        transform.rotation = Quaternion.identity;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!invinc)
        {
            if (other.gameObject.tag == "Building")
                TakeDamage(20);
            if (other.gameObject.tag == "Enemy")
                TakeDamage(5);
        }
    }
    public void TakeDamage (int amount)
    {
        damage += amount;
        if (damage >= 100 && dead == false)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            dead = true;

            menuController.SetEndState(true);
            GM.Save();
        }
        else
        {
            invincTime = Time.time;
            invinc = true;
        }
    }
}