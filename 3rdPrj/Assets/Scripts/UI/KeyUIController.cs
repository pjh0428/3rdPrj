using UnityEngine;
using UnityEngine.UI;

public class KeyUIController : MonoBehaviour
{
    public static KeyUIController Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject keyUIPanel; // ���� �г� ������Ʈ
    [SerializeField] private Image keyIcon;         // ���� ������(UI Image)
    [SerializeField] private Text countText;        // KeyCountText

    [Header("Settings")]
    [SerializeField] private int totalKeys = 6;     // �� ���� ����

    private int collectedKeys = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // �ʱ� UI ����
        keyIcon.gameObject.SetActive(true);
        countText.text = $"{collectedKeys} / {totalKeys}";
        // ���� ���̾ƿ� ����
        LayoutRebuilder.ForceRebuildLayoutImmediate(
            keyUIPanel.GetComponent<RectTransform>());
    }

    /// <summary>
    /// Ű�� �� �� ȹ���� ������ ȣ��
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
            // ��� Ű ���� �Ϸ� ����
            keyIcon.gameObject.SetActive(false);

            // 1) KeyUIPanel�� HorizontalLayoutGroup�� ���� Control Child Size üũ�ϱ�
            var hlg = keyUIPanel.GetComponent<HorizontalLayoutGroup>();
            hlg.childControlWidth = true;
            hlg.childControlHeight = true;

            // �ؽ�Ʈ�� �ȳ� ������ ��ü�ϰ� ���� ����
            countText.alignment = TextAnchor.MiddleRight;
            countText.text = " �� �߾� �������� ����";
        }

        // �г� ũ�⸦ ��� �ٽ� ���
        LayoutRebuilder.ForceRebuildLayoutImmediate(
            keyUIPanel.GetComponent<RectTransform>());
    }
}
