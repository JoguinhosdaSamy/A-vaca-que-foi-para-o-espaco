// using UnityEngine;
//
// using UnityEditor;  
// using System.Collections.Generic;
//
// namespace Audio
// {
//     public class AudioManager : MonoBehaviour
//     {
//         public AudioSource soundEffectSource; // AudioSource para efeitos sonoros
//         public List<AudioClip> musicTracks; // Lista de faixas da trilha sonora
//         private float soundtrackVolume = 1.0f; // Volume da trilha sonora
//
//     private int currentTrackIndex = 0; // Índice da faixa atual
//
//     public static AudioManager instance; // Referência estática para acesso global
//
//     public float SoundtrackVolume
//     {
//         get { return soundtrackVolume; }
//         set
//         {
//             soundtrackVolume = value;
//             ApplyVolumeToMusicTracks();
//         }
//     }
//
//     private void Awake()
//     {
//         if (instance == null)
//         {
//             instance = this;
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }
//
//     private void Start()
//     {
//         // Inicia a reprodução da primeira faixa da trilha sonora
//         PlayMusicTrack(currentTrackIndex);
//     }
//
//     // Método para reproduzir um efeito sonoro
//     public void PlaySoundEffect(AudioClip clip)
//     {
//         soundEffectSource.clip = clip;
//         soundEffectSource.Play();
//     }
//
//     // Método para reproduzir uma faixa específica da trilha sonora
//     public void PlayMusicTrack(int trackIndex)
//     {
//         if (trackIndex < 0 || trackIndex >= musicTracks.Count)
//         {
//             Debug.LogError("Índice de faixa inválido: " + trackIndex);
//             return;
//         }
//
//         currentTrackIndex = trackIndex;
//         AudioClip track = musicTracks[currentTrackIndex];
//         soundEffectSource.clip = track;
//         ApplyVolumeToMusicTracks(); // Aplica o volume da trilha sonora
//         soundEffectSource.Play();
//     }
//
//     // Método para reproduzir a próxima faixa da trilha sonora
//     private void PlayNextTrack()
//     {
//         currentTrackIndex++;
//         if (currentTrackIndex >= musicTracks.Count)
//         {
//             currentTrackIndex = 0; // Volta para a primeira faixa quando chegar ao fim da lista
//         }
//
//         PlayMusicTrack(currentTrackIndex);
//     }
//
//     // Evento chamado quando uma faixa da trilha sonora terminar de tocar
//     private void OnTrackFinished()
//     {
//         PlayNextTrack();
//     }
//
//     // Método para parar a trilha sonora
//     public void StopMusic()
//     {
//         soundEffectSource.Stop();
//     }
//
//     // Aplica o volume da trilha sonora às faixas de música
//     private void ApplyVolumeToMusicTracks()
//     {
//         foreach (var track in musicTracks)
//         {
//             track.LoadAudioData();
//             float[] data = new float[track.samples];
//             track.GetData(data, 0);
//
//             for (int i = 0; i < data.Length; i++)
//             {
//                 data[i] *= soundtrackVolume;
//             }
//
//             track.SetData(data, 0);
//         }
//     }
// }
//
// #if UNITY_EDITOR
//
// [CustomEditor(typeof(AudioManager))]
// public class AudioManagerEditor : Editor
// {
//     private SerializedProperty soundEffectSourceProp;
//     private SerializedProperty musicTracksProp;
//     private SerializedProperty soundtrackVolumeProp;
//
//     private void OnEnable()
//     {
//         soundEffectSourceProp = serializedObject.FindProperty("soundEffectSource");
//         musicTracksProp = serializedObject.FindProperty("musicTracks");
//         soundtrackVolumeProp = serializedObject.FindProperty("SoundtrackVolume");
//     }
//
//     public override void OnInspectorGUI()
//     {
//         serializedObject.Update();
//
//         EditorGUILayout.PropertyField(soundEffectSourceProp);
//         EditorGUILayout.PropertyField(musicTracksProp, true);
//         EditorGUILayout.Slider(soundtrackVolumeProp, 0f, 1f, new GUIContent("Soundtrack Volume"));
//
//         if (GUILayout.Button("Add Music Track"))
//         {
//             AddMusicTrack();
//         }
//
//         serializedObject.ApplyModifiedProperties();
//     }
//
//     private void AddMusicTrack()
//     {
//         AudioManager audioManager = (AudioManager)target;
//         AudioClip newTrack = null;
//
//         newTrack = (AudioClip)EditorGUILayout.ObjectField("New Music Track", newTrack, typeof(AudioClip), false);
//
//         if (newTrack != null)
//         {
//             audioManager.musicTracks.Add(newTrack);
//         }
//     }
//     }
// #endif
// }
//
