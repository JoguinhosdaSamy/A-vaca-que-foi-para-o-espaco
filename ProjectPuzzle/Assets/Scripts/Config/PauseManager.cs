using System.Collections;
using System.Collections.Generic;
using Audio;
using Save;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Config
{
    public class PauseManager : MonoBehaviour
    {
        public AudioManager audioManager; 
        public GameObject pauseConfig;
        public GameObject pauseMain;
        public Slider soundtrackSlider;
        public Slider soundEffectSlider;
        public SaveManager saveManager;

        public static PauseManager Instance;
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
            saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
            pauseMain.SetActive(false);
            pauseConfig.SetActive(false);
        }
        
        public void Pause()
        {
            Time.timeScale = 0;
            pauseConfig.SetActive(false);
            pauseMain.SetActive(true);
        }
        
        public void Resume()
        {
            Time.timeScale = 1;
            pauseConfig.SetActive(false);
            pauseMain.SetActive(false);
        }
        
        public void Quit()
        {
            Time.timeScale = 1;
            pauseConfig.SetActive(false);
            pauseMain.SetActive(false);
            audioManager.PlayMusicTrack(0);
            SceneManager.LoadScene("Menu");
        }
        
        public void Config()
        {
            pauseConfig.SetActive(true);
            pauseMain.SetActive(false);
        }
        
        public void BackConfig()
        {
            pauseConfig.SetActive(false);
            pauseMain.SetActive(true);
        }
        
        public void SoundtrackVolume(float volume)
        {
            audioManager.SoundtrackVolume = volume;
            saveManager.SaveSoundSettings(volume);
        }
        
        public void SoundEffectsVolume(float volume)
        {
            audioManager.SoundEffectVolume = volume;
            saveManager.SaveSoundSettings(0,volume);
        }
    }
}