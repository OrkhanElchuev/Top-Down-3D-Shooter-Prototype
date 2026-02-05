using UnityEngine;

public class RecoveryStateMelee : EnemyState
{
    private EnemyMelee enemy;

    public RecoveryStateMelee(Enemy enemyBase, EnemyStateMachine stateMachine, string animationBoolName) : base(enemyBase, stateMachine, animationBoolName)
    {
        enemy = enemyBase as EnemyMelee;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.transform.rotation = enemy.FaceTarget(enemy.playerTransform.position);
    }
}
