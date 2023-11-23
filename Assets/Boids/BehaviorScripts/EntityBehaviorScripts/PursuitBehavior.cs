using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuitBehavior : EntityBehavior
{
    public override CreatureAction ChooseAction(BoidAgent agent, List<Transform> context, Boid boid, CreatureCategory category)
    {
        if(category == CreatureCategory.Passive){
            urgencyValue=0;
            return CreatureAction.None;
        }else{
            float hunger = agent.hunger *100;
            urgencyValue = (hunger);
            
        }
        return CreatureAction.GoingToFood;
    }
}
