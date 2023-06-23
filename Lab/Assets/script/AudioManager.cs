using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sounds[] sounds;
    // Start is called before the first frame update
    void Awake()
    {
        foreach(Sounds s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.Clip;
            s.source.volume = s.Volume;
            s.source.pitch = s.Pitch;
            s.source.loop = s.Loop;
            s.source.spatialBlend = s.Dimension;

        }
        
    }
    private void Start()
    {
        Play("theme");

    }

    public void Play(string name)
    {
        Sounds s =Array.Find(sounds, sound => sound.Nom == name);
        if (s == null)
        {
            Debug.Log("le nom du son n'exist pas");
            return;
        }
       
        if (!s.source.isPlaying)
        {
            s.source.Play();
        }
        else
        {
          
        }

}
    // Update is called once per frame
  
}
