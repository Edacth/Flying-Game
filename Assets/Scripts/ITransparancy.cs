using System.Collections;
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
        Component[] subMeshRenderers = gameObject.GetComponentsInChildren(typeof(Renderer));
        StartCoroutine(DetectSubMeshCoroutine(subMeshRenderers));
    }
    IEnumerator DetectSubMeshCoroutine(Component[] subMeshList)
    {
        for(int i = 0; i < subMeshList.Length; i++)
        {
            string materialName = ((Renderer)subMeshList[i]).material.name.Replace(" (Instance)", "");
            //Adds a new SubMesh to the list, with the renderer and 2 material types
            subMeshes.Add(new SubMesh(((Renderer)subMeshList[i]), materialName, Resources.Load<Material>(materialName + "Opaque"), Resources.Load<Material>(materialName + "Fade")));
            //Sets the mesh to opaque upon spawning.
            subMeshes[subMeshes.Count - 1].meshRenderer.material = subMeshes[subMeshes.Count - 1].opaqueMat;
            yield return null;
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
    void OnDestroy()
    {
        foreach(SubMesh mesh in subMeshes)
        {
            Destroy(mesh.opaqueMat);
            Destroy(mesh.fadeMat);
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

    //public Dictionary<string, Material> materialDct;

// IEnumerator DetectSubMeshCoroutine(Component[] subMeshList)
    // {
    //     for(int i = 0; i < subMeshList.Length; i++)
    //     {
    //         string materialName = ((Renderer)subMeshList[i]).material.name.Replace(" (Instance)", "");
    //         //Checks if the opaque/fade materials do not exist in the dictionary
    //         //If so, add material to the dictionary
    //         //Else, reuse the material.
    //         if(!materialDct.ContainsKey(materialName + "Opaque"))
    //         {
    //             materialDct.Add(materialName + "Opaque", Resources.Load<Material>(materialName + "Opaque"));
    //         }
    //         if(!materialDct.ContainsKey(materialName + "Fade"))
    //         {
    //             materialDct.Add(materialName + "Fade", Resources.Load<Material>(materialName + "Fade"));
    //         }
    //         //Adds a new SubMesh to the list, with the renderer and 2 material types
    //         subMeshes.Add(new SubMesh(((Renderer)subMeshList[i]), materialName, materialDct[materialName + "Opaque"], materialDct[materialName + "Fade"]));
    //         //Sets the mesh to opaque upon spawning.
    //         subMeshes[subMeshes.Count - 1].meshRenderer.material = subMeshes[subMeshes.Count - 1].opaqueMat;
    //         yield return null;
    //     }
    // }
    