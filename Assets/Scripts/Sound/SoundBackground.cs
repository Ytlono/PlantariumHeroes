using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameProject
{
    public class SoundBackground : MonoBehaviour
    {
        List<AudioSource> audioSources;
        void Start()
        {
            audioSources = new List<AudioSource>();
            AudioSource[] sources = GetComponentsInChildren<AudioSource>();
            audioSources.AddRange(sources);
        }

        public void ChangeVolume(float value)
        {
            foreach (AudioSource source in audioSources)
            {
                source.volume = value;
            }
        }
        public void OffAll()
        {
            foreach (AudioSource source in audioSources)
            {
                source.enabled = false;
            }
        }

        public void OnAll()
        {
            foreach (AudioSource source in audioSources)
            {
                source.enabled = true;
            }
        }
    }
}
