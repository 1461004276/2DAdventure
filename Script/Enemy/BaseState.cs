public abstract class BaseState
{
    protected Enemy nowEnemy;
    
    public abstract void OnEnter(Enemy enemy);//初始状态
    public abstract void LogicUpdate();//逻辑更新
    public abstract void PhysicsUpdate();//物理更新
    public abstract void OnExit();//退出状态
}
