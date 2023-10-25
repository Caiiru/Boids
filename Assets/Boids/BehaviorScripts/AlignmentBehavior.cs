using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Boid/Behavior/Alignment")]
public class AlignmentBehavior : FilterBoidBehavior
{  
    public override Vector2 CalculateMove(BoidAgent agent, List<Transform> context, Boid boid)
    {
        //if no neighbors, maintain current alignment
        if(context.Count ==0){
            return agent.transform.up;
        }
        else{
            //add all points together and average
            Vector2 alignmentMove = Vector2.zero;
            List<Transform> filteredContext = (filter ==null) ? context : filter.Filter(agent,context);
            foreach(Transform item in filteredContext)
            {
                alignmentMove+=(Vector2)item.transform.up;
            }
            alignmentMove /= context.Count;
            //Debug.DrawLine(agent.transform.position,alignmentMove,Color.white,0.2f);

            return alignmentMove;
        }
    }
}
