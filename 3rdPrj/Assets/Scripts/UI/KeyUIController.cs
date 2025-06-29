using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyUIController : MonoBehaviour
{
    public static KeyUIController Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject keyUIPanel; // 투명 패널 오브젝트
    [SerializeField] private Image keyIcon;         // 열쇠 아이콘(UI Image)
    [SerializeField] private TMP_Text countText;    // TextMeshPro 텍스트

    [Header("Settings")]
    [SerializeField] private int totalKeys = 6;     // 총 열쇠 개수

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

        // 기본 텍스트 스타일 설정: 회색(#D8D8D8), 붉은 테두리(#2A0000)
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
        // 초기 UI 세팅
        keyIcon.gameObject.SetActive(true);
        countText.text = $"{collectedKeys} / {totalKeys}";
        LayoutRebuilder.ForceRebuildLayoutImmediate(
            keyUIPanel.GetComponent<RectTransform>());
    }

    /// <summary>
    /// 키 UI 패널을 활성화
    /// </summary>
    public void ShowKeyUI()
    {
        keyUIPanel.SetActive(true);
    }

    /// <summary>
    /// 키를 획득할 때마다 호출
    /// </summary>
    public void OnKeyCollected()
    {
        collectedKeys = Mathf.Clamp(collectedKeys + 1, 0, totalKeys);

        if (collectedKeys < totalKeys)
        {
            // 수집 중 상태
            keyIcon.gameObject.SetActive(true);
            countText.text = $"{collectedKeys} / {totalKeys}";
        }
        else
        {
            // 모든 키 수집 완료
            keyIcon.gameObject.SetActive(false);

            var hlg = keyUIPanel.GetComponent<HorizontalLayoutGroup>();
            hlg.childControlWidth = true;
            hlg.childControlHeight = true;

            countText.alignment = TextAlignmentOptions.Right;
            countText.text = " ※ 중앙 현관으로 가기";
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(
            keyUIPanel.GetComponent<RectTransform>());
    }

    // 획득한 키 개수
    public int CollectedKeys => collectedKeys;
    // 총 키 개수
    public int TotalKeys => totalKeys;
}
