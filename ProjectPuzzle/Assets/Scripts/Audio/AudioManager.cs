using System;
using System.Collections;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Config;
using Save;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        public AudioMixer audioMixer;
        public AudioSource soundEffectSource; // AudioSource para efeitos sonoros
        public AudioSource trackSoundSource; // AudioSource para efeitos sonoros
        public List<AudioClip> musicTracks; // Lista de faixas da trilha sonora
        public float soundtrackVolume = 1.0f; // Volume da trilha sonora
        public float soundEffectVolume = 1.0f; // Volume da trilha sonora

        public PauseManager pauseManager;

        public int CurrentTrackIndex = 0; // Índice da faixa atual
        
        private SaveManager _saveManager;

        public static AudioManager Instance; // Referência estática para acesso global

        public float SoundtrackVolume
        {
            get { return soundtrackVolume; }
            set
            {
                soundtrackVolume = value;
                ApplySounds();
            }
        } 
        
        public float SoundEffectVolume
        {
            get { return soundEffectVolume; }
            set
            {
                soundEffectVolume = value;
                ApplySounds();
                
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            _saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();
            PlayMusicTrack(CurrentTrackIndex);
        }

        public void PlaySoundEffect(AudioClip clip)
        {
            soundEffectSource.clip = clip;
            soundEffectSource.Play();
        }

        private void PlayNextTrack()
        {
            CurrentTrackIndex++;
            if (CurrentTrackIndex >= musicTracks.Count)
            {
                CurrentTrackIndex = 0;
            }

            PlayMusicTrack(CurrentTrackIndex);
        }

        public void PlayMusicTrack(int trackIndex)
        {
            StopAllCoroutines();
            if (trackIndex < 0 || trackIndex >= musicTracks.Count)
            {
                Debug.LogError("Índice de faixa inválido: " + trackIndex);
                return;
            }

            CurrentTrackIndex = trackIndex;
            AudioClip track = musicTracks[CurrentTrackIndex];
            trackSoundSource.clip = track;
            ApplySounds(); // Aplica o volume da trilha sonora
            trackSoundSource.Play();

            // Chama o método PlayNextTrack() quando a faixa atual terminar de tocar
            Debug.Log(track.length);
            StartCoroutine(WaitForTrackToEnd(track.length));
        }

        private IEnumerator WaitForTrackToEnd(float trackLength)
        {
            yield return new WaitForSeconds(trackLength);
            PlayNextTrack();
        }


        // Método para parar a trilha sonora
        public void StopMusic()
        {
            trackSoundSource.Stop();
        }

        private void ApplySounds()
        {
            audioMixer.SetFloat("Music", soundtrackVolume);
            audioMixer.SetFloat("Effects", soundEffectVolume);
            pauseManager.soundtrackSlider.value = soundtrackVolume;
            pauseManager.soundEffectSlider.value = soundEffectVolume;
        }
        
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor
{
    SerializedProperty soundEffectSourceProp;
    SerializedProperty trackSourceProp;
    SerializedProperty musicTracksProp;
    SerializedProperty soundeffectVolumeProp;
    SerializedProperty soundtrackVolumeProp;
    SerializedProperty soundMixerProp;
    bool musicTracksFoldout = true;

    private void OnEnable()
    {
        soundEffectSourceProp = serializedObject.FindProperty("soundEffectSource");
        trackSourceProp = serializedObject.FindProperty("trackSoundSource");
        musicTracksProp = serializedObject.FindProperty("musicTracks");
        soundtrackVolumeProp = serializedObject.FindProperty("soundtrackVolume");
        soundeffectVolumeProp = serializedObject.FindProperty("soundEffectVolume");
        soundMixerProp = serializedObject.FindProperty("audioMixer");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        GUILayout.Space(10);
        EditorGUILayout.PropertyField(soundMixerProp, new GUIContent("Mixer"));
        GUILayout.Space(10);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Space(10);
        EditorGUILayout.PropertyField(trackSourceProp, new GUIContent("Source da Trilha Sonora"));
        EditorGUILayout.PropertyField(soundEffectSourceProp, new GUIContent("Source de Efeitos Sonoros"));

        GUILayout.Space(10);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Space(10);
        musicTracksFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(musicTracksFoldout, "Music Tracks");
        if (musicTracksFoldout)
        {
            DrawMusicTracksArray();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

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
