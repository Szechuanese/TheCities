using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitManager : MonoBehaviour
{
    public PortraitsIcon portraitIconsDatabase;
    public Transform PortraitBroad;
    public GameObject PortraitPrefab;

    private Dictionary<string, PortraitPrefabController> portraitIcons = new Dictionary<string, PortraitPrefabController>();
    private string currentPortraitId;

    public GameObject portraitPanel;
    //批量修改头像
    public List<Image> targetImages = new List<Image>();
    public void Start()
    {
        if(portraitIconsDatabase!= null)
        {
            portraitIconsDatabase.Initialize();
        }
        GeneratePortraitPrefab();
    }
    public void GeneratePortraitPrefab()
    {
        //清空旧的
        foreach (Transform child in PortraitBroad)
        {
            Destroy(child.gameObject);
        }
        portraitIcons.Clear();

        //生成头像框
        foreach (var entry in portraitIconsDatabase.portraitIcons)
        {
            GameObject portraitButton = Instantiate(PortraitPrefab, PortraitBroad);
            PortraitPrefabController controller = portraitButton.GetComponent<PortraitPrefabController>();


            Sprite portraitIcon = portraitIconsDatabase != null
                    ? portraitIconsDatabase.GetPortraitIcon(entry.portraitId)
                    : null;
            //设置图标和ID
            controller.GetPortraitid(entry.portraitId,portraitIcon);
            portraitIcons[entry.portraitId] = controller;

            //按钮绑定点击事件
            Button btn = portraitButton.GetComponent<Button>();
            if (btn != null)
            {
                string id = entry.portraitId; // 避免闭包问题
                btn.onClick.AddListener(() => SelectPortrait(id));
            }
        }
    }

    //选择头像方法
    public void SelectPortrait(string id)
    {
        
        currentPortraitId = id;

        var sprite = portraitIconsDatabase != null
        ? portraitIconsDatabase.GetPortraitIcon(id)
        : null;

        if (targetImages != null)
        {
            foreach (var img in targetImages)
            {
                if (img == null) continue;
                img.sprite = sprite;
                img.type = Image.Type.Simple;     //防止被拉伸
                img.preserveAspect = true;        //保持比例
                img.enabled = true;
            }
        }

        if(portraitPanel != null)
        {
            portraitPanel.SetActive(false);

        }
        else
        {
            portraitPanel.SetActive(false);
        }
    }
}
