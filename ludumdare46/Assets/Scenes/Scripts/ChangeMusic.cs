using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusic : MonoBehaviour
{
    public AudioClip audio1;
    public AudioClip audio2;
    public AudioClip audio3;

    public Population_Counter counter;

    private AudioSource source;
    private int valtotte = 0;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.clip = audio1;
        source.loop = true;
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(valtotte == 0 && counter.dead * 9 >= counter.alive)
        {
            source.loop = false;
            if(!source.isPlaying)
            {
                source.clip = audio2;
                source.loop = true;
                source.Play();
                valtotte = 1;
            }
        }
        else if(valtotte == 1 && counter.dead / 9 >= counter.alive)
        {
            source.loop = false;
            if (!source.isPlaying)
            {
                source.clip = audio3;
                source.loop = true;
                source.Play();
                valtotte = 2;
            }
        }
    }
}
