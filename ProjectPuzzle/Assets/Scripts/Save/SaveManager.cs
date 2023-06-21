using System;
using System.Collections.Generic;
using System.IO;
using Audio;
using Newtonsoft.Json;
using UnityEngine;

namespace Save
{
    public class SaveManager : MonoBehaviour
    {
        private static SaveManager _instance;
        private AudioManager _audioManager;
        
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
            var info = ReadFaseData("00");
            if (info == null)
            {
                SaveFaseData("00", false, null);
            }
            
            _audioManager.SoundtrackVolume = LoadSoundSettings().soundTrackVolume;
            _audioManager.SoundEffectVolume = LoadSoundSettings().soundEffectsVolume;

        }

        public static void SaveFaseData(string numeroFase, bool concluida, int ?pontosUtilizados)
        {
            var filePath = Path.Combine(Application.persistentDataPath, "save.json");

            FaseData data;
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                data = JsonConvert.DeserializeObject<FaseData>(json);

                if (data.fases == null)
                {
                    data.fases = new Dictionary<string, FaseInfo>();
                }
            }
            else
            {
                data = new FaseData
                {
                    fases = new Dictionary<string, FaseInfo>()
                };
            }

            if (data.fases.ContainsKey(numeroFase))
            {
                var faseInfo = data.fases[numeroFase];
                faseInfo.concluida = concluida;
                faseInfo.pontosUtilizados = pontosUtilizados;
            }
            else
            {
                data.fases.Add(numeroFase, new FaseInfo(concluida, pontosUtilizados));
            }

            var updatedJson = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, updatedJson);
        }

        public static FaseInfo ReadFaseData(string numeroFase)
        {
            var filePath = Path.Combine(Application.persistentDataPath, "save.json");

            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var data = JsonConvert.DeserializeObject<FaseData>(json);

                if (data.fases != null && data.fases.ContainsKey(numeroFase))
                {
                    return data.fases[numeroFase];
                }
            }

            return null; 
        }
        public void SaveSoundSettings(float soundTrackVolume = 0, float soundEffectsVolume = 0)
        {
            var filePath = Path.Combine(Application.persistentDataPath, "save.json");

            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var data = JsonConvert.DeserializeObject<FaseData>(json);

                // Atualizar configurações de som
                if( soundEffectsVolume != 0){
                    data.soundEffectsVolume = soundEffectsVolume;
                }
                if( soundTrackVolume != 0){
                    data.soundTrackVolume = soundTrackVolume;
                }

                var updatedJson = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(filePath, updatedJson);
            }
        }

        public static (float soundTrackVolume, float soundEffectsVolume) LoadSoundSettings()
        {
            var filePath = Path.Combine(Application.persistentDataPath, "save.json");

            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var data = JsonConvert.DeserializeObject<FaseData>(json);

                // Carregar configurações de som
                return (data.soundTrackVolume, data.soundEffectsVolume);
            }
            else
            {
                // Valores padrão se o arquivo de salvamento não existir
                return (1f, 1f);
            }
        }
    }
}