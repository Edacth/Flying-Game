using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public GameObject target;
    public GameObject defaultTarget;
    public Vector3 start, mid, end;
    public float speed;
    public float startDelay;


    float timer = 0;
    float delayTimer = 0;
    [SerializeField]
    float elapsed = 0;
    public float lifetime;
    bool falling;
    bool enemiesExist = false;
    bool hit;

    void OnEnable()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0)
        {
            enemiesExist = true;
            target = closestEnemy(enemies);
        }

        falling = true;
        elapsed = 0;
        hit = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime && gameObject.activeSelf)
        {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.SetActive(false);
            timer = 0;
            delayTimer = 0;

        }
        if (delayTimer >= startDelay)
        {
            if (enemiesExist)
            {
                if (falling)
                {
                    falling = false;
                    calculatePoints();
                    elapsed = 0;
                }
                elapsed += Time.deltaTime;
                transform.position = quadBezier(start, mid, end, elapsed / Vector3.Distance(start, end) * speed);
                end = target.transform.position;

                transform.LookAt(quadBezier(start, mid, end, elapsed + Time.deltaTime / Vector3.Distance(start, end) * speed));

            }
            else
            {
                transform.Translate(transform.forward);
            }
        }
        else
        {
            delayTimer += Time.deltaTime;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            hit = true;
        }
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
        mid = start + (transform.forward * Vector3.Distance(start, end) / 2);
    }

    GameObject closestEnemy(GameObject[] arr)
    {
        GameObject closest = arr[0];

        for (int i = 1; i < arr.Length; ++i)
        {
            if (Vector3.Distance(transform.position, arr[i].transform.position) <
                Vector3.Distance(transform.position, closest.transform.position))
            {
                closest = arr[i];
            }
        }
        return closest;
    }
}
