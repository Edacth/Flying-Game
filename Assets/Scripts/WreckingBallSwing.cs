using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckingBallSwing : MonoBehaviour
{
    public float timer;
    public float angle;
    // Use this for initialization
    void Awake()
    {
        timer = Random.Range(0, 2 * Mathf.PI + 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 2 * Mathf.PI)
        {
            timer = 0;
        }
        transform.eulerAngles = new Vector3(0, 0, Mathf.Sin(timer) * angle);
    }
}