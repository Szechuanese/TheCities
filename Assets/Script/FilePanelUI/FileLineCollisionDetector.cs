using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileLineCollisionDetector : MonoBehaviour
{
    [Header("��ײĿ��")]
    public RectTransform targetUIObject;    // Ҫ�����ײ��UI����

    [Header("���ƶ���")]
    public GameObject fileNavBox;              // Ĭ����ʾ�Ķ���
    public GameObject fileNavBoxf;              // ��ײʱ��ʾ�Ķ���

    [Header("�������")]
    public float detectionThreshold = 10f;  // �����ֵ�����ؾ��룩
    public bool useOverlapDetection = true; // �Ƿ�ʹ���ص����

    private RectTransform lineRectTransform;
    private Canvas canvas;
    private bool isColliding = false;
    private bool lastCollisionState = false;

    private void Start()
    {
        lineRectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        // ��ʼ״̬����ʾObjectA������ObjectB
        SetInitialState();
    }

    private void Update()
    {
        CheckCollision();
    }

    private void CheckCollision()
    {
        if (targetUIObject == null || lineRectTransform == null)
            return;

        bool currentCollisionState = false;

        if (useOverlapDetection)
        {
            // ����1��ʹ��RectTransform�ص����
            currentCollisionState = IsOverlapping(lineRectTransform, targetUIObject);
        }
        else
        {
            // ����2��ʹ�þ�����
            currentCollisionState = IsWithinDistance();
        }

        // ֻ����ײ״̬�����仯ʱִ���л�
        if (currentCollisionState != lastCollisionState)
        {
            if (currentCollisionState)
            {
                OnCollisionEnter();
            }
            else
            {
                OnCollisionExit();
            }
            lastCollisionState = currentCollisionState;
        }

        isColliding = currentCollisionState;
    }

    // �������RectTransform�Ƿ��ص�
    private bool IsOverlapping(RectTransform rect1, RectTransform rect2)
    {
        // ��ȡ���������µľ��α߽�
        Vector3[] rect1Corners = new Vector3[4];
        Vector3[] rect2Corners = new Vector3[4];

        rect1.GetWorldCorners(rect1Corners);
        rect2.GetWorldCorners(rect2Corners);

        // ������α߽�
        Rect worldRect1 = new Rect(
            rect1Corners[0].x, rect1Corners[0].y,
            rect1Corners[2].x - rect1Corners[0].x,
            rect1Corners[2].y - rect1Corners[0].y
        );

        Rect worldRect2 = new Rect(
            rect2Corners[0].x, rect2Corners[0].y,
            rect2Corners[2].x - rect2Corners[0].x,
            rect2Corners[2].y - rect2Corners[0].y
        );

        // �������Ƿ��ص�
        return worldRect1.Overlaps(worldRect2);
    }

    // �������Ƿ�����ֵ��
    private bool IsWithinDistance()
    {
        Vector3 lineWorldPos = GetWorldPosition(lineRectTransform);
        Vector3 targetWorldPos = GetWorldPosition(targetUIObject);

        float distance = Vector3.Distance(lineWorldPos, targetWorldPos);
        return distance <= detectionThreshold;
    }

    // ��ȡRectTransform����������λ��
    private Vector3 GetWorldPosition(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        // �������ĵ�
        return (corners[0] + corners[2]) / 2f;
    }

    // ��ײ��ʼʱ����
    private void OnCollisionEnter()
    {
        Debug.Log("Line��ײ��Ŀ�꣡");

        // �ر�ObjectA����ObjectB
        if (fileNavBox != null)
            fileNavBox.SetActive(false);

        if (fileNavBoxf != null)
            fileNavBoxf.SetActive(true);
    }

    // ��ײ����ʱ����
    private void OnCollisionExit()
    {
        Debug.Log("Line�뿪Ŀ�꣡");

        // �ر�ObjectB����ObjectA
        if (fileNavBoxf != null)
            fileNavBoxf.SetActive(false);

        if (fileNavBox != null)
            fileNavBox.SetActive(true);
    }

    // ���ó�ʼ״̬
    private void SetInitialState()
    {
        if (fileNavBox != null)
            fileNavBox.SetActive(true);

        if (fileNavBoxf != null)
            fileNavBoxf.SetActive(false);
    }

    // ���ӻ����ԣ���Scene��ͼ����ʾ�������
    private void OnDrawGizmos()
    {
        if (targetUIObject == null || lineRectTransform == null)
            return;

        // ����������
        Vector3 linePos = GetWorldPosition(lineRectTransform);
        Vector3 targetPos = GetWorldPosition(targetUIObject);

        Gizmos.color = isColliding ? Color.red : Color.green;
        Gizmos.DrawLine(linePos, targetPos);

        // ���Ƽ�����
        if (!useOverlapDetection)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(targetPos, detectionThreshold);
        }
    }
}
