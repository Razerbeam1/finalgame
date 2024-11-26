using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Searching
{
    public class OOPEnemyRunner : Character
    {
        private int moveCount = 0; // ตัวแปรนับจำนวนการเดิน

        public void Start()
        {
            GetRemainEnergy();
        }

        public override void Hit()
        {
            mapGenerator.player.Attack(this);
            this.Attack(mapGenerator.player);
        }

        public void Attack(OOPPlayer _player)
        {
            _player.TakeDamage(AttackPoint);
        }

        protected override void CheckDead()
        {
            base.CheckDead();
            if (energy <= 0)
            {
                mapGenerator.enemyrunners[positionX, positionY] = null;
                mapGenerator.mapdata[positionX, positionY] = mapGenerator.empty;
            }
        }

        public virtual void RandomMove()
        {
            moveCount++; // เพิ่มจำนวนการเดินทุกครั้ง

            int toX = positionX;
            int toY = positionY;

            // ทุกๆ การเดินครั้งที่ 1 ให้เดินเข้าหาผู้เล่น
            if (moveCount % 2 == 0)
            {
                MoveTowardsPlayer(ref toX, ref toY);  // เรียกฟังก์ชันเพื่อเดินเข้าหาผู้เล่น
            }
            else
            {
                // การเคลื่อนที่พื้นฐาน (เช่น การเดินแบบสุ่ม)
                int random = Random.Range(0, 4);
                switch (random)
                {
                    case 0: toY += 1; break; // up
                    case 1: toY -= 1; break; // down
                    case 2: toX -= 1; break; // left
                    case 3: toX += 1; break; // right
                }
            }

            // ตรวจสอบว่าตำแหน่งปลายทางว่างหรือไม่
            if (!HasPlacement(toX, toY))
            {
                // อัปเดต mapGenerator
                mapGenerator.mapdata[positionX, positionY] = mapGenerator.empty;
                positionX = toX;
                positionY = toY;
                mapGenerator.mapdata[positionX, positionY] = mapGenerator.enemyrunner;
                transform.position = new Vector3(positionX, positionY, 0);
            }
        }

        // ฟังก์ชันเดินเข้าหาผู้เล่น
        private void MoveTowardsPlayer(ref int toX, ref int toY)
        {
            int playerX = mapGenerator.player.positionX;
            int playerY = mapGenerator.player.positionY;

            // คำนวณทิศทางที่จะเดินเข้าใกล้ผู้เล่น
            int deltaX = playerX - positionX;
            int deltaY = playerY - positionY;

            // เดินในแนวแกนที่ระยะห่างมากกว่า
            if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY))
            {
                toX += (deltaX > 0) ? 1 : -1; // เดินในแนว X
            }
            else
            {
                toY += (deltaY > 0) ? 1 : -1; // เดินในแนว Y
            }

            Debug.Log($"Enemy is moving towards player to ({toX}, {toY}).");
        }
    }
}















