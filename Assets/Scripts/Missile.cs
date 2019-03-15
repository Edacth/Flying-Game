using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    Rigidbody rBody;

    public Transform target;
    public Vector3 start, mid, end;
    public float speed;
    public float startDelay;

    public Quaternion initialRot, rot;
    float timer = 0;
    float delayTimer = 0;
    [SerializeField]
    float elapsed = 0;
    bool falling;

    void OnEnable()
    {
        target = GameObject.FindGameObjectWithTag("Enemy").transform;
        falling = true;
        elapsed = 0;
        initialRot = transform.rotation;
    }

    // Use this for initialization
    void Start ()
    {
        rBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
        if (timer >= 3 && gameObject.activeSelf)
        {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.SetActive(false);
            timer = 0;
            delayTimer = 0;

        }
        if (delayTimer >= startDelay)
        {

            if (falling)
            {
                falling = false;
                calculatePoints();
                elapsed = 0;
            }
            elapsed += Time.deltaTime;
            rBody.position = quadBezier(start, mid, end, elapsed / Vector3.Distance(start, end) * speed);
            end = target.transform.position;
            rot = Quaternion.LookRotation(target.position - transform.position) * initialRot;
            transform.rotation = Quaternion.Slerp(initialRot, rot, elapsed / Vector3.Distance(start, end) * speed);

        } else
        {
            delayTimer += Time.deltaTime;
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    Vector3 quadBezier(Vector3 a, Vector3 b, Vector3 c,  float t)
    {
        Vector3 x = Vector3.Lerp(a, b, t);
        Vector3 y = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(x, y, t);
    }

    void calculatePoints()
    {
        start = transform.position;
        end = target.transform.position;
        mid = start + (transform.up * Vector3.Distance(start, end) / 2);
    }
}
