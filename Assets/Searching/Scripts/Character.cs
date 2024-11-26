using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

namespace Searching
{
    public class Character : Identity
    {
        [Header("Character")]
        public int energy;
        public int AttackPoint;

        protected bool isAlive;
        protected bool isFreeze;

        // Start is called before the first frame update
        protected void GetRemainEnergy()
        {
            Debug.Log(Name + " : " + energy);
        }
        

        public virtual void Move(Vector2 direction)
        {
            if (isFreeze == true)
            {
                GetComponent<SpriteRenderer>().color = Color.white;
                isFreeze = false;
                return;
            }
            int toX = (int)(positionX + direction.x);
            int toY = (int)(positionY + direction.y);
            int fromX = positionX;
            int fromY = positionY;

            if (HasPlacement(toX, toY))
            {
                if (IsDemonWalls(toX, toY))
                {
                    mapGenerator.walls[toX, toY].Hit();
                }
                else if (IsPotion(toX, toY))
                {
                    mapGenerator.potions[toX, toY].Hit();
                    positionX = toX;
                    positionY = toY;
                    transform.position = new Vector3(positionX, positionY, 0);
                }
                else if (IsPotionBonus(toX, toY))
                {
                    mapGenerator.potions[toX, toY].Hit();
                    positionX = toX;
                    positionY = toY;
                    transform.position = new Vector3(positionX, positionY, 0);
                }
                else if (IsExit(toX, toY))
                {
                    mapGenerator.Exit.Hit();
                    positionX = toX;
                    positionY = toY;
                    transform.position = new Vector3(positionX, positionY, 0);
                }
                else if (IsKey(toX, toY))
                {
                    mapGenerator.keys[toX, toY].Hit();
                    positionX = toX;
                    positionY = toY;
                    transform.position = new Vector3(positionX, positionY, 0);
                }
                else if (IsFireStorm(toX, toY))
                {
                    mapGenerator.fireStorms[toX, toY].Hit();
                    positionX = toX;
                    positionY = toY;
                    transform.position = new Vector3(positionX, positionY, 0);
                }
                else if (IsEnemy(toX, toY))
                {
                    mapGenerator.enemies[toX, toY].Hit(); //82
                    positionX = toX;
                    positionY = toY;
                    transform.position = new Vector3(positionX, positionY, 0);
                    
                }
                else if (IsEnemyRunner(toX,toY))
                {
                    mapGenerator.enemyrunners[toX,toY].Hit();
                    positionX = toX;
                    positionY = toY;
                    transform.position = new Vector3(positionX, positionY, 0);
                }
                else if (IsSpawnenemy(toX, toY))
                {
                    mapGenerator.SpawnEnemies[toX,toY].Hit();
                    positionX = toX;
                    positionY = toY;
                    transform.position = new Vector3(positionX, positionY, 0);
                }
                
                //---------------------------------------
                else if (IsWood(toX, toY))  // ตรวจสอบถ้าเป็น Wood
                {
                    if (mapGenerator.woods[toX, toY] != null)
                    {
                        mapGenerator.woods[toX, toY].Hit(); // บรรทัดที่ 87
                        if (this is OOPPlayer player)
                        {
                            player.CollectWood(0);  // ให้ OOPPlayer เก็บ Wood
                        }
                        positionX = toX;
                        positionY = toY;
                        transform.position = new Vector3(positionX, positionY, 0);
                    }
                    else
                    {
                        Debug.LogError("Wood at position (" + toX + ", " + toY + ") is null!");
                    }
                }

                else if (IsScrap(toX, toY))  // ตรวจสอบถ้าเป็น Scrap
                {
                    mapGenerator.scraps [toX, toY].Hit();
                    if (this is OOPPlayer player)
                    {
                        player.CollectScrap(0);  // ให้ OOPPlayer เก็บ Scrap
                    }
                    positionX = toX;
                    positionY = toY;
                    transform.position = new Vector3(positionX, positionY, 0);
                }
            }
            else
            {
                positionX = toX;
                positionY = toY;
                transform.position = new Vector3(positionX, positionY, 0);
                TakeDamage(1);
            }

            if (this is OOPPlayer)
            {
                if (fromX != positionX || fromY != positionY)
                {
                    mapGenerator.mapdata[fromX, fromY] = mapGenerator.empty;
                    mapGenerator.mapdata[positionX, positionY] = mapGenerator.playerBlock;
                    mapGenerator.MoveEnemies();
                    mapGenerator.MoveEnemyRunners();
                }
            }

        }

        public bool IsWood(int x, int y)
        {
            int mapData = mapGenerator.GetMapData(x, y);
            return mapData == mapGenerator.wood;  // แก้ไขเป็น == เพื่อให้เช็คว่าเป็นไม้จริงๆ
        }
        public bool IsScrap(int x, int y)
        {
            int mapData = mapGenerator.GetMapData(x, y);
            return mapData == mapGenerator.scrap;  // แก้ไขเป็น == เพื่อให้เช็คว่าเป็นเศษได้
        }


        // hasPlacement คืนค่า true ถ้ามีการวางอะไรไว้บน map ที่ตำแหน่ง x,y
        public bool HasPlacement(int x, int y)
        {
            int mapData = mapGenerator.GetMapData(x, y);
            return mapData != mapGenerator.empty;
        }
        public bool IsDemonWalls(int x, int y)
        {
            int mapData = mapGenerator.GetMapData(x, y);
            return mapData == mapGenerator.demonWall;
        }
        public bool IsPotion(int x, int y)
        {
            int mapData = mapGenerator.GetMapData(x, y);
            return mapData == mapGenerator.potion;
        }
        public bool IsPotionBonus(int x, int y)
        {
            int mapData = mapGenerator.GetMapData(x, y);
            return mapData == mapGenerator.potion;
        }
        public bool IsFireStorm(int x, int y)
        {
            int mapData = mapGenerator.GetMapData(x, y);
            return mapData == mapGenerator.fireStorm;
        }
        public bool IsKey(int x, int y)
        {
            int mapData = mapGenerator.GetMapData(x, y);
            return mapData == mapGenerator.key;
        }
        public bool IsEnemy(int x, int y)
        {
            int mapData = mapGenerator.GetMapData(x, y);
            return mapData == mapGenerator.enemy;
        }

        public bool IsEnemyRunner(int x, int y)
        {
            int mapData = mapGenerator.GetMapData(x, y);
            return mapData == mapGenerator.enemyrunner;
        }

        public bool IsSpawnenemy(int x, int y)
        {
            int mapData = mapGenerator.GetMapData(x, y);
            return mapData == mapGenerator.spawnenemy;
        }
        public bool IsExit(int x, int y)
        {
            int mapData = mapGenerator.GetMapData(x, y);
            return mapData == mapGenerator.exit;
        }

        public virtual void TakeDamage(int Damage)
        {
            energy -= Damage;
            Debug.Log(Name + " Current Energy : " + energy);
            CheckDead();
        }
        public virtual void TakeDamage(int Damage, bool freeze)
        {
            energy -= Damage;
            isFreeze = freeze;
            GetComponent<SpriteRenderer>().color = Color.blue;
            Debug.Log(Name + " Current Energy : " + energy);
            Debug.Log("you is Freeze");
            CheckDead();
        }


        public void Heal(int healPoint)
        {
            // energy += healPoint;
            // Debug.Log("Current Energy : " + energy);
            // เราสามารถเรียกใช้ฟังก์ชัน Heal โดยกำหนดให้ Bonuse = false ได้ เพื่อที่จะให้ logic ในการ heal อยู่ที่ฟังก์ชัน Heal อันเดียวและไม่ต้องเขียนซ้ำ
            Heal(healPoint, false);
        }

        public void Heal(int healPoint, bool Bonuse)
        {
            energy += healPoint * (Bonuse ? 2 : 1);
            Debug.Log("Current Energy : " + energy);
        }

        protected virtual void CheckDead()
        {
            if (energy <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}