using UnityEngine;

public class FileNavCollision : MonoBehaviour
{
    [Header("要被隐藏的对象（ScrollView 中）")]
    public GameObject fileNavBox;

    [Header("fileNavBox 被隐藏时显示的替代对象（可选）")]
    public GameObject fileNavBoxF;

    private Collider2D navCollider;       // 当前物体的 Collider
    private Collider2D targetCollider;    // fileNavBox 的 Collider
    private bool isFloating = false;      // 当前状态标记

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
        Debug.Log("进入浮动状态：fileNavBox 隐藏，fileNavBoxF 显示");
    }

    private void fileNavActive()
    {
        fileNavBox.SetActive(true);
        fileNavBoxF.SetActive(false);
        Debug.Log("离开浮动状态：fileNavBox 显示，fileNavBoxF 隐藏");
    }
}