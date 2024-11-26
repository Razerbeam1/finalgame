using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Searching
{

    public class OOPEnemy : Character
    {
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
                mapGenerator.enemies[positionX, positionY] = null;
                mapGenerator.mapdata[positionX, positionY] = mapGenerator.empty;

                // Respawn the enemy near its spawn point
                RespawnEnemy();
            }
        }
        private void RespawnEnemy()
        {
            // Find the corresponding SpawnEnemy location for this enemy (you may want to adjust this logic)
            for (int x = 0; x < mapGenerator.X; x++)
            {
                for (int y = 0; y < mapGenerator.Y; y++)
                {
                    if (mapGenerator.mapdata[x, y] == mapGenerator.spawnenemy)
                    {
                        // Spawn the enemy around this point (you can customize the radius here)
                        mapGenerator.PlaceEnemy(x, y);
                        return;
                    }
                }
            }
        }

        public virtual void RandomMove()
        {
            // การเคลื่อนที่พื้นฐาน (เช่น การเดินแบบสุ่ม)
            int toX = positionX;
            int toY = positionY;
            int random = Random.Range(0, 4);
            switch (random)
            {
                case 0:
                    toY += 1; // up
                    break;
                case 1:
                    toY -= 1; // down
                    break;
                case 2:
                    toX -= 1; // left
                    break;
                case 3:
                    toX += 1; // right
                    break;
            }

            if (!HasPlacement(toX, toY))
            {
                mapGenerator.mapdata[positionX, positionY] = mapGenerator.empty;
                positionX = toX;
                positionY = toY;
                mapGenerator.mapdata[positionX, positionY] = mapGenerator.enemy;
                transform.position = new Vector3(positionX, positionY, 0);
            }
        }
    }
}