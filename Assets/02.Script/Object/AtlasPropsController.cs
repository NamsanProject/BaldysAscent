using UnityEngine;

[ExecuteAlways] // 에디터에서도 실시간으로 보이게 함
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class AtlasPropController : MonoBehaviour
{
    [Header("사용할 스프라이트")]
    public Sprite targetSprite;

    private MeshRenderer _renderer;
    private MaterialPropertyBlock _propBlock;

    // 셰이더 프로퍼티 ID 미리 캐싱 (성능 최적화)
    private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");
    private static readonly int BaseMapST = Shader.PropertyToID("_BaseMap_ST");
    private static readonly int AlphaClipThreshold = Shader.PropertyToID("_Cutoff");

    void OnEnable()
    {
        UpdateVisuals();
    }

    // 인스펙터 값이 바뀔 때마다 호출
    void OnValidate()
    {
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        if (targetSprite == null) return;

        if (_renderer == null) _renderer = GetComponent<MeshRenderer>();
        if (_propBlock == null) _propBlock = new MaterialPropertyBlock();

        // 1. 아틀라스 텍스처와 UV 좌표 가져오기
        Texture atlasTexture = targetSprite.texture;
        Rect textureRect = targetSprite.textureRect;

        // 2. UV Tiling & Offset 계산 (전체 아틀라스 중 내 그림의 위치)
        float tilingX = textureRect.width / atlasTexture.width;
        float tilingY = textureRect.height / atlasTexture.height;
        float offsetX = textureRect.x / atlasTexture.width;
        float offsetY = textureRect.y / atlasTexture.height;

        // 3. PropertyBlock에 값 입력 (GPU Instancing 유지)
        _renderer.GetPropertyBlock(_propBlock);

        _propBlock.SetTexture(BaseMap, atlasTexture);
        _propBlock.SetVector(BaseMapST, new Vector4(tilingX, tilingY, offsetX, offsetY));
        _propBlock.SetFloat(AlphaClipThreshold, 0.5f); // 알파 클리핑 강도

        _renderer.SetPropertyBlock(_propBlock);

        // 4. Quad 크기 자동 조절 (PPU 유지)
        // 2.5D에서는 PPU에 맞춰 스케일을 조절해야 픽셀 크기가 일정합니다.
        // float pixelWidth = textureRect.width;
        // float pixelHeight = textureRect.height;
        // float ppu = targetSprite.pixelsPerUnit;

        // transform.localScale = new Vector3(pixelWidth / ppu, pixelHeight / ppu, 1f);
    }
}