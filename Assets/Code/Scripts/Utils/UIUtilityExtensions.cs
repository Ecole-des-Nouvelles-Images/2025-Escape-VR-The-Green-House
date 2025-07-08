using UnityEngine;

namespace Code.Scripts.Utils
{
    public static class UIUtilityExtensions
    {
        /// <summary>
        /// Helper function to find and convert a target anchored position to a position delta on the X axis.<br/>
        /// Useful for DOTWeen DOAnchorPos method family.
        /// </summary>
        /// <param name="uiElement">The RectTransform of the element to move.</param>
        /// <param name="normalizedTarget">The displacement factor on the x-axis.<br/>A factor of 2 will be equivalent to X-anchors (1, 2).</param>
        /// <returns></returns>
        public static float TargetAnchoredPosX(this RectTransform uiElement, float normalizedTarget)
        {
            Rect parentRect = uiElement.parent.GetComponent<RectTransform>().rect;

            Vector2 minAnchor = uiElement.anchorMin;
            Vector2 maxAnchor = uiElement.anchorMax;

            Vector2 minPosition = new (minAnchor.x * parentRect.width, minAnchor.y * parentRect.height);
            Vector2 maxPosition = new (maxAnchor.x * parentRect.width, maxAnchor.y * parentRect.height);

            float targetAnchoredPos = Mathf.LerpUnclamped(minPosition.x, maxPosition.x, normalizedTarget);

            return targetAnchoredPos;
        }
    }
}
