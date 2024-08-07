using UnityEditor;
using UnityEngine;

public class SayEventWindow : EditorWindow
{
    private DataManager dataManager;
    private SerializedObject serializedDataManager;
    private SerializedProperty sayEventList;
    private Vector2 scrollPos;
    private bool[] foldouts;
    private int[] dialogueEntries;
    private DataManager.SayView[] sayViews;

    [MenuItem("Tools/スライムディストピア/会話イベント一覧")]
    public static void ShowWindow()
    {
        GetWindow<SayEventWindow>("会話イベント一覧");
    }

    private void OnEnable()
    {
        dataManager = FindObjectOfType<DataManager>();
        if (dataManager != null)
        {
            serializedDataManager = new SerializedObject(dataManager);
            sayEventList = serializedDataManager.FindProperty("sayEvent");
            foldouts = new bool[sayEventList.arraySize];
            InitializeArrays();
        }
    }

    private void InitializeArrays()
    {
        dialogueEntries = new int[sayEventList.arraySize];
        sayViews = new DataManager.SayView[sayEventList.arraySize];
        for (int i = 0; i < sayEventList.arraySize; i++)
        {
            sayViews[i] = new DataManager.SayView();
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

        EditorGUILayout.LabelField("会話イベント設定一覧", EditorStyles.boldLabel);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < sayEventList.arraySize; i++)
        {
            SerializedProperty sayEvent = sayEventList.GetArrayElementAtIndex(i);

            EditorGUILayout.BeginVertical(GUI.skin.box);

            foldouts[i] = EditorGUILayout.Foldout(foldouts[i], "イベント. " + i);
            if (foldouts[i])
            {
                EditorGUILayout.PropertyField(sayEvent.FindPropertyRelative("EventName"), new GUIContent("イベント識別名"));

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("会話ダイアログエントリー数", GUILayout.Width(120));
                dialogueEntries[i] = EditorGUILayout.IntField(dialogueEntries[i], GUILayout.Width(50));
                if (dialogueEntries[i] > 0)
                {
                    if (sayViews[i].characterNames == null || sayViews[i].characterNames.Length != dialogueEntries[i])
                    {
                        InitializeSayViewArrays(i);
                    }

                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.LabelField("イベント内全会話内容", EditorStyles.boldLabel);
                    for (int d = 0; d < dialogueEntries[i]; d++)
                    {
                        EditorGUILayout.BeginVertical(GUI.skin.box);
                        sayViews[i].characterNames[d] = EditorGUILayout.TextField("表示キャラ名", sayViews[i].characterNames[d]);
                        sayViews[i].characterIDs[d] = EditorGUILayout.IntField("キャラID", sayViews[i].characterIDs[d]);
                        if (sayViews[i].characterIDs[d] >= 0)
                        {
                            if (dataManager!= null && dataManager.Pstatus != null && dataManager.Pstatus.Count > sayViews[i].characterIDs[d])
                            {
                                var pstatus = dataManager.Pstatus[sayViews[i].characterIDs[d]];
                                if (pstatus.pimage != null && pstatus.pimage.texture != null)
                                {
                                    GUILayout.Label(pstatus.pimage.texture, GUILayout.Width(100), GUILayout.Height(100));
                                }
                            }
                        }
                        EditorGUILayout.LabelField("会話内容");
                        sayViews[i].dialogText[d]=EditorGUILayout.TextArea(sayViews[i].dialogText[d], GUILayout.Height(60));
                        sayViews[i].animationIDs[d] = EditorGUILayout.IntField("再生アニメーションID", sayViews[i].animationIDs[d]);
                        sayViews[i].eventIDs[d] = EditorGUILayout.IntField("イベントID", sayViews[i].eventIDs[d]);
                        sayViews[i].targetNames[d] = EditorGUILayout.TextField("イベント対象オブジェクト名", sayViews[i].targetNames[d]);
                        EditorGUILayout.EndVertical();
                    }
                }
                else
                {
                    EditorGUILayout.EndHorizontal();
                }

                SerializedProperty dialogue = sayEvent.FindPropertyRelative("Dialogue");
                EditorGUILayout.LabelField("ダイアログプレビュー");
                EditorGUILayout.TextArea(dialogue.stringValue, GUILayout.Height(60));

                if (GUILayout.Button("ダイアログ更新"))
                {
                    string dialogueText = "";

                    for (int d = 0; d < dialogueEntries[i]; d++)
                    {
                        dialogueText += sayViews[i].characterNames[d] + ":";
                        dialogueText += sayViews[i].characterIDs[d] + ":";
                        dialogueText += sayViews[i].animationIDs[d] + ":";
                        dialogueText += sayViews[i].eventIDs[d] + ":";
                        dialogueText += sayViews[i].targetNames[d] + "\n";
                        dialogueText += sayViews[i].dialogText[d] + "\n\n";
                    }

                    sayEvent.FindPropertyRelative("Dialogue").stringValue = dialogueText;
                }

                if (GUILayout.Button("Remove Event"))
                {
                    sayEventList.DeleteArrayElementAtIndex(i);
                    ArrayUtility.RemoveAt(ref foldouts, i);
                    InitializeArrays();
                }
            }
            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("Add New Event"))
        {
            sayEventList.InsertArrayElementAtIndex(sayEventList.arraySize);
            ArrayUtility.Add(ref foldouts, true);
            InitializeArrays();
        }

        EditorGUILayout.EndScrollView();

        serializedDataManager.ApplyModifiedProperties();
    }

    private void InitializeSayViewArrays(int index)
    {
        sayViews[index].characterNames = new string[dialogueEntries[index]];
        sayViews[index].characterIDs = new int[dialogueEntries[index]];
        sayViews[index].animationIDs = new int[dialogueEntries[index]];
        sayViews[index].eventIDs = new int[dialogueEntries[index]];
        sayViews[index].targetNames = new string[dialogueEntries[index]];
        sayViews[index].dialogText = new string[dialogueEntries[index]];
    }
}
