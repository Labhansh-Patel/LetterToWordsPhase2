using Gameplay;
using UnityEngine;

namespace LetterGameNew.ScriptableObj
{
    [CreateAssetMenu(fileName = "BonusColors", menuName = "ScriptableObjects/BonusColors", order = 1)]
    public class BonusColorsScriptable : ScriptableObject
    {
        public BonusColors[] BonusColorsArray;

        public Color GetBonusColor(PowerUpType powerUpType)
        {
            foreach (var bonusColor in BonusColorsArray)
            {
                if (bonusColor.powerUpType == powerUpType)
                {
                    return bonusColor.BonusColor;
                }
            }

            return Color.white;
        }
    }
}