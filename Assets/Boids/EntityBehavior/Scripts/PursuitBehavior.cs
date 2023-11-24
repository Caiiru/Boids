using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boid/EntityBehavior/Pursuiting")]
public class PursuitBehavior : EntityBehavior
{
    public override CreatureAction ChooseAction(BoidAgent agent, List<Transform> context, Boid boid, CreatureCategory category)
    {
        var currentHealth = agent.currentHealth;
        var currentHunger = agent.hunger * 100;

        
        urgencyValue =(1/(currentHealth/2)) * (currentHunger*2);
        return CreatureAction.Pursuiting;
    }

}
