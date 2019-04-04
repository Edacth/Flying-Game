using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITransparancy : MonoBehaviour {

    public Color opaque;
    public Color transparent;
    public float fadeIncrement;

    Renderer myRenderer;
    public Material opaqueMat;
    public Material fadeMat;
    float t;

    public float fadeInTimer;
    public float fadeInDuration;
    public bool fadingIn = false;


    void Start () {
        myRenderer = gameObject.GetComponent<Renderer>();
        t = 1;
        myRenderer.material = opaqueMat;
        //StartCoroutine(Fade(0.0f) );
    }
	
    public void Fade(float goal)
    {
        StartCoroutine(FadeCoroutine(goal));
        myRenderer.material = fadeMat;
    }

    void Update()
    {
        if (fadingIn)
        {
            fadeInTimer += Time.deltaTime; //Add to the timer
            float interp = fadeInTimer / fadeInDuration; //Calculate the opacity of the object
            myRenderer.material.color = Color.Lerp(transparent, opaque, interp); //Change the opacity of the material
            //Debug.Log(myRenderer.material.color);

            if (fadeInTimer >= fadeInDuration) //Check to see if fading is finished
                fadingIn = false; //Turn off timer and opacity changes
        }
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

            //float interp = (0 * (1 - t) + 1 * t);
            //Debug.Log(interp.ToString());
            myRenderer.material.color = Color.Lerp(transparent, opaque, t);
            yield return null;
        }
    }
    public void startFadingIn()
    {
        fadeInTimer = 0;
        //myRenderer.material.color = transparent;
        fadingIn = true;
        //Debug.Break();
    }
}
