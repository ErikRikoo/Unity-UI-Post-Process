using UnityEngine;

namespace UI.PostProcess.Scripts.Utilities
{
    public static class UIAnchorSnap
    {
        // The code is based on the Asset Anchor Snap which has been modified
        public static void SnapAnchors(this RectTransform _instance)
        {
            if (_instance.parent == null)
            {
                return;
            }
            
            RectTransform parentTransform = _instance.parent as RectTransform;
            if (_instance.parent != null)
            {
                Vector2 offsetMin = _instance.offsetMin;
                Vector2 offsetMax = _instance.offsetMax;
                Vector2 anchorMin = _instance.anchorMin;
                Vector2 anchorMax = _instance.anchorMax;
                Vector2 parentDimension = new Vector2(parentTransform.rect.width, parentTransform.rect.height);
                _instance.anchorMin = new Vector2(anchorMin.x + (offsetMin.x / parentDimension.x), anchorMin.y + (offsetMin.y / parentDimension.y));
                _instance.anchorMax = new Vector2(anchorMax.x + (offsetMax.x / parentDimension.x), anchorMax.y + (offsetMax.y / parentDimension.y));
                _instance.offsetMin = Vector2.zero;
                _instance.offsetMax = Vector2.zero;
            }
        }

        public static void SnapHierarchyAnchors(this RectTransform _instance)
        {
            _instance.SnapAnchors();
            foreach (Transform child in _instance)
            {
                (child as RectTransform).SnapHierarchyAnchors();
            }
        }
    }
}