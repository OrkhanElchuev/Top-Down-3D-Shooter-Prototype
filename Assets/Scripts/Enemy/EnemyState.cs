using UnityEngine;

public class EnemyState
{
    protected Enemy enemyBase;
    protected EnemyStateMachine stateMachine;
    protected string animationBoolName;
    protected float stateTimer;

    public EnemyState(Enemy enemyBase, EnemyStateMachine stateMachine, string animationBoolName)
    {
        this.enemyBase = enemyBase;
        this.stateMachine = stateMachine;
        this.animationBoolName = animationBoolName;
    }

    public virtual void Enter()
    {
        enemyBase.animator.SetBool(animationBoolName, true);   
    }

    public virtual void Exit()
    {
        enemyBase.animator.SetBool(animationBoolName, false);  
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }
}
