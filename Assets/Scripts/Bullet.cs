using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    
    public int damage = 0;
    public float lifetime;
    public bool hasParent = true;

    [SerializeField]
    GameObject explosion;
    TrailRenderer trail;
    float timer = 0;
    bool hit;


    void OnEnable()
    {
        hit = false;
        // wait before turning on particle system
        StartCoroutine("activateTrail");
    }

    void OnDisable()
    {
        if (!hasParent)
        {
            Destroy(gameObject);
        }
    }

    void Awake ()
    {
        trail = gameObject.GetComponent<TrailRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
        // if bullet reaches its lifetime or hits something
        if (timer >= lifetime || hit)
        {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            trail.enabled = false;
            gameObject.SetActive(false);
            timer = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            hit = true;
            other.GetComponent<Enemy>().takeDamage(damage);
            Instantiate(explosion, transform.position, transform.rotation);
        }
        if (other.tag == "Player")
        {
            hit = true;
            other.GetComponent<PlayerController>().TakeDamage(damage, 0.2f);
            Instantiate(explosion, transform.position, transform.rotation);
        }
        if (other.tag == "Building")
        {
            hit = true;
            Instantiate(explosion, transform.position, transform.rotation);
        }
    }

    private IEnumerator activateTrail()
    {
        for (int i = 0; i < 1; ++i)
            yield return null;

        trail.enabled = true;
    }
}
