using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class TileTester : MonoBehaviour
{
    public Grid grid;
    public Tilemap tilemap;

    void Update()
    {
        // 최신 Input System에서 마우스 왼쪽 클릭 확인
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // 현재 마우스 위치 가져오기
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            // 카메라를 통해 월드 좌표로 변환하기 위한 레이(Ray) 생성
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            // 타일맵이 눕혀져 있는 평면(Y=0) 정의
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            if (plane.Raycast(ray, out float distance))
            {
                Vector3 worldPosition = ray.GetPoint(distance);

                // 월드 좌표를 타일맵의 셀 좌표(int)로 변환
                Vector3Int cellPosition = grid.WorldToCell(worldPosition);

                Debug.Log($"[New Input System] 클릭한 타일 좌표: {cellPosition}");
            }
        }
    }
}