using System.Collections;
using System.Collections.Generic;
using Searching;
using UnityEngine;

namespace Searching
{
    public class OOPWood : Identity
    {
        public int wood = 3; // จำนวนไม้ที่เก็บได้

        public override void Hit()
        {
            // ตรวจสอบว่า mapGenerator.player เป็น OOPPlayer
            if (mapGenerator.player is OOPPlayer player)
            {
                // เรียกฟังก์ชัน CollectWood บน OOPPlayer
                player.CollectWood(wood);
                Debug.Log("Wood collected by player!");
                Destroy(gameObject); // ลบไม้หลังจากเก็บแล้ว
            }
        }
    }
}





