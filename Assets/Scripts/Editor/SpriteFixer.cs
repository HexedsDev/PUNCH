using UnityEngine;

/// <summary>
/// Editor-only utility: assigns the Unity built-in white square sprite to any
/// SpriteRenderer that has no sprite assigned, so greybox objects are visible.
/// Run via Tools > Fix Sprites menu.
/// </summary>
#if UNITY_EDITOR
using UnityEditor;

public class SpriteFixer : MonoBehaviour
{
    [MenuItem("Tools/Fix Missing Sprites")]
    static void FixSprites()
    {
        // Get Unity's built-in white square sprite
        Sprite whiteSquare = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        if (whiteSquare == null)
        {
            // Fallback: create a white texture sprite
            Texture2D tex = new Texture2D(64, 64);
            Color[] pixels = new Color[64 * 64];
            for (int i = 0; i < pixels.Length; i++) pixels[i] = Color.white;
            tex.SetPixels(pixels);
            tex.Apply();
            whiteSquare = Sprite.Create(tex, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f), 100f);
        }

        // Also create a circle sprite for projectiles / trees
        Sprite circleSprite = GetCircleSprite();

        // Apply to all SpriteRenderers in scene missing a sprite
        SpriteRenderer[] all = FindObjectsOfType<SpriteRenderer>(true);
        int count = 0;
        foreach (var sr in all)
        {
            if (sr.sprite == null)
            {
                // Use circle for objects named with circle-like hints
                string n = sr.gameObject.name.ToLower();
                if (n.Contains("tree") || n.Contains("projectile") || n.Contains("proj"))
                    sr.sprite = circleSprite ?? whiteSquare;
                else
                    sr.sprite = whiteSquare;

                EditorUtility.SetDirty(sr);
                count++;
            }
        }

        // Also fix prefabs
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs" });
        foreach (var guid in prefabGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;
            SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite == null)
            {
                string n = prefab.name.ToLower();
                if (n.Contains("projectile"))
                    sr.sprite = circleSprite ?? whiteSquare;
                else
                    sr.sprite = whiteSquare;
                EditorUtility.SetDirty(prefab);
                count++;
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"[SpriteFixer] Fixed {count} missing sprites.");
    }

    static Sprite GetCircleSprite()
    {
        // Try getting a built-in circle
        Sprite s = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
        return s;
    }
}
#else
public class SpriteFixer : MonoBehaviour { }
#endif
