using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AgentState : MonoBehaviour
{
    [SerializeField] private List<Transition> transitions;
    private GameObject target;
    protected Player player;
    protected Agent agent;
    protected Animator anim;
    protected bool lockState;

    protected virtual void Awake()
    {
        agent = GetComponentInParent<Agent>();
        anim = GetComponentInParent<Animator>();
    }

    protected abstract void OnEnable();

    protected abstract void OnDisable();

    protected virtual void Update()
    {
        agent.SetSteering(GetSteering());
    }

    private void LateUpdate()
    {
        if(!lockState)
        {
            foreach (var transition in transitions)
            {
                if (transition.TestAllConditions())
                {
                    transition.target.enabled = true;
                    enabled = false;
                    return;
                }
            }
        }
    }

    public void Init(Player player)
    {
        this.player = player;
    }

    public virtual Steering GetSteering()
    {
        return new Steering();
    }
}
