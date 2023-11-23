using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Boid/MovementBehavior/StayInRadius")]
public class StayInRadiusBehavior : FilterBoidBehavior
{
    public Vector2 center;
    public float radius = 15f;
    public override Vector2 CalculateMove(BoidAgent agent, List<Transform> context, Boid boid)
    {
        center = GameObject.FindWithTag("Player").transform.position;
        Vector2 centerOffset = center - (Vector2)agent.transform.position;
        float t = centerOffset.magnitude  / radius ;
        if(t < 0.9f){
            return Vector2.zero;
        }
        return centerOffset * t * t;
    } 

}
