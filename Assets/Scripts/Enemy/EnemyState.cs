using UnityEngine;

public class EnemyState
{
    protected Enemy enemyBase;
    protected EnemyStateMachine stateMachine;
    protected string animationBoolName;

    public EnemyState(Enemy enemyBase, EnemyStateMachine stateMachine, string animationBoolName)
    {
        this.enemyBase = enemyBase;
        this.stateMachine = stateMachine;
        this.animationBoolName = animationBoolName;
    }

    public virtual void EnterState()
    {
        Debug.Log("I enter " + animationBoolName + " state!");
    }

    public virtual void UpdateState()
    {
        Debug.Log("I m running " + animationBoolName + " state!");
    }

    public virtual void ExitState()
    {
        Debug.Log("I exit " + animationBoolName + " state!");
    }
}
