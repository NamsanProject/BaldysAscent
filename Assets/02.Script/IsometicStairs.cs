using UnityEngine;

public class IsometricStairs : MonoBehaviour
{
    [Header("1. 계단 기준점 (진행률 계산용)")]
    public Transform bottomPoint;
    public Transform topPoint;

    [Header("2. 렌더링 레이어 이름 (Sorting Layer)")]
    public string floor1SortLayer = "Floor1";
    public string floor2SortLayer = "Floor2";

    [Header("3. 물리 레이어 이름 (Physics Layer)")]
    public string playerLayer1 = "Player_L1";
    public string playerLayer2 = "Player_L2";

    private SpriteRenderer playerRenderer;
    private GameObject playerObject;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerObject = other.gameObject;
            playerRenderer = other.GetComponentInChildren<SpriteRenderer>();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || playerObject == null) return;

        // 계단 진행률 계산 (0.0 ~ 1.0)
        Vector2 stairVector = topPoint.position - bottomPoint.position;
        Vector2 playerVector = other.transform.position - bottomPoint.position;

        if (stairVector.sqrMagnitude < 0.0001f) return;

        float progress = Vector2.Dot(playerVector, stairVector) / stairVector.sqrMagnitude;
        progress = Mathf.Clamp01(progress);

        // 딱 중간(0.5)을 넘어가면 2층 레이어로, 아니면 1층 레이어로 스위칭!
        if (progress > 0.5f)
        {
            ChangeLayer(floor2SortLayer, playerLayer2);
        }
        else
        {
            ChangeLayer(floor1SortLayer, playerLayer1);
        }
    }

    private void ChangeLayer(string sortLayerName, string physicsLayerName)
    {
        // 렌더링 레이어 변경
        if (playerRenderer != null && playerRenderer.sortingLayerName != sortLayerName)
        {
            playerRenderer.sortingLayerName = sortLayerName;
        }

        // 물리 레이어 변경 전에 이름이 진짜 있는지 검사! (안전장치)
        int targetLayerIndex = LayerMask.NameToLayer(physicsLayerName);

        if (targetLayerIndex == -1) // 레이어를 못 찾았을 때
        {
            Debug.LogError($"[계단 에러] '{physicsLayerName}'이라는 물리 레이어를 찾을 수 없습니다! Layers 설정에 오타나 띄어쓰기가 없는지 확인하세요.");
            return; // 에러를 막고 함수를 빠져나갑니다.
        }

        if (playerObject.layer != targetLayerIndex)
        {
            playerObject.layer = targetLayerIndex;
        }
    }
}