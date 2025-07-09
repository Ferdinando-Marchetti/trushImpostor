using UnityEditor;
using UnityEngine;
using TMPro;

public class TMPFontEditorReplacer : EditorWindow
{
    TMP_FontAsset newFont;

    [MenuItem("Tools/Replace TMP Fonts In Scene")]
    static void Init()
    {
        TMPFontEditorReplacer window = (TMPFontEditorReplacer)EditorWindow.GetWindow(typeof(TMPFontEditorReplacer));
        window.titleContent = new GUIContent("TMP Font Replacer");
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Sostituisci tutti i font TMP nella scena", EditorStyles.boldLabel);
        newFont = (TMP_FontAsset)EditorGUILayout.ObjectField("Nuovo Font", newFont, typeof(TMP_FontAsset), false);

        if (GUILayout.Button("Sostituisci nella scena"))
        {
            if (newFont == null)
            {
                Debug.LogError("Seleziona un font TMP prima.");
                return;
            }

            int count = 0;
            TextMeshProUGUI[] texts = FindObjectsOfType<TextMeshProUGUI>(true);

            foreach (var text in texts)
            {
                Undo.RecordObject(text, "Cambia Font TMP");
                text.font = newFont;
                EditorUtility.SetDirty(text);
                count++;
            }

            Debug.Log($"Font sostituito su {count} oggetti TextMeshProUGUI.");
        }
    }
}
