using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private List<AudioSource> _loopingSounds = null;

    private static SoundManager _instance;
    public static SoundManager Instance { get { return _instance; } }

    private void Awake()
    {
        _instance = this;
    }

    public AudioSource PlaySound(AudioClip sfx, Vector3 location, bool loop = false)
    {
        if (sfx == null)
            return null;
        GameObject temporaryAudioHost = new GameObject("TempAudio");
        temporaryAudioHost.transform.position = location;
        AudioSource audioSource = temporaryAudioHost.AddComponent<AudioSource>() as AudioSource;
        audioSource.clip = sfx;
        audioSource.volume = 1f;
        audioSource.loop = loop;
        audioSource.Play();

        if (!loop)
        {
            Destroy(temporaryAudioHost, sfx.length);
        }
        else
        {
            _loopingSounds.Add(audioSource);
        }

        return audioSource;
    }

    public virtual void StopLoopingSound(AudioSource source)
    {
        if (source != null)
        {
            _loopingSounds.Remove(source);
            Destroy(source.gameObject);
        }
    }
}
