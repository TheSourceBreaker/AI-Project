using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanderer : MonoBehaviour
{
    public int health;
    public float speed;

    public GameObject player;
    public GameObject enemy;
    public GameObject bush;

    public IDecision currentDecision;
    IDecision WandererAI;

    private void Update()
    {
        WandererAI = new DoISeeEnemy(new AmIInBush(new StayInBush(this), new DoISeeBush(new HideInBush(this), new Panic(this), this), this), new ContinuePath(this), this);

        currentDecision = WandererAI;

        if (currentDecision != null)
        {
            currentDecision = currentDecision.makeDecision();
        }
    }
}

public interface IDecision
{
    IDecision makeDecision();
}

public class DoISeeEnemy : IDecision
{
    bool value;
    IDecision trueBranch;
    IDecision falseBranch;
    public DoISeeEnemy(IDecision trueBranch, IDecision falseBranch,Wanderer wanderer)
    {
        if (Vector3.Distance(wanderer.player.transform.position, wanderer.enemy.transform.position) < 1) // use radius/collider
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

    public IDecision makeDecision()
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

public class AmIInBush : IDecision
{
    bool value;
    IDecision trueBranch;
    IDecision falseBranch;
    public AmIInBush(IDecision trueBranch, IDecision falseBranch, Wanderer wanderer) // use radius
    {
        if (Vector3.Distance(wanderer.player.transform.position, wanderer.bush.transform.position) < 1)
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

    public IDecision makeDecision()
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

public class DoISeeBush : IDecision
{
    bool value;
    IDecision trueBranch;
    IDecision falseBranch;
    public DoISeeBush(IDecision trueBranch, IDecision falseBranch, Wanderer wanderer)
    {
        if (Vector3.Distance(wanderer.player.transform.position, wanderer.bush.transform.position) < 1 && wanderer.enemy.CompareTag("enemy")) // use radius
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
    public IDecision makeDecision()
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

public class ContinuePath : IDecision
{
    Wanderer wanderer;
    public ContinuePath(Wanderer _wanderer)
    {
        wanderer = _wanderer;
    }

    public IDecision makeDecision()
    {
        // continue Pathfinding
        return null;
    }
}

public class StayInBush : IDecision
{
    Wanderer wanderer;
    public StayInBush(Wanderer _wanderer)
    {
        wanderer = _wanderer;
    }

    public IDecision makeDecision()
    {
        // Hold position
        return null;
    }
}


public class Panic : IDecision
{
    Wanderer wanderer;
    public Panic(Wanderer _wanderer)
    {
        wanderer = _wanderer;
    }

    public IDecision makeDecision()
    {
        //Run around the place
        return null;
    }
}


public class HideInBush : IDecision
{
    Wanderer wanderer;
    public HideInBush(Wanderer _wanderer)
    {
        wanderer = _wanderer;
    }

    public IDecision makeDecision()
    {
        //Run to Bush
        return null;
    }
}


