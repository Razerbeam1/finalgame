using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace Searching
{
    public class OOPMapGenerator : MonoBehaviour
    {
        [Header("Set MapGenerator")]
        public int X;
        public int Y;

        public Transform itemPotionParent1
        {
            get => itemPotionParent;
            set => itemPotionParent = value;
        }
        [Header("Set Player")]
        public OOPPlayer player;
        public Vector2Int playerStartPos;

        [Header("Set Exit")]
        public OOPExit Exit;
        
        [Header("Set Enemy")]
        private int currentEnemyCount = 0; // จำนวนศัตรูที่มีอยู่ตอนนี้
        private bool isEnemyAlive = false; // สถานะว่าศัตรูมีชีวิตอยู่
        private OOPEnemy currentEnemy; // ตัวแปรเก็บศัตรูที่เกิดล่าสุด
        int maxEnemies = 3; // จำกัดจำนวนศัตรูรอบ spawn
        int spawnedCount = 0; // ตัวนับจำนวนศัตรูที่เกิด
        int radius = 1; // รัศมีการเกิดศัตรู
        [Header("Set Prefab")]
        public GameObject[] floorsPrefab;
        public GameObject[] wallsPrefab;
        public GameObject[] demonWallsPrefab;
        public GameObject[] itemsPrefab;
        public GameObject[] keysPrefab;
        public GameObject[] enemiesPrefab;
        public GameObject[] enemiesrunnerPrefab;
        public GameObject[] fireStormPrefab;
        public GameObject[] fireStormExtraPrefab;
        public GameObject[] woodPrefab;
        public GameObject[] scrapPrefab;
        public GameObject[] spawnEnemyPrefab;

        [Header("Set Transform")]
        public Transform floorParent;
        public Transform wallParent;
        public Transform itemPotionParent;
        public Transform enemyParent;
        public Transform enemyrunnerParent;
        public Transform WoodParent;
        public Transform ScrapParent;
        public Transform SpawnEnemyParent;

        [Header("Set object Count")]
        public int obsatcleCount;
        public int itemPotionCount;
        public int itemKeyCount;
        public int itemFireStormCount;
        public int enemyCount;
        public int enemyrunnerCount;
        public int woodCount; 
        public int scrapCount;
        public int SpawnEnemyCount;

        public int[,] mapdata;

        public OOPWall[,] walls;
        public OOPItemPotion[,] potions;
        public OOPFireStormItem[,] fireStorms;
        public OOPFireStormItem[,] firestormExters;
        public OOPItemKey[,] keys;
        public OOPEnemy[,] enemies;
        public OOPEnemyRunner[,] enemyrunners;
        public OOPWood[,] woods;
        public OOPScrap[,] scraps;
        public OOPSpawnEnemy[,] SpawnEnemies;

        // block types ...
        [Header("Block Types")]
        public int playerBlock = 99;
        public int empty = 0;
        public int demonWall = 1;
        public int potion = 2;
        public int bonuesPotion = 3;
        public int exit = 4;
        public int key = 5;
        public int enemy = 6;
        public int fireStorm = 7;
        public int wood = 8;
        public int scrap = 9;
        public int enemyrunner = 10;
        public int spawnenemy = 11;

        // Start is called before the first frame update
        void Start()
        {
            mapdata = new int[X, Y];
            for (int x = -1; x < X + 1; x++)
            {
                for (int y = -1; y < Y + 1; y++)
                {
                    if (x == -1 || x == X || y == -1 || y == Y)
                    {
                        int r = Random.Range(0, wallsPrefab.Length);
                        GameObject obj = Instantiate(wallsPrefab[r], new Vector3(x, y, 0), Quaternion.identity);
                        obj.transform.parent = wallParent;
                        obj.name = "Wall_" + x + ", " + y;
                    }
                    else
                    {
                        int r = Random.Range(0, floorsPrefab.Length);
                        GameObject obj = Instantiate(floorsPrefab[r], new Vector3(x, y, 1), Quaternion.identity);
                        obj.transform.parent = floorParent;
                        obj.name = "floor_" + x + ", " + y;
                    }
                }
            }

            player.mapGenerator = this;
            player.positionX = playerStartPos.x;
            player.positionY = playerStartPos.y;
            player.transform.position = new Vector3(playerStartPos.x, playerStartPos.y, -0.1f);
            mapdata[playerStartPos.x, playerStartPos.y] = playerBlock;

            walls = new OOPWall[X, Y];
            var count = 0;
            while (count < obsatcleCount)
            {
                int x = Random.Range(0, X);
                int y = Random.Range(0, Y);
                if (mapdata[x, y] == 0)
                {
                    PlaceDemonWall(x, y);
                    count++;
                }
            }

            potions = new OOPItemPotion[X, Y];
            count = 0;
            while (count < itemPotionCount)
            {
                int x = Random.Range(0, X);
                int y = Random.Range(0, Y);
                if (mapdata[x, y] == empty)
                {
                    PlaceItem(x, y);
                    count++;
                }
            }

            /*keys = new OOPItemKey[X, Y];
            count = 0;
            while (count < itemKeyCount)
            {
                int x = Random.Range(0, X);
                int y = Random.Range(0, Y);
                if (mapdata[x, y] == empty)
                {
                    PlaceKey(x, y);
                    count++;
                }
            }*/

            enemies = new OOPEnemy[X, Y];
            count = 0;
            while (count < enemyCount)
            {
                int x = Random.Range(0, X);
                int y = Random.Range(0, Y);
                if (mapdata[x, y] == empty)
                {
                    PlaceEnemy(x, y);
                    count++;
                }
            }

            enemyrunners = new OOPEnemyRunner[X, Y];
            count = 0;
            while (count < enemyrunnerCount)
            {
                int x = Random.Range(0, X);
                int y = Random.Range(0, Y);
                if (mapdata[x, y] == empty)
                {
                    PlaceEnemyRunner(x, y);
                    count++;
                }
            }

            SpawnEnemies = new OOPSpawnEnemy[X, Y];
            count = 0;
            while (count < SpawnEnemyCount)
            {
                int x = Random.Range(0, X);
                int y = Random.Range(0, Y);
                if (mapdata[x,y] == empty)
                {
                    PlaceSpawnEnemy(x,y);
                    count++;
                }
            }
            
            fireStorms = new OOPFireStormItem[X, Y];
            count = 0;
            while (count < itemFireStormCount)
            {
                int x = Random.Range(0, X);
                int y = Random.Range(0, Y);
                if (mapdata[x, y] == empty)
                {
                    PlaceFireStorm(x, y);
                    count++;
                }
            }
            woods = new OOPWood[X, Y];
            count = 0;
            while (count < woodCount)
            {
                int x = Random.Range(0, X);
                int y = Random.Range(0, Y);
                if (mapdata[x, y] == empty)
                {
                    PlaceWood(x, y);
                    count++;
                }
            }
            scraps = new OOPScrap[X, Y];
            count = 0;
            while (count < scrapCount)
            {
                int x = Random.Range(0, X);
                int y = Random.Range(0, Y);
                if (mapdata[x, y] == empty)
                {
                    PlaceScrap(x, y);
                    count++;
                }
            }
            mapdata[X - 1, Y - 1] = exit;
            Exit.transform.position = new Vector3(X - 1, Y - 1, 0);
        }
        

        

        public int GetMapData(float x, float y)
        {
            if (x >= X || x < 0 || y >= Y || y < 0) return -1;
            return mapdata[(int)x, (int)y];
        }

        public void PlaceItem(int x, int y)
        {
            int r = Random.Range(0, itemsPrefab.Length);
            GameObject obj = Instantiate(itemsPrefab[r], new Vector3(x, y, 0), Quaternion.identity);
            obj.transform.parent = itemPotionParent;
            mapdata[x, y] = potion;
            potions[x, y] = obj.GetComponent<OOPItemPotion>();
            potions[x, y].positionX = x;
            potions[x, y].positionY = y;
            potions[x, y].mapGenerator = this;
            obj.name = $"Item_{potions[x, y].Name} {x}, {y}";
        }

        /*public void PlaceKey(int x, int y)
        {
            int r = Random.Range(0, keysPrefab.Length);
            GameObject obj = Instantiate(keysPrefab[r], new Vector3(x, y, 0), Quaternion.identity);
            obj.transform.parent = itemPotionParent;
            mapdata[x, y] = key;
            keys[x, y] = obj.GetComponent<OOPItemKey>();
            keys[x, y].positionX = x;
            keys[x, y].positionY = y;
            keys[x, y].mapGenerator = this;
            obj.name = $"Item_{keys[x, y].Name} {x}, {y}";
        }*/
        
        public void PlaceSpawnEnemy(int x, int y)
        {
            int r = Random.Range(0, spawnEnemyPrefab.Length);
            GameObject obj = Instantiate(spawnEnemyPrefab[r], new Vector3(x, y, 0), Quaternion.identity);
            obj.transform.parent = SpawnEnemyParent;
            mapdata[x, y] = spawnenemy;
            SpawnEnemies[x, y] = obj.GetComponent<OOPSpawnEnemy>();
            SpawnEnemies[x, y].positionX = x;
            SpawnEnemies[x, y].positionY = y;
            SpawnEnemies[x, y].mapGenerator = this;
            obj.name = $"SpawnEnemy_{SpawnEnemies[x, y].Name} {x}, {y}";

            // เริ่มการเกิดศัตรูรอบ ๆ spawn
            StartCoroutine(SpawnEnemiesPeriodically(x, y));
        }


        private IEnumerator SpawnEnemiesPeriodically(int spawnX, int spawnY)
        {

            // Loop เพื่อสร้างศัตรูทีละตัว
            while (spawnedCount < maxEnemies)
            {
                for (int dx = -radius; dx <= radius; dx++)
                {
                    for (int dy = -radius; dy <= radius; dy++)
                    {
                        if (dx == 0 && dy == 0) continue; // ข้ามตำแหน่ง spawn center
                        int x = spawnX + dx;
                        int y = spawnY + dy;

                        // ตรวจสอบว่าตำแหน่งว่างและอยู่ในขอบเขต
                        if (x >= 0 && x < X && y >= 0 && y < Y && mapdata[x, y] == empty)
                        {
                            PlaceEnemy(x, y); // สร้างศัตรู
                            spawnedCount++; // เพิ่มตัวนับ

                            // หยุดรอ 5 วินาทีก่อนเกิดตัวถัดไป
                            yield return new WaitForSeconds(5f);
                        }

                        if (spawnedCount >= maxEnemies) 
                            yield break; // ออกจาก Coroutine หากครบจำนวน
                        
                    }
                }
            }
        }


        public void PlaceEnemy(int x, int y)
        {
            int r = Random.Range(0, enemiesPrefab.Length);
            GameObject obj = Instantiate(enemiesPrefab[r], new Vector3(x, y, 0), Quaternion.identity);
            obj.transform.parent = enemyParent;
            mapdata[x, y] = enemy;
            enemies[x, y] = obj.GetComponent<OOPEnemy>();
            enemies[x, y].positionX = x;
            enemies[x, y].positionY = y;
            enemies[x, y].mapGenerator = this;
            obj.name = $"Enemy_{enemies[x, y].Name} {x}, {y}";
        }

        public void PlaceEnemyRunner(int x, int y)
        {
            int r = Random.Range(0, enemiesrunnerPrefab.Length);
            GameObject obj = Instantiate(enemiesrunnerPrefab[r], new Vector3(x, y, 0), Quaternion.identity);
            obj.transform.parent = itemPotionParent;
            mapdata[x, y] = enemyrunner;
            enemyrunners[x, y] = obj.GetComponent<OOPEnemyRunner>();
            enemyrunners[x, y].positionX = x;
            enemyrunners[x, y].positionY = y;
            enemyrunners[x, y].mapGenerator = this;
            obj.name = $"Enemy_{enemyrunners[x, y].Name} {x},{y}";
        }


        public void PlaceDemonWall(int x, int y)
        {
            int r = Random.Range(0, demonWallsPrefab.Length);
            GameObject obj = Instantiate(demonWallsPrefab[r], new Vector3(x, y, 0), Quaternion.identity);
            obj.transform.parent = wallParent;
            mapdata[x, y] = demonWall;
            walls[x, y] = obj.GetComponent<OOPWall>();
            walls[x, y].positionX = x;
            walls[x, y].positionY = y;
            walls[x, y].mapGenerator = this;
            obj.name = $"DemonWall_{walls[x, y].Name} {x}, {y}";
        }

        public void PlaceFireStorm(int x, int y)
        {
            int r = Random.Range(0, fireStormPrefab.Length);
            GameObject obj = Instantiate(fireStormPrefab[r], new Vector3(x, y, 0), Quaternion.identity);
            obj.transform.parent = wallParent;
            mapdata[x, y] = fireStorm;
            fireStorms[x, y] = obj.GetComponent<OOPFireStormItem>();
            fireStorms[x, y].positionX = x;
            fireStorms[x, y].positionY = y;
            fireStorms[x, y].mapGenerator = this;
            obj.name = $"FireStorm_{fireStorms[x, y].Name} {x}, {y}";
        }
        
        public void PlaceWood(int x, int y)
        {
            int r = Random.Range(0, woodPrefab.Length);
            GameObject obj = Instantiate(woodPrefab[r], new Vector3(x, y, 0), Quaternion.identity);
            obj.transform.parent = WoodParent;
            mapdata[x, y] = wood;
            woods[x, y] = obj.GetComponent<OOPWood>();
            woods[x, y].positionX = x;
            woods[x, y].positionY = y; // แก้ไขให้เป็น positionY แทน
            woods[x, y].mapGenerator = this;
            obj.name = $"Wood_{woods[x, y].Name} {x}, {y}";
        }


        public void PlaceScrap(int x, int y)
        {
            int r = Random.Range(0, scrapPrefab.Length);
            GameObject obj = Instantiate(scrapPrefab[r], new Vector3(x, y, 0), Quaternion.identity);
            obj.transform.parent = ScrapParent;
            mapdata[x, y] = scrap;
            scraps[x, y] = obj.GetComponent<OOPScrap>();
            scraps[x, y].positionX = x;
            scraps[x, y].positionY = y;
            scraps[x, y].mapGenerator = this;
            obj.name = $"Scrap_{scraps[x, y].Name} {x}, {y}";
        }


        public OOPEnemy[] GetEnemies()
        {
            List<OOPEnemy> list = new List<OOPEnemy>();
            foreach (var enemy in enemies)
            {
                if (enemy != null)
                {
                    list.Add(enemy);
                }
            }
            return list.ToArray();
        }

        public OOPEnemyRunner[] GetEnemyRunners()
        {
            List<OOPEnemyRunner> list = new List<OOPEnemyRunner>();
            foreach (var enemyrunner in enemyrunners)
            {
                    if (enemyrunner != null)
                    {
                        list.Add(enemyrunner);
                    }
            }

            return list.ToArray();
        }

        public OOPSpawnEnemy[] GetSpawnEnemies()
        {
            List<OOPSpawnEnemy> list = new List<OOPSpawnEnemy>();
            foreach (var spawnenemy in SpawnEnemies)
            {
                if (spawnenemy != null)
                {
                    list.Add(spawnenemy);
                }
            }
            return list.ToArray();
        }


        public void MoveEnemies()
        {
            List<OOPEnemy> list = new List<OOPEnemy>();
            foreach (var enemy in enemies)
            {
                if (enemy != null)
                {
                    list.Add(enemy);
                }
            }
            foreach (var enemy in list)
            {
                enemy.RandomMove();
            }
        }
        public void MoveEnemyRunners()
        {
            List<OOPEnemyRunner> list = new List<OOPEnemyRunner>();
            foreach (var enemyrunner in enemyrunners)
            {
                if (enemyrunner != null)
                {
                    list.Add(enemyrunner);
                }
            }

            foreach (var enemyrunner in list)
            {
                enemyrunner.RandomMove();
            }
        }
    }
}