using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Elementario
{
    public class SoundManager
    {
        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;
        AudioCategory audioCategory;

        List<string> soundsPlayed;
        bool soundPlayed;
        public bool muted;
        int counter;
        public float volume = 1f;

        public SoundManager()
        {
            audioEngine = new AudioEngine(@"Content\Audio\GameAudio.xgs");
            waveBank = new WaveBank(audioEngine, @"Content\Audio\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, @"Content\Audio\Sound Bank.xsb");

            soundsPlayed = new List<string>();
        }

        public void Update()
        {
            audioEngine.Update();
            audioCategory = audioEngine.GetCategory("Default");
            audioCategory.SetVolume(volume);

            if(soundPlayed)
            --counter;
            if (counter == 0)
            {
                soundPlayed = false;
                soundsPlayed = new List<string>();
                counter = 5;
            }
        }

        public void PlaySound(string cueName)
        {
            if (soundPlayed && soundsPlayed.Any(x => x == cueName))     //earrape prevention
                return;
            Cue cue = soundBank.GetCue(cueName);
            cue.Play();
            soundPlayed = true;

            soundsPlayed.Add(cueName);
        }

    }
}
