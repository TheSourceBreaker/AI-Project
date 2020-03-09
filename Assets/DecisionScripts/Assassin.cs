 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin : MonoBehaviour
{
    public Wander wander;
    public Seek seek;
    public bool iSeePlayer;
    public float playerBushDist;

    public List<GameObject> player = new List<GameObject>();
    // is there an ignore function for colliders
    
    public IDecision currentDecision;
    IDecision AssassinAI;

    private void Start()
    {
        wander = GetComponent<Wander>();
        seek = GetComponent<Seek>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Wanderer")
        {
            iSeePlayer = true;
            player.Add(other.gameObject);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Wanderer")
        {
            iSeePlayer = false;
            player.Remove(other.gameObject);
        }
    }

    private void Update()
    {
        AssassinAI = new DoISeePlayer(
                        new IsPlayerInBush(
                            new WanderAround(this), 
                            new AmICloseToPlayer(
                                new AttackPlayer(this), 
                                new ChasePlayer(this), this), this), 
                        new WanderAround(this), this);

        currentDecision = AssassinAI;

        while (currentDecision != null)
        {
            currentDecision = currentDecision.MakeDecision();
        }
    }
}

public class DoISeePlayer : IDecision
{
    bool value;
    IDecision trueBranch;
    IDecision falseBranch;

    public DoISeePlayer(IDecision _trueBranch, IDecision _falseBranch, Assassin assassin)
    {
        if (assassin.iSeePlayer) // use radius/collider
        {
            value = true;
        }
        else
        {
            value = false;
        }
        trueBranch = _trueBranch;
        falseBranch = _falseBranch;
    }

    public IDecision MakeDecision()
    {
        if(value)
        {
            return trueBranch;
        }
        else if(!value)
        {
            return falseBranch;
        }

        return null;
    }
}

public class IsPlayerInBush : IDecision //-----------------------------------------------------------------------
{
    Wanderer wanderer;
    bool value;
    IDecision trueBranch;
    IDecision falseBranch;

    public IsPlayerInBush(IDecision _trueBranch, IDecision _falseBranch, Assassin assassin)
    {
        for(int i = 0; i < assassin.player.Count; i++)
        {
            wanderer = assassin.player[i].GetComponent<Wanderer>();

            if (wanderer.InBush)
            {
                value = true;
            }
            
        }

        if(value != true)
        {
            value = false;
        }

        trueBranch = _trueBranch;
        falseBranch = _falseBranch;
    }
    public IDecision MakeDecision()
    {
        if (value)
        {
            return trueBranch;
        }
        else if (!value)
        {
            return falseBranch;
        }
        return null;
    }
}

public class AmICloseToPlayer : IDecision
{
    bool value;
    IDecision trueBranch;
    IDecision falseBranch;

    public AmICloseToPlayer(IDecision _trueBranch, IDecision _falseBranch, Assassin assassin)
    {
        for(int i = 0; i < assassin.player.Count; i++)
        {
            if (Vector3.Distance(assassin.player[i].transform.position, assassin.transform.position) < assassin.playerBushDist)
            {
                value = true;
            }
        }
        
        if(value != true)
        {
            value = false;
        }

        trueBranch = _trueBranch;
        falseBranch = _falseBranch;
    }

    public IDecision MakeDecision()
    {
        if (value)
        {
            return trueBranch;
        }
        else if (!value)
        {
            return falseBranch;
        }
        return null;
    }
}

public class WanderAround : IDecision
{
    Assassin assassin;

    public WanderAround(Assassin _assassin)
    {
        assassin = _assassin;
    }
    public IDecision MakeDecision()
    {

        assassin.seek.isSeeking = false;
        assassin.wander.isWandering = true;
        return null;
    }
}

public class AttackPlayer : IDecision //------------------------------------------------------------
{
    Assassin assassin;

    public AttackPlayer(Assassin _assassin)
    {
        assassin = _assassin;
    }
    public IDecision MakeDecision()
    {
        // attack per second
        assassin.wander.isWandering = false;
        return null;
    }
}

public class ChasePlayer : IDecision
{
    Assassin assassin;

    public ChasePlayer(Assassin _assassin)
    {
        assassin = _assassin;
    }
    public IDecision MakeDecision()
    {
        assassin.wander.isWandering = false;
        assassin.seek.isSeeking = true;
        return null;
    }
}
