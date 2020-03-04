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
        AssassinAI = new 

        currentDecision = AssassinAI;

        if (currentDecision != null)
        {
            currentDecision = currentDecision.makeDecision();
        }
    }
}

public class DoISeePlayer
{
    bool value;
    IDecision trueBranch;
    IDecision falseBranch;

    DoISeePlayer(IDecision trueBranch, IDecision falseBranch, Assassin assassin)
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

    IDecision makeDecision()
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

public class IsPlayerInBush
{
    bool value;
    IDecision trueBranch;
    IDecision falseBranch;

    IsPlayerInBush(IDecision trueBranch, IDecision falseBranch, Assassin assassin)
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
    IDecision makeDecision()
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

public class AmICloseToPlayer
{
    bool value;
    IDecision trueBranch;
    IDecision falseBranch;

    AmICloseToPlayer(IDecision trueBranch, IDecision falseBranch, Assassin assassin)
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
    IDecision makeDecision()
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

public class WanderAround
{
    Assassin assassin;

    WanderAround(Assassin ass
        )
    {

    }
}

public class AttackPlayer
{

}

public class ChasePlayer
{

}
