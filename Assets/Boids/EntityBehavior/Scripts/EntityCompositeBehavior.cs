using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boid/EntityBehavior/Composite")]
public class EntityCompositeBehavior : EntityBehavior
{
    public EntityBehavior[] behaviors;
    public float[] weights;

   

    public override CreatureAction ChooseAction(BoidAgent agent, List<Transform> context, Boid boid, CreatureCategory category)
    {
        if (weights.Length != behaviors.Length)
        {
            Debug.LogError("Data mistmatch in " + name, this);
        }

        CreatureAction selectedAction = CreatureAction.None;
        float maxUrgency = float.MinValue;

        for (int i = 0; i < behaviors.Length; i++)
        {
            CreatureAction partialAction = behaviors[i].ChooseAction(agent, context, boid, category);
            float urgencyValue = behaviors[i].urgencyValue * weights[i]; 

            if(urgencyValue > maxUrgency){
                maxUrgency = urgencyValue;
                selectedAction = partialAction; 
            }

        }
        return selectedAction;
    }

}
