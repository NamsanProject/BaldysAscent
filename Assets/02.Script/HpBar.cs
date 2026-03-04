using UnityEngine;

public class HpBar : MonoBehaviour
{
    private float playerHp;
    private float maxHp;

    void Awake()
    {
    }

    void Start()
    {
        maxHp = 100.0f;
    }

    void Update()
    {
        playerHp = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>().hp;
        if(playerHp < 0.0f || playerHp > maxHp)
            return;
        GetComponent<RectTransform>().localScale = new Vector3(playerHp/maxHp, 1.0f, 1.0f);
    }
}
