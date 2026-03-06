using UnityEngine;

[ExecuteAlways]
public class PropVisualSync : MonoBehaviour
{
    [Header("Single Source (공용 데이터)")]
    [SerializeField] private Sprite targetSprite;
    [SerializeField] private Color mainColor = Color.white;
    [Range(0f, 1f)][SerializeField] private float shadowAlpha = 0.5f;

    [Header("References")]
    [SerializeField] private SpriteRenderer mainRenderer;   // 기존 Quad 대신 SpriteRenderer 사용
    [SerializeField] private SpriteRenderer shadowRenderer;

#if UNITY_EDITOR
    private bool isValidateQueued;
#endif

    private void OnEnable()
    {
        EnsureReferences();
        SyncAllVisuals();
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.delayCall -= HandleDelayedValidate;
        isValidateQueued = false;
#endif
    }

    private void OnValidate()
    {
        shadowAlpha = Mathf.Clamp01(shadowAlpha);

#if UNITY_EDITOR
        if (isValidateQueued) return;

        isValidateQueued = true;
        UnityEditor.EditorApplication.delayCall -= HandleDelayedValidate;
        UnityEditor.EditorApplication.delayCall += HandleDelayedValidate;
#endif
    }

#if UNITY_EDITOR
    private void HandleDelayedValidate()
    {
        isValidateQueued = false;
        if (this == null) return;

        EnsureReferences();
        SyncAllVisuals();
    }
#endif

    private void EnsureReferences()
    {
        // 자식 오브젝트 중 첫 번째를 main, 두 번째를 shadow로 자동 할당 (이름이나 태그로 구분해도 좋습니다)
        if (mainRenderer == null || shadowRenderer == null)
        {
            SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>(true);
            if (renderers.Length >= 2)
            {
                mainRenderer = renderers[0];
                shadowRenderer = renderers[1];
            }
        }
    }

    [ContextMenu("Sync All Visuals")]
    public void SyncAllVisuals()
    {
        if (targetSprite == null) return;

        EnsureReferences();

        // 1. 메인 오브젝트 (본체) 동기화
        if (mainRenderer != null)
        {
            mainRenderer.sprite = targetSprite;
            mainRenderer.color = mainColor;
        }

        // 2. 그림자 오브젝트 동기화
        if (shadowRenderer != null)
        {
            shadowRenderer.sprite = targetSprite;

            Color shadowColor = Color.black;
            shadowColor.a = shadowAlpha;
            shadowRenderer.color = shadowColor;
        }
    }
}