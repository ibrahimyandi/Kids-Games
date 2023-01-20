using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static bool music = true;
    GameObject[] musicObj;
    public AudioClip[] clips;
    private AudioSource audioSource;

    private void Start() {
        audioSource = this.GetComponent<AudioSource>();
    }

    private AudioClip GetRandomClip(){
        return clips[Random.Range(0, clips.Length)];
    }

    private void Update() {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = GetRandomClip();
            audioSource.Play();
        }
        if (music == true)
        {
            audioSource.mute = false;
        }
        else
        {
            audioSource.mute = true;           
        }
    }
    private void Awake() {
        GameObject[] musicObj = GameObject.FindGameObjectsWithTag("SoundController");
        if (musicObj.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
