using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TGA.Utilities;
using UnityEngine.UI;
using TGA.SceneManagement;
using UnityEngine.SceneManagement;

namespace TGA.GameData
{
    public class GameDataManager : MonoBehaviour
    {
        [SerializeField]
        private const string gameDataPath = "GameData/TGA-gameData";

        private bool isInitialize;
        public bool IsIniialize => isInitialize;

        #region General Data
        [GameData("skill_config")]
        private SkillInfo skillConfig;


        private SceneController sceneController;

        #endregion

        private void Awake()
        {
   
        }

        void Start()
        {
  
        }

        public void StartInitialize()
        {
            DontDestroyOnLoad(this);
            SharedContext.Instance.Add(this);
            LoadLocalGameConfig();
        }

        private void LoadLocalGameConfig()
        {
            var jsonFile = Resources.Load(gameDataPath) as TextAsset;

            ParseAndApplyGameDataFromJson(jsonFile.text);
        }

        private void ParseAndApplyGameDataFromJson(string jsonText)
        {
            var gameDataDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonText);

            var gameDataFields = typeof(GameDataManager).GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.GetCustomAttributes(typeof(GameDataAttribute), true).Any()).ToList();

            foreach (var item in gameDataFields)
            {
                try
                {
                    var dictName = item.GetCustomAttribute(typeof(GameDataAttribute)) as GameDataAttribute;
                    var fieldGameDataString = gameDataDictionary[dictName.JsonElement];
                    var fieldType = item.FieldType;
                    var stringValue = fieldGameDataString.ToString();
                    var itemValue = JsonConvert.DeserializeObject(stringValue, fieldType);
                    item.SetValue(this, itemValue);

                }
                catch (Exception ex)
                {
                    Debug.Log($"Error parsing data: {item.Name}");
                    Debug.LogException(ex);
                }
            }

            Debug.Log("================  Load GameData From Json Complete  =================");

            Debug.Log(skillConfig);
        }
    }
}

