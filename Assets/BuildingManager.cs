using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class BuildingManager : MonoBehaviour {
    [Serializable]
    public class Building {
        public string name;
        public string desc;
        public int cost;
    };

    [SerializeField] Tilemap tileMap;
    public int money = 200;
    public int expensiveMultiplier = 3;
    public static bool canBuild = true;
    [SerializeField] Building[] Buildings;
    [SerializeField] GameObject grid;
    public int selectedIndex = 0;
    [SerializeField] SpriteRenderer buildingShow;
    [SerializeField] TMP_Text expensiveText;
    [SerializeField] TMP_Text moneyText;
    [SerializeField] TMP_Text costText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text descText;

    public static BuildingManager instance;

    private void Awake() {
        if (instance != null) {
            Debug.LogWarning("Multiple BuildingManagers detected, destroying this one");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void Start() {
        canBuild = true;
        for (int i = 0; i < grid.transform.childCount; i++) {
            if (Buildings.Length - 1 < i) Destroy(grid.transform.GetChild(i).gameObject);
        }
        ChangeMoney(0);
    }

    public void UpdateExpensiveSign(bool expensiveLand) {
        //print("testststsetsetse");
        expensiveText.text = (expensiveLand ? "<!> EXPENSIVE LAND (3x) <!>" : "");
    }

    public bool CanBuild(bool expensiveLand) {
        if ((Buildings[selectedIndex].cost * (expensiveLand ? expensiveMultiplier : 1)) > money || !canBuild) return false;

        if (selectedIndex == 0) return false;

        return true;
    }
    public void BuildingSelected(int indexSprite) {
        if (indexSprite == 3) {
            buildingShow.sprite = LevelManager.instance.tileSprites[3];
            selectedIndex = 3;
            nameText.text = Buildings[selectedIndex].name;
            descText.text = Buildings[selectedIndex].desc;
            costText.text = Buildings[selectedIndex].cost + "$";
            buildingShow.gameObject.SetActive(buildingShow.sprite != null);
            return;
        }
        buildingShow.sprite = LevelManager.instance.tileSprites[indexSprite];
        selectedIndex = indexSprite;
        nameText.text = Buildings[indexSprite].name;
        descText.text = Buildings[indexSprite].desc;
        costText.text = Buildings[indexSprite].cost + "$";
        buildingShow.gameObject.SetActive(buildingShow.sprite != null);
    }

    public void Build(bool expensiveLand) {
        ChangeMoney(Buildings[selectedIndex].cost * (expensiveLand ? expensiveMultiplier : 1));
    }

    public void ChangeMoney(int amount) {
        money -= amount;
        moneyText.text = money + "$";
    }

}
