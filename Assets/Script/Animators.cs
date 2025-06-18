using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
//StoryCard生成动画代码
public static class Animators
{
    #region 卡牌进入动画
    private static int cardCounter = 0; //用于Type3动画的顺序延迟

    // 自定义AnimationCurve，用于加速感分布
    private static AnimationCurve spawnCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    /// <summary>
    /// 播放卡片进入动画。
    /// </summary>
    /// <param name="card">目标卡片GameObject（需有RectTransform与CanvasGroup）</param>
    /// <param name="parent">父容器Transform（如storyBroad或contentParent）</param>
    /// <param name="type">动画类型：1=向上滑入，2=横向滑入，3=缩放弹入</param>
    public static void cardEntrancePlay(GameObject card, Transform parent, int type = 3)
    {
        if (card == null || parent == null) return;

        RectTransform rect = card.GetComponent<RectTransform>();
        CanvasGroup group = card.GetComponent<CanvasGroup>();
        if (group == null) group = card.AddComponent<CanvasGroup>();

        group.alpha = 0;
        card.transform.SetParent(parent, false);
        card.transform.localScale = Vector3.one;

        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        LayoutRebuilder.ForceRebuildLayoutImmediate(parent.GetComponent<RectTransform>());

        if (rect == null) return;

        Vector2 target = rect.anchoredPosition;

        switch (type)
        {
            case 1:
                Vector2 offsetPos1 = target + new Vector2(0, -200f);
                rect.anchoredPosition = offsetPos1;
                rect.DOAnchorPos(target, 0.4f).SetEase(Ease.OutCubic);
                group.DOFade(1, 0.4f);
                break;

            case 2:
                Vector2 offsetPos2 = target + new Vector2(-600f, 0);
                rect.anchoredPosition = offsetPos2;
                rect.DOAnchorPos(target, 0.45f).SetEase(Ease.OutExpo);
                group.DOFade(1, 0.35f);
                break;

            case 3: // type 3：缩放弹入 + 顺序延迟
            default:
                rect.anchoredPosition = target; // 先确保卡片位于它该在的位置（避免 layout 冲突）
                card.transform.localScale = Vector3.zero; // 初始缩放为 0：从无到有

                //计算当前卡片的“动画延迟时间”

                // 10张一循环，重新由慢到快依次生成卡片
                float normalized = (cardCounter % 10) / 10f;


                // curvedDelay 使用 AnimationCurve 做非线性加速控制
                // spawnCurve.Evaluate(0) = 0（第一张卡），Evaluate(1) = 1（第10张及以后）
                // 整体延迟范围是 0s ~ 0.5s
                float curvedDelay = spawnCurve.Evaluate(normalized) * 0.5f;

                //主动画：从0放大到1（弹入）
                card.transform
                    .DOScale(Vector3.one, 0.3f)           // 0 → 1 缩放，持续 0.3s
                    .SetEase(Ease.OutBack)               // 弹性曲线，末尾微弹
                    .SetDelay(curvedDelay)               // 设置延迟（由cardCounter决定）
                    .OnComplete(() =>                    // 主动画完成后：
                    {
                        //次动画：轻微弹跳（1.0 → 1.05 → 1.0）
                        card.transform
                            .DOScale(Vector3.one * 1.05f, 0.1f)
                            .SetEase(Ease.OutSine)
                            .SetLoops(2, LoopType.Yoyo); // 来回一次
                    });

                //同步淡入透明度
                group
                    .DOFade(1, 0.25f)
                    .SetDelay(curvedDelay); // 与缩放动画同步延迟

                cardCounter++; // 增加卡片索引计数
                break;
        }
    }

    /// <summary>
    /// 重置卡片动画计数（在新一批动画前调用）
    /// </summary>
    public static void ResetCounter()
    {
        cardCounter = 0;
    }
    #endregion
}