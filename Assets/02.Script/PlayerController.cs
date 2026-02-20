using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // 마찰력 제로 설정 (벽에 걸림 방지)
        if (rb.sharedMaterial == null)
        {
            PhysicsMaterial2D mat = new PhysicsMaterial2D("Slippery");
            mat.friction = 0f;
            rb.sharedMaterial = mat;
        }
    }

    void Update()
    {
        // 입력 값: W(0,1), S(0,-1), A(-1,0), D(1,0)
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // 화면 좌표계 그대로 이동 (대각선 보정 없음)
        if (moveInput != Vector2.zero)
        {
            rb.linearVelocity = moveInput.normalized * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}