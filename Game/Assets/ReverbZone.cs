using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverbZone : MonoBehaviour {

    private FMOD.Studio.Bus Reverb;
    public float Volume;
    bool inCave;

    private void Awake() {
        Reverb = FMODUnity.RuntimeManager.GetBus("bus:/Reverb");
    }

    private void Update()
    {

        Reverb.setVolume(Volume);
        
        if (inCave)
        {
            Volume = Mathf.Lerp(Volume, 1f, Time.deltaTime * 3f);
        }
        else
        {
            Volume = Mathf.Lerp(Volume, 0f, Time.deltaTime * 3f);
        }
    }

    private void Start()
    {
        Volume = 0f;
    }

    void OnTriggerEnter (Collider col)
    {
        inCave = true;
    }

    void OnTriggerExit (Collider col)
    {
        inCave = false;
    }
}