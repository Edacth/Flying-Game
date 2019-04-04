using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public GameObject target;
    public GameObject defaultTarget;
    public MeshRenderer mesh;
    public ParticleSystem exhaust;
    public Vector3 start, mid, end;
    public float speed;
    public float startDelay;

    PlayerShoot player;
    [SerializeField]
    GameObject explosion;
    GameManager GM;
    GameObject[] enemies;
    ParticleSystem.EmissionModule em;
    float timer = 0;
    float delayTimer = 0;
    [SerializeField]
    float elapsed = 0;
    public int damage = 0;
    public float lifetime;
    bool falling;
    bool hit;
    bool cannotFindTarget = false;

    void Awake()
    {
        mesh = gameObject.GetComponentInChildren<MeshRenderer>();
        exhaust = GetComponentInChildren<ParticleSystem>();
        em = exhaust.emission;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShoot>();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
    void OnEnable()
    {
        target = null;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0)
        {
            target = closestEnemy(enemies);
        }

        falling = true;
        elapsed = 0;
        hit = false;

        mesh.enabled = true;
        em.enabled = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime || hit)
        {
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                mesh.enabled = false;
                em.enabled = false;
                delayTimer = 0;

            if (exhaust.particleCount <= 0 && exhaust.isPlaying)
            {
                if (GM.missileAmmo >= player.missileCost)
                {
                    gameObject.SetActive(false);
                    exhaust.Stop();
                    timer = 0;
                    GM.missileAmmo -= player.missileCost;
                }
            }
        }
        if (delayTimer >= startDelay)
        {
            if (cannotFindTarget)
            {
                transform.Translate(transform.forward);
            }
            else if (target != null && target.transform.position.z > 0)
            {
                if (falling)
                {
                    falling = false;
                    calculatePoints();
                }
                elapsed += Time.deltaTime;
                transform.position = quadBezier(start, mid, end, elapsed / Vector3.Distance(start, end) * speed);
                end = target.transform.position;

                transform.LookAt(quadBezier(start, mid, end, (elapsed + Time.deltaTime) / Vector3.Distance(start, end) * speed));

            }
            else
            {
                // find all enemies in the scene and find the closest one, then get new coordinates to calculate trajectory
                enemies = GameObject.FindGameObjectsWithTag("Enemy");

                if (enemies.Length == 0)
                {
                    cannotFindTarget = true;
                }
                else
                {
                target = closestEnemy(enemies);
                calculatePoints();
                }
            }

            exhaust.Play();
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
            other.GetComponent<Enemy>().takeDamage(damage);
            Instantiate(explosion, transform.position, transform.rotation);
        }
        if (other.tag == "Building")
        {
            hit = true;
            Instantiate(explosion, transform.position, transform.rotation);
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
        elapsed = 0;
        start = transform.position;
        end = target.transform.position;
        mid = start + (transform.forward * Vector3.Distance(start, end) * 0.5f);
    }

    GameObject closestEnemy(GameObject[] arr)
    {

        GameObject closest = arr[0];

        for (int i = 1; i < arr.Length; ++i)
        {
            if (arr[i].activeSelf)
            {
                if (closest.transform.position.z < 0)
                    closest = arr[i];
                else
                {
                    if (Vector3.Distance(transform.position, arr[i].transform.position) <
                    Vector3.Distance(transform.position, closest.transform.position))
                        closest = arr[i];
                }
            }
        }
        if (closest.transform.position.z < 0)
            closest = null;

        return closest;
    }
}
