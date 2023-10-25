using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Boid/Behavior/Steered Cohesion")]
public class SteeredCohesionBehavior : FilterBoidBehavior
{
    Vector2 currentVelocity;
    [Range(0f,2f)]public float agentSmoothTime = 0.5f;
    public override Vector2 CalculateMove(BoidAgent agent, List<Transform> context, Boid boid)
    {
        //if no neighbors, return no adjustment, return vector with no magnitude
        if(context.Count ==0){
            return Vector2.zero;
        }
        else{
            //add all points together and average
            Vector2 cohesionMove = Vector2.zero;
            List<Transform> filteredContext = (filter ==null) ? context : filter.Filter(agent,context);
            foreach(Transform item in filteredContext)
            {
                cohesionMove+=(Vector2)item.position;
            }
            cohesionMove /= context.Count;

            //create offset from agent position

            cohesionMove -= (Vector2)agent.transform.position;
            cohesionMove = Vector2.SmoothDamp(agent.transform.up,cohesionMove,ref currentVelocity,agentSmoothTime);
            Debug.DrawLine(agent.transform.position, cohesionMove,Color.magenta,0.1f,false);
            return cohesionMove;
        }
    }
}
