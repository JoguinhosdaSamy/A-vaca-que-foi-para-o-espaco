using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public enum Movement {Moving, Alien, Vaca};
    [SerializeField] public Movement MovementStatus ;
    public Enemy Enemy;
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
#if UNITY_EDITOR
[CustomEditor(typeof(GameController), true)]
public class GameControllerEditor : Editor
{
    SerializedProperty powerSleep;
    SerializedProperty movementStatus;
    
    void OnEnable()
    {
        // Setup the SerializedProperties.
        powerSleep = serializedObject.FindProperty ("SleepPowerUp");
        movementStatus = serializedObject.FindProperty ("MovementStatus");
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
        
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.EnumPopup("Status do movimento", (GameController.Movement)movementStatus.intValue);
        EditorGUI.EndDisabledGroup();
        serializedObject.ApplyModifiedProperties();
    }
}
#endif