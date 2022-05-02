using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateDead : BossState
{
    // Start is called before the first frame update
    void Start()
    {
        canBeStunned = false;
        base.Start();
    }
    public override void Run() {
        base.Run();
    }
}
