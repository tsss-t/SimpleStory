using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{

    public static AudioManager _instance;
    public AudioClip[] audioClipArray;
    public bool isQuiet = false;

    private Dictionary<string, AudioClip> audioDict = new Dictionary<string, AudioClip>();

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        foreach (AudioClip ac in audioClipArray)
        {
            audioDict.Add(ac.name, ac);
        }
    }




    public void Play(string audioName)
    {
        if (isQuiet) return;
        AudioClip ac;
        if (audioDict.TryGetValue(audioName, out ac))
        {

             AudioSource.PlayClipAtPoint(ac,PlayerState._instance.playerTransform.position,0.1f);
            //this.audioSource.PlayOneShot(ac);
        }
    }

    public void Play(string audioName, AudioSource audioSource)
    {
        if (isQuiet) return;
        AudioClip ac;
        if (audioDict.TryGetValue(audioName, out ac))
        {
            //AudioSource.PlayClipAtPoint(ac, Vector3.zero);
            audioSource.PlayOneShot(ac);
        }
    }

}
