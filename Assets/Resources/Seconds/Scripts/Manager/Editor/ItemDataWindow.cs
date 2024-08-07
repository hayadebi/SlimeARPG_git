using UnityEditor;
using UnityEngine;

public class ItemDataWindow : EditorWindow
{
    private DataManager dataManager;
    private SerializedObject serializedDataManager;
    private SerializedProperty itemList;
    private Vector2 scrollPos;
    private bool[] foldouts;
    private AudioSource audioSource;

    [MenuItem("Tools/スライムディストピア/アイテム一覧")]
    public static void ShowWindow()
    {
        GetWindow<ItemDataWindow>("アイテム一覧");
    }

    private void OnEnable()
    {
        dataManager = FindObjectOfType<DataManager>();
        if (dataManager != null)
        {
            serializedDataManager = new SerializedObject(dataManager);
            itemList = serializedDataManager.FindProperty("ItemID");
            foldouts = new bool[itemList.arraySize];
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

        EditorGUILayout.LabelField("アイテム設定一覧", EditorStyles.boldLabel);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < itemList.arraySize; i++)
        {
            SerializedProperty item = itemList.GetArrayElementAtIndex(i);

            EditorGUILayout.BeginVertical(GUI.skin.box);

            foldouts[i] = EditorGUILayout.Foldout(foldouts[i], "アイテム. " + i);
            if (foldouts[i])
            {
                EditorGUILayout.PropertyField(item.FindPropertyRelative("itemname"), new GUIContent("アイテム名"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("itemdescription"), new GUIContent("アイテム説明"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("dropMonster"), new GUIContent("ドロップするモンスター"));
                // Item Image Field with Preview
                SerializedProperty itemImage = item.FindPropertyRelative("itemimage");
                EditorGUILayout.PropertyField(itemImage, new GUIContent("アイテムアイコン"));
                if (itemImage.objectReferenceValue != null)
                {
                    Texture2D texture = AssetPreview.GetAssetPreview(itemImage.objectReferenceValue);
                    if (texture != null)
                    {
                        GUILayout.Label(texture, GUILayout.Width(100), GUILayout.Height(100));
                    }
                }

                EditorGUILayout.PropertyField(item.FindPropertyRelative("itemType"), new GUIContent("アイテムイベントタイプ"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("fluctuationeffect"), new GUIContent("付与値"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("itemprice"), new GUIContent("アイテムの販売価格"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("itemnumber"), new GUIContent("アイテムの所持数"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("rare"), new GUIContent("レア度"));

                // Item Sound with Play/Stop Buttons
                SerializedProperty itemSound = item.FindPropertyRelative("itemSound");
                EditorGUILayout.PropertyField(itemSound, new GUIContent("使用時の効果音"));
                if (itemSound.objectReferenceValue != null)
                {
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("▶", GUILayout.Width(50)))
                    {
                        PlaySound((AudioClip)itemSound.objectReferenceValue);
                    }
                    if (GUILayout.Button("II", GUILayout.Width(50)))
                    {
                        StopSound();
                    }
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.PropertyField(item.FindPropertyRelative("cooltime"), new GUIContent("行動終了後のクールタイム"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("starttime"), new GUIContent("行動開始前のクールタイム"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("targetType"), new GUIContent("対象タイプ"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("isDeadUsed"), new GUIContent("死んでる対象も選べるか"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("isMenuUse"), new GUIContent("メインメニューでの使用可否"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("isBattleUse"), new GUIContent("戦闘時使用可否"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("isFieldUse"), new GUIContent("クイックセレクトでの使用可否"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("isShake"), new GUIContent("シェイクするかどうか"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("effectId"), new GUIContent("使用エフェクトID"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("charaAnimId"), new GUIContent("キャラアニメーションID"));

                if (GUILayout.Button("Remove Item"))
                {
                    itemList.DeleteArrayElementAtIndex(i);
                    ArrayUtility.RemoveAt(ref foldouts, i);
                }
            }
            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("Add New Item"))
        {
            itemList.InsertArrayElementAtIndex(itemList.arraySize);
            ArrayUtility.Add(ref foldouts, true);
        }

        EditorGUILayout.EndScrollView();

        serializedDataManager.ApplyModifiedProperties();
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
