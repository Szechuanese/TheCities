using UnityEngine;

public class Rotate : MonoBehaviour
{
    public RectTransform a;
    public float speed= 20f;
    public Vector3 rotateAxis = Vector3.forward;

    private void Awake()
    {
    }
    void Update()
    {
        if (this != null)
        a.localRotation *= Quaternion.Euler(rotateAxis * speed * Time.deltaTime);
    }
}
