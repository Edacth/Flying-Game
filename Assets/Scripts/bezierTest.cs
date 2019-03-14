using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bezierTest : MonoBehaviour
{

    public GameObject myDude;

    public Transform start, mid, end;

    public float timer;

    void Start()
    {

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            timer = 0;
        myDude.transform.position = quadBezier(start.position, mid.position, end.position, timer);
        timer += Time.deltaTime;
    }
    Vector3 quadBezier(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 x = Vector3.Lerp(a, b, t);
        Vector3 y = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(x, y, t);
    }
}
