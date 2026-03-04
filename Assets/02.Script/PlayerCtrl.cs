using System.Collections;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private Rigidbody rigid;
    private SpriteRenderer spr;
    private Animator anim;
    private CapsuleCollider capCol;

    [Header("Movement Settings")]
    public float speed;
    private float gravity;
    // [Header("Jump Force")]
    // public float jumpForce;

    [Header("Hp")]
    public float hp;
    private float maxHp;
    [HideInInspector]
    public bool attackState;
    
    private Vector3 moveInput;

    // private float originY;
    // private float jumpVelocity;
    // private bool isGrounded;
    private bool dirLeft;
    private bool hitRecovery;
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capCol = GetComponent<CapsuleCollider>();
    }

    void Start()
    {
        speed = 5.0f;
        gravity = 500.0f;
        //jumpForce = 3.0f;
        //isGrounded = true;
        hp = 100.0f;
        maxHp = 100.0f;
        attackState = false;
        dirLeft = true;
        hitRecovery = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X) && attackState == false)
        {
            anim.SetTrigger("Attack");
            attackState = true;
            return;
        }

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.z = Input.GetAxisRaw("Vertical");

// 점프 관련
////////////////////////////////////////////////////////////////////////////
        // if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        // {
        //     originY = transform.position.y;
            
        //     anim.SetBool("Jump", true);
        //     jumpVelocity = jumpForce;
        //     isGrounded = false;
        // }

        // // 점프하는 동안
        // if(!isGrounded)
        // {
        //     jumpVelocity -= 9.81f * Time.deltaTime;
        //     shadow.transform.position = new Vector3(transform.position.x, originY, transform.position.z);
        // }

        // Vector3 pos = transform.position;
        // pos.y += jumpVelocity * Time.deltaTime;

        // // 착지
        // if ((pos.y <= originY) && !isGrounded)
        // {
        //     anim.SetBool("Jump", false);
        //     pos.y = originY;
        //     jumpVelocity = 0;
        //     isGrounded = true;
        // }

        // transform.position = pos;
////////////////////////////////////////////////////////////////////////////

        if (moveInput.x > 0 && dirLeft)
            Flip();
        else if (moveInput.x < 0 && !dirLeft)
            Flip();
    }

    void FixedUpdate()
    {
        // 중력 작용
        if(transform.position.y > 0.0f)
            rigid.AddForce(Vector3.down * gravity);
        else
        {
            Vector3 temp = transform.position;
            temp.y = 0.0f;
            transform.position = temp;
        }

        // 캐릭터 움직임
        if (moveInput != Vector3.zero && attackState == false)
        {
            rigid.linearVelocity = moveInput.normalized * speed;
            anim.SetBool("Run", true);
        }
        else
        {
            rigid.linearVelocity = Vector3.zero;
            anim.SetBool("Run", false);
        }
    }

    // 스프라이트 반전
    void Flip()
    {
        dirLeft = !dirLeft;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    // 체력 관련
    public void SetHp(float hp)
    {
        if(hp < 0.0f || hp > 100.0f)
        {
            Debug.Log("Out Of Range");
            return;        
        }
        this.hp = hp;
    }

    public void HpDown(float hp)
    {
        if(this.hp - hp < 0.0f)
        {
            Debug.Log("Out Of Range");
            return;  
        }
        this.hp -= hp;
    }

    public void HpUp(float hp)
    {
        if(this.hp + hp > maxHp)
        {
            Debug.Log("Out Of Range");
            return;  
        }
        this.hp += hp;
    }

    // 공격 애니메이션 종료
    public void AtkAnimEnd()
    {
        attackState = false;
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Enemy" && !hitRecovery)
        {
            HpDown(30.0f);
            hitRecovery = true;
            StartCoroutine("InvincibleFrame");
            StartCoroutine("Blink");
        }
    }

    // 무적 프레임
    IEnumerator InvincibleFrame()
    {
        yield return new WaitForSeconds(5.0f);
        StopCoroutine("Blink");
        spr.enabled = true;
        hitRecovery = false;
    }

    // 피격 시 이펙트
    IEnumerator Blink()
    {
        while(true)
        {
            spr.enabled = !spr.enabled;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
