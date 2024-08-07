using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(UniLang.OriginalDialogueSystem))]
public class SayEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        UniLang.OriginalDialogueSystem myScript = (UniLang.OriginalDialogueSystem)target;
        if (GUILayout.Button("Assign GameObject"))
        {
            myScript.DialogueUI=GameObject.Find("SayDialog").GetComponent<Animator>();
            myScript.CommentText=GameObject.Find("SayDialog_Doc").GetComponent<Text>(); // 会話内容を表示するUIテキスト
            myScript.CharaNameText = GameObject.Find("SayDialog_Name").GetComponent<Text>(); ; // キャラクター名を表示するUIテキスト
        }
    }
}