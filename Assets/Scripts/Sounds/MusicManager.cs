using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource music;

    private bool musicFadeOut;

    public float musicFadeRate = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        musicFadeOut = false;
        music = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(musicFadeOut)
        {
            if(music.volume > 0)
            {
                music.volume -= musicFadeRate * Time.deltaTime;
            }
        }
    }

    public void FadeOut()
    {
        musicFadeOut = true;
    }
}
