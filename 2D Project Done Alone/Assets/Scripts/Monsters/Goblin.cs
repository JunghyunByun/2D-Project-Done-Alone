using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : CloseMonster
{
    protected override void Start()
    {
        isGetAway = true;

        base.Start();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
