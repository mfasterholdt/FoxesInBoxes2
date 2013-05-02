using UnityEngine;
using System.Collections;

public class GridPos
{
	private int posX;
	public int x
    {
        get { return posX; }
		
        set
        {
            if(value < 0)
			{
				posX = 0;
			}
			else if(value >= Level.levelWidth)
			{
				posX = Level.levelWidth - 1;
			}
			else
			{
				posX = value;
			}
        }
    }
	
	private int posY;	
	public int y
    {
        get { return posY; }
		
        set
        {
			if(value < 0)
			{
				posY = 0;
			}
			else if(value >= Level.levelHeight)
			{
				posY = Level.levelHeight - 1;
			}
			else
			{
				posY = value;
			}
        }
    }
		
    public GridPos (int inputX, int inputY) 
	{
        x = inputX;
		y = inputY;
    }   
	
	public void Log()
	{
		Debug.Log( x + "," + y);
	}
}