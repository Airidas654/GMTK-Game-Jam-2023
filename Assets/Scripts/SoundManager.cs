using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0, 1)]
    public float volume = 1f;

    [Range(-3, 3)]
    public float pitch = 1f;

    [Range(0, 0.5f)]
    public float pitchRandomness = 0f;


    public bool loop = false;

    [HideInInspector]
    public AudioSource audioSource;
}

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    List<Sound> sounds = new List<Sound>();

    Dictionary<string, int> hash;

    public static SoundManager Instance = null;
    // public GameManager gameManag;
    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;

        hash = new Dictionary<string, int>(sounds.Count);
        int index = 0;
        foreach (Sound i in sounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();

            i.audioSource = source;
            source.pitch = i.pitch;
            source.loop = i.loop;
            source.volume = i.volume;
            source.clip = i.clip;
            source.playOnAwake = false;

            hash.Add(i.name,index++);
        }
    }
    public static float RandomNum(float nuo, float iki)
    {
        return Random.Range(nuo, iki);
    }

    public int GetIndexWithName(string name)
    {
        int index;
        if (hash.TryGetValue(name, out index))
        {
            return index;
        }
        Debug.LogError("Couldn't find "+name+ " sound");
        return -1;
    }


    public void Play(int id)
    {
        if (id >= 0 && id < sounds.Count)
        {
            sounds[id].audioSource.pitch = sounds[id].pitch + RandomNum(-sounds[id].pitchRandomness, sounds[id].pitchRandomness);
            sounds[id].audioSource.Play();
            return;
        }
        Debug.LogError("Sound not found!");
    }
    public void Play(string name)
    {
        Play(GetIndexWithName(name));
    }


    public void PlayIfEnded(int id)
    {
        if (id >= 0 && id < sounds.Count)
        {
            if (!sounds[id].audioSource.isPlaying)
            {
                sounds[id].audioSource.pitch = sounds[id].pitch + RandomNum(-sounds[id].pitchRandomness, sounds[id].pitchRandomness);
                sounds[id].audioSource.Play();
            }
            return;
        }
        Debug.LogError("Sound not found!");
    }
    public void PlayIfEnded(string name)
    {
        PlayIfEnded(GetIndexWithName(name));
    }


    public void Stop(int id)
    {
        if (id >= 0 && id < sounds.Count)
        {
            sounds[id].audioSource.Stop();
            return;
        }
        Debug.LogError("Sound not found!");
    }
    public void Stop(string name)
    {
        Stop(GetIndexWithName(name));
    }


    public void PlayOneShot(int id, float pitch, float volume)
    {
        if (id >= 0 && id < sounds.Count)
        {
            //sounds[i].audioSource.mute = !GameManager.sound;
            sounds[id].audioSource.pitch = pitch + RandomNum(-sounds[id].pitchRandomness, sounds[id].pitchRandomness);
            sounds[id].audioSource.PlayOneShot(sounds[id].clip, volume);
            return;
        }
        Debug.LogError("Sound not found!");
    }
    public void PlayOneShot(int id, float pitch)
    {
        PlayOneShot(id, pitch, 1);
    }
    public void PlayOneShot(int id)
    {
        if (id >= 0 && id < sounds.Count)
        {
            PlayOneShot(id, sounds[id].pitch, 1);
            return;
        }
        Debug.LogError("Sound not found!");
    }
    public void PlayOneShot(string name, float pitch, float volume)
    {
        PlayOneShot(GetIndexWithName(name), pitch, volume);
    }
    public void PlayOneShot(string name, float pitch)
    {
        PlayOneShot(GetIndexWithName(name), pitch);
    }
    public void PlayOneShot(string name)
    {
        PlayOneShot(GetIndexWithName(name));
    }


    public void ChangePitch(int id, float pitch)
    {
        if (id >= 0 && id < sounds.Count)
        {
            sounds[id].audioSource.pitch = pitch;
            return;
        }
        Debug.LogError("Sound not found!");
    }
    public void ChangePitch(string name, float pitch)
    {
        ChangePitch(GetIndexWithName(name), pitch);
    }



    public void ChangeVolume(int id, float volume)
    {
        if (id >= 0 && id < sounds.Count)
        {
            sounds[id].audioSource.volume = volume;
            return;
        }
        Debug.LogError("Sound not found!");
    }
    public void ChangeVolume(string name, float volume)
    {
        ChangeVolume(GetIndexWithName(name), volume);
    }



    public void ChangeLooping(int id, bool loop)
    {
        if (id >= 0 && id < sounds.Count)
        {
            sounds[id].audioSource.loop = loop;
            return;
        }
        Debug.LogError("Sound not found!");
    }
    public void ChangeLooping(string name, bool loop)
    {
        ChangeLooping(GetIndexWithName(name), loop);
    }


    public float GetPitch(int id)
    {
        if (id >= 0 && id < sounds.Count)
        {
            return sounds[id].pitch;
        }
        Debug.LogError("Sound not found!");
        return 0;
    }
    public float GetPitch(string name)
    {
        return GetPitch(GetIndexWithName(name));
    }



    public float GetVolume(int id)
    {
        if (id >= 0 && id < sounds.Count)
        {
            return sounds[id].volume;
        }
        Debug.LogError("Sound not found!");
        return 0;
    }
    public float GetVolume(string name)
    {
        return GetVolume(GetIndexWithName(name));
    }

}
