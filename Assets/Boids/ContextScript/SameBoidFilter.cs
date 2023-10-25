using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boid/Filter/Same Boid")]
public class SameBoidFilter : ContextFilter
{
    public override List<Transform> Filter(BoidAgent boid, List<Transform> original)
    {
        List<Transform> filtered = new List<Transform>();
        foreach(Transform item in original){
            BoidAgent itemAgent = item.GetComponent<BoidAgent>();
            if(itemAgent != null && itemAgent.AgentBoid == boid.AgentBoid){
                filtered.Add(item);
            }
        }
        return filtered;
    }
}
