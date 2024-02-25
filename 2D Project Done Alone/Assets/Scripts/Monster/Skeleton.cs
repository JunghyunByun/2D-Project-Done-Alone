using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : LongDistanceMonster
{
    protected override void Start()
    {
        prefabName = "Arrow";

        isGetAway = true;

        base.Start();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
