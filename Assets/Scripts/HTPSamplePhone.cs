﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTPSamplePhone : MonoBehaviour
{
    public float timer = 0;
    public bool axis;
    public float angle;

    void Update()
    {
        timer += Time.deltaTime * 3;
        if(axis)
            transform.eulerAngles = new Vector3(0, Mathf.Sin(timer) * angle, 0);
        else
            transform.eulerAngles = new Vector3(Mathf.Sin(timer) * angle, 0, 0);
        if (timer >= 2 * Mathf.PI)
        {
            timer = 0;
            axis = !axis;
        }
    }
}