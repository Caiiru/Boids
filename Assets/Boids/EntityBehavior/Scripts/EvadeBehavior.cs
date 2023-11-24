
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boid/EntityBehavior/Evade")]
public class EvadeBehavior : EntityBehavior
{
    public override CreatureAction ChooseAction(BoidAgent agent, List<Transform> context, Boid boid, CreatureCategory category)
    {
        var currentHealth = agent.currentHealth;
        if (agent.GetComponent<Entity>().creatureCategory == CreatureCategory.Passive)
        {
            foreach (var obj in context)
            {
                if (obj.GetComponent<Entity>().creatureCategory == CreatureCategory.Agressive)
                {
                    if (obj.transform.GetComponent<BoidAgent>().target != null)
                    {
                        urgencyValue = Vector3.Distance(obj.transform.position, agent.transform.position * agent.evadeRadius) * 10;
                        //Debug.Log(agent.transform.name + " urgency value to Evade: " + urgencyValue);
                        return CreatureAction.Evading;

                    }
                    else
                    {
                        urgencyValue = 0;
                    }
                    return CreatureAction.Exploring;
                }
            }

        }
        return CreatureAction.Exploring;
    }

}



