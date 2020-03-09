using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanderer : MonoBehaviour
{
    public Seek seek;
    public Flee flee;

    [Header("Wanderer Stats")]
    public int wandererHealth;
    public bool InBush;
    public bool iSeeBush;
    public bool iSeeEnemy;

    [Header("Bush Stats")]
    public GameObject[] bush; // might change this to vector
    public int numberOfBushes;

    [Header("Enemy Stats")]
    public GameObject enemy; // might change this to vector

    public IDecision currentDecision;
    IDecision WandererAI;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bush") // if wanderer makes collision with bush
        {
            iSeeBush = true; // they see the bush
        }

        if (collision.gameObject.tag == "Enemy") // if wanderer makes collision with enemy
        {
            iSeeEnemy = true; // they see the enemy
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Bush") // if wanderer stops making col with bush
        {
            iSeeBush = false; // they don't see bush
        }

        if (collision.gameObject.tag == "Enemy") // if wanderer stops making col with enemy
        {
            iSeeEnemy = false; // they don't see enemy
        }
    }

    void Start() // set everything to false
    {
        bush = new GameObject[numberOfBushes];
        seek = GetComponent<Seek>();
        flee = GetComponent<Flee>();
        InBush = false;
        iSeeBush = false;
        iSeeEnemy = false;
    }

    void Update()
    {
        if(wandererHealth > 0) // if wanderer has health, then it's alive
        {
            WandererAI = new DoISeeEnemy(new AmIInBush(new StayInBush(this), new DoISeeBush(new HideInBush(this), new Panic(this), this), this), new ContinuePath(this), this);

            currentDecision = WandererAI;

            if (currentDecision != null)
            {
                currentDecision = currentDecision.MakeDecision();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Methods will use distance for spotting the enemy, the main class will use triggers/collisions for the bush and running from the enemy
}

public interface IDecision
{
    IDecision MakeDecision();
}

public class DoISeeEnemy : IDecision
{
    bool value;
    IDecision trueBranch;
    IDecision falseBranch;

    public DoISeeEnemy(IDecision trueBranch, IDecision falseBranch, Wanderer wanderer)
    {
        if (wanderer.iSeeEnemy == true) // collider controls the outcome
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

public class AmIInBush : IDecision //-------------------------------------------------------------------------PROBLEM in maxIndex
{
    bool value;
    IDecision trueBranch;
    IDecision falseBranch;

    public AmIInBush(IDecision trueBranch, IDecision falseBranch, Wanderer wanderer) // use radius
    {
        float maxDistance = Mathf.Infinity; // setup distance
        float newDist; // challenger distance

        for(int i = 0; i < wanderer.numberOfBushes; i++) // could be .Count or .size for vectors
        {
            int desiredIndex = 0; // index for closest bush

            newDist = Vector3.Distance(wanderer.transform.position, wanderer.bush[i].transform.position); // Distance check

            if(newDist < maxDistance) // if new distance is less than set distance
            {
                maxDistance = newDist; // set dist becomes new dist
                desiredIndex = i; // the current index gets saved
            }

            if(Vector3.Distance(wanderer.transform.position, wanderer.bush[desiredIndex].transform.position) < 0.6) // distance check between the closest bush and the wanderer
            {
                wanderer.InBush = true;
            }
            else
            {
                wanderer.InBush = false;
            }
        }

        if (wanderer.InBush == true) // if wanderer is in bush
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

public class DoISeeBush : IDecision
{
    bool value;
    IDecision trueBranch;
    IDecision falseBranch;

    public DoISeeBush(IDecision trueBranch, IDecision falseBranch, Wanderer wanderer)
    {
        if (wanderer.iSeeBush == true) // collider controls the outcome
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

public class ContinuePath : IDecision
{
    Wanderer wanderer;
    public ContinuePath(Wanderer _wanderer)
    {
        wanderer = _wanderer;
    }

    public IDecision MakeDecision()
    {
        // continue Pathfinding/do nothing
        // OnCollision controls outcome
        
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

    public IDecision MakeDecision()
    {
        // Hold position/Do nothing
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

    public IDecision MakeDecision()
    {
        //Run around the place/flee
        
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

    public IDecision MakeDecision()
    {
        //Run to Bush/Seek
        
        return null;
    }
}


