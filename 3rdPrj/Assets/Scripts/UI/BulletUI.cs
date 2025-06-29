using System;
using System.Reflection;
using UnityEngine;
using TMPro;

public class BulletUI : MonoBehaviour
{
    [Header("Rifle 스크립트가 붙은 오브젝트")]
    [SerializeField] private MonoBehaviour rifleComponent;

    [Header("UI Text (TextMeshPro)")]
    [SerializeField] private TMP_Text ammoText;   // ← 변경: Text → TMP_Text

    private FieldInfo fiCurrent;
    private FieldInfo fiMagazine;

    private void Awake()
    {
        // 1) 리플렉션 필드 세팅
        if (rifleComponent == null)
        {
            rifleComponent =
              UnityEngine.Object.FindFirstObjectByType(Type.GetType("Rifle"))
              as MonoBehaviour;
        }

        var rifleType = rifleComponent.GetType();
        fiCurrent = rifleType.GetField("CurrentAmmo",
            BindingFlags.Instance | BindingFlags.NonPublic);
        fiMagazine = rifleType.GetField("MagazineSize",
            BindingFlags.Instance | BindingFlags.NonPublic);

        if (fiCurrent == null || fiMagazine == null)
            Debug.LogError("Rifle 스크립트에서 private 필드를 찾지 못했습니다.");

        // 2) TextMeshPro 스타일 초기화
        if (ammoText != null)
        {
            // 기본 텍스트 색상: #D8D8D8
            ammoText.color = new Color32(0xD8, 0xD8, 0xD8, 0xFF);

            // Outline 설정: 색상 #2A0000, 두께 0.2
            ammoText.fontMaterial.SetColor(
                ShaderUtilities.ID_OutlineColor,
                new Color32(0x2A, 0x00, 0x00, 0xFF)
            );
            ammoText.fontMaterial.SetFloat(
                ShaderUtilities.ID_OutlineWidth,
                0.2f
            );

            // (선택) Underlay 효과: 그림자처럼 붉은색 아래로 약간 퍼지게
            ammoText.fontMaterial.EnableKeyword("UNDERLAY_ON");
            ammoText.fontMaterial.SetColor(
                "_UnderlayColor",
                new Color32(0x2A, 0x00, 0x00, 0x80)
            );
            ammoText.fontMaterial.SetFloat("_UnderlaySoftness", 0.5f);
            ammoText.fontMaterial.SetFloat("_UnderlayOffsetX", 1f);
            ammoText.fontMaterial.SetFloat("_UnderlayOffsetY", -1f);
        }
    }

    private void Update()
    {
        if (fiCurrent == null || fiMagazine == null || ammoText == null) return;

        int current = (int)fiCurrent.GetValue(rifleComponent);
        int total = (int)fiMagazine.GetValue(rifleComponent);

        ammoText.text = $"{current} / {total}";
    }
}
