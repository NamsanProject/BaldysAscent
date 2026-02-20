using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Animator anim;

    [Header("Movement Settings")]
    public float speed;

    [Header("Hp")]
    public float hp;
    
    private Vector2 moveInput;
    private bool dirLeft;
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        speed = 1.0f;
        hp = 100.0f;
        dirLeft = true;
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (moveInput.x > 0 && dirLeft)
            Flip();
        else if (moveInput.x < 0 && !dirLeft)
            Flip();
    }

    void FixedUpdate()
    {
        if (moveInput != Vector2.zero)
        {
            rigid.linearVelocity = moveInput.normalized * speed;
            anim.SetBool("Run", true);
        }
        else
        {
            rigid.linearVelocity = Vector2.zero;
            anim.SetBool("Run", false);
        }
    }

    void Flip()
    {
        dirLeft = !dirLeft;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
