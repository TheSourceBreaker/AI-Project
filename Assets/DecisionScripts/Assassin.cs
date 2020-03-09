using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin : MonoBehaviour
{
    public int health;
    public float speed;

    public GameObject player;
    public GameObject enemy;
    public GameObject bush; //for each bush, go through an array and measure the distance between the player and the bush. the closest bush index gets saved then checked again with the player distance and to see if they are close together before choosing

    public IDecision currentDecision;
    IDecision AssassinAI;

    private void Update()
    {
        AssassinAI = new DoISeePlayer(new IsPlayerInBush(new WanderAround(this), new AmICloseToPlayer(new AttackPlayer(this), new ChasePlayer(this), this), this), new WanderAround(this), this);

        currentDecision = AssassinAI;

        if (currentDecision != null)
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

    public DoISeePlayer(IDecision trueBranch, IDecision falseBranch, Assassin assassin)
    {
        if (Vector3.Distance(assassin.enemy.transform.position, assassin.player.transform.position) < 1) // use radius/collider
        {
            value = true;
            this.trueBranch = trueBranch;
        }
        else
        {
            value = false;
            this.falseBranch = falseBranch;
        }
    }

    public IDecision MakeDecision()
    {
        if(value == true)
        {
            return trueBranch;
        }
        else if(value == false)
        {
            return falseBranch;
        }

        return null;
    }
}

public class IsPlayerInBush : IDecision
{
    bool value;
    IDecision trueBranch;
    IDecision falseBranch;

    public IsPlayerInBush(IDecision trueBranch, IDecision falseBranch, Assassin assassin)
    {
        if(Vector3.Distance(assassin.player.transform.position, assassin.bush.transform.position) < 0.6)
        {
            value = true;
            this.trueBranch = trueBranch;
        }
        else
        {
            value = false;
            this.falseBranch = falseBranch;
        }
    }
    public IDecision MakeDecision()
    {
        if (value == true)
        {
            return trueBranch;
        }
        else if (value == false)
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

    public AmICloseToPlayer(IDecision trueBranch, IDecision falseBranch, Assassin assassin)
    {
        if (Vector3.Distance(assassin.player.transform.position, assassin.bush.transform.position) < 0.6)
        {
            value = true;
            this.trueBranch = trueBranch;
        }
        else
        {
            value = false;
            this.falseBranch = falseBranch;
        }
    }

    public IDecision MakeDecision()
    {
        if (value == true)
        {
            return trueBranch;
        }
        else if (value == false)
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
        return null;
    }
}

public class AttackPlayer : IDecision
{
    Assassin assassin;

    public AttackPlayer(Assassin _assassin)
    {
        assassin = _assassin;
    }
    public IDecision MakeDecision()
    {
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
        return null;
    }
}
