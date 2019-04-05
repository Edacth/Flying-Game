﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITransparancy : MonoBehaviour
{
    [System.Serializable] public class SubMesh
    {
        public Renderer meshRenderer;
        public string materialName;
        public Material opaqueMat;
        public Material fadeMat;

        public SubMesh(Renderer _meshRenderer, string _materialName, Material _opaqueMat, Material _fadeMat)
        {
            materialName = _materialName;
            meshRenderer = _meshRenderer;
            opaqueMat = _opaqueMat;
            fadeMat = _fadeMat;
        }
    }

    public Color opaque;
    public Color transparent;

    public List<SubMesh> subMeshes;
    public GameManager GM;

    public float fadeIncrement;

    void Start()
    {
        fadeIncrement = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().fadeIncrement;
        findSubMeshes();
    }

    public void Fade(float goal)
    {
        convertToFadeMat();
        StartCoroutine(FadeCoroutine(goal));
    }

    IEnumerator FadeCoroutine(float goal)
    {
        for (float i = 0; i < 1; i += fadeIncrement)
        {
            fadeMeshColors(i);
            yield return null;
        }
    }
    public void findSubMeshes()
    {
        //Returns all Renderer components under the current object.
        Component[] subMeshRenderers = gameObject.GetComponentsInChildren(typeof(Renderer));
        //Iterate through the child renderers.
        foreach(Renderer renderer in subMeshRenderers)
        {
            string materialName = renderer.material.name.Replace(" (Instance)", "");
            //Adds a new SubMesh to the list, with the renderer and 2 material types
            subMeshes.Add(new SubMesh(renderer, materialName, Resources.Load<Material>(materialName + "Opaque"), Resources.Load<Material>(materialName + "Fade")));
            //Sets the mesh to opaque upon spawning.
            subMeshes[subMeshes.Count - 1].meshRenderer.material = subMeshes[subMeshes.Count - 1].opaqueMat;
        }
    }
    public void convertToFadeMat()
    {
        foreach(SubMesh subMesh in subMeshes)
        {
            subMesh.meshRenderer.material = subMesh.fadeMat;
        }
    }
    public void fadeMeshColors(float t)
    {
        foreach (SubMesh subMesh in subMeshes)
        {
            subMesh.meshRenderer.material.color = Color.Lerp(opaque, transparent, t);
        }
    }
}

//Legacy code

/*
    while (goal >= t)
    {
        t += fadeIncrement;

        float interp = (0 * (1 - t) + 1 * t);
        fadeMeshColors(interp);
        //Color newColor = Color.Lerp(opaque, transparent, interp);
        //if (myRenderer != null)
        //{
        //    myRenderer.material.color = newColor;
        //}
        yield return null;
    }

    while (goal <= t)
    {
        t -= fadeIncrement;
        //float interp = (0 * (1 - t) + 1 * t);
        //Debug.Log(interp.ToString());
        //myRenderer.material.color = Color.Lerp(transparent, opaque, t);
        fadeMeshColors(t);
        yield return null;
    }
*/