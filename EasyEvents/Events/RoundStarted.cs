using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;

namespace EasyEvents.Events
{
    public class RoundStarted
    {
        private List<int[]> classIds;
        private int finalClassId;

        public RoundStarted(List<int[]> list, int classId)
        {
            classIds = list;
            finalClassId = classId;
        }

        public void onRoundStarted()
        {
            Timing.RunCoroutine(SetRoles());
        }

        private IEnumerator<float> SetRoles()
        {
            yield return Timing.WaitForSeconds(1f);

            var players = Player.List.ToList();
            players.Shuffle();
            
            foreach (var classId in classIds)
            {
                var role = (RoleType) classId[0];
                var percent = classId[1];
                
                for (var i = 0; i < players.Count; i++)
                {
                    if ((i != 0) && (i * percent / 100) <= ((i - 1) * percent / 100)) continue;
                    
                    players[i].SetRole(role);
                    players.RemoveAt(i);
                }
                
                players.Shuffle();
            }

            if (players.Count > 0)
            {
                var role = (RoleType) finalClassId;
                
                foreach (var player in players)
                {
                    player.SetRole(role);
                }
            }
        }
    }
}