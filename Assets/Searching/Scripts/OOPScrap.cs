using System.Collections;
using System.Collections.Generic;
using Searching;
using UnityEngine;

public class OOPScrap : Identity
{
    public int scrap = 3; // จำนวนไม้ที่เก็บได้

    public override void Hit()
    {
        // ตรวจสอบว่า mapGenerator.player เป็น OOPPlayer
        if (mapGenerator.player is OOPPlayer player)
        {
            // เรียกฟังก์ชัน CollectWood บน OOPPlayer
            player.CollectScrap(scrap);
            Debug.Log("Scrap collected by player!");
            Destroy(gameObject); // ลบไม้หลังจากเก็บแล้ว
        }
    }
}
    





