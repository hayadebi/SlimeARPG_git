using UnityEditor;
using UnityEngine;

public class StateanomalyDataWindow : EditorWindow
{
    private DataManager stateanomalyData;
    private SerializedObject serializedStateanomalyData;
    private SerializedProperty stateanomalyList;
    private Vector2 scrollPos;
    private bool[] foldouts;
    private AudioSource audioSource;

    [MenuItem("Tools/スライムディストピア/状態異常一覧")]
    public static void ShowWindow()
    {
        GetWindow<StateanomalyDataWindow>("状態異常一覧");
    }

    private void OnEnable()
    {
        stateanomalyData = FindObjectOfType<DataManager>();
        if (stateanomalyData != null)
        {
            serializedStateanomalyData = new SerializedObject(stateanomalyData);
            stateanomalyList = serializedStateanomalyData.FindProperty("anomalyID");
            foldouts = new bool[stateanomalyList.arraySize];
        }

    }

    private void OnDisable()
    {
        
    }

    private void OnGUI()
    {
        if (stateanomalyData == null)
        {
            EditorGUILayout.HelpBox("StateanomalyData component not found in the scene.", MessageType.Warning);
            if (GUILayout.Button("Retry"))
            {
                OnEnable();
            }
            return;
        }

        serializedStateanomalyData.Update();

        EditorGUILayout.LabelField("状態異常一覧", EditorStyles.boldLabel);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < stateanomalyList.arraySize; i++)
        {
            SerializedProperty magic = stateanomalyList.GetArrayElementAtIndex(i);

            EditorGUILayout.BeginVertical(GUI.skin.box);

            foldouts[i] = EditorGUILayout.Foldout(foldouts[i], "状態異常. " + i);
            if (foldouts[i])
            {
                EditorGUILayout.PropertyField(magic.FindPropertyRelative("anomalyName"), new GUIContent("状態異常名"));

                // Magic Image Field with Preview
                SerializedProperty magicImage = magic.FindPropertyRelative("icon");
                EditorGUILayout.PropertyField(magicImage, new GUIContent("状態異常アイコン"));
                if (magicImage.objectReferenceValue != null)
                {
                    Texture2D texture = AssetPreview.GetAssetPreview(magicImage.objectReferenceValue);
                    if (texture != null)
                    {
                        GUILayout.Label(texture, GUILayout.Width(100), GUILayout.Height(100));
                    }
                }

                EditorGUILayout.PropertyField(magic.FindPropertyRelative("stateAnomaly"), new GUIContent("該当状態異常タイプ"));
                EditorGUILayout.PropertyField(magic.FindPropertyRelative("se"), new GUIContent("発症時の効果音"));

                if (GUILayout.Button("Remove Stateanomaly"))
                {
                    stateanomalyList.DeleteArrayElementAtIndex(i);
                    ArrayUtility.RemoveAt(ref foldouts, i);
                }
            }
            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("Add New Stateanomaly"))
        {
            stateanomalyList.InsertArrayElementAtIndex(stateanomalyList.arraySize);
            ArrayUtility.Add(ref foldouts, true);
        }

        EditorGUILayout.EndScrollView();

        serializedStateanomalyData.ApplyModifiedProperties();
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void StopSound()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}