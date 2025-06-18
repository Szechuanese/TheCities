using UnityEngine;
using UnityEngine.UI;

public class FileNavScrollWatcher : MonoBehaviour
{//����filePanel������������
    public ScrollRect scrollRect;
    public GameObject fileNavBox;
    public GameObject fileNavBoxF;

    private bool isAtTop = false;

    void Update()
    {
        if (scrollRect == null) return;

        // verticalNormalizedPosition == 1 ��ʾ�������˶���
        bool nowAtTop = scrollRect.verticalNormalizedPosition >= .45f;//��һ��ValueBox�ͼ�0.5

        if (nowAtTop && !isAtTop)
        {
            isAtTop = true;
            fileNavBox.SetActive(false);
            fileNavBoxF.SetActive(true);
            Debug.Log("����������fileNavBox ���أ�fileNavBoxF ��ʾ");
        }
        else if (!nowAtTop && isAtTop)
        {
            isAtTop = false;
            fileNavBox.SetActive(true);
            fileNavBoxF.SetActive(false);
            Debug.Log("�����뿪���ˣ�fileNavBox ��ʾ��fileNavBoxF ����");
        }
    }
}
