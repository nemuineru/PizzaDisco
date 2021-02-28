using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixerSoundset : MonoBehaviour
{
    public AudioMixer mix;
    // Start is called before the first frame update

    public void SESet(float values) {
        float vols = Mathf.Clamp(20f * Mathf.Log10(Mathf.Clamp(values, 0f, 1f)), -80f, 0f);
        mix.SetFloat("SEVol", vols);
    }

    public void MusSet(float values)
    {
        float vols = Mathf.Clamp(20f * Mathf.Log10(Mathf.Clamp(values, 0f, 1f)), -80f, 0f);
        mix.SetFloat("MusicVol", vols);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
