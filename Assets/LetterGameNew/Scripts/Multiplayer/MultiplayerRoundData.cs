using System;

namespace Gameplay
{
    [Serializable]
    public class MultiplayerRoundData
    {
        public int photonID;
        public GameSyncData GameSyncData;

        public MultiplayerRoundData(int photonID, GameSyncData gameSyncData)
        {
            this.photonID = photonID;
            GameSyncData = gameSyncData;
        }
    }
}