using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class BulletUI : MonoBehaviour
{
    [Header("Rifle 스크립트가 붙은 오브젝트")]
    [SerializeField] private MonoBehaviour rifleComponent;  // Rifle이 붙은 컴포넌트 (인스펙터에 드래그)  

    [Header("UI Text")]
    [SerializeField] private Text ammoText;                // 표시할 Text 컴포넌트  

    // 리플렉션용 필드 인포  
    private FieldInfo fiCurrent;
    private FieldInfo fiMagazine;

    private void Awake()
    {
        if (rifleComponent == null)
        {
            // UnityEngine.Object를 명시적으로 사용하여 모호성 제거  
            rifleComponent = UnityEngine.Object.FindFirstObjectByType(Type.GetType("Rifle")) as MonoBehaviour;
        }

        // Rifle 타입 가져오기  
        var rifleType = rifleComponent.GetType();

        // private int CurrentAmmo 필드  
        fiCurrent = rifleType.GetField("CurrentAmmo",
            BindingFlags.Instance | BindingFlags.NonPublic);
        // private int MagazineSize 필드  
        fiMagazine = rifleType.GetField("MagazineSize",
            BindingFlags.Instance | BindingFlags.NonPublic);

        if (fiCurrent == null || fiMagazine == null)
            Debug.LogError("Rifle 스크립트에서 private 필드를 찾지 못했습니다.");
    }

    private void Update()
    {
        if (fiCurrent == null || fiMagazine == null) return;

        // 리플렉션으로 private 필드값 읽기  
        int current = (int)fiCurrent.GetValue(rifleComponent);
        int total = (int)fiMagazine.GetValue(rifleComponent);

        ammoText.text = $"{current} / {total}";
    }
}
