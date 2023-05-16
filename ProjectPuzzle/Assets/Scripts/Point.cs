using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Point : MonoBehaviour
{
    public enum Tipo {Neutro, Alien, Vaca};
    public enum Prop {None, EndPoint, SleepPower};
    public Point[] points;
    public Tipo[] tipo;
    private List<GameObject> _lines = new List<GameObject>();
    public GameObject linhasPrefab;
    private readonly Color _redLine = Color.red;
    private readonly Color _yellowLine = Color.yellow;
    private GameController _controller;
    [FormerlySerializedAs("Property")] public Prop property;
    public Point prev; // novo atributo

    public Material materialAceso;
    public Material materialApagado;
    [HideInInspector]public MeshRenderer _meshRenderer;

    private void OnDrawGizmos()
    {
        for (var i = 0; i < points.Length; i++)
        {
            Gizmos.color = Color.white;
            switch (tipo[i])
            {
                case Tipo.Alien:
                    Gizmos.color = _redLine;
                    break;
                case Tipo.Vaca:
                    Gizmos.color = _yellowLine;
                    break;
                    
            }
            Gizmos.DrawLine(transform.position, points[i].transform.position);
        }
    }
    
    void Start()
    {
        _controller = GameObject.Find("GameController").GetComponent<GameController>();
        _meshRenderer = GetComponent<MeshRenderer>();
        
        for (var i = 0; i < points.Length; i++)
        {
            var transform1 = transform;
            var position = transform1.position;
            
            GameObject linha = Instantiate(linhasPrefab, position, Quaternion.identity, transform1);
            LineRenderer lineRenderer = linha.GetComponent<LineRenderer>();
            switch (tipo[i])
            {
                case Tipo.Alien:
                    lineRenderer.SetColors(_redLine, _redLine);
                    break;
                case Tipo.Vaca:
                    lineRenderer.SetColors(_yellowLine, _yellowLine);
                    break;
                    
            }

            _lines.Add(linha);
            lineRenderer.SetPosition(0, position);
            lineRenderer.SetPosition(1, points[i].transform.position);
        }
        
    }


    void OnMouseDown()
    {
        if(_controller.movementStatus == GameController.Movement.Vaca){
            Point[] listaPoints = Player.player.target.GetComponent<Point>().points;
            Tipo[] listaTarget = Player.player.target.GetComponent<Point>().tipo;
            for (var i = 0; i < listaPoints.Length; i++){
                if (listaPoints[i].transform == transform){
                    if (listaTarget[i] == Tipo.Alien)
                    {
                        return;
                    }

                    HideLights();
                    _controller.SetMovementStatus(GameController.Movement.Moving);
                    Player.player.target = this;
                    Player.player.istargetNull = false;
                }
            }
        }
    }
    
    public void UpdateConnectedPoints()
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].prev == this)
            {
                points[i].tipo[i] = tipo[i];
                points[i].UpdateConnectedPoints();
            }
        }
    }

    public void ShowLights()
    {
        for (var i = 0; i < points.Length; i++)
        {
            
            if (tipo[i] == Tipo.Alien)
            {
                return;
            }

            points[i]._meshRenderer.material = points[i].materialAceso;
        }
    }

    private void HideLights()
    {
        Point[] points = FindObjectsOfType<Point>();

        foreach (Point point in points)
        {
            point._meshRenderer.material = point.materialApagado;
        }
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(Point))]
public class PointEditor : Editor
{
    private SerializedProperty _points;
    private SerializedProperty _tipo;
    private SerializedProperty _materialAcesoProperty;
    private SerializedProperty _materialApagadoProperty;
    private SerializedProperty _linhasPrefab;
    private const float PreviewAspectRatio = 1f;

    private void OnEnable()
    {
        _points = serializedObject.FindProperty("points");
        _tipo = serializedObject.FindProperty("tipo");
        _materialAcesoProperty = serializedObject.FindProperty("materialAceso");
        _materialApagadoProperty = serializedObject.FindProperty("materialApagado");
        _linhasPrefab = serializedObject.FindProperty("linhasPrefab");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawPointsArray();

        GUILayout.Space(10);
        
        DrawMaterials();
        GUILayout.Space(10);
        
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.LabelField("Linhas", EditorStyles.boldLabel);
        
        EditorGUILayout.PropertyField(_linhasPrefab,GUIContent.none);
        
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawMaterials()
    {
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.LabelField("Materiais", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Apagado", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Aceso", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(_materialAcesoProperty, GUIContent.none);
        EditorGUILayout.PropertyField(_materialApagadoProperty, GUIContent.none);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        // Exibir o preview do materialAceso
        Material materialAceso = (Material)_materialAcesoProperty.objectReferenceValue;
        Rect previewRect = GUILayoutUtility.GetAspectRect(PreviewAspectRatio);
        EditorGUI.DrawPreviewTexture(previewRect, AssetPreview.GetAssetPreview(materialAceso));

        // Exibir o preview do materialApagado
        Material materialApagado = (Material)_materialApagadoProperty.objectReferenceValue;
        previewRect = GUILayoutUtility.GetAspectRect(PreviewAspectRatio);
        EditorGUI.DrawPreviewTexture(previewRect, AssetPreview.GetAssetPreview(materialApagado));
        EditorGUILayout.EndHorizontal();


    }
    
    private void DrawPointsArray()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("WayPoints", EditorStyles.boldLabel);
        if (GUILayout.Button("+"))
        {
            _points.InsertArrayElementAtIndex(_points.arraySize);
            _tipo.InsertArrayElementAtIndex(_tipo.arraySize);
        }
        EditorGUILayout.EndHorizontal();
        
        
        for (int i = 0; i < _points.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();
            SerializedProperty point = _points.GetArrayElementAtIndex(i);
            SerializedProperty type = _tipo.GetArrayElementAtIndex(i);

            EditorGUILayout.PropertyField(point, GUIContent.none);
            EditorGUILayout.PropertyField(type, GUIContent.none);

            if (GUILayout.Button("-", GUILayout.Width(20)))
            {
                _points.DeleteArrayElementAtIndex(i);
                _tipo.DeleteArrayElementAtIndex(i);
                break;
            }

            EditorGUILayout.EndHorizontal();

        }
    }
}
#endif