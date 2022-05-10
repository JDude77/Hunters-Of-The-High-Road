public class BossStateDead : BossState
{
    // Start is called before the first frame update
    void Start()
    {
        canBeStunned = false;
        base.Start();
    }//End Start
}