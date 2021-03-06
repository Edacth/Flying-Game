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
    public GameObject mr;
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
    [Tooltip("PC Version only.")] public float rotationLimiter;
    private PlayerShoot playerShootScript;
    public GameObject exhaustParticle;
    public GameObject crossHairs;
    public BoxCollider hitbox;
    public CameraFollow cameraScript;
    public GameObject F16, A10, Gyro;

    [Header("Misc")]
    public GameObject explosion;

    Matrix4x4 baseMat = Matrix4x4.identity;
    Vector3 adjustedGravity
    {
        get
        {
            return baseMat.MultiplyVector(Input.gyro.gravity);
        }
    }

    void Awake()
    {
        Input.gyro.enabled = true;
    }

    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        switch(GM.planeType)
        {
            case GameManager.PLANETYPE.F16:
                F16.SetActive(true);
                mr = F16;
                break;
            case GameManager.PLANETYPE.A10:
                A10.SetActive(true);
                mr = A10;
                break;
            case GameManager.PLANETYPE.GYRO:
                Gyro.SetActive(true);
                mr = Gyro;
                break;
        }
        SetCalibration();
        menuController = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MenuController>();
        moveSpeed = defaultMoveSpeed * GM.sensitivity * 2;
        playerShootScript = gameObject.GetComponent<PlayerShoot>();
        cameraScript = Camera.main.GetComponent<CameraFollow>();
        // get player's collider
        hitbox = GetComponent<BoxCollider>();
    }
    

    void FixedUpdate()
    {
        if (godMode) damage = 0;
        if (!dead)
        {
            //Physics/Movement

            if (!GM.PC) //Android
            {
                //Rotate the player based on gyro input
                Vector3 rot = new Vector3(adjustedGravity.y * (GM.yAxisFlipped ? -40 : 40), adjustedGravity.x * 30, -adjustedGravity.x * 45);
                //Lerp current rotation to new rotation
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rot), rotLerp);
                //Add force in the direction the plane is facing
                rBody.AddForce(transform.forward * moveSpeed);
            }
            else //PC
            {
                //Add force in direction of input (WASD)
                rBody.AddForce(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical") * (GM.yAxisFlipped ? -1 : 1), 0) * moveSpeed);
                Vector3 correctedVelocity = rBody.velocity / rotationLimiter;
                Vector3 rot = new Vector3(correctedVelocity.y * -40, correctedVelocity.x * 30, -correctedVelocity.x * 45);
                //Lerp current rotation to new rotation
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rot), rotLerp);
            }

            

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -xBoundary, xBoundary), Mathf.Clamp(transform.position.y, -yBoundary, yBoundary), transform.position.z);
        
            if (Time.time - invincTime > invincDuration)
                invinc = false;
            if (invinc)
                mr.SetActive(!mr.activeSelf);
            else mr.SetActive(true);
        }
        else
        {
            //Death Sequence
            mr.SetActive(false);
            hitbox.enabled = false;
            rBody.velocity = Vector3.zero;
            GM.gunAmmo = 0;
            GM.missileAmmo = 0;
            playerShootScript.dead = true;
            //exhaustParticle.SetActive(false);
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
        Quaternion rot = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, -1.0f), Input.gyro.gravity);

        Matrix4x4 mat = Matrix4x4.TRS(Vector3.zero, rot, new Vector3(1.0f, 1.0f, 1.0f));

        baseMat = mat.inverse;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!invinc)
        {
            if (other.gameObject.tag == "Building")
                TakeDamage(20, 0.5f);
            if (other.gameObject.tag == "Enemy")
                TakeDamage(5, 0.2f);
        }
    }
    public void TakeDamage (int amount, float shakeFactor)
    {
        damage += amount;
        cameraScript.Shake(shakeFactor);
        if (damage >= 100 && dead == false)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            dead = true;
            Handheld.Vibrate();
            menuController.StartCoroutine("EndScreenDelay");
            GM.Save();
        }
        else
        {
            invincTime = Time.time;
            invinc = true;
        }
    }
}