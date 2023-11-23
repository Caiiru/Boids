using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boid/EntityBehavior/Exploring")]
public class ExploringBehavior : EntityBehavior
{
    public override CreatureAction ChooseAction(BoidAgent agent, List<Transform> context, Boid boid, CreatureCategory category)
    {
        var currentHealth = agent.currentHealth;
        var currentHunger = agent.hunger * 100;

        urgencyValue = ((101-currentHunger) + (currentHealth))/2;
        return CreatureAction.Exploring;
    }
}
