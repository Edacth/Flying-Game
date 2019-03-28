using System.Collections;
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

    [Header("Physics/Input")]
    public Rigidbody rBody;
    public MeshRenderer mr;
    public Vector2 axisInput;
    public Vector3 defaultPos;
    public float rotLerp;
    public float moveSpeed;
    public float xBoundary;
    public float yBoundary;
    public Vector3 origin;
    public GameManager GM;
    public MenuController menuController;
    private float scoreTimer;
    public float scoreInterval;

    

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
    }
    

    void FixedUpdate()
    {
        
        if (!dead)
        {
            //Physics/Movement

            //rBody.rotation = Quaternion.Lerp(rBody.rotation, Quaternion.Euler(Input.gyro.rotationRateUnbiased * 10), rotLerp);
            Vector3 rot = new Vector3(Input.gyro.gravity.y * 40, Input.gyro.gravity.x * 30, -Input.gyro.gravity.x * 45);
            transform.eulerAngles = rot;
            rBody.AddForce(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * moveSpeed);
            rBody.AddForce(transform.forward * moveSpeed);
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
            else if (other.gameObject.tag == "Enemy")
                TakeDamage(5);
        }
    }
    private void TakeDamage (int amount)
    {
        damage += amount;
        if (damage >= 100 && dead == false)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            dead = true;

            menuController.SetEndState(true);
        }
        else
        {
            invincTime = Time.time;
            invinc = true;
        }
    }
}