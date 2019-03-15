using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillScript : MonoBehaviour {

    GameObject blades;


	void Start () {
        blades = gameObject;
	}
	

	void Update () {
        blades.transform.Rotate(0, 0, 1.5f);
	}
}
