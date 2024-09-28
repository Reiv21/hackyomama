using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class BuildingManager : MonoBehaviour
{
    [Serializable]
    public class Building
    {
        public string name;
        public string desc;
        public int cost;
        public Sprite icon;
    };

    [SerializeField] Tilemap tileMap;
    int money = 1500;
    [SerializeField] Building[] Buildings;
    [SerializeField] GameObject grid;
    public int selectedIndex = 0;
    [SerializeField] SpriteRenderer buildingShow;
    [SerializeField] TMP_Text moneyText;
    [SerializeField] TMP_Text costText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text descText;

    void Start()
    {
        for (int i = 0; i < grid.transform.childCount; i++)
        {
            if (Buildings.Length-1 < i) Destroy( grid.transform.GetChild(i).gameObject );
        }
        ChangeMoney(0);
    }

    public bool CanBuild()
    {
        if (Buildings[selectedIndex].cost > money) return false;

        if (selectedIndex == 0) return false;

        return true;
    }
    public void BuildingSelected(int index)
    {
        buildingShow.sprite = Buildings[index].icon;
        selectedIndex = index;
        nameText.text = Buildings[index].name;
        descText.text = Buildings[index].desc;
        costText.text = Buildings[index].cost + "$";
        buildingShow.gameObject.SetActive(buildingShow.sprite != null);
    }

    public void Build()
    {
        ChangeMoney(Buildings[selectedIndex].cost);
    }

    public void ChangeMoney(int amount)
    {
        money -= amount;
        moneyText.text = money + "$";
    }
    
}
