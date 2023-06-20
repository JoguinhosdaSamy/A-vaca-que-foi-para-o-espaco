using System;
using System.Collections;
using UnityEngine;

using UnityEditor;  
using System.Collections.Generic;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        public AudioSource soundEffectSource; // AudioSource para efeitos sonoros
        public List<AudioClip> musicTracks; // Lista de faixas da trilha sonora
        public float soundtrackVolume = 1.0f; // Volume da trilha sonora

    private int currentTrackIndex = 0; // Índice da faixa atual

    public static AudioManager instance; // Referência estática para acesso global

    public float SoundtrackVolume
    {
        get { return soundtrackVolume; }
        set
        {
            soundtrackVolume = value;
            ApplyVolumeToMusicTracks();
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Inicia a reprodução da primeira faixa da trilha sonora
        PlayMusicTrack(currentTrackIndex);
    }

    // Método para reproduzir um efeito sonoro
    public void PlaySoundEffect(AudioClip clip)
    {
        soundEffectSource.clip = clip;
        soundEffectSource.Play();
    }
    
    // Método para reproduzir a próxima faixa da trilha sonora
    private void PlayNextTrack()
    {
        currentTrackIndex++;
        if (currentTrackIndex >= musicTracks.Count)
        {
            currentTrackIndex = 0; // Volta para a primeira faixa quando chegar ao fim da lista
        }

        PlayMusicTrack(currentTrackIndex);
    }

// Método para reproduzir uma faixa específica da trilha sonora
    public void PlayMusicTrack(int trackIndex)
    {
        if (trackIndex < 0 || trackIndex >= musicTracks.Count)
        {
            Debug.LogError("Índice de faixa inválido: " + trackIndex);
            return;
        }

        currentTrackIndex = trackIndex;
        AudioClip track = musicTracks[currentTrackIndex];
        soundEffectSource.clip = track;
        ApplyVolumeToMusicTracks(); // Aplica o volume da trilha sonora
        soundEffectSource.Play();

        // Chama o método PlayNextTrack() quando a faixa atual terminar de tocar
        StartCoroutine(WaitForTrackToEnd(track.length));
    }

        // Aguarda o tempo especificado antes de chamar o método PlayNextTrack()
        private IEnumerator WaitForTrackToEnd(float trackLength)
    {
        yield return new WaitForSeconds(trackLength);
        PlayNextTrack();
    }



    // Método para parar a trilha sonora
    public void StopMusic()
    {
        soundEffectSource.Stop();
    }

    // Aplica o volume da trilha sonora às faixas de música
    private void ApplyVolumeToMusicTracks()
    {
        foreach (var track in musicTracks)
        {
            track.LoadAudioData();
            float[] data = new float[track.samples];
            track.GetData(data, 0);

            for (int i = 0; i < data.Length; i++)
            {
                data[i] *= soundtrackVolume;
            }

            track.SetData(data, 0);
        }
    }

}

#if UNITY_EDITOR

    [CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor
{
    SerializedProperty soundEffectSourceProp;
    SerializedProperty musicTracksProp;
    SerializedProperty soundtrackVolumeProp;
    bool musicTracksFoldout = true;

    private void OnEnable()
    {
        soundEffectSourceProp = serializedObject.FindProperty("soundEffectSource");
        musicTracksProp = serializedObject.FindProperty("musicTracks");
        soundtrackVolumeProp = serializedObject.FindProperty("soundtrackVolume");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(soundEffectSourceProp);

        GUILayout.Space(10);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Space(10);
        musicTracksFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(musicTracksFoldout, "Music Tracks");
        if (musicTracksFoldout)
        {
            DrawMusicTracksArray();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        GUILayout.Space(10);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Slider(soundtrackVolumeProp, 0f, 1f, new GUIContent("Soundtrack Volume"));

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawMusicTracksArray()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("", EditorStyles.boldLabel);
        EditorGUILayout.LabelField(GetTotalSoundtrackDuration(), GUILayout.Width(40));
        if (GUILayout.Button("+", GUILayout.Width(20)))
        {
            musicTracksProp.arraySize++;
        }
        EditorGUILayout.EndHorizontal();

        for (int i = 0; i < musicTracksProp.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();
            SerializedProperty track = musicTracksProp.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(track, GUIContent.none);

            if (GUILayout.Button("▲", GUILayout.Width(20)))
            {
                if (i > 0)
                {
                    musicTracksProp.MoveArrayElement(i, i - 1);
                }
            }
            if (GUILayout.Button("▼", GUILayout.Width(20)))
            {
                if (i < musicTracksProp.arraySize - 1)
                {
                    musicTracksProp.MoveArrayElement(i, i + 1);
                }
            }

            if (GUILayout.Button("-", GUILayout.Width(20)))
            {
                musicTracksProp.DeleteArrayElementAtIndex(i);
                break;
            }

            EditorGUILayout.EndHorizontal();
            
        }
    }
    
    private string FormatTime(float durationInSeconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(durationInSeconds);
        return string.Format("{0}:{1:D2}", (int)timeSpan.TotalMinutes, timeSpan.Seconds);
    }

    private string GetTotalSoundtrackDuration()
    {
        float totalDuration = 0f;
        for (int i = 0; i < musicTracksProp.arraySize; i++)
        {
            SerializedProperty track = musicTracksProp.GetArrayElementAtIndex(i);
            AudioClip clip = track.objectReferenceValue as AudioClip;
            if (clip != null)
            {
                totalDuration += clip.length;
            }
        }
        return FormatTime(totalDuration);
    }

    private string GetTrackDuration(int index)
    {
        SerializedProperty track = musicTracksProp.GetArrayElementAtIndex(index);
        AudioClip clip = track.objectReferenceValue as AudioClip;
        return (clip != null) ? FormatTime(clip.length) : "00:00";
    }
}
#endif
}

