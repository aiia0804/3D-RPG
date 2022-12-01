using UnityEngine;
using RPG.Stats;
using System.Collections.Generic;

[CreateAssetMenu(fileName ="Progression", menuName = "Stats/New Progression",order =0) ]
public class Progression:ScriptableObject
{
    [SerializeField] ProgressionCharacterClass[] characterClassinProgression = null; //角色的數量


    Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookUpTable = null;

    [System.Serializable]
    class ProgressionCharacterClass
    {
        public CharacterClass characterClass;
        public ProgressionStats[] stats;


    }

    [System.Serializable]
    class ProgressionStats
    {
        public Stat stats;
        public float[] Levels;


    }

    public int GetLevel(Stat stat, CharacterClass characterClass)
    {
        buildLookUp();
        float[] levels = lookUpTable[characterClass][stat];
        return levels.Length;
    }

    public float GetStat(Stat stat,CharacterClass characterClass, int level)
    {

        buildLookUp();

        float[] levels= lookUpTable[characterClass][stat];

        if (levels.Length < level) 
        { 
            return 0; 
        }
        return levels[level-1];
        

    }

    private void buildLookUp()
    {
        if (lookUpTable != null) { return; }

        lookUpTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

        foreach (ProgressionCharacterClass progressionClass in characterClassinProgression)
        {
            var statLookUpTable = new Dictionary<Stat, float[]>();

            foreach(ProgressionStats progressionStat in progressionClass.stats)
            {
                statLookUpTable[progressionStat.stats] = progressionStat.Levels;
            }

            lookUpTable[progressionClass.characterClass] = statLookUpTable;
        }


    }

   






}