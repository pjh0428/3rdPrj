using UnityEngine;
using UnityEngine.UI;

public class KeyUIController : MonoBehaviour
{
    public static KeyUIController Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject keyUIPanel; // 투명 패널 오브젝트
    [SerializeField] private Image keyIcon;         // 열쇠 아이콘(UI Image)
    [SerializeField] private Text countText;        // KeyCountText

    [Header("Settings")]
    [SerializeField] private int totalKeys = 6;     // 총 열쇠 개수

    private int collectedKeys = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // ① 시작할 때 비활성화
        keyUIPanel.SetActive(false);
    }

    /// <summary>
    /// 외부에서 호출해서 키 UI를 켭니다.
    /// </summary>
    public void ShowKeyUI()
    {
        keyUIPanel.SetActive(true);
    }

    private void Start()
    {
        // 초기 UI 세팅
        keyIcon.gameObject.SetActive(true);
        countText.text = $"{collectedKeys} / {totalKeys}";
        // 강제 레이아웃 갱신
        LayoutRebuilder.ForceRebuildLayoutImmediate(
            keyUIPanel.GetComponent<RectTransform>());
    }

    /// <summary>
    /// 키를 한 개 획득할 때마다 호출
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
            // 모든 키 수집 완료 상태
            keyIcon.gameObject.SetActive(false);

            // 1) KeyUIPanel의 HorizontalLayoutGroup에 가서 Control Child Size 체크하기
            var hlg = keyUIPanel.GetComponent<HorizontalLayoutGroup>();
            hlg.childControlWidth = true;
            hlg.childControlHeight = true;

            // 텍스트를 안내 문구로 교체하고 우측 정렬
            countText.alignment = TextAnchor.MiddleRight;
            countText.text = " ※ 중앙 현관으로 가기";
        }

        // 패널 크기를 즉시 다시 계산
        LayoutRebuilder.ForceRebuildLayoutImmediate(
            keyUIPanel.GetComponent<RectTransform>());
    }

    // 획득한 키 개수
    public int CollectedKeys => collectedKeys;
    // 총 키 개수
    public int TotalKeys => totalKeys;
}
