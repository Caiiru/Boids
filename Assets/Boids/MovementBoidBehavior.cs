using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementBoidBehavior : ScriptableObject {
    public abstract Vector2 CalculateMove(BoidAgent agent, List<Transform> context, Boid boid);
}  
