using UnityEngine;

/// <summary>
/// [v2 Update] 유저의 기본 배치(그림자가 바닥에 누워있는 상태)에 맞춰
/// 평면 회전 및 스케일링을 수행하는 시니어급 컨트롤러입니다.
/// </summary>
public class DynamicBlobShadow : MonoBehaviour
{
    private enum ShadowLengthAxis { X, Y, Z }
    private const float MinHeightDiff = 0.1f;
    private const float DirectionEpsilon = 0.001f;
    private const float StretchBaseMultiplier = 0.5f;

    [Header("Target Props")]
    [SerializeField] private Transform lightSource;      // 광원 오브젝트 (Torch 등)
    [SerializeField] private Transform characterBase;    // 캐릭터의 발밑 위치 (Pivot)
    [SerializeField] private Transform shadowTransform;  // 바닥에 누워있는 그림자 Quad/Sprite

    [Header("Shadow Tuning")]
    [Tooltip("그림자가 늘어나는 정도를 조절합니다.")]
    [Range(0.1f, 5f)][SerializeField] private float stretchIntensity = 1.5f;
    [Tooltip("그림자의 최소 길이 (Z축 스케일)")]
    [SerializeField] private float minShadowLength = 0.5f;
    [Tooltip("그림자의 최대 길이 (너무 길어짐 방지)")]
    [SerializeField] private float maxShadowLength = 3.0f;
    [Tooltip("그림자 길이로 사용할 로컬 축 (Quad/Sprite 방향에 맞춰 설정)")]
    [SerializeField] private ShadowLengthAxis lengthAxis = ShadowLengthAxis.Y;

    [Header("Runtime Control")]
    [Tooltip("끄면 이 스크립트가 그림자 변형을 적용하지 않습니다.")]
    [SerializeField] private bool enableDynamicShadow = true;

    // 초기 그림자 스케일과 회전을 저장하여 비율 유지
    private Vector3 initialScale;
    private Quaternion initialRotation;

    public bool EnableDynamicShadow
    {
        get => enableDynamicShadow;
        set => enableDynamicShadow = value;
    }

    public void SetDynamicShadowEnabled(bool enabled)
    {
        enableDynamicShadow = enabled;
    }

    private void Awake()
    {
        CacheReferences();
    }

    private void OnValidate()
    {
        if (minShadowLength > maxShadowLength)
        {
            maxShadowLength = minShadowLength;
        }

        if (shadowTransform != null)
        {
            CacheReferences();
        }
    }

    private void LateUpdate()
    {
        if (!HasRequiredTargets())
        {
            return;
        }

        if (!enableDynamicShadow)
        {
            return;
        }

        ApplyShadowProps();
    }

    private bool HasRequiredTargets()
    {
        return lightSource != null && shadowTransform != null && characterBase != null;
    }

    private void CacheReferences()
    {
        initialScale = shadowTransform.localScale;
        initialRotation = shadowTransform.rotation;
    }

    private void ApplyShadowProps()
    {
        // 1) 방향 벡터 계산 (광원 -> 캐릭터 발밑)
        Vector3 lightToCharDir = characterBase.position - lightSource.position;
        float distance = lightToCharDir.magnitude;

        // XZ 평면상의 방향만 추출
        Vector3 planarDir = new Vector3(lightToCharDir.x, 0, lightToCharDir.z);

        // 방향 벡터가 유효한 경우에만 회전 처리
        if (planarDir.sqrMagnitude > DirectionEpsilon)
        {
            planarDir.Normalize();
            ApplyShadowRotation(planarDir);
        }

        // 2) 그림자 길이 계산 (광원 높이와 거리에 따라)
        // 광원이 낮을수록, 거리가 멀수록 그림자가 길어짐
        float heightDiff = Mathf.Max(lightSource.position.y - characterBase.position.y, MinHeightDiff);
        float stretchFactor = distance / heightDiff * stretchIntensity * StretchBaseMultiplier;

        ApplyShadowScale(stretchFactor);

    }

    private void ApplyShadowRotation(Vector3 planarDir)
    {
        float yRotation = Mathf.Atan2(planarDir.x, planarDir.z) * Mathf.Rad2Deg;
        Vector3 currentEuler = initialRotation.eulerAngles;
        shadowTransform.rotation = Quaternion.Euler(currentEuler.x, yRotation, currentEuler.z);
    }

    private void ApplyShadowScale(float stretchFactor)
    {
        float scaledLength = GetScaledLength(stretchFactor);

        Vector3 targetScale = initialScale;
        switch (lengthAxis)
        {
            case ShadowLengthAxis.X:
                targetScale.x = scaledLength;
                break;
            case ShadowLengthAxis.Y:
                targetScale.y = scaledLength;
                break;
            case ShadowLengthAxis.Z:
                targetScale.z = scaledLength;
                break;
        }

        shadowTransform.localScale = targetScale;
    }

    private float GetScaledLength(float stretchFactor)
    {
        float axisInitialLength = lengthAxis switch
        {
            ShadowLengthAxis.X => initialScale.x,
            ShadowLengthAxis.Y => initialScale.y,
            _ => initialScale.z
        };

        return Mathf.Clamp(axisInitialLength * stretchFactor, minShadowLength, maxShadowLength);
    }


}