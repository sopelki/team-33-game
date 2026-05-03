using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Misc
{
    public class ShadowCreator : MonoBehaviour
    {
        public Sprite shadowSprite;
        public Vector2 offset = new(0f, -0.05f);
        public Vector2 scale = new(1.2f, 0.4f);
        [Range(0,1)] public float alpha = 0.4f;

#if UNITY_EDITOR
        public void CreateShadow()
        {
            const string shadowName = "_Shadow";

            if (transform.Find(shadowName) != null)
            {
                Debug.Log("Shadow already exists.");
                return;
            }

            var parentRenderer = GetComponent<SpriteRenderer>();
            if (parentRenderer == null)
            {
                Debug.LogWarning("No SpriteRenderer found.");
                return;
            }

            GameObject shadow = new GameObject(shadowName);
            shadow.transform.SetParent(transform, false);

            shadow.transform.localPosition = offset;
            shadow.transform.localScale = new Vector3(scale.x, scale.y, 1f);

            var renderer = shadow.AddComponent<SpriteRenderer>();
            renderer.sprite = shadowSprite;
            renderer.color = new Color(0, 0, 0, alpha);

            renderer.sortingLayerID = parentRenderer.sortingLayerID;
            renderer.sortingOrder = parentRenderer.sortingOrder - 1;

            // ВАЖНО — сохраняем изменения в префаб
            PrefabUtility.RecordPrefabInstancePropertyModifications(this);
            EditorUtility.SetDirty(gameObject);
        }

        public void RemoveShadow()
        {
            var existing = transform.Find("_Shadow");
            if (existing != null)
            {
                DestroyImmediate(existing.gameObject);
            }

            EditorUtility.SetDirty(gameObject);
        }
#endif
    }
}