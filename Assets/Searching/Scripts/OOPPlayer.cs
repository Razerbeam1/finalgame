using System;
using System.Collections;
using System.Collections.Generic;
using Tree;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace Searching
{
    public class OOPPlayer : Character
    {
        public Inventory inventory;  // เพิ่ม Inventory ในการเก็บไอเทม
        public int woodCount = 0;  // จำนวนไม้ที่เก็บได้
        public int scrapCount = 0;  // จำนวนขยะที่เก็บได้
        public Text woodCountText;  // UI สำหรับแสดงจำนวนไม้
        public Text scrapCountText;
        public Text woodCountText1;  // UI สำหรับแสดงจำนวนไม้
        public Text scrapCountText1;
        
        public static OOPPlayer Instance;
        public GameObject craftingPanel;  // UI Panel สำหรับ Crafting
        public Text redKeyRequirementsText;  // UI Text แสดงรายละเอียดการ Craft "Red Key"
        public Button craftButton;
        public Text newItemRequirementsText; // UI Text แสดงรายละเอียดการ Craft Item ใหม่
        public Button newItemCraftButton; // ปุ่ม Craft Item ใหม่
        public Text Extrafirestorm; // UI Text แสดงรายละเอียดการ Craft Item ใหม่อีกชิ้น
        public Button ExtrafirestormButton; // ปุ่ม Craft ไอเทมใหม่อีกชิ้น

        public void Start()
        {
            PrintInfo();
            GetRemainEnergy();
            craftingPanel.SetActive(false);  // เริ่มต้นซ่อนหน้าต่าง Crafting
            craftButton.onClick.AddListener(CreateRedKey);  // ตั้งค่าการคลิกปุ่ม
            newItemCraftButton.onClick.AddListener(CreateNewItem);  // ตั้งค่าการคลิกปุ่มสำหรับไอเทมใหม่
            ExtrafirestormButton.onClick.AddListener(CreateExtrafirestorm);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Move(Vector2.up);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Move(Vector2.down);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                Move(Vector2.left);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Move(Vector2.right);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                UseFireStorm();
            }
            if (Input.GetKeyDown(KeyCode.T))  // กด T เพื่อเปิด/ปิดหน้า Crafting
            {
                ToggleCraftingPanel();
            }
        }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void ToggleCraftingPanel()
        {
            // สลับการแสดงผลของหน้าต่าง Crafting
            craftingPanel.SetActive(!craftingPanel.activeSelf);

            // อัพเดตข้อความที่แสดงในหน้าต่าง Crafting
            if (craftingPanel.activeSelf)
            {
                UpdateCraftingUI();
            }
        }

        private void UpdateCraftingUI()
        {
            // แสดงจำนวนทรัพยากรที่ผู้เล่นมี
            redKeyRequirementsText.text = "Red Key Requirements: \n" +
                                          "Wood: 3\nScrap: 3\n";
            newItemRequirementsText.text = "Potion Item Requirements: \n" +
                                          "Wood: 1\nScrap: 1\n";
            Extrafirestorm.text = "Extrafirestorm: \n "+"Wood: 2\nScrap: 2\n";
            craftButton.interactable = scrapCount >= 3 && woodCount >= 3;
            newItemCraftButton.interactable = scrapCount >= 1 && woodCount >= 1;
            ExtrafirestormButton.interactable = scrapCount >= 2 && woodCount >= 2;
        }

        // ฟังก์ชันสร้าง Red Key
        public void CreateRedKey()
        {
            if (scrapCount >= 3 && woodCount >= 3)
            {
                scrapCount -= 3;
                woodCount -= 3;
                inventory.AddItem("Red Key");

                UpdateItemUI();  // อัพเดต UI เพื่อแสดงจำนวนที่เหลือ
                UpdateCraftingUI();  // อัพเดต UI ของหน้าต่าง Crafting

                Debug.Log("Red Key Created!");
            }
            else
            {
                Debug.Log("Not enough materials to create Red Key.");
            }
        }

        // ฟังก์ชันสร้างไอเทมใหม่
        public void CreateNewItem()
        {
            if (scrapCount >= 1 && woodCount >= 1)
            {
                scrapCount -= 1;
                woodCount -= 1;
                inventory.AddItem("FireStorm");

                UpdateItemUI();  // อัพเดต UI เพื่อแสดงจำนวนที่เหลือ
                UpdateCraftingUI();  // อัพเดต UI ของหน้าต่าง Crafting

                Debug.Log("FireStorm Created!");
            }
            else
            {
                Debug.Log("Not enough materials to create FireStorm.");
            }
        }

        public void CreateExtrafirestorm()
        {
            if (scrapCount >= 2 && woodCount >= 2)
            {
                scrapCount -= 2;
                woodCount -= 2;
                inventory.AddItem("FireStorm");
                inventory.AddItem("FireStorm");

                UpdateItemUI();  // อัพเดต UI เพื่อแสดงจำนวนที่เหลือ
                UpdateCraftingUI();  // อัพเดต UI ของหน้าต่าง Crafting

                Debug.Log("FireStormExtra Created!");
            }
            else
            {
                Debug.Log("Not enough materials to create FireStorm.");
            }
        }

        public void Attack(OOPEnemy _enemy)
        {
            _enemy.TakeDamage(AttackPoint);
        }

        public void Attack(OOPEnemyRunner _enemyRunner)
        {
            _enemyRunner.TakeDamage(AttackPoint);
        }

        protected override void CheckDead()
        {
            base.CheckDead();
            if (energy <= 0)
            {
                Debug.Log("Player is Dead");
            }
        }

        public void UseFireStorm()
        {
            if (inventory.numberOfItem("FireStorm") > 0)
            {
                inventory.UseItem("FireStorm");
                OOPEnemy[] enemies = SortEnemiesByRemainningEnergy2();
                int count = 3;
                if (count > enemies.Length)
                {
                    count = enemies.Length;
                }
                for (int i = 0; i < count; i++)
                {
                    enemies[i].TakeDamage(10);
                }
            }
            else
            {
                Debug.Log("No FireStorm in inventory");
            }
            if (inventory.numberOfItem("FireStorm") > 0)
            {
                inventory.UseItem("FireStorm");
                OOPEnemyRunner[] enemyRunners = SortEnemyRunnersByRemainningEnergy2();
                int count = 3;
                if (count > enemyRunners.Length)
                {
                    count = enemyRunners.Length;
                }
                for (int i = 0; i < count; i++)
                {
                    enemyRunners[i].TakeDamage(10);
                }
            }
            else
            {
                Debug.Log("No FireStorm in inventory");
            }
        }

        public OOPEnemy[] SortEnemiesByRemainningEnergy2()
        {
            var enemies = mapGenerator.GetEnemies();
            Array.Sort(enemies, (a, b) => a.energy.CompareTo(b.energy));
            return enemies;
        }

        public OOPEnemyRunner[] SortEnemyRunnersByRemainningEnergy2()
        {
            var enemyrunners = mapGenerator.GetEnemyRunners();
            Array.Sort(enemyrunners,(a,b)=> a.energy.CompareTo(b.energy));
            return enemyrunners;  // แก้ไขให้คืนค่าได้
        }

        // เพิ่มฟังก์ชันใหม่เพื่อให้สามารถใช้งาน OOPEnemyRunner
        public void AttackEnemyRunner(OOPEnemyRunner _enemyRunner)
        {
            _enemyRunner.TakeDamage(AttackPoint);
        }

        public void CollectScrap(int scrapAmount, bool isBonues)
        {
            scrapCount += scrapAmount;
            Debug.Log("Collected Scrap! Total: " + scrapCount);
            UpdateItemUI();
        }

        // ฟังก์ชันเก็บ Wood
        public void CollectWood(int woodAmount, bool isBonues)
        {
            woodCount += woodAmount;
            Debug.Log("Collected Wood! Total: " + woodCount);
            UpdateItemUI();
        }

        private void UpdateItemUI()
        {
            if (woodCountText != null)
            {
                woodCountText.text = "Wood: " + woodCount.ToString();  // อัพเดตจำนวนไม้
            }

            if (scrapCountText != null)
            {
                scrapCountText.text = "Scrap: " + scrapCount.ToString();  // อัพเดตจำนวนขยะ
            }
            if (woodCountText1 != null)
            {
                woodCountText1.text = "Wood: " + woodCount.ToString();  // อัพเดตจำนวนไม้
            }

            if (scrapCountText1 != null)
            {
                scrapCountText1.text = "Scrap: " + scrapCount.ToString();  // อัพเดตจำนวนขยะ
            }
        }

        // ฟังก์ชันเก็บ Wood และ Scrap ที่ไม่มีเงื่อนไขโบนัส
        public void CollectScrap(int scrapAmount)
        {
            scrapCount += scrapAmount;
            Debug.Log("Collected Scrap! Total: " + scrapCount);
            UpdateItemUI();
        }

        public void CollectWood(int woodAmount)
        {
            woodCount += woodAmount;
            Debug.Log("Collected Wood! Total: " + woodCount);
            UpdateItemUI();
        }
    }
}

