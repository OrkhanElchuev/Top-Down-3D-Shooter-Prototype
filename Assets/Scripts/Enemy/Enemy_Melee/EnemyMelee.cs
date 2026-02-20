using UnityEngine;

/// <summary>
/// Enemy implementation for melee behavior.
/// Creates melee states and delegates per-frame logic to the active state.
/// </summary>
public class EnemyMelee : Enemy
{
    #region States

    public IdleStateMelee idleState { get; private set; }
    public MoveStateMelee moveState { get; private set; }
    public RecoveryStateMelee recoveryState { get; private set; }
    public ChaseStateMelee chaseState { get; private set; }
    public AttackStateMelee attackState { get; private set; }
    public DeadStateMelee deadState { get; private set; }

    #endregion

    private EnemyWeaponModel currentWeapon;
    private bool isAttackReady;

    #region Unity Lifecycle

    protected override void Awake()
    {
        base.Awake();
        CacheCurrentWeapon();

        // Instantiate all states once, then switch between them.
        idleState = new IdleStateMelee(this, stateMachine, "Idle");
        moveState = new MoveStateMelee(this, stateMachine, "Move");
        recoveryState = new RecoveryStateMelee(this, stateMachine, "Recovery");
        chaseState = new ChaseStateMelee(this, stateMachine, "Chase");
        attackState = new AttackStateMelee(this, stateMachine, "Attack");
        deadState = new DeadStateMelee(this, stateMachine, "Idle"); // Idle is just a placeholder, no enemy death animation.
    }

    protected override void Start()
    {
        base.Start();
        
        // Start in idle.
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        // Let the active state run its per-frame logic.
        stateMachine.currentState.Update();

        if (ShouldEnterBattleMode())
        {
            EnterBattleMode();
        }
    }

    #endregion

    private void CacheCurrentWeapon()
    {
        currentWeapon = GetComponentInChildren<EnemyWeaponModel>();

        if (currentWeapon == null)
            Debug.LogError("No weapon found!");
    }

    public void EnableAttackCheck(bool enable)
    {
        if (currentWeapon == null) return;

        if (enable)
            currentWeapon.EnableDamage();
        else
            currentWeapon.DisableDamage();
    }

    #region Overrides

    public override void Die()
    {
        base.Die();

        if (stateMachine.currentState != deadState)
            stateMachine.ChangeState(deadState);
    }

    public override void EnterBattleMode()
    {
        if (inBattleMode)
            return;

        base.EnterBattleMode();
        stateMachine.ChangeState(recoveryState);
    }

    #endregion
}
