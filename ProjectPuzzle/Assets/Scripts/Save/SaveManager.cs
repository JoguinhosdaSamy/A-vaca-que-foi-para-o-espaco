using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Save
{
    public class SaveManager : MonoBehaviour
    {
        private static SaveManager _instance;

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
            var info = ReadFaseData("00");
            if (info == null)
            {
                SaveFaseData("00", false, null);
            }
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
    }
}