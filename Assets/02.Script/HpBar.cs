using UnityEngine;

public class HpBar : MonoBehaviour
{
    private float playerHp;
    private float fullHp;

    void Awake()
    {
    }

    void Start()
    {
        fullHp = 100.0f;
    }

    void Update()
    {
        playerHp = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>().hp;
        if(playerHp < 0.0f || playerHp > fullHp)
            return;
        GetComponent<RectTransform>().localScale = new Vector3(playerHp/fullHp, 1.0f, 1.0f);
    }
}
