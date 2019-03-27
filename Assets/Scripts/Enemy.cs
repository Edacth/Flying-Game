using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    GameObject Player;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (health <= 0)
        {
            gameObject.SetActive(false);
        }
	}

    public void takeDamage(int amount)
    {
        health -= amount;
    }
}
