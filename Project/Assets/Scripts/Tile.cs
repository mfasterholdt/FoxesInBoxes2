using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour 
{
	public GameObject visuals;
	
	public enum Type{fox, box, environemnt, background};
	public Type type;

	public int depth;
	
	protected Level level;
	protected GridPos pos;
	
	virtual public void Setup(Vector3 p, int x, int y, Level level)
	{
		pos = new GridPos(x, y);		
		this.level = level;
		
		p.z = depth;
		this.transform.position = p;
	}
}