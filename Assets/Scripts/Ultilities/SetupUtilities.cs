using UnityEngine;

namespace VitsehLand.Scripts.Ultilities
{
    public static class SetupUtilities
    {
        public static void SetLayers(Transform obj, string layerName, string layerChildName, string ignoreTag)
        {
            if (ignoreTag == null) ignoreTag = "DevEnv";

            if (!obj.gameObject.CompareTag(ignoreTag)) obj.gameObject.layer = LayerMask.NameToLayer(layerName);

            if (layerChildName == null) return;

            foreach (Transform child in obj)
            {
                if (!child.gameObject.CompareTag(ignoreTag))
                {
                    SetLayers(child, layerChildName, layerChildName, ignoreTag);
                }
            }
        }
    }
}