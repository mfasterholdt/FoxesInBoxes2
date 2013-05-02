using UnityEngine;
using System.Collections;

public class Fox : Tile {
	
	float dir = 1;
	
	delegate void State();
	State state;
	
	Vector3 velocity;
	Vector3 acceleration;
	
	public float bounce = -0.4f;
	
	void Start()
	{	
			
	}
	
	void Update () 
	{
		if(state != null) state();
	}
	
	void MoveForward()
	{
		MoveTile(1, 0);		
		state = new State(MoveState);
	}
	
	
	void MoveState()
	{
		Vector3 pos = this.transform.position;
		velocity.y += Level.gravity * Time.deltaTime;  
		
		
		if(pos.y + velocity.y > moveTarget.y)
		{
			pos += velocity;		
			this.transform.position = pos;	
		}
		else
		{
			/*if(FrontCheck())
			{
				this.transform.position = moveTarget;
				velocity *= bounce;
			}
			else
			{
				MoveForward();
			}*/
		}
	}
}
