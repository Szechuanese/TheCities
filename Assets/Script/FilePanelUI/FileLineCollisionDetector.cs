using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileLineCollisionDetector : MonoBehaviour
{
    [Header("碰撞目标")]
    public RectTransform targetUIObject;    // 要检测碰撞的UI对象

    [Header("控制对象")]
    public GameObject fileNavBox;              // 默认显示的对象
    public GameObject fileNavBoxf;              // 碰撞时显示的对象

    [Header("检测设置")]
    public float detectionThreshold = 10f;  // 检测阈值（像素距离）
    public bool useOverlapDetection = true; // 是否使用重叠检测

    private RectTransform lineRectTransform;
    private Canvas canvas;
    private bool isColliding = false;
    private bool lastCollisionState = false;

    private void Start()
    {
        lineRectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        // 初始状态：显示ObjectA，隐藏ObjectB
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
            // 方法1：使用RectTransform重叠检测
            currentCollisionState = IsOverlapping(lineRectTransform, targetUIObject);
        }
        else
        {
            // 方法2：使用距离检测
            currentCollisionState = IsWithinDistance();
        }

        // 只在碰撞状态发生变化时执行切换
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

    // 检测两个RectTransform是否重叠
    private bool IsOverlapping(RectTransform rect1, RectTransform rect2)
    {
        // 获取世界坐标下的矩形边界
        Vector3[] rect1Corners = new Vector3[4];
        Vector3[] rect2Corners = new Vector3[4];

        rect1.GetWorldCorners(rect1Corners);
        rect2.GetWorldCorners(rect2Corners);

        // 计算矩形边界
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

        // 检测矩形是否重叠
        return worldRect1.Overlaps(worldRect2);
    }

    // 检测距离是否在阈值内
    private bool IsWithinDistance()
    {
        Vector3 lineWorldPos = GetWorldPosition(lineRectTransform);
        Vector3 targetWorldPos = GetWorldPosition(targetUIObject);

        float distance = Vector3.Distance(lineWorldPos, targetWorldPos);
        return distance <= detectionThreshold;
    }

    // 获取RectTransform的世界坐标位置
    private Vector3 GetWorldPosition(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        // 返回中心点
        return (corners[0] + corners[2]) / 2f;
    }

    // 碰撞开始时调用
    private void OnCollisionEnter()
    {
        Debug.Log("Line碰撞到目标！");

        // 关闭ObjectA，打开ObjectB
        if (fileNavBox != null)
            fileNavBox.SetActive(false);

        if (fileNavBoxf != null)
            fileNavBoxf.SetActive(true);
    }

    // 碰撞结束时调用
    private void OnCollisionExit()
    {
        Debug.Log("Line离开目标！");

        // 关闭ObjectB，打开ObjectA
        if (fileNavBoxf != null)
            fileNavBoxf.SetActive(false);

        if (fileNavBox != null)
            fileNavBox.SetActive(true);
    }

    // 设置初始状态
    private void SetInitialState()
    {
        if (fileNavBox != null)
            fileNavBox.SetActive(true);

        if (fileNavBoxf != null)
            fileNavBoxf.SetActive(false);
    }

    // 可视化调试：在Scene视图中显示检测区域
    private void OnDrawGizmos()
    {
        if (targetUIObject == null || lineRectTransform == null)
            return;

        // 绘制连接线
        Vector3 linePos = GetWorldPosition(lineRectTransform);
        Vector3 targetPos = GetWorldPosition(targetUIObject);

        Gizmos.color = isColliding ? Color.red : Color.green;
        Gizmos.DrawLine(linePos, targetPos);

        // 绘制检测距离
        if (!useOverlapDetection)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(targetPos, detectionThreshold);
        }
    }
}
