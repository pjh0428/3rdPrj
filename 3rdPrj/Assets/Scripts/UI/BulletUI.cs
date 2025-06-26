using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class BulletUI : MonoBehaviour
{
    [Header("Rifle ��ũ��Ʈ�� ���� ������Ʈ")]
    [SerializeField] private MonoBehaviour rifleComponent;  // Rifle�� ���� ������Ʈ (�ν����Ϳ� �巡��)  

    [Header("UI Text")]
    [SerializeField] private Text ammoText;                // ǥ���� Text ������Ʈ  

    // ���÷��ǿ� �ʵ� ����  
    private FieldInfo fiCurrent;
    private FieldInfo fiMagazine;

    private void Awake()
    {
        if (rifleComponent == null)
        {
            // UnityEngine.Object�� ��������� ����Ͽ� ��ȣ�� ����  
            rifleComponent = UnityEngine.Object.FindFirstObjectByType(Type.GetType("Rifle")) as MonoBehaviour;
        }

        // Rifle Ÿ�� ��������  
        var rifleType = rifleComponent.GetType();

        // private int CurrentAmmo �ʵ�  
        fiCurrent = rifleType.GetField("CurrentAmmo",
            BindingFlags.Instance | BindingFlags.NonPublic);
        // private int MagazineSize �ʵ�  
        fiMagazine = rifleType.GetField("MagazineSize",
            BindingFlags.Instance | BindingFlags.NonPublic);

        if (fiCurrent == null || fiMagazine == null)
            Debug.LogError("Rifle ��ũ��Ʈ���� private �ʵ带 ã�� ���߽��ϴ�.");
    }

    private void Update()
    {
        if (fiCurrent == null || fiMagazine == null) return;

        // ���÷������� private �ʵ尪 �б�  
        int current = (int)fiCurrent.GetValue(rifleComponent);
        int total = (int)fiMagazine.GetValue(rifleComponent);

        ammoText.text = $"{current} / {total}";
    }
}
