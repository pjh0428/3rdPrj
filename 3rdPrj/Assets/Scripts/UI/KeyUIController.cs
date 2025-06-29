using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyUIController : MonoBehaviour
{
    public static KeyUIController Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject keyUIPanel; // ���� �г� ������Ʈ
    [SerializeField] private Image keyIcon;         // ���� ������(UI Image)
    [SerializeField] private TMP_Text countText;    // TextMeshPro �ؽ�Ʈ

    [Header("Settings")]
    [SerializeField] private int totalKeys = 6;     // �� ���� ����

    private int collectedKeys = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        keyUIPanel.SetActive(false);

        // �⺻ �ؽ�Ʈ ��Ÿ�� ����: ȸ��(#D8D8D8), ���� �׵θ�(#2A0000)
        if (countText != null)
        {
            countText.color = new Color32(0xD8, 0xD8, 0xD8, 0xFF);
            countText.fontMaterial.SetColor(
                ShaderUtilities.ID_OutlineColor,
                new Color32(0x2A, 0x00, 0x00, 0xFF)
            );
            countText.fontMaterial.SetFloat(
                ShaderUtilities.ID_OutlineWidth,
                0.2f
            );
        }
    }

    private void Start()
    {
        // �ʱ� UI ����
        keyIcon.gameObject.SetActive(true);
        countText.text = $"{collectedKeys} / {totalKeys}";
        LayoutRebuilder.ForceRebuildLayoutImmediate(
            keyUIPanel.GetComponent<RectTransform>());
    }

    /// <summary>
    /// Ű UI �г��� Ȱ��ȭ
    /// </summary>
    public void ShowKeyUI()
    {
        keyUIPanel.SetActive(true);
    }

    /// <summary>
    /// Ű�� ȹ���� ������ ȣ��
    /// </summary>
    public void OnKeyCollected()
    {
        collectedKeys = Mathf.Clamp(collectedKeys + 1, 0, totalKeys);

        if (collectedKeys < totalKeys)
        {
            // ���� �� ����
            keyIcon.gameObject.SetActive(true);
            countText.text = $"{collectedKeys} / {totalKeys}";
        }
        else
        {
            // ��� Ű ���� �Ϸ�
            keyIcon.gameObject.SetActive(false);

            var hlg = keyUIPanel.GetComponent<HorizontalLayoutGroup>();
            hlg.childControlWidth = true;
            hlg.childControlHeight = true;

            countText.alignment = TextAlignmentOptions.Right;
            countText.text = " �� �߾� �������� ����";
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(
            keyUIPanel.GetComponent<RectTransform>());
    }

    // ȹ���� Ű ����
    public int CollectedKeys => collectedKeys;
    // �� Ű ����
    public int TotalKeys => totalKeys;
}
