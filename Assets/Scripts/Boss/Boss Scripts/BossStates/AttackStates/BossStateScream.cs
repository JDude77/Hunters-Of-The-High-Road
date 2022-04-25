using UnityEngine;

[DisallowMultipleComponent]
public class BossStateScream : AttackState
{
    [Header("Swipe settings")]
    [Tooltip("Radius that the player can be inside to receive damage (For activation radius, see BossStateDecision component)")]
    [SerializeField] private float radius;
    [Header("Sounds")]
    [SerializeField] private AK.Wwise.Event screamNoise;

    #region Anim Event Params
    //[SerializeField] [ReadOnlyProperty] private string[] AnimationEventParameters = new string[] { "ScreamSound", "AnimationEnd", "DamageCheck" };
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
        {
            print("Hit player");
            BossEventsHandler.current.HitPlayer(GetDamageValue());
            BossEventsHandler.current.StunPlayer();
        }
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
    }//End SetDefaultValues
    
    private void InitEvents()
    {
        //Animation events
        eventResponder.AddSoundEffect("ScreamSound", screamNoise, gameObject);
        eventResponder.AddAction("AnimationEnd", boss.ReturnToMainState);
        eventResponder.AddAction("DamageCheck", DoSphereCast);
    }
}