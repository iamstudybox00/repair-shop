using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    Item item;      // 진열된 아이템
    Vector3 itemScale;
    Vector3 itemPosition;

    void Start()
    {
        gameObject.tag = "Table";
        itemScale = new Vector3(0.3f, 0.3f, 0.3f);
        itemPosition = new Vector3(0.0f, 1.0f, 0.0f);
    }

    void Update()
    {
        if (item)
        {
            item.Rotating();
        }
    }

    public virtual void PickItem(Item item, MoveController con = null)
    {
        this.item = item;
        item.transform.parent = transform;
        item.transform.localScale = itemScale;
        item.transform.localPosition = itemPosition;
        item.OnTable(this);
    }

    public void ReleaseItem()
    {
        item = null;
    }

    public Item GetItem()
    {
        return item;
    }

}
