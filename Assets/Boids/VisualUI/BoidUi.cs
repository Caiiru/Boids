using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoidUi : MonoBehaviour
{
    public TextMeshProUGUI currentStatus;
    public Slider hungerslider;

    public ParticleSystem trail;
    BoidAgent agent;

    private void Start()
    {
        agent = GetComponent<BoidAgent>();
        if(agent.creatureCategory == CreatureCategory.Passive){
            trail.startColor = Color.green;
        }
    }
     
    private void FixedUpdate()
    {
        hungerslider.value = agent.hunger;
        switch (agent.currentAction)
        {
            case CreatureAction.Exploring:
                currentStatus.text = "*Exploring the Space*";
                currentStatus.color = Color.blue;
                break;
            case CreatureAction.Pursuiting:
                currentStatus.text = "*Searching food*";
                currentStatus.color = Color.red;
                break;
            case CreatureAction.Evading:
                currentStatus.text = "*Running*";
                currentStatus.color = Color.green;
                break;
        }
    }

}
