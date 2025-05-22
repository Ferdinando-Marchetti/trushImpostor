using UnityEngine;
using UnityEngine.Rendering.Universal;

#if UNITY_EDITOR
using UnityEditor;

public class URPMaterialConverter
{
    [MenuItem("Tools/Convert Materials to URP")]
    public static void ConvertMaterialsToURP()
    {
        string[] materialGuids = AssetDatabase.FindAssets("t:Material");

        int convertedCount = 0;

        foreach (string guid in materialGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (mat != null && mat.shader.name != "Universal Render Pipeline/Lit")
            {
                Shader urpShader = Shader.Find("Universal Render Pipeline/Lit");

                if (urpShader != null)
                {
                    // Preserve old main texture and color
                    Texture oldMainTex = mat.HasProperty("_MainTex") ? mat.mainTexture : null;
                    Color oldColor = mat.HasProperty("_Color") ? mat.color : Color.white;

                    mat.shader = urpShader;

                    // Reassign texture & color
                    if (mat.HasProperty("_BaseMap") && oldMainTex != null)
                        mat.SetTexture("_BaseMap", oldMainTex);

                    if (mat.HasProperty("_BaseColor"))
                        mat.SetColor("_BaseColor", oldColor);

                    EditorUtility.SetDirty(mat);
                    convertedCount++;
                }
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"✅ Materiali convertiti a URP: {convertedCount}");
    }
}
#endif
