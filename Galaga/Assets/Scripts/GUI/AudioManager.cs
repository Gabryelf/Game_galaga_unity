using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; } // ��������

    [SerializeField] private int poolSize = 10;
    [SerializeField] private AudioClip[] soundClips;
    private List<AudioSource> audioSources = new List<AudioSource>();

    private void Awake()
    {
        // ���������� ���������
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ���������� ������ ��� �������� ����� �����
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // �������� ���� ����� ����������
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.playOnAwake = false;
            audioSources.Add(newSource);
        }
    }

    public void PlaySound(string clipName)
    {
        AudioClip clip = GetClipByName(clipName);
        if (clip != null)
        {
            AudioSource source = GetAvailableSource();
            if (source != null)
            {
                source.clip = clip;
                source.Play();
            }
        }
        else
        {
            Debug.LogWarning($"���� {clipName} �� ������ � AudioManager");
        }
    }

    private AudioSource GetAvailableSource()
    {
        foreach (var source in audioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        return audioSources[0];
    }

    private AudioClip GetClipByName(string name)
    {
        foreach (var clip in soundClips)
        {
            if (clip.name == name)
                return clip;
        }
        return null;
    }
}


