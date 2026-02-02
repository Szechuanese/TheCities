using UnityEngine;
//这个脚本用于顶部钟指针的旋转，以后我给这个世界添加时间系统。
public class ClockRotate : MonoBehaviour
{
    [Header("旋转速度（度/秒）")]
    public float rotateSpeed = 1f;

    [Header("旋转轴向")]
    public Vector3 rotateAxis = Vector3.forward;

    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogWarning("RotateImage：找不到 RectTransform 组件！");
        }
    }

    void Update()
    {
        if (rectTransform != null)
        {
            //Rotate(围绕的轴，速度*时间);
            rectTransform.Rotate(rotateAxis, rotateSpeed * Time.deltaTime);
        }
    }
}
