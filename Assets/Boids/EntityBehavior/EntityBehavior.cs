using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityBehavior : ScriptableObject
{
    public float urgencyValue;
 
    public abstract CreatureAction ChooseAction(BoidAgent agent, List<Transform> context,Boid boid, CreatureCategory category);
}
