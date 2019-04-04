using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    float time;

    void Awake()
    {
        Destroy(gameObject, time);
    }
}
