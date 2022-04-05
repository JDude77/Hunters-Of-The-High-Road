using UnityEngine;

[DisallowMultipleComponent]
public class BossStateScream : AttackState
{
    [Header("Swipe settings")]
    [SerializeField] private float radius;
    [Header("Sounds")]
    [SerializeField] private AK.Wwise.Event screamNoise;
    [Header("Animation Names")]
    [SerializeField] private string animationName;

    #region Anim Event Params
    [Space(10f)]
    [SerializeField] [ReadOnlyProperty] private string[] AnimationEventParameters = new string[] { "ScreamSound", "AnimationEnd", "DamageCheck" };
    #endregion

    public void Start()
    {
        base.Start();
        InitEvents();
    }//End Start

    public override void OnEnter()
    {
        base.OnEnter();
        boss.animator.SetTrigger("DoScream");
    }//End OnEnter

    public override void OnExit()
    {
        base.OnExit();
    }//End OnExit

    public void DoSphereCast()
    {
        //Store the intersections with the 'Player' layer
        Collider[] collisions = Physics.OverlapSphere(transform.position, radius, boss.attackLayer);

        if (collisions.Length > 0)
            BossEventsHandler.current.HitPlayer(GetDamageValue());
    }//End DoSphereCast

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }//End OnDrawGizmos

    [ContextMenu("Fill default values")]    
    public override void SetDefaultValues()
    {
        radius = 6f;
        animationName = "Boss_Scream";
    }//End SetDefaultValues
    
    private void InitEvents()
    {
        //Animation events
        eventResponder.AddSoundEffect("ScreamSound", screamNoise, gameObject);
        eventResponder.AddAction("AnimationEnd", boss.ReturnToMainState);
        eventResponder.AddAction("DamageCheck", DoSphereCast);
    }
}
