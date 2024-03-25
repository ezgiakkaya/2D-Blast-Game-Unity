using UnityEngine;
using System.Collections;

public class GamePiece : MonoBehaviour
{

	public int score;
	private int x;
	private int y;

	public int Hit;
	//public Sprite vaseSprite; // Normal vase sprite
	//public Sprite damagedVaseSprite; // Damaged vase sprite

	public int X
	{
		get { return x; }
		set
		{
			if (IsMovable())
			{
				x = value;
			}
		}
	}

	public int Y
	{
		get { return y; }
		set
		{
			if (IsMovable())
			{
				y = value;
			}
		}
	}

	private Grid.PieceType type;

	public Grid.PieceType Type
	{
		get { return type; }
	}

	private Grid grid;

	public Grid GridRef
	{
		get { return grid; }
	}

	public MovablePiece movableComponent;

	public MovablePiece MovableComponent
	{
		get { return movableComponent; }
		set { movableComponent = value; }

	}

	private ColorPiece colorComponent;

	public ColorPiece ColorComponent
	{
		get { return colorComponent; }
	}
	private ClearablePiece clearableComponent;

	public ClearablePiece ClearableComponent
	{
		get { return clearableComponent; }
	}

	void Awake()
	{
		movableComponent = GetComponent<MovablePiece>();
		clearableComponent = GetComponent<ClearablePiece>();
		//Debug.Log("type : " + type);
		if (type != Grid.PieceType.TNT)
		{
			colorComponent = GetComponent<ColorPiece>();
		}
	}

	// Use this for initialization
	public void SetMovable()
	{
		movableComponent = GetComponent<MovablePiece>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void Init(int _x, int _y, Grid _grid, Grid.PieceType _type)
	{
		x = _x;
		y = _y;
		grid = _grid;
		type = _type;
		// if (_type == Grid.PieceType.TNT)
		// {
		// 	// Option 1: Disable the ColorComponent gameObject if it's not supposed to be colored.
		// 	// or Option 2: Directly manipulate the ColorComponent to indicate it has no color (if such a mechanism exists).
		// }
		// else if (colorComponent != null)
		// {
		// 	// Ensure the component is enabled for non-TNT types, or reset its color as needed
		// 	colorComponent.enabled = true;
		// 	// You might also want to set the default color here if applicable
		// }
	}


	void OnMouseDown()
	{
		Debug.Log("mouse tap detected.");

		//grid.PressPiece(this);
		grid.HandlePieceTapped(this);
	}


	/*void OnMouseUp()
	{
		grid.ReleasePiece();
	}*/

	public bool IsMovable()
	{
		return movableComponent != null;
	}

	public bool IsColored()
	{
		return colorComponent != null;
	}

	public bool IsClearable()
	{
		return clearableComponent != null;
	}


}
