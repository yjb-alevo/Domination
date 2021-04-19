﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    [HideInInspector]
    public bool isGuiOpen;

    public List<string> buttonTags = new List<string>();

    private GameObject ui;
    private List<Button> buttons = new List<Button>();
    private Node center;

    private UnitHandler unitHandler;
    private Grid grid;

    private float buttonPadding = 10;

    void Awake()
    {
        this.ui = GameObject.FindGameObjectWithTag("UI");

        GameObject unitHandlerObj = GameObject.FindGameObjectWithTag("UnitHandler");
        this.unitHandler = unitHandlerObj.GetComponent<UnitHandler>();
        this.grid = unitHandlerObj.GetComponent<Grid>();

        foreach (Button child in this.ui.GetComponentsInChildren<Button>())
        {
            if (buttonTags.Contains(child.gameObject.name)) this.buttons.Add(child);
        }
        //this.unitSpawnPoint = transform.position - new Vector3(transform.localScale.x, 0, transform.localScale.z);
    }

    public void showGui()
    {
        for (int i = 0; i < this.buttons.Count; i++)
        {
            Button button = this.buttons[i];

            RectTransform rectTransform = button.gameObject.GetComponent<RectTransform>();

            float buttonSizeX = rectTransform.rect.width * rectTransform.localScale.x;
            float buttonSizeY = rectTransform.rect.height * rectTransform.localScale.y;

            rectTransform.anchoredPosition = new Vector2(Screen.width / 2 - buttonSizeX / 2 - (i * buttonSizeX + buttonPadding), -1 * (Screen.height / 2 - buttonSizeY / 2 - buttonPadding));

            ActionButton actionButton = button.gameObject.GetComponent<ActionButton>();
            actionButton.building = this;
            actionButton.action = button.gameObject.name;

            button.gameObject.SetActive(true);
        }
        this.isGuiOpen = true;
    }

    public void ExecuteAction(string action)
    {
        switch (action)
        {
            case "SpawnLongbowman":
                if (center == null)
                {
                    this.center = this.grid.NodeFromWorldPoint(transform.position);
                }
                unitHandler.CreateUnits(unitHandler.longbowman, 1, 1, this.grid.FindNearestAvailableNode2(center).worldPos);
                break;
        }
    }

    public void hideGui()
    {
        foreach (Button button in this.buttons) button.gameObject.SetActive(false);
        this.isGuiOpen = false;
    }
}
