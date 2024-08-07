using UnityEditor;
using UnityEngine;

public class MagicDataWindow : EditorWindow
{
    private DataManager magicData;
    private SerializedObject serializedMagicData;
    private SerializedProperty magicList;
    private Vector2 scrollPos;
    private bool[] foldouts;
    private AudioSource audioSource;

    [MenuItem("Tools/スライムディストピア/魔法一覧")]
    public static void ShowWindow()
    {
        GetWindow<MagicDataWindow>("魔法一覧");
    }

    private void OnEnable()
    {
        magicData = FindObjectOfType<DataManager>();
        if (magicData != null)
        {
            serializedMagicData = new SerializedObject(magicData);
            magicList = serializedMagicData.FindProperty("MagicID");
            foldouts = new bool[magicList.arraySize];
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
        if (magicData == null)
        {
            EditorGUILayout.HelpBox("MagicData component not found in the scene.", MessageType.Warning);
            if (GUILayout.Button("Retry"))
            {
                OnEnable();
            }
            return;
        }

        serializedMagicData.Update();

        EditorGUILayout.LabelField("魔法設定一覧", EditorStyles.boldLabel);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < magicList.arraySize; i++)
        {
            SerializedProperty magic = magicList.GetArrayElementAtIndex(i);

            EditorGUILayout.BeginVertical(GUI.skin.box);

            foldouts[i] = EditorGUILayout.Foldout(foldouts[i], "魔法. " + i);
            if (foldouts[i])
            {
                EditorGUILayout.PropertyField(magic.FindPropertyRelative("magicname"), new GUIContent("魔法名"));
                EditorGUILayout.PropertyField(magic.FindPropertyRelative("magicdescription"), new GUIContent("魔法説明"));

                // Magic Image Field with Preview
                SerializedProperty magicImage = magic.FindPropertyRelative("magicimage");
                EditorGUILayout.PropertyField(magicImage, new GUIContent("魔法アイコン"));
                if (magicImage.objectReferenceValue != null)
                {
                    Texture2D texture = AssetPreview.GetAssetPreview(magicImage.objectReferenceValue);
                    if (texture != null)
                    {
                        GUILayout.Label(texture, GUILayout.Width(100), GUILayout.Height(100));
                    }
                }

                EditorGUILayout.PropertyField(magic.FindPropertyRelative("magicprice"), new GUIContent("魔法の販売価格"));
                EditorGUILayout.PropertyField(magic.FindPropertyRelative("magicType"), new GUIContent("魔法の攻撃属性"));
                EditorGUILayout.PropertyField(magic.FindPropertyRelative("magicEvent"), new GUIContent("魔法イベント(効果)"));
                EditorGUILayout.PropertyField(magic.FindPropertyRelative("addAnomaly"), new GUIContent("付与する状態"));
                // Magic Sound with Play/Stop Buttons
                SerializedProperty magicSound = magic.FindPropertyRelative("magicSound");
                EditorGUILayout.PropertyField(magicSound, new GUIContent("発動時の効果音"));
                if (magicSound.objectReferenceValue != null)
                {
                    if (GUILayout.Button("▶(再生)", GUILayout.Width(80)))
                    {
                        PlaySound((AudioClip)magicSound.objectReferenceValue);
                    }
                    if (GUILayout.Button("II(停止)", GUILayout.Width(80)))
                    {
                        StopSound();
                    }
                }

                EditorGUILayout.PropertyField(magic.FindPropertyRelative("Damage"), new GUIContent("与える値(-はダメージ、+は回復)"));
                EditorGUILayout.PropertyField(magic.FindPropertyRelative("removeMp"), new GUIContent("消費MP"));
                EditorGUILayout.PropertyField(magic.FindPropertyRelative("cooltime"), new GUIContent("行動終了後クールタイム"));
                EditorGUILayout.PropertyField(magic.FindPropertyRelative("starttime"), new GUIContent("行動開始前クールタイム"));
                EditorGUILayout.PropertyField(magic.FindPropertyRelative("VitalpointAttack"), new GUIContent("急所率('急所率'分の一)"));
                EditorGUILayout.PropertyField(magic.FindPropertyRelative("targetType"), new GUIContent("使用対象"));
                EditorGUILayout.PropertyField(magic.FindPropertyRelative("isShake"), new GUIContent("シェイクするかどうか"));
                EditorGUILayout.PropertyField(magic.FindPropertyRelative("useWord"), new GUIContent("技ワード(○○は'技ワード'しようとしている)"));
                EditorGUILayout.PropertyField(magic.FindPropertyRelative("damageWord"), new GUIContent("ダメージワード(○○は1'ダメージワード'を受けた！)"));
                EditorGUILayout.PropertyField(magic.FindPropertyRelative("effectId"), new GUIContent("使用時のエフェクトID"));
                EditorGUILayout.PropertyField(magic.FindPropertyRelative("charaAnimId"), new GUIContent("使用する際のキャラアニメーションtrgを指定"));

                if (GUILayout.Button("Remove Magic"))
                {
                    magicList.DeleteArrayElementAtIndex(i);
                    ArrayUtility.RemoveAt(ref foldouts, i);
                }
            }
            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("Add New Magic"))
        {
            magicList.InsertArrayElementAtIndex(magicList.arraySize);
            ArrayUtility.Add(ref foldouts, true);
        }

        EditorGUILayout.EndScrollView();

        serializedMagicData.ApplyModifiedProperties();
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