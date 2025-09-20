using System.Linq;
using UnityEngine;
using UnityEditor;


// PURELY GPT-GENERATED CODE SO PLS BE CAREFUL WHEN USING THIS
public class MoveSelected : EditorWindow
{
    float xoffset = 1f;
    float yoffset = 0f;

    string baseName = "Object";
    int startIndex = 0;
    bool zeroPad = false;

    [MenuItem("Tools/Move & Rename Selected")]
    public static void ShowWindow()
    {
        GetWindow<MoveSelected>("Move & Rename");
    }

    void OnGUI()
    {
        GUILayout.Label("Move", EditorStyles.boldLabel);
        xoffset = EditorGUILayout.FloatField("Offset (X):", xoffset);
        yoffset = EditorGUILayout.FloatField("Offset (Y):", yoffset);

        if (GUILayout.Button("Move (+X, +Y)"))
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                Undo.RecordObject(obj.transform, "Move Selected");
                obj.transform.position += new Vector3(xoffset, yoffset, 0);
            }
        }

        GUILayout.Space(10);
        GUILayout.Label("Rename", EditorStyles.boldLabel);

        baseName = EditorGUILayout.TextField("Base Name:", baseName);
        startIndex = EditorGUILayout.IntField("Start Index:", startIndex);
        zeroPad = EditorGUILayout.Toggle("Zero Pad", zeroPad);

        if (GUILayout.Button("Rename Selected"))
        {
            RenameSelected();
        }
    }

    void RenameSelected()
    {
        var gos = Selection.gameObjects;

        // Hierarchy 순서대로 정렬
        var ordered = gos
            .OrderBy(g => GetHierarchyKey(g.transform))
            .ToArray();

        int digits = zeroPad ? Mathf.CeilToInt(Mathf.Log10(Mathf.Max(1, ordered.Length + startIndex))) : 0;

        Undo.RecordObjects(ordered, "Batch Rename");

        for (int i = 0; i < ordered.Length; i++)
        {
            int idx = startIndex + i;
            string idxStr = (digits > 0) ? idx.ToString(new string('0', digits)) : idx.ToString();
            ordered[i].name = $"{baseName}_{idxStr}";
            EditorUtility.SetDirty(ordered[i]);
        }
    }

    // 계층 경로 키 (정렬용)
    static string GetHierarchyKey(Transform t)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        while (t != null)
        {
            sb.Insert(0, t.GetSiblingIndex().ToString("D6"));
            t = t.parent;
        }
        return sb.ToString();
    }
}
