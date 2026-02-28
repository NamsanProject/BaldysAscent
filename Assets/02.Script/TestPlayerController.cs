using UnityEngine;

/// <summary>
/// 2.5D 환경에서 플레이어의 이동을 제어하는 클래스입니다.
/// X, Z 축을 기반으로 이동하며 Rigidbody를 통해 물리 충돌을 처리합니다.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class TestPlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float acceleration = 10.0f;

    private Rigidbody _rb;
    private Vector3 _moveInput;
    private Vector3 _currentVelocity;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        // 2.5D 특성상 캐릭터가 넘어지지 않도록 회전을 고정합니다.
        _rb.constraints = RigidbodyConstraints.FreezeRotationX |
                          RigidbodyConstraints.FreezeRotationY |
                          RigidbodyConstraints.FreezeRotationZ;

        // 중력 사용 여부는 프로젝트 기획에 따라 결정 (기본값 true)
        _rb.useGravity = true;
    }

    private void Update()
    {
        // 입력값 수집 (WASD 또는 방향키)
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        _moveInput = new Vector3(h, 0, v).normalized;
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        // 목표 속도 계산
        Vector3 targetVelocity = _moveInput * moveSpeed;

        // 부드러운 가감속 적용
        _currentVelocity = Vector3.MoveTowards(_currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);

        // Y축 속도(중력)는 유지하면서 X, Z 속도만 변경
        Vector3 newVelocity = new Vector3(_currentVelocity.x, _rb.linearVelocity.y, _currentVelocity.z);
        _rb.linearVelocity = newVelocity;
    }
}