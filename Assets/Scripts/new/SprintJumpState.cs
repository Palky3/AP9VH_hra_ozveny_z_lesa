using UnityEngine;
public class SprintJumpState:State
{
    float timePassed;
    float jumpTime;

    public SprintJumpState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
	{
		character = _character;
		stateMachine = _stateMachine;
	}

    public override void Enter()
	{
		base.Enter();
        character.canJump = false;
        character.animator.applyRootMotion = true;
        timePassed = 0f;
        character.animator.SetTrigger("sprintJump");

        jumpTime = 0.8f;
    }

	public override void Exit()
	{
		base.Exit();
        character.animator.applyRootMotion = false;
    }

	public override void LogicUpdate()
    {
        
        base.LogicUpdate();

        // Pohyb postavy dopøedu bìhem skoku
        Vector3 forwardMovement = character.transform.forward * (character.sprintSpeed - 2.0f) * Time.deltaTime;
        character.controller.Move(forwardMovement);

        if (timePassed> jumpTime)
		{
            character.animator.SetTrigger("move");

            if (sprintAction.ReadValue<float>() > 0f)
            {
                stateMachine.ChangeState(character.sprinting);
            }
            else
            {
                stateMachine.ChangeState(character.standing);
            }
            
        }
        timePassed += Time.deltaTime;
    }



}

