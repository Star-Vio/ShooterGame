using UnityEngine;
using UnityEditor;
using System.IO;

public class FixMaterialsEditor : MonoBehaviour
{
    [MenuItem("Tools/Fix All Materials - Convert to Standard Shader")]
    static void FixAllMaterials()
    {
        // Try multiple possible shader names
        Shader standardShader = Shader.Find("Standard");
        Debug.Log("Trying Shader.Find(\"Standard\") = " + (standardShader != null ? standardShader.name : "NULL"));

        if (standardShader == null)
        {
            // Try alternative shader names for different pipelines
            standardShader = Shader.Find("Universal Render Pipeline/Lit");
            Debug.Log("Trying Shader.Find(\"Universal Render Pipeline/Lit\") = " + (standardShader != null ? standardShader.name : "NULL"));
        }
        if (standardShader == null)
        {
            standardShader = Shader.Find("HDRP/Lit");
            Debug.Log("Trying Shader.Find(\"HDRP/Lit\") = " + (standardShader != null ? standardShader.name : "NULL"));
        }
        if (standardShader == null)
        {
            // List all available shaders to help debug
            Debug.LogError("=== No PBR shader found! ===");
            EditorUtility.DisplayDialog("Fix Failed",
                "No Standard/URP/HDRP Lit shader found!\n\n" +
                "Check the Console window (Window > General > Console) for available shader names.\n\n" +
                "Possible fix: Install Built-in Render Pipeline or URP package.",
                "OK");
            return;
        }

        Debug.Log("Using shader: " + standardShader.name);

        string materialsPath = Application.dataPath + "/Materials";
        if (!Directory.Exists(materialsPath))
        {
            Debug.LogError("Materials folder not found: " + materialsPath);
            return;
        }

        string targetShaderName = standardShader.name;
        string[] matFiles = Directory.GetFiles(materialsPath, "*.mat", SearchOption.TopDirectoryOnly);
        int fixedCount = 0;
        int skippedCount = 0;

        foreach (string matFile in matFiles)
        {
            string relativePath = "Assets/Materials/" + Path.GetFileName(matFile);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(relativePath);

            if (mat == null)
            {
                Debug.LogWarning("Could not load: " + relativePath);
                continue;
            }

            string currentShaderName = mat.shader != null ? mat.shader.name : "NULL";
            bool needsFix = mat.shader == null ||
                            currentShaderName.Contains("Hidden") ||
                            currentShaderName.Contains("Error") ||
                            (currentShaderName != targetShaderName &&
                             !currentShaderName.Contains("Particle") &&
                             !currentShaderName.Contains("Skybox"));

            if (needsFix)
            {
                string oldName = currentShaderName;
                mat.shader = standardShader;
                EditorUtility.SetDirty(mat);
                Debug.Log("Fixed: " + mat.name + " | " + oldName + " -> " + targetShaderName);
                fixedCount++;
            }
            else
            {
                skippedCount++;
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("Done!",
            "Fixed: " + fixedCount + " materials\n" +
            "Skipped: " + skippedCount + " materials\n" +
            "Shader: " + targetShaderName + "\n\n" +
            "Check Console for details.",
            "OK");
    }
}
