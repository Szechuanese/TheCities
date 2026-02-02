using UnityEngine;
using UnityEngine.UI;

public class BurdenSlotButton : MonoBehaviour
{
    public BurdenSlot slot;
    public Button button;

    public BurdenEquipPopup popup;          // 拖入弹窗管理器
    public BurdenSystem burdenSystem;       // 可不拖，自动找

    private void Awake()
    {
        if (button == null) button = GetComponent<Button>();
        if (burdenSystem == null) burdenSystem = FindFirstObjectByType<BurdenSystem>();
    }

    private void OnEnable()
    {
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);
        }
    }

    private void OnClick()
    {
        if (popup == null || burdenSystem == null) return;
        popup.Open(slot, burdenSystem);
    }
}
