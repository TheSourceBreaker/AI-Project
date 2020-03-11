using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanderer : MonoBehaviour
{
    public Seek seek;
    public Flee flee;
    public GameObject assassin;
    public float timeBtwHits;
    public float startTimeBtwHits;

    [Header("Wanderer Stats")]
    public int wandererHealth;
    public bool InBush;
    public bool stayingInBush;
    public bool iSeeBush;
    public bool iSeeEnemy;
    private Rigidbody rb;

    [Header("Bush Stats")]
    public List<GameObject> bush = new List<GameObject>();

    [Header("Enemy Stats")]
    public List<GameObject> enemy = new List<GameObject>();

    public IDecision currentDecision;
    IDecision WandererAI;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bush") // if wanderer makes collision with bush
        {
            iSeeBush = true; // they see the bush
            bush.Add(other.gameObject);
        }

        if (other.gameObject.tag == "Enemy") // if wanderer makes collision with enemy
        {
            iSeeEnemy = true; // they see the enemy
            enemy.Add(other.gameObject);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Bush") // if wanderer stops making col with bush
        {
            iSeeBush = false; // they don't see bush
            bush.Remove(other.gameObject);
        }

        if (other.gameObject.tag == "Enemy") // if wanderer stops making col with enemy
        {
            iSeeEnemy = false; // they don't see enemy
            enemy.Remove(other.gameObject);
        }
    }

    void OnDestroy()
    {
        Assassin assassin;
        float minDistance = Mathf.Infinity;
        float newDist = 0;
        int index = 0;

        Collider[] colliders = Physics.OverlapSphere(transform.position, 1);

        for(int i = 0; i < colliders.Length; i++)
        {
            newDist = Vector3.Distance(transform.position, colliders[i].gameObject.transform.position);

            if (newDist < minDistance)
            {
                minDistance = newDist;
                index = i;
            }

        }

        if(colliders.Length > 0 && colliders[index] != null)
        {
            assassin = colliders[index].gameObject.GetComponent<Assassin>();
            if (assassin != null)
                assassin.player.Remove(gameObject);
        }
    }

    void Start() // set everything to false
    {
        timeBtwHits = startTimeBtwHits;
        rb = GetComponent<Rigidbody>();
        seek = GetComponent<Seek>();
        flee = GetComponent<Flee>();
        stayingInBush = false;
        InBush = false;
        iSeeBush = false;
        iSeeEnemy = false;
    }

    void Update()
    {
        if(stayingInBush == true)
        {
            if (timeBtwHits <= 0)
            {
                stayingInBush = false;
                timeBtwHits = startTimeBtwHits;
            }
            else
            {
                timeBtwHits -= Time.deltaTime;
            }
        }

        if (wandererHealth > 0 && stayingInBush == false) // if wanderer has health, then it's alive
        {
            WandererAI =    new DoISeeEnemy(
                                new AmIInBush(
                                    new StayInBush(this), 
                                    new DoISeeBush(
                                        new HideInBush(this), 
                                        new Panic(this), this), this), 
                                new ContinuePath(this), this);

            currentDecision = WandererAI;

            while (currentDecision != null)
            {
                currentDecision = currentDecision.MakeDecision();
            }
        }
        else if(wandererHealth <= 0)
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

    public DoISeeEnemy(IDecision _trueBranch, IDecision _falseBranch, Wanderer wanderer)
    {
        if (wanderer.iSeeEnemy == true) // collider controls the outcome
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

public class AmIInBush : IDecision //-------------------------------------------------------------------------PROBLEM in maxIndex
{
    bool value;
    IDecision trueBranch;
    IDecision falseBranch;

    public AmIInBush(IDecision _trueBranch, IDecision _falseBranch, Wanderer wanderer) // use radius
    {
        float minDistance = Mathf.Infinity; // setup distance
        float newDist; // challenger distance

        for(int i = 0; i < wanderer.bush.Count; i++) // could be .Count or .size for vectors
        {

            newDist = Vector3.Distance(wanderer.transform.position, wanderer.bush[i].transform.position); // Distance check

            if(newDist < minDistance) // if new distance is less than set distance
            {
                minDistance = newDist; // set dist becomes new dist
            }

        }

        if(minDistance < 0.6f) // distance check between the closest bush and the wanderer
        {
            wanderer.InBush = true;
            value = true;
        }
        else
        {
            wanderer.InBush = false;
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

public class DoISeeBush : IDecision
{
    bool value;
    IDecision trueBranch;
    IDecision falseBranch;

    public DoISeeBush(IDecision _trueBranch, IDecision _falseBranch, Wanderer wanderer)
    {
        if (wanderer.iSeeBush) // collider controls the outcome
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
        wanderer.flee.isFleeing = false;
        wanderer.seek.isSeeking = false;


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

        wanderer.stayingInBush = true;
        wanderer.flee.isFleeing = false;
        wanderer.seek.isSeeking = false;
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
        wanderer.flee.isFleeing = true;
        wanderer.seek.isSeeking = false;
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
        //Add closest bush as new target

        GameObject closestBush = null;
        float minDistance = Mathf.Infinity;
        float newDist;

        for(int i = 0; i < wanderer.bush.Count; i++)
        {
            newDist = Vector3.Distance(wanderer.transform.position, wanderer.bush[i].transform.position);

            if (newDist < minDistance)
            {
                minDistance = newDist;
                closestBush = wanderer.bush[i];
            }
        }
        wanderer.seek.Target = closestBush;
        wanderer.seek.isSeeking = true;
        wanderer.flee.isFleeing = false;
        return null;
    }
}


