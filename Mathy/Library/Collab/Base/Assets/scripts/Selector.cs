using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Selector : NetworkBehaviour
{
    public int choice1, choice2, prevchoice1, prevchoice2;
    [SerializeField] private GameObject s1, s2;
    [SerializeField] private Image[] tiles = new Image[9];
    [SerializeField] private float ADJx;
    [SerializeField] private bool p1down, p2down = false;
    [SerializeField] private float startx, celldim;


    // Start is called before the first frame update
    void Start()
    {
        ADJx = Screen.width / 2f; //should be screen width / 2
        choice1 = 1;
        prevchoice1 = 1;
        choice2 = 9;
        prevchoice2 = 9;
        celldim = tiles[0].GetComponent<RectTransform>().rect.width;
        startx = tiles[0].GetComponent<RectTransform>().localPosition.x - celldim / 2;
    }

    // Update is called once per frame
    
    void Update()
    {
        changeChoiceSlider1();
        changeChoiceSlider2();
    }

    private int getNumFromX(float x)
    {
        if (x < startx + celldim) { return 1; }
        else if (x > startx + celldim && x < startx + 2 * celldim) { return 2; }
        else if (x > startx + 2 * celldim && x < startx + 3 * celldim) { return 3; }
        else if (x > startx + 3 * celldim && x < startx + 4 * celldim) { return 4; }
        else if (x > startx + 4 * celldim && x < startx + 5 * celldim) { return 5; }
        else if (x > startx + 5 * celldim && x < startx + 6 * celldim) { return 6; }
        else if (x > startx + 6 * celldim && x < startx + 7 * celldim) { return 7; }
        else if (x > startx + 7 * celldim && x < startx + 8 * celldim) { return 8; }
        else if (x > startx + 8 * celldim) { return 9; }
        return 0; //should never happen
    }

    // move a slider
    public void changeChoiceSlider1()
    {
        if (p1down == true)
        {
            if (choice2 == prevchoice2)
            {
                float x = Input.mousePosition.x - ADJx;

                // figure out which one they chose
                int c = getNumFromX(x);


                
                float targetx = tiles[c - 1].GetComponent<RectTransform>().position.x;
                MoveADude(s1, targetx, c, 1);
                
            }

        }

    }

    [Command (ignoreAuthority = true)]
    public void MoveADude(GameObject dude, float targetX, int c, int choice)
    {
        Vector3 pos = dude.GetComponent<RectTransform>().position;
        dude.GetComponent<RectTransform>().position = new Vector3(targetX, pos[1], 0); //set new position
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

    [ClientRpc]
    public void NSyncChoice(int c, int choiceVar)
    {
        if (choiceVar == 1) { choice1 = c; }
        else { choice2 = c; }
    }
    public void changeChoiceSlider2()
    {
        if (p2down == true)
        {
            if (choice1 == prevchoice1)
            {
                float x = Input.mousePosition.x - ADJx;

                // figure out which one they chose
                int c = getNumFromX(x);

                float targetx = tiles[c - 1].GetComponent<RectTransform>().position.x;
                MoveADude(s2, targetx, c, 2);
            }

        }

    }


    // push and hold vars
    public void set1False() { p1down = false; }
    public void set1True() { p1down = true; }
    public void set2False() { p2down = false; }
    public void set2True() { p2down = true; }
}