using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class TowerState_Upgrading_Base : TowerState_Base
{
    public TowerState_Upgrading_Base(Tower_Base parent)
        : base(parent)
    {

    }

    public override void OnEnter()
    {
        _parentTower.DisableTargetDetection();
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {

    }
}
