using UnityEngine;
using System.Collections;

public class Fox : DynamicTile
{
	
	int dir = 1;	
	
	float walkSpeed = 0.06f;
	float maxWalkSpeed = 0.023f;
	
	float delayTimer;
	float turnTime = 0.3f;
	
		
	override public void IdleState()
	{
		//Ground check		
		Tile t = level.GetTile(pos.x, pos.y + 1);

		if(t == null)
		{
			StartFall();
		}
		else
		{
			//Forward
			t = level.GetTile(pos.x + dir, pos.y);
			
			if(t == null)
			{
				MoveForward();
			}
			else
			{
				SetTurnState();
			}
		}
	}
	
	void SetTurnState()
	{
		delayTimer = turnTime;
		state = new State(TurnState);
	}
	
	void TurnState()
	{
		delayTimer -= Time.deltaTime;
		
		if(delayTimer <= 0)
		{
			dir *= -1;
			velocity.x = 0;
			visuals.transform.rotation = Quaternion.Euler( new Vector3(0, dir == 1 ? 0 : 180, 0) );
			
			state = new State(IdleState);
		}
	}
	
	void SetMoveState()
	{
		state = new State(MoveState);
	}
	
	void MoveState()
	{
		Vector3 p = this.transform.position;
		velocity.x += dir * Time.deltaTime * walkSpeed;
		velocity.x = Mathf.Clamp(velocity.x, -maxWalkSpeed, maxWalkSpeed);
		
		if((p.x + velocity.x) * dir < moveTarget.x * dir)
		{
			p += velocity;		
			this.transform.position = p;	
		}
		else
		{
			this.transform.position = moveTarget;
			
			SetIdleState();
		}
	}
	
	override public void FallState()
	{
		Vector3 p = this.transform.position;
		velocity.y += Level.gravity * Time.deltaTime;  
		
		if(p.y + velocity.y > moveTarget.y)
		{
			p += velocity;		
			this.transform.position = p;	
		}
		else
		{
			Tile t = level.GetTile(pos.x, pos.y + 1);
			
			if(t != null && t.type != Tile.Type.fox)
			{
				this.transform.position = moveTarget;
					
				velocity *= bounce;
				
				if(Mathf.Abs(velocity.y) < Level.minVelocity)
				{
					state = new State(IdleState);
				}
			}
			else
			{
				SetIdleState();
			}
		}
	}
	
	void MoveForward()
	{
		MoveTile(dir, 0);		
		
		SetMoveState();
	}
}
