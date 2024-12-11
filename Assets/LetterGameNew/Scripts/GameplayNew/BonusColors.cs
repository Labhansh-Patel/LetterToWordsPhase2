using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    
    [Serializable]
    public class BonusColors
    {
        [FormerlySerializedAs("BonusType")] public PowerUpType powerUpType;
        public Color BonusColor;
    }
}