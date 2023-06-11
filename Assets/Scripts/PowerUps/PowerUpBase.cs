using Entities;
using Team;
using UnityEngine;

namespace PowerUps
{
    public class PowerUpBase : CollectableItem
    {
        [Space]
        [Header("POWER UP")]
        public PowerUpData PowerUpData;

        public override void Consume(TeamController teamController, float resourcesPickedUp)
        {
            base.Consume(teamController, resourcesPickedUp);
            
            teamController.AddPowerUP(PowerUpData);
        }
    }
}