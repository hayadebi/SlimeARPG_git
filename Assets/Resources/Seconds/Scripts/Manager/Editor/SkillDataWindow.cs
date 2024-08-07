using UnityEditor;
using UnityEngine;

public class SkillDataWindow : EditorWindow
{
    private DataManager dataManager;
    private SerializedObject serializedDataManager;
    private SerializedProperty skillList;
    private Vector2 scrollPos;
    private bool[] foldouts;
    private AudioSource audioSource;

    [MenuItem("Tools/スライムディストピア/スキル一覧")]
    public static void ShowWindow()
    {
        GetWindow<SkillDataWindow>("スキル一覧");
    }

    private void OnEnable()
    {
        dataManager = FindObjectOfType<DataManager>();
        if (dataManager != null)
        {
            serializedDataManager = new SerializedObject(dataManager);
            skillList = serializedDataManager.FindProperty("skillData");
            foldouts = new bool[skillList.arraySize];
        }

        if (audioSource == null)
        {
            GameObject audioSourceObject = new GameObject("AudioSourceObject");
            audioSource = audioSourceObject.AddComponent<AudioSource>();
        }
    }

    private void OnDisable()
    {
        if (audioSource != null)
        {
            DestroyImmediate(audioSource.gameObject);
        }
    }

    private void OnGUI()
    {
        if (dataManager == null)
        {
            EditorGUILayout.HelpBox("DataManager component not found in the scene.", MessageType.Warning);
            if (GUILayout.Button("Retry"))
            {
                OnEnable();
            }
            return;
        }

        serializedDataManager.Update();

        EditorGUILayout.LabelField("スキル設定一覧", EditorStyles.boldLabel);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < skillList.arraySize; i++)
        {
            SerializedProperty skill = skillList.GetArrayElementAtIndex(i);

            EditorGUILayout.BeginVertical(GUI.skin.box);

            foldouts[i] = EditorGUILayout.Foldout(foldouts[i], "スキル. " + i);
            if (foldouts[i])
            {
                EditorGUILayout.PropertyField(skill.FindPropertyRelative("skillname"), new GUIContent("スキル名"));
                EditorGUILayout.PropertyField(skill.FindPropertyRelative("skilldescription"), new GUIContent("スキル説明"));

                // Skill Icon Field with Preview
                SerializedProperty skillIcon = skill.FindPropertyRelative("skillicon");
                EditorGUILayout.PropertyField(skillIcon, new GUIContent("スキルアイコン"));
                if (skillIcon.objectReferenceValue != null)
                {
                    Texture2D texture = AssetPreview.GetAssetPreview(skillIcon.objectReferenceValue);
                    if (texture != null)
                    {
                        GUILayout.Label(texture, GUILayout.Width(100), GUILayout.Height(100));
                    }
                }

                //EditorGUILayout.PropertyField(skill.FindPropertyRelative("skillobject"), new GUIContent("スキルオブジェクト"));
                EditorGUILayout.PropertyField(skill.FindPropertyRelative("skillEvent"), new GUIContent("スキルイベントタイプ"));
                //EditorGUILayout.PropertyField(skill.FindPropertyRelative("skillSound"), new GUIContent("スキルサウンド"));

                if (GUILayout.Button("Remove Skill"))
                {
                    skillList.DeleteArrayElementAtIndex(i);
                    ArrayUtility.RemoveAt(ref foldouts, i);
                }
            }
            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("Add New Skill"))
        {
            skillList.InsertArrayElementAtIndex(skillList.arraySize);
            ArrayUtility.Add(ref foldouts, true);
        }

        EditorGUILayout.EndScrollView();

        serializedDataManager.ApplyModifiedProperties();
    }
}
