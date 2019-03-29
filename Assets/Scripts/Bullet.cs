using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    
    public int damage = 0;
    public float lifetime;

    TrailRenderer trail;
    float timer = 0;
    bool hit;

    void OnEnable()
    {
        hit = false;
        StartCoroutine("activateTrail");
    }

    void Awake ()
    {
        trail = gameObject.GetComponent<TrailRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
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
        }
        if (other.tag == "Player")
        {
            hit = true;
            other.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }

    private IEnumerator activateTrail()
    {
        for (int i = 0; i < 2; ++i)
            yield return null;

        trail.enabled = true;
    }
}
