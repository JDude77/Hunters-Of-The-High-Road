using UnityEngine;

public class BossStateScream : AttackState
{
    [Header("Swipe settings")]
    [SerializeField] private float radius;
    [Header("Sounds")]
    [SerializeField] private AK.Wwise.Event screamNoise;
    [Header("Animation Names")]
    [SerializeField] private string animationName;

    #region Anim Event Params
    [Header("Available Animation Event Parameters")]
    [SerializeField] [ReadOnlyProperty] private string screamSound = "ScreamSound";
    [SerializeField] [ReadOnlyProperty] private string animationEnd = "AnimationEnd";
    [SerializeField] [ReadOnlyProperty] private string damageCheck = "DamageCheck";
    #endregion

    public void Start()
    {
        base.Start();
        eventResponder.AddAnimation("Scream", animationName, false);
        //Animation events
        eventResponder.AddSoundEffect(screamSound, screamNoise, gameObject);
        eventResponder.AddAction(animationEnd, boss.ReturnToMainState);
        eventResponder.AddAction(damageCheck, DoSphereCast);
    }//End Start

    public override void OnEnter()
    {
        base.OnEnter();
        eventResponder.ActivateAnimation("Scream");
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
}
