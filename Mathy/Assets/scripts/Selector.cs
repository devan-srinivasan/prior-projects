using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Selector : NetworkBehaviour
{
    public int choice1, choice2, prevchoice1, prevchoice2;
    [SerializeField] public GameObject s1, s2;
    [SerializeField] public Image[] tiles = new Image[9];
    [SerializeField] public bool p1down, p2down = false;
    [SerializeField] public int c;
    private Color clickedS = new Color32(211, 211, 138, 255);
    private Color original = new Color32(153, 158, 211, 255);

    public void Start()
    {
        choice1 = 1;
        prevchoice1 = 1;
        choice2 = 9;
        prevchoice2 = 9;
    }

    public void changeChoiceSlider1()
    {
        if (p1down && !p2down)
        {
            if (choice2 == prevchoice2 || GameObject.Find("gm").GetComponent<GM>().turnNumber == 1)
            {
                IWannaMoveADude(s1, c, 1);
                p1down = false;
                s1.GetComponent<Image>().color = original;
            }
        }
    }

    public void changeChoiceSlider2()
    {
        if (p2down && !p1down)
        {
            if (choice1 == prevchoice1 || GameObject.Find("gm").GetComponent<GM>().turnNumber == 1)
            {
                IWannaMoveADude(s2, c, 2);
                p2down = false;
                s2.GetComponent<Image>().color = original;
            }
        }
    }

    //request server to move the slider
    [Command (ignoreAuthority = true)]
    public void IWannaMoveADude(GameObject dude, int c, int choice) 
    {
        MoveADude(dude, c, choice);
    }

    //move the slider using ur local x positions
    [ClientRpc]
    public void MoveADude(GameObject dude, int c, int choice) 
    {
        float targetx = tiles[c - 1].GetComponent<RectTransform>().position.x;
        Vector3 pos = dude.GetComponent<RectTransform>().position;
        dude.GetComponent<RectTransform>().position = new Vector3(targetx, pos[1], pos[2]); //set new position
        if (choice == 1)
        {
            choice1 = c;
            NSyncChoice(c, 1);
        }
        else
        {
            choice2 = c;
            NSyncChoice(c, 2);
        }
    }

    public void NSyncChoice(int c, int choiceVar)
    {
        if (choiceVar == 1) { choice1 = c; }
        else { choice2 = c; }
    }

    public void set1() {
        if(p2down) return;
        p1down = !p1down;
        if (p1down) { s1.GetComponent<Image>().color = clickedS; }
        else { s1.GetComponent<Image>().color = original; }
    }
    public void set2() {
        if(p1down) return;
        p2down = !p2down;
        if (p2down) { s2.GetComponent<Image>().color = clickedS; }
        else { s2.GetComponent<Image>().color = original; }
    }
    public void setC(int x) { c = x; }
}