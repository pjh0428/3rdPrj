using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class Rifle : MonoBehaviour
{
    [Header("�ѱ�����")]
    //[SerializeField] private float Damage = 10f; // �Ѿ� ���ط�
    [SerializeField] private float FireRate = 0.2f; // �߻� ����
    [SerializeField] int MagazineSize = 30; // źâ ũ��
    [SerializeField] private float ReloadTime = 2.0f; // ������ �ð�

    [Header("�߻��������")]
    public Transform FirePoint; // �ѱ�
    public GameObject FireFlash; // �ѱ� ����
    public AudioClip ShootSound; // �߻� ����
    public AudioClip ReloadSound; // ������ ����

    public int CurrentAmmo; // ���� ź��
    private bool isReloading = false; // ������ ����
    private float NextFireTime = 0f; // ���� �߻� ���� �ð�
    private AudioSource AudioSource; 




    void Start()
    {
        CurrentAmmo = MagazineSize; // ���� ź�� �ʱ�ȭ
        AudioSource = gameObject.AddComponent<AudioSource>();
    }

   
    void Update()
    {
        // ������ ���� �ƴ� ���� �߻�
        if (isReloading)
            return;

        // �߻��Լ� - ���콺 ��Ŭ��
        if (Input.GetMouseButton(0) && Time.time >= NextFireTime)
        {
            NextFireTime = Time.time + FireRate; // �ð�����
            Fire();
            Debug.Log("�߻�, ���� ź��: " + CurrentAmmo);
        }

        // ������(R)
        if (Input.GetKeyDown(KeyCode.R))
        {
            // ������ �ڷ�ƾ
            StartCoroutine(Reload());
        }

        void Fire()
        {
            if (CurrentAmmo <= 0)
            {
                // �Ѿ��� ������ ������ �õ�
                StartCoroutine(Reload());
                return;
            }

            CurrentAmmo--;

            // �ѱ� ���� ����Ʈ ����
            if (FireFlash != null)
                Instantiate(FireFlash, FirePoint.position, FirePoint.rotation);

            // �߻� ���� ���
            if (ShootSound != null)
                AudioSource.PlayOneShot(ShootSound);

            // �߻� ���� �߰��ʿ�
        }

        IEnumerator Reload()
        {
            // ������ ���̰ų�, źâ�� ���� �� ������ ���������� ����
            if (isReloading || CurrentAmmo == MagazineSize)
            {
                yield break; // �ڷ�ƾ ��� ����
            }

            isReloading = true;
            Debug.Log("������ ����");

            // ������ ���� ���
            if (ReloadSound != null)
                AudioSource.PlayOneShot(ReloadSound);

            // ������ �ִϸ��̼� �����Լ� �ʿ�

            // ������ �ð���ŭ ���
            yield return new WaitForSeconds(ReloadTime);

            // �Ѿ� ä���
            CurrentAmmo = MagazineSize;
            isReloading = false;
            Debug.Log("������ �Ϸ�");
        }



    }
}
