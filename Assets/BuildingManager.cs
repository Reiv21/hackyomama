using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    [SerializeField] Building[] Buildings;
    [SerializeField] GameObject grid;
    int selectedIndex = 0;
    [SerializeField] Image buildingShow;
    [SerializeField] TMP_Text costText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text descText;

    void Start()
    {
        for (int i = 0; i < grid.transform.childCount; i++)
        {
            if (Buildings.Length-1 < i) Destroy( grid.transform.GetChild(i).gameObject );
        }
    }

    public void BuildingSelected(int index)
    {
        buildingShow.sprite = Buildings[index].icon;
        nameText.text = Buildings[index].name;
        descText.text = Buildings[index].desc;
        costText.text = Buildings[index].cost + "$";
        buildingShow.gameObject.SetActive(buildingShow.sprite != null);
    }
    
    
}
