using UnityEditor;
using UnityEngine;

public class PlayerDataWindow : EditorWindow
{
    private DataManager dataManager;
    private SerializedObject serializedDataManager;
    private SerializedProperty playerList;
    private Vector2 scrollPos;
    private bool[] foldouts;
    private AudioSource audioSource;

    [MenuItem("Tools/スライムディストピア/プレイヤー一覧")]
    public static void ShowWindow()
    {
        GetWindow<PlayerDataWindow>("プレイヤー一覧");
    }

    private void OnEnable()
    {
        dataManager = FindObjectOfType<DataManager>();
        if (dataManager != null)
        {
            serializedDataManager = new SerializedObject(dataManager);
            playerList = serializedDataManager.FindProperty("Pstatus");
            foldouts = new bool[playerList.arraySize];
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

        EditorGUILayout.LabelField("プレイヤー設定一覧", EditorStyles.boldLabel);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < playerList.arraySize; i++)
        {
            SerializedProperty player = playerList.GetArrayElementAtIndex(i);

            EditorGUILayout.BeginVertical(GUI.skin.box);

            foldouts[i] = EditorGUILayout.Foldout(foldouts[i], "キャラ. " + i);
            if (foldouts[i])
            {
                EditorGUILayout.PropertyField(player.FindPropertyRelative("pname"), new GUIContent("キャラ名"));
                EditorGUILayout.PropertyField(player.FindPropertyRelative("pdescription"), new GUIContent("キャラ説明"));

                // Player Image Field with Preview
                SerializedProperty playerImage = player.FindPropertyRelative("pimage");
                EditorGUILayout.PropertyField(playerImage, new GUIContent("キャラアイコン"));
                if (playerImage.objectReferenceValue != null)
                {
                    Texture2D texture = AssetPreview.GetAssetPreview(playerImage.objectReferenceValue);
                    if (texture != null)
                    {
                        GUILayout.Label(texture, GUILayout.Width(100), GUILayout.Height(100));
                    }
                }

                EditorGUILayout.PropertyField(player.FindPropertyRelative("maxHP"), new GUIContent("最大HP"));
                EditorGUILayout.PropertyField(player.FindPropertyRelative("hp"), new GUIContent("現在のHP"));
                EditorGUILayout.PropertyField(player.FindPropertyRelative("maxMP"), new GUIContent("最大MP"));
                EditorGUILayout.PropertyField(player.FindPropertyRelative("mp"), new GUIContent("現在のMP"));
                EditorGUILayout.PropertyField(player.FindPropertyRelative("speed"), new GUIContent("速度"));
                EditorGUILayout.PropertyField(player.FindPropertyRelative("defense"), new GUIContent("防御"));
                EditorGUILayout.PropertyField(player.FindPropertyRelative("attack"), new GUIContent("攻撃"));
                EditorGUILayout.PropertyField(player.FindPropertyRelative("maxExp"), new GUIContent("次のレベルアップまでの必要経験値"));
                EditorGUILayout.PropertyField(player.FindPropertyRelative("inputskill"), new GUIContent("所持スキル"));

                // Player Magic Fields
                SerializedProperty getMagic = player.FindPropertyRelative("getMagic");
                for (int j = 0; j < getMagic.arraySize; j++)
                {
                    SerializedProperty magic = getMagic.GetArrayElementAtIndex(j);
                    EditorGUILayout.PropertyField(magic.FindPropertyRelative("magicid"), new GUIContent("獲得可能な魔法ID"));
                    EditorGUILayout.PropertyField(magic.FindPropertyRelative("inputlevel"), new GUIContent("取得レベル"));
                }

                EditorGUILayout.PropertyField(player.FindPropertyRelative("magicSet"), new GUIContent("セットされている魔法"));
                EditorGUILayout.PropertyField(player.FindPropertyRelative("badtype"), new GUIContent("弱点タイプ1"));
                EditorGUILayout.PropertyField(player.FindPropertyRelative("badtype2"), new GUIContent("弱点タイプ2"));
                EditorGUILayout.PropertyField(player.FindPropertyRelative("attacktype"), new GUIContent("攻撃タイプ1"));
                EditorGUILayout.PropertyField(player.FindPropertyRelative("attacktype2"), new GUIContent("攻撃タイプ2"));
                EditorGUILayout.PropertyField(player.FindPropertyRelative("getpl"), new GUIContent("キャラ取得トリガー"));
                EditorGUILayout.PropertyField(player.FindPropertyRelative("animC"), new GUIContent("アニメーターコントローラー"));
                EditorGUILayout.PropertyField(player.FindPropertyRelative("maxAnime"), new GUIContent("最大アニメ数"));

                if (GUILayout.Button("Remove Player"))
                {
                    playerList.DeleteArrayElementAtIndex(i);
                    ArrayUtility.RemoveAt(ref foldouts, i);
                }
            }
            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("Add New Player"))
        {
            playerList.InsertArrayElementAtIndex(playerList.arraySize);
            ArrayUtility.Add(ref foldouts, true);
        }

        EditorGUILayout.EndScrollView();

        serializedDataManager.ApplyModifiedProperties();
    }
}
