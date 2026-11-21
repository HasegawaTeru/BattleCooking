
using UnityEngine;
using UnityEngine.EventSystems;

public class IngredientDrag : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;

    private Transform originalParent;
    private Vector2 originalPosition;
    private int originalSiblingIndex;

    public RectTransform cookingArea;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;
        originalSiblingIndex = transform.GetSiblingIndex();

        // Canvasíºâ∫Ç…à⁄ìÆ
        transform.SetParent(canvas.transform, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        bool inside = RectTransformUtility.RectangleContainsScreenPoint(
            cookingArea,
            eventData.position,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null : canvas.worldCamera
        );

        if (inside)
        {
            // óøóùÉGÉäÉAÇ…écÇ∑
            transform.SetParent(cookingArea, true);
        }
        else
        {
            // å≥ÇÃèÍèäÇ…ñﬂÇ∑
            transform.SetParent(originalParent, false);
            rectTransform.anchoredPosition = originalPosition;
            transform.SetSiblingIndex(originalSiblingIndex);
        }
    }
}
