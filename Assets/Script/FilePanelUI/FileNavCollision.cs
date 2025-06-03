using UnityEngine;

public class FileNavCollision : MonoBehaviour
{
    [Header("Ҫ�����صĶ���ScrollView �У�")]
    public GameObject fileNavBox;

    [Header("fileNavBox ������ʱ��ʾ��������󣨿�ѡ��")]
    public GameObject fileNavBoxF;

    private Collider2D navCollider;       // ��ǰ����� Collider
    private Collider2D targetCollider;    // fileNavBox �� Collider
    private bool isFloating = false;      // ��ǰ״̬���

    void Start()
    {
        navCollider = GetComponent<Collider2D>();
        targetCollider = fileNavBox.GetComponent<Collider2D>();
    }

    void Update()
    {
        if (navCollider == null || targetCollider == null) return;

        bool isOverlapping = navCollider.IsTouching(targetCollider);

        if (isOverlapping && !isFloating)
        {
            fileNavFloating();
            isFloating = true;
        }
        else if (!isOverlapping && isFloating)
        {
            fileNavActive();
            isFloating = false;
        }
    }

    private void fileNavFloating()
    {
        fileNavBox.SetActive(false);
        fileNavBoxF.SetActive(true);
        Debug.Log("���븡��״̬��fileNavBox ���أ�fileNavBoxF ��ʾ");
    }

    private void fileNavActive()
    {
        fileNavBox.SetActive(true);
        fileNavBoxF.SetActive(false);
        Debug.Log("�뿪����״̬��fileNavBox ��ʾ��fileNavBoxF ����");
    }
}