using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public enum Movement {Moving, Alien, Vaca};
    [HideInInspector] public Movement MovementStatus ;
    [HideInInspector] public Enemy Enemy;
    [SerializeField] public string NextScene;
    [SerializeField] public int SleepPowerUp;

    void Start()
    {
        Enemy = GameObject.Find("Enemy").GetComponent<Enemy>();
        MovementStatus = Movement.Moving;
    }

    public void SetMovementStatus(Movement tipo)
    {
        if (tipo == Movement.Alien)
        {
            Enemy.FindShortestPath();
        }
        MovementStatus = tipo;
    }
    public void Victory()
    {
        SceneManager.LoadScene(NextScene);
    }
    
    public void GameOver()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
    
    public void PowerSleep()
    {
        Enemy.counter = SleepPowerUp;
    }
}
[CustomEditor(typeof(GameController), true)]
public class GameControllerEditor : Editor
{
    SerializedProperty powerSleep;
    
    void OnEnable()
    {
        // Setup the SerializedProperties.
        powerSleep = serializedObject.FindProperty ("SleepPowerUp");

    }
    public override void OnInspectorGUI()
    {
        var picker = target as GameController;
        var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(picker.NextScene);

        serializedObject.Update();

        EditorGUILayout.IntSlider (powerSleep, 0, 5, new GUIContent ("Counter do enemy(PowerUP)"));
        
        EditorGUI.BeginChangeCheck();
        var newScene = EditorGUILayout.ObjectField("Proxima Cena", oldScene, typeof(SceneAsset), false) as SceneAsset;

        if (EditorGUI.EndChangeCheck())
        {
            var newPath = AssetDatabase.GetAssetPath(newScene);
            var scenePathProperty = serializedObject.FindProperty("NextScene");
            scenePathProperty.stringValue = newPath;
        }
        serializedObject.ApplyModifiedProperties();
    }
}