using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Graphic))]
public class RoundedRectAutoAspect : MonoBehaviour
{
    private MaterialPropertyBlock mpb;
    private Graphic graphic;
    private RectTransform rt;

    private void Awake()
    {
        graphic = GetComponent<Graphic>();
        rt = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (graphic.material == null) return;

        float width = rt.rect.width;
        float height = rt.rect.height;

        if (height <= 0) return;

        float aspect = width / height;
        graphic.material.SetFloat("_Aspect", aspect);
    }
}