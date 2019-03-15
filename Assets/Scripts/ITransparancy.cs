using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITransparancy : MonoBehaviour {

    public Color opaque;
    public Color transparent;
    public float fadeIncrement;

    Renderer myRenderer;
    float t;


    void Start () {
        myRenderer = gameObject.GetComponent<Renderer>();
        t = 1;

        //StartCoroutine(Fade(0.0f) );
    }
	
    public void Fade(float goal)
    {
        StartCoroutine(FadeCoroutine(goal));
    }

    IEnumerator FadeCoroutine(float goal)
    {
        while (goal >= t)
        {
            t += fadeIncrement;

            float interp = (0 * (1 - t) + 1 * t);
            myRenderer.material.color = Color.Lerp(opaque, transparent, interp);
            yield return null;
        }

        while (goal <= t)
        {
            t -= fadeIncrement;

            float interp = (0 * (1 - t) + 1 * t);
            //Debug.Log(interp.ToString());
            myRenderer.material.color = Color.Lerp(transparent, opaque, t);
            yield return null;
        }

        
    }
}
