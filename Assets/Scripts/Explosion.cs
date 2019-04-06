using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    float time;
    private AudioSource audioSource;
    public AudioClip[] sounds;

    void Awake()
    {
        //sounds = Resources.LoadAll<AudioClip>("Sounds");
        audioSource = gameObject.GetComponent<AudioSource>();
        if (sounds.Length > 0)
        {
            audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
        }
        
        Destroy(gameObject, time);
    }
}
