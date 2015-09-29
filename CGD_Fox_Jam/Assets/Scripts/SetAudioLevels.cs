using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetAudioLevels : MonoBehaviour {

    public AudioMixer mainMixer;
    
    public void SetMusicLevel(float musicLevel)
    {
        mainMixer.SetFloat("musicVol", musicLevel);
    }

    public void SetSfxLevel(float sfxLevel)
    {
        mainMixer.SetFloat("sfxVol", sfxLevel);
    }
}
