using UnityEditor;
using UnityEngine;

public class EnemyDataWindow : EditorWindow
{
    private DataManager dataManager;
    private SerializedObject serializedDataManager;
    private SerializedProperty enemyList;
    private Vector2 scrollPos;
    private bool[] foldouts;
    private AudioSource audioSource;

    [MenuItem("Tools/スライムディストピア/敵一覧")]
    public static void ShowWindow()
    {
        GetWindow<EnemyDataWindow>("敵一覧");
    }

    private void OnEnable()
    {
        dataManager = FindObjectOfType<DataManager>();
        if (dataManager != null)
        {
            serializedDataManager = new SerializedObject(dataManager);
            enemyList = serializedDataManager.FindProperty("enemynoteID");
            foldouts = new bool[enemyList.arraySize];
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

        EditorGUILayout.LabelField("敵設定一覧", EditorStyles.boldLabel);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < enemyList.arraySize; i++)
        {
            SerializedProperty enemy = enemyList.GetArrayElementAtIndex(i);

            EditorGUILayout.BeginVertical(GUI.skin.box);

            foldouts[i] = EditorGUILayout.Foldout(foldouts[i], "敵. " + i);
            if (foldouts[i])
            {
                EditorGUILayout.PropertyField(enemy.FindPropertyRelative("name"), new GUIContent("敵名"));
                EditorGUILayout.PropertyField(enemy.FindPropertyRelative("description"), new GUIContent("敵説明"));

                // Enemy Image Field with Preview
                SerializedProperty enemyImage = enemy.FindPropertyRelative("image");
                EditorGUILayout.PropertyField(enemyImage, new GUIContent("敵アイコン"));
                if (enemyImage.objectReferenceValue != null)
                {
                    Texture2D texture = AssetPreview.GetAssetPreview(enemyImage.objectReferenceValue);
                    if (texture != null)
                    {
                        GUILayout.Label(texture, GUILayout.Width(100), GUILayout.Height(100));
                    }
                }

                EditorGUILayout.PropertyField(enemy.FindPropertyRelative("maxHP"), new GUIContent("最大HP"));
                EditorGUILayout.PropertyField(enemy.FindPropertyRelative("maxMP"), new GUIContent("最大MP"));
                EditorGUILayout.PropertyField(enemy.FindPropertyRelative("speed"), new GUIContent("速度"));
                EditorGUILayout.PropertyField(enemy.FindPropertyRelative("defense"), new GUIContent("防御"));
                EditorGUILayout.PropertyField(enemy.FindPropertyRelative("attack"), new GUIContent("攻撃"));
                EditorGUILayout.PropertyField(enemy.FindPropertyRelative("getExp"), new GUIContent("次のレベルアップまでの必要経験値"));
                EditorGUILayout.PropertyField(enemy.FindPropertyRelative("inputskill"), new GUIContent("所持スキル"));
                EditorGUILayout.PropertyField(enemy.FindPropertyRelative("getAchievements"), new GUIContent("討伐時の解放実績"));
                EditorGUILayout.PropertyField(enemy.FindPropertyRelative("habitatDescription"), new GUIContent("生息場所"));
                // Enemy Magic Fields
                SerializedProperty getMagic = enemy.FindPropertyRelative("getMagic");
                for (int j = 0; j < getMagic.arraySize; j++)
                {
                    SerializedProperty magic = getMagic.GetArrayElementAtIndex(j);
                    EditorGUILayout.PropertyField(magic.FindPropertyRelative("magicid"), new GUIContent("獲得可能な魔法ID"));
                    EditorGUILayout.PropertyField(magic.FindPropertyRelative("inputlevel"), new GUIContent("取得レベル"));
                }

                EditorGUILayout.PropertyField(enemy.FindPropertyRelative("badtype"), new GUIContent("弱点タイプ1"));
                EditorGUILayout.PropertyField(enemy.FindPropertyRelative("badtype2"), new GUIContent("弱点タイプ2"));
                EditorGUILayout.PropertyField(enemy.FindPropertyRelative("attacktype"), new GUIContent("攻撃タイプ1"));
                EditorGUILayout.PropertyField(enemy.FindPropertyRelative("attacktype2"), new GUIContent("攻撃タイプ2"));


                EditorGUILayout.PropertyField(enemy.FindPropertyRelative("aiType"), new GUIContent("AIタイプ"));
                EditorGUILayout.PropertyField(enemy.FindPropertyRelative("escapeRate"), new GUIContent("逃走成功率"));
                EditorGUILayout.PropertyField(enemy.FindPropertyRelative("animC"), new GUIContent("敵キャラアニメーターコントローラー"));
                EditorGUILayout.PropertyField(enemy.FindPropertyRelative("isBoss"), new GUIContent("ボスかどうか"));
                EditorGUILayout.PropertyField(enemy.FindPropertyRelative("isTrg"), new GUIContent("討伐後のTrigger変更有無"));
                EditorGUILayout.PropertyField(enemy.FindPropertyRelative("isTargetEvent"), new GUIContent("討伐後の対象イベント進行度"));
                EditorGUILayout.PropertyField(enemy.FindPropertyRelative("isAddEvent"), new GUIContent("討伐後のイベント進行度加算"));
                EditorGUILayout.PropertyField(enemy.FindPropertyRelative("maxAnim"), new GUIContent("最大アニメ数"));

                EditorGUILayout.PropertyField(enemy.FindPropertyRelative("battleSize"), new GUIContent("バトル時の表示サイズ"));
                EditorGUILayout.PropertyField(enemy.FindPropertyRelative("yPosition"), new GUIContent("バトル時の表示位置"));

                // Drop Data Fields
                SerializedProperty dropData = enemy.FindPropertyRelative("dropData");
                for (int k = 0; k < dropData.arraySize; k++)
                {
                    SerializedProperty drop = dropData.GetArrayElementAtIndex(k);
                    EditorGUILayout.PropertyField(drop.FindPropertyRelative("dropItemId"), new GUIContent("ドロップするアイテムID"));
                    EditorGUILayout.PropertyField(drop.FindPropertyRelative("dropRate"), new GUIContent("ドロップ率"));
                }

                if (GUILayout.Button("Remove Enemy"))
                {
                    enemyList.DeleteArrayElementAtIndex(i);
                    ArrayUtility.RemoveAt(ref foldouts, i);
                }
            }
            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("Add New Enemy"))
        {
            enemyList.InsertArrayElementAtIndex(enemyList.arraySize);
            ArrayUtility.Add(ref foldouts, true);
        }

        EditorGUILayout.EndScrollView();

        serializedDataManager.ApplyModifiedProperties();
    }
}
