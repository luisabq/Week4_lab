using System.Collections.Generic;
using UnityEngine;

/*
DESIGN PLAN

START

Declare Inspector variables:
- character name
- class
- level
- constitution score
- race
- Tough feat (boolen)
- Stout feat (boolen)
- averaged or rolled HP (boolen)

Create dictionary for class hit dice
Create dictionary for race HP bonuses

Calculate Constitution modifier:
(CON - 10) / 2

Get hit die from class dictionary

Level 1 HP:
    max hit die + CON modifier

FOR level 2 to character level
    IF averaged
        add expected value of hit die
    ELSE
        roll random hit die
    ENDIF
    add CON modifier


IF race gives bonus
    add race bonus * level


IF Tough feat
    add 2 * level


IF Stout feat
    add 1 * level


Print character description and total HP

END
*/

public class SolutionOne : MonoBehaviour
{
    public string characterName;
    public int level = 1;
    public int constitutionScore = 10;
    public string race;
    public string characterClass;
    public bool hasToughFeat;
    public bool hasStoutFeat;
    public bool useAveragedHP;

    // Class and Race Data
    private string[] classNames = { "Fighter", "Paladin", "Rogue", "Cleric", "Wizard" };
    private int[] classHitDie = { 10, 10, 8, 8, 6 };

    private string[] raceNames = { "Dwarf", "Orc", "Goliath" };
    private int[] raceBonus = { 2, 1, 1 }; // Bonus per level

    void Start()
    {
        CalculateHP();
    }

    void CalculateHP()
    {
         // check class
         int hitDie = 0;
        bool validClass = false;

        // Find hit die for the class
        for (int i = 0; i < classNames.Length; i++)
        {
            if (classNames[i] == characterClass)
            {
                hitDie = classHitDie[i];
                validClass = true;
                break;
            }
        }

        // If class is invalid, log error and exit
        if (!validClass)
        {
            Debug.LogError("Invalid class entered.");
            return;
        }

         // Constitution modifier
         int conModifier = Mathf.FloorToInt((constitutionScore - 10) / 2f);

         // Level 1 HP
         int totalHP = hitDie + conModifier;

         // Levels 2+
         for (int lvl = 2; lvl <= level; lvl++)
        {
            int hpThisLevel = 0;

            // Calculate HP for this level
            if (useAveragedHP)
            {
                hpThisLevel = Mathf.FloorToInt((hitDie + 1) / 2f);
            }
            else
            {
                hpThisLevel = Random.Range(1, hitDie + 1);
            }

            // Add Constitution modifier
            hpThisLevel += conModifier;
            totalHP += hpThisLevel;
        }

         // Race bonus
         for (int i = 0; i < raceNames.Length; i++)
        {
            // If race matches, add bonus per level
            if (raceNames[i] == race)
            {
                totalHP += raceBonus[i] * level;
                break;
            }
        }

         // Feat bonuses
        if (hasToughFeat) 
        {        
            totalHP += 2 * level;
        }        
        if (hasStoutFeat)  
        {
            totalHP += 1 * level;
        }

         // Build Feat Text
         string featText;

        // Determine feat text based on which feats are present
        if (hasToughFeat && hasStoutFeat)
        {            
            featText = "has Tough and Stout feats";
        }        
        else if (hasToughFeat)
        {
            featText = "has Tough feat";
        }
        else if (hasStoutFeat)
        {
             featText = "has Stout feat";
        }
        else
        {            
            featText = "has no HP-related feats";
        }

         // Roll Type
        string rollType;

         if (useAveragedHP)
        {
            rollType = "using average HP";
        }
        else
        {
            rollType = "using dice rolls";
        }

         // Output 
         Debug.Log(
            "My character " + characterName +
            " is a level " + level + " " + characterClass +
            " with a CON score of " + constitutionScore +
            " and is of " + race + " race and " + featText +
            ". I want the HP " + rollType + ". Total HP: " + totalHP
        );
    }
}