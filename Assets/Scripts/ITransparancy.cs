using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ITransparancy : MonoBehaviour {

    public Color opaque;
    public Color transparent;

    public Renderer myRenderer;
    public Material opaqueMat;
    public Material fadeMat;
    public GameManager GM;
    float t;

    public float fadeIncrement;
    public float fadeInTimer;
    public float fadeInDuration;
    public bool fadingIn = false;
    public string materialName;

    void Start () {
        myRenderer = gameObject.GetComponent<Renderer>();
        materialName = myRenderer.material.name;
        materialName = materialName.Replace(" (Instance)", "");
        opaqueMat = Resources.Load<Material>(materialName + "Opaque");
        fadeMat = Resources.Load<Material>(materialName + "Fade");
        fadeIncrement = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().fadeIncrement;
        myRenderer.material = opaqueMat;
        //myRenderer.material = opaqueMat;
        //StartCoroutine(Fade(0.0f) );
        t = 1;
    }

    public void Fade(float goal)
    {
        if (fadeMat != null && myRenderer != null)
        {
            myRenderer.material = fadeMat;
        }
        StartCoroutine(FadeCoroutine(goal));
    }

    void Update()
    {
        //if (fadingIn)
        //{
        //    fadeInTimer += Time.deltaTime; //Add to the timer
        //    float interp = fadeInTimer / fadeInDuration; //Calculate the opacity of the object
        //    myRenderer.material.color = Color.Lerp(transparent, opaque, interp); //Change the opacity of the material
        //    //Debug.Log(myRenderer.material.color);

        //    if (fadeInTimer >= fadeInDuration) //Check to see if fading is finished
        //    {
        //        fadingIn = false; //Turn off timer and opacity changes
        //        myRenderer.material = opaqueMat;
        //    }
        //}
    }

    IEnumerator FadeCoroutine(float goal)
    {
        while (goal >= t)
        {
            t += fadeIncrement;

            float interp = (0 * (1 - t) + 1 * t);
            Color newColor = Color.Lerp(opaque, transparent, interp);
            if (myRenderer != null)
            {
                myRenderer.material.color = newColor;
            }
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
        myRenderer.material = fadeMat;
        //Debug.Break();
    }
}
