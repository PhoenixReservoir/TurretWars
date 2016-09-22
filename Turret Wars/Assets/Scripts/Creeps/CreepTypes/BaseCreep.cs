﻿using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Base Class for creeps. Inherit from this and adjust Creep region as necessary.
/// <see cref="WalkerCreep"/> for an example. Override Update loop for different movement.
/// </summary>
public class BaseCreep : Poolable {

    #region Creep
    public BoxCollider spawnzone;
    public Transform target;

    protected float maxHp = 100.0f;
    private uint creepId;
    
    public float hp = 100.0f;
    public float speed = 0.07f;
    public int value = 10;

    private bool inUse = false;

    private CreepController controller;
    #endregion

    #region IPoolable overrides
    public override uint ObjectId
    {
        get { return creepId; }
    }

    public override bool InUse
    {
        get { return inUse; }
    }

    public override Poolable Create(Player creator, uint creepId, Vector3 position)
    {
        this.spawnzone = creator.targetBattlezone.CreepSpawner;
        this.target = creator.targetBattlezone.Nexus.transform;
        this.creepId = creepId;

        transform.position = Util.GetPointInCollider(creator.TargetBattlezone.CreepSpawner);
        this.hp = maxHp;
        this.gameObject.SetActive(true);
        this.inUse = true;
        return this;
    }

    public override void Die()
    {
        if (controller == null)
            controller = GameObject.Find("LocalCreepController").GetComponent<CreepController>();
        controller.RecallCreep(this.creepId);
        //this.gameObject.SetActive(false);
        //this.inUse = false;
    }

    public override void Recall()
    {
        this.gameObject.SetActive(false);
        this.inUse = false;
    }

    public override string ToString()
    {
        return "BaseCreep";
    }

    public virtual void SetLevel(int level)
    {
        this.maxHp = 100.0f;

        this.hp = this.maxHp;
        this.speed = 0.1f * (level / 2.0f);
        this.value = 10 * level;
    }
    #endregion

    #region Monobehaviour
    void Awake()
    {
        this.Recall();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed);
    }
    #endregion
}
