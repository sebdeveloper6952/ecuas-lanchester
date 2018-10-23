using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ArmyManager : MonoBehaviour {

    public GameObject redSoldierPrefab;
    public GameObject blueSoldierPrefab;

    private int redArmyUnits;
    private float redArmyEff;
    private int blueArmyUnits;
    private float blueArmyEff;
    private List<Soldier> redArmy;
    private List<Soldier> blueArmy;

	private void Awake()
    {
        redArmy = new List<Soldier>();
        blueArmy = new List<Soldier>();
    }

    private void Start()
    {
        redArmyUnits = PlayerPrefs.GetInt(BattleConfig.redArmyUnitsKey);
        redArmyEff = PlayerPrefs.GetFloat(BattleConfig.redArmyEffKey);
        blueArmyUnits = PlayerPrefs.GetInt(BattleConfig.blueArmyUnitsKey);
        blueArmyEff = PlayerPrefs.GetFloat(BattleConfig.blueArmyEffKey);
        InstantiateArmies();
        AssignTargets();
        StartBattle();
    }

    private void OnTriggerEnter(Collider other)
    {
        Soldier s = other.GetComponent<Soldier>();
        if (s != null)
        {
            Debug.Log(s.soldierId + " OnTriggerEnter");
            s.SetAttack(true, true);
        }
    }

    public void StartBattle()
    {
        foreach (Soldier s in redArmy)
        {
            s.Move();
        }
        foreach (Soldier s in blueArmy)
        {
            s.Move();
        }
    }

    public void ResetBattle()
    {

    }

    private void InstantiateArmies()
    {
        int wide = 10;
        Vector3 pos0 = new Vector3(transform.position.x, transform.position.y, transform.position.z - 20f);
        Vector3 pos1 = new Vector3(transform.position.x, transform.position.y, transform.position.z + 20f);
        for (int r = 0; r < redArmyUnits; r++)
        {
            if (r % wide == 0)
            {
                pos0 -= Vector3.forward * 3f;
                pos0 = new Vector3(transform.position.x, transform.position.y, pos0.z);
            }
            redArmy.Add(Instantiate(redSoldierPrefab, pos0, redSoldierPrefab.transform.rotation).GetComponent<Soldier>().Initialize(true, r));
            pos0 += Vector3.right * 3f;
        }
        for (int r = 0; r < blueArmyUnits; r++)
        {
            if (r % wide == 0)
            {
                pos1 += Vector3.forward * 3f;
                pos1 = new Vector3(transform.position.x, transform.position.y, pos1.z);
            }
            blueArmy.Add(Instantiate(blueSoldierPrefab, pos1, blueSoldierPrefab.transform.rotation).GetComponent<Soldier>().Initialize(false, r));
            pos1 += Vector3.right * 3f;
        }
    }

    private void AssignTargets()
    {
        List<Soldier> aux = new List<Soldier>();
        if (redArmy.Count >= blueArmy.Count)
        {
            for (int i = 0; i < redArmyUnits; i++) aux.Add(blueArmy[i % blueArmy.Count]);
            for (int i = 0; i < redArmyUnits; i++)
            {
                /// Generate random number
                int rand = UnityEngine.Random.Range(0, aux.Count - 1);
                Soldier target = aux[rand];
                /// Assign soldier objectives
                redArmy[i].SetTarget(target);
                target.SetTarget(redArmy[i]);
                aux.RemoveAt(rand);
            }
        }
        else
        {
            for (int i = 0; i < blueArmyUnits; i++) aux.Add(redArmy[i % redArmy.Count]);
            for (int i = 0; i < blueArmyUnits; i++)
            {
                /// Generate random number
                int rand = UnityEngine.Random.Range(0, aux.Count - 1);
                Soldier target = aux[rand];
                /// Assign soldier objectives
                blueArmy[i].SetTarget(target);
                target.SetTarget(blueArmy[i]);
                aux.RemoveAt(rand);
            }
        }
    }

    private void TestDiffEq()
    {
        
    }

}
