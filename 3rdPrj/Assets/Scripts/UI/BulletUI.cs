using System;
using System.Reflection;
using UnityEngine;
using TMPro;

public class BulletUI : MonoBehaviour
{
    [Header("Rifle ��ũ��Ʈ�� ���� ������Ʈ")]
    [SerializeField] private MonoBehaviour rifleComponent;

    [Header("UI Text (TextMeshPro)")]
    [SerializeField] private TMP_Text ammoText;   // �� ����: Text �� TMP_Text

    private FieldInfo fiCurrent;
    private FieldInfo fiMagazine;

    private void Awake()
    {
        // 1) ���÷��� �ʵ� ����
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
            Debug.LogError("Rifle ��ũ��Ʈ���� private �ʵ带 ã�� ���߽��ϴ�.");

        // 2) TextMeshPro ��Ÿ�� �ʱ�ȭ
        if (ammoText != null)
        {
            // �⺻ �ؽ�Ʈ ����: #D8D8D8
            ammoText.color = new Color32(0xD8, 0xD8, 0xD8, 0xFF);

            // Outline ����: ���� #2A0000, �β� 0.2
            ammoText.fontMaterial.SetColor(
                ShaderUtilities.ID_OutlineColor,
                new Color32(0x2A, 0x00, 0x00, 0xFF)
            );
            ammoText.fontMaterial.SetFloat(
                ShaderUtilities.ID_OutlineWidth,
                0.2f
            );

            // (����) Underlay ȿ��: �׸���ó�� ������ �Ʒ��� �ణ ������
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
