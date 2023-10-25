using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Boid/Behavior/Avoidance")]
public class AvoidanceBehavior : FilterBoidBehavior
{
    public override Vector2 CalculateMove(BoidAgent agent, List<Transform> context, Boid boid)
    {
        //if no neighbors, maintain no ajudstament
        if (context.Count == 0)
        {
            return agent.transform.up;
        }
        else
        {
            //add all points together and average
            Vector2 avoidanceMove = Vector2.zero;
            int nAvoid = 0;
            List<Transform> filteredContext = (filter ==null) ? context : filter.Filter(agent,context);
            foreach(Transform item in filteredContext)
            {
                if(Vector2.SqrMagnitude(item.position-agent.transform.position) < boid.SquareAvoidanceRadius)
                {
                    avoidanceMove += (Vector2)(agent.transform.position - item.position);
                    nAvoid++;
                } 
            } 
            if(nAvoid>0)
                avoidanceMove /= nAvoid;
            
            return avoidanceMove;
        }
    }
}
