using System.Collections.Generic;
using Newtonsoft.Json;
using Photon.Realtime;

namespace Gameplay
{
    public static class MultiplayerJsonHelper
    {
        public static string GetFinalScoreDictJson(Dictionary<Player, int> finalScoreData)
        {
            List<FinalScoreDictData> finalScoreDictDatas = new List<FinalScoreDictData>();
            
            foreach (var scoreData in finalScoreData)
            {
                FinalScoreDictData finalData = new FinalScoreDictData();
                finalData.score = scoreData.Value;
                finalData.pPhotonID = scoreData.Key.ActorNumber;
                finalScoreDictDatas.Add(finalData);
                
            }
            string finalJson = JsonConvert.SerializeObject(finalScoreDictDatas);
            return finalJson;
        }
    }
}