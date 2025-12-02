using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairTable : Table
{
    public override void PickItem(Item item, MoveController con = null)
    {
        base.PickItem(item);
        if (!item.IsRepaired())
        {
            MinigameManager.Get().GameStart(con, item, item.GetGameType());
        }
    }
}
