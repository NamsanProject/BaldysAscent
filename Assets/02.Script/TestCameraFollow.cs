using UnityEngine;

/// <summary>
/// 대상(Target)을 일정한 오프셋을 유지하며 부드럽게 추적하는 카메라 스크립트입니다.
/// </summary>
public class TestCameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 10, -10);

    [Header("Smoothing")]
    [SerializeField] private float smoothTime = 0.2f;

    private Vector3 _currentVelocity = Vector3.zero;

    private void LateUpdate()
    {
        if (target == null) return;

        // 목표 위치 계산 (대상 위치 + 설정된 오프셋)
        Vector3 targetPosition = target.position + offset;

        // 위치 보간 이동
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
    }

    /// <summary>
    /// 에디터에서 오프셋 설정을 직관적으로 확인하기 위한 도구입니다.
    /// </summary>
    public void SetTarget(Transform newTarget) => target = newTarget;
}