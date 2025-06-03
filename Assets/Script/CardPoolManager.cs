using System.Collections.Generic;
using UnityEngine;
//卡片池系统
public class CardPoolManager : MonoBehaviour
{
    public GameObject cardPrefab;
    private Queue<GameObject> pool = new Queue<GameObject>(); //存储卡片对象池

    public GameObject GetCard()
    {
        if (pool.Count > 0)
        {
            GameObject card = pool.Dequeue();
            card.SetActive(true);
            return card;
        }
        return Instantiate(cardPrefab);
    }

    public void Release(GameObject card)
    {
        card.transform.SetParent(null); //断开父子关系，防止残留
        card.SetActive(false);
        pool.Enqueue(card);
    }
    //一次性回收所有已生成的卡片
    public void ReclaimAllCards(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Transform child = parent.GetChild(i);

            //排除非卡片对象（比如 HeaderCard）
            if (child.gameObject == null || !child.gameObject.activeSelf)
                continue;

            if (child.GetComponent<EventCardController>() != null)
            {
                Release(child.gameObject); //回收到对象池中
            }
        }
    }
}
