using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameController : MonoBehaviour
{
    public enum Movement {Moving, Alien, Vaca};
    [SerializeField] public Movement movementStatus ;
    public Enemy enemy;
    [SerializeField] public string nextScene;
    [SerializeField] public int sleepPowerUp;

    void Start()
    {
        enemy = GameObject.Find("Enemy").GetComponent<Enemy>();
        movementStatus = Movement.Moving;
    }

    public void SetMovementStatus(Movement tipo)
    {
        if (tipo == Movement.Alien)
        {
            enemy.FindShortestPath();
        }
        movementStatus = tipo;
    }
    public void Victory()
    {
        SceneManager.LoadScene(nextScene);
    }
    
    public void GameOver()
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
    
    void OnEnable()
    {
        // Setup the SerializedProperties.
        _powerSleep = serializedObject.FindProperty ("sleepPowerUp");
        _movementStatus = serializedObject.FindProperty ("movementStatus");
    }
    public override void OnInspectorGUI()
    {
        var picker = target as GameController;
        var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(picker.nextScene);

        serializedObject.Update();

        EditorGUILayout.IntSlider (_powerSleep, 0, 5, new GUIContent ("Counter do enemy(PowerUP)"));
        
        EditorGUI.BeginChangeCheck();
        var newScene = EditorGUILayout.ObjectField("Proxima Cena", oldScene, typeof(SceneAsset), false) as SceneAsset;

        if (EditorGUI.EndChangeCheck())
        {
            var newPath = AssetDatabase.GetAssetPath(newScene);
            var scenePathProperty = serializedObject.FindProperty("nextScene");
            scenePathProperty.stringValue = newPath;
        }
        
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.EnumPopup("Status do movimento", (GameController.Movement)_movementStatus.intValue);
        EditorGUI.EndDisabledGroup();
        serializedObject.ApplyModifiedProperties();
    }
}
#endif