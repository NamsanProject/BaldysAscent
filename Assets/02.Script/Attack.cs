using UnityEngine;

public class Attack : MonoBehaviour
{
    private bool attackState;
    private BoxCollider atkBox;

    void Awake()
    {
        atkBox = GetComponent<BoxCollider>();
    }

    void Update()
    {
        attackState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>().attackState;
        if(attackState)
            atkBox.enabled = true;
        else
            atkBox.enabled = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Enemy")
        {
            Debug.Log("Hit Enemy");
        }
    }
}
