using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using Save;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


public class GameController : MonoBehaviour
{
    public enum Movement
    {
        Moving,
        Alien,
        Vaca
    };

    [SerializeField] public Movement movementStatus;
    public Enemy enemy;
    [SerializeField] public string nextScene;
    [SerializeField] public int sleepPowerUp;

    [CanBeNull] public GameObject TelaTutorial;
    public bool Tutorial = true;
    public static GameController controller;
    public GameObject abduzido;


    void Start()
    {
        controller = this;
        enemy = GameObject.Find("Enemy").GetComponent<Enemy>();
        movementStatus = Movement.Moving;

        FaseInfo faseInfo = SaveManager.ReadFaseData(SceneManager.GetActiveScene().name);
        if (Tutorial)
        {
            TelaTutorial.SetActive(true);
        }
        if (faseInfo == null)
        {
            SaveManager.SaveFaseData(SceneManager.GetActiveScene().name, false, null);
        }
    }

    public void SetMovementStatus(Movement tipo)
    {
        if (tipo == Movement.Alien)
        {
            enemy.FindShortestPath();
        }

        movementStatus = tipo;
    }

    public void Victory(int pulos)
    {
        SaveManager.SaveFaseData(SceneManager.GetActiveScene().name, true, pulos);
        SaveManager.SaveFaseData(Path.GetFileNameWithoutExtension(nextScene), false, null);
        SceneManager.LoadScene(nextScene);
    }

    public void GameOver()
    {
        enemy.gameObject.SetActive(false);
        Player.player.gameObject.SetActive(false);
        abduzido.transform.position = enemy.transform.position;
        abduzido.SetActive(true);
        Invoke("LoadGameOver", 2.0f);
    }

    void LoadGameOver()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);

    }


    public void PowerSleep()
    {
        enemy.counter = sleepPowerUp;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(GameController), true)]
public class GameControllerEditor : Editor
{
    SerializedProperty _powerSleep;
    SerializedProperty _movementStatus;
    SerializedProperty _tutorial;
    SerializedProperty _tutorialTela;

    void OnEnable()
    {
        // Setup the SerializedProperties.
        _powerSleep = serializedObject.FindProperty("sleepPowerUp");
        _movementStatus = serializedObject.FindProperty("movementStatus");
        _tutorial = serializedObject.FindProperty("Tutorial");
        _tutorialTela = serializedObject.FindProperty("TelaTutorial");
    }

    public override void OnInspectorGUI()
    {
        var picker = target as GameController;
        var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(picker.nextScene);

        serializedObject.Update();

        EditorGUILayout.IntSlider(_powerSleep, 0, 5, new GUIContent("Counter do enemy(PowerUP)"));

        EditorGUI.BeginChangeCheck();
        var newScene = EditorGUILayout.ObjectField("Proxima Cena", oldScene, typeof(SceneAsset), false) as SceneAsset;

        if (EditorGUI.EndChangeCheck())
        {
            var newPath = AssetDatabase.GetAssetPath(newScene);
            var scenePathProperty = serializedObject.FindProperty("nextScene");
            scenePathProperty.stringValue = newPath;
        }

        EditorGUILayout.PropertyField(_tutorial);

        if (_tutorial.boolValue)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_tutorialTela);
            EditorGUI.indentLevel--;
        }

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.EnumPopup("Status do movimento", (GameController.Movement)_movementStatus.intValue);
        EditorGUI.EndDisabledGroup();

        serializedObject.ApplyModifiedProperties();
    }
}
#endif