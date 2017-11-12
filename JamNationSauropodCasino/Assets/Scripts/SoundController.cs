using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class SoundController : MonoBehaviour
    {
        public AudioSource fuse;
        public AudioSource thruster;
        public AudioSource detonation;
        public AudioSource postDetonation;

        public void Fuse(AudioClip clip)
        {
            fuse.clip = clip;
            fuse.Play();
        }
        public void Thruster(AudioClip clip)
        {
            thruster.clip = clip;
            thruster.Play();
        }
        public void Detonation(AudioClip clip)
        {
            detonation.clip = clip;
            detonation.Play();
        }
        public void PostDetonation(AudioClip clip)
        {
            postDetonation.clip = clip;
            postDetonation.Play();
        }
    }
}
