using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AchievementsDataWindow : EditorWindow
{
    private DataManager dataManager;
    private SerializedObject serializedDataManager;
    private SerializedProperty achievementsList;
    private Vector2 scrollPos;
    private bool[] foldouts;

    [MenuItem("Tools/スライムディストピア/実績一覧")]
    public static void ShowWindow()
    {
        GetWindow<AchievementsDataWindow>("実績一覧");
    }

    private void OnEnable()
    {
        dataManager = FindObjectOfType<DataManager>();
        if (dataManager != null)
        {
            serializedDataManager = new SerializedObject(dataManager);
            achievementsList = serializedDataManager.FindProperty("achievementsID");
            foldouts = new bool[achievementsList.arraySize];
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

        EditorGUILayout.LabelField("実績設定一覧", EditorStyles.boldLabel);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < achievementsList.arraySize; i++)
        {
            SerializedProperty achievement = achievementsList.GetArrayElementAtIndex(i);

            EditorGUILayout.BeginVertical(GUI.skin.box);

            foldouts[i] = EditorGUILayout.Foldout(foldouts[i], "実績. " + i);
            if (foldouts[i])
            {
                EditorGUILayout.PropertyField(achievement.FindPropertyRelative("name"), new GUIContent("実績名"));
                EditorGUILayout.PropertyField(achievement.FindPropertyRelative("description"), new GUIContent("実績説明"));
                EditorGUILayout.PropertyField(achievement.FindPropertyRelative("gettrg"), new GUIContent("取得トリガー"));

                // Achievement Image Field with Preview
                SerializedProperty achievementImage = achievement.FindPropertyRelative("image");
                EditorGUILayout.PropertyField(achievementImage, new GUIContent("実績アイコン"));
                if (achievementImage.objectReferenceValue != null)
                {
                    Texture2D texture = AssetPreview.GetAssetPreview(achievementImage.objectReferenceValue);
                    if (texture != null)
                    {
                        GUILayout.Label(texture, GUILayout.Width(100), GUILayout.Height(100));
                    }
                }

                if (GUILayout.Button("Remove Achievement"))
                {
                    achievementsList.DeleteArrayElementAtIndex(i);
                    ArrayUtility.RemoveAt(ref foldouts, i);
                }
            }
            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("Add New Achievement"))
        {
            achievementsList.InsertArrayElementAtIndex(achievementsList.arraySize);
            ArrayUtility.Add(ref foldouts, true);
        }

        EditorGUILayout.EndScrollView();

        serializedDataManager.ApplyModifiedProperties();
    }
}
