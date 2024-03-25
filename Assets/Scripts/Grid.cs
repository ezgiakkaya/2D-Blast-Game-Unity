using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class Grid : MonoBehaviour
{

	public enum PieceType
	{
		EMPTY,
		NORMAL,
		BOX,
		COUNT,
		TNT,

		STONE,

		VASE,

		BROKEN_VASE,
	};
	public HUD hud;

	[System.Serializable]
	public struct PiecePrefab
	{
		public PieceType type;
		public GameObject prefab;
	};

	[System.Serializable]
	public struct PiecePosition
	{
		public PieceType type;
		public int x;
		public int y;
		public ColorPiece.ColorType color;
	};

	public int xDim;
	public int yDim;
	public float fillTime;

	public Level level;
	//public int hitCount = 0

	private bool gameOver = false;
	public PiecePrefab[] piecePrefabs;
	public GameObject backgroundPrefab;

	public PiecePosition[] initialPieces;
	private Dictionary<PieceType, GameObject> piecePrefabDict;

	private GamePiece[,] pieces;
	private bool inverse;
	private GamePiece pressedPiece;
	private GamePiece enteredPiece;

	public LevelData current_level;
	public class LevelData
	{
		public int level_number;
		public int grid_width;
		public int grid_height;
		public int move_count;
		public List<string> grid;
	}
	// Use this for initialization
	void Awake()
	{

		// PlayerPrefs.DeleteAll();
		// // Immediately saves changes to disk
		// PlayerPrefs.Save();

		LevelData[] levelDataArray = new LevelData[]
		{
	new LevelData
	{
		level_number = 1,
		grid_width = 9,
		grid_height = 10,
		move_count = 20,
		grid = new List<string>
		{ "bo", "bo", "bo", "bo", "bo", "bo", "bo", "bo", "bo", "bo", "bo", "bo", "bo", "bo", "bo", "bo", "bo", "bo", "bo", "bo", "bo", "bo", "bo", "bo", "bo", "bo", "bo", "r", "r", "r", "r", "g", "b", "b", "b", "b", "y", "y", "y", "y", "g", "y", "y", "y", "y", "b", "b", "b", "b", "y", "r", "r", "r", "r", "rand", "rand", "rand", "rand", "y", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand" }
	},
	new LevelData
	{
		level_number = 2,
		grid_width = 10,
		grid_height = 7,
		move_count = 15,
		grid = new List<string>
		{ "s", "s", "s", "s", "s", "s", "s", "s", "s", "s", "s", "s", "s", "s", "s", "s", "s", "s", "s", "s", "b", "b", "b", "b", "b", "g", "g", "g", "g", "g", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand"}
	}, new LevelData
	{
	   level_number=3,
	   grid_width=9,
	   grid_height= 8,
	   move_count= 30,
	   grid= new List<string>
	   { "v", "v", "v", "v", "v", "v", "v", "v", "v", "b", "v", "v", "v", "v", "v", "v", "v", "y", "b", "r", "v", "v", "v", "v", "v", "g", "y", "rand", "r", "r", "v", "v", "v", "g", "g", "rand", "rand", "r", "r", "y", "v", "b", "g", "g", "rand", "rand", "rand", "rand", "y", "rand", "b", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand" }
},new LevelData
	{
		level_number = 4,
		grid_width = 7,
		grid_height = 8,
		move_count = 17,
		grid = new List<string>
		{
			"s", "bo", "b", "r", "y", "bo", "s", "s", "bo", "b", "r", "y", "bo", "s",
			"s", "bo", "b", "r", "y", "bo", "s", "s", "bo", "b", "r", "y", "bo", "s",
			"rand", "bo", "b", "r", "y", "bo", "rand", "rand", "rand", "rand", "rand",
			"rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand",
			"rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand"
		}
	},new LevelData
{
	level_number = 5,
	grid_width = 9,
	grid_height = 9,
	move_count = 12,
	grid = new List<string>
	{
		"rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "t", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "t", "bo", "t", "rand", "rand", "rand", "rand", "rand", "t", "bo", "bo", "bo", "t", "rand", "rand", "rand", "t", "bo", "bo", "bo", "bo", "bo", "t", "rand", "rand", "rand", "t", "bo", "bo", "bo", "t", "rand", "rand", "rand", "rand", "rand", "t", "bo", "t", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "t", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand"
	}
},
new LevelData
{
	level_number = 6,
	grid_width = 8,
	grid_height = 9,
	move_count = 23,
	grid = new List<string>
	{
		"rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "r", "r", "b", "b", "r", "r", "b", "b", "g", "g", "y", "y", "g", "g", "y", "y", "r", "r", "b", "b", "r", "r", "b", "b", "g", "g", "y", "y", "g", "g", "y", "y", "v", "v", "v", "v", "v", "v", "v", "v", "v", "v", "v", "v", "v", "v", "v", "v"
	}
},new LevelData
{
	level_number = 7,
	grid_width = 9,
	grid_height = 8,
	move_count = 24,
	grid = new List<string>
	{
		"s", "s", "s", "s", "bo", "s", "s", "s", "s", "s", "s", "s", "bo", "rand", "bo", "s", "s", "s", "s", "s", "bo", "rand", "rand", "rand", "bo", "s", "s", "s", "bo", "b", "rand", "rand", "rand", "g", "bo", "s", "bo", "rand", "b", "rand", "rand", "rand", "g", "rand", "bo", "rand", "rand", "b", "rand", "rand", "rand", "g", "rand", "rand", "y", "y", "y", "y", "v", "r", "r", "r", "r", "y", "y", "v", "y", "v", "r", "v", "r", "r"
	}
}, new LevelData{
	   level_number= 8,
	   grid_width= 9,
	   grid_height= 7,
	   move_count= 18,
	   grid=new List<string>
	   {"s", "s", "s", "s", "s", "s", "s", "s", "s", "s", "bo", "bo", "bo", "v", "bo", "bo", "bo", "s", "s", "bo", "b", "bo", "v", "bo", "r", "bo", "s", "s", "bo", "b", "bo", "v", "bo", "r", "bo", "s", "rand", "bo", "b", "bo", "bo", "bo", "r", "bo", "rand", "rand", "b", "b", "b", "v", "r", "r", "r", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand", "rand"}
}, new LevelData {
	   level_number= 9,
	   grid_width= 6,
	   grid_height= 9,
	   move_count= 20,
	   grid= new List<string>
		{"g", "y", "b", "g", "y", "b", "g", "y", "b", "g", "y", "b", "g", "y", "b", "g", "y", "b", "g", "y", "b", "g", "y", "b", "g", "y", "b", "g", "y", "b", "g", "y", "v", "v", "y", "b", "g", "v", "r", "r", "v", "b", "v", "r", "r", "r", "r", "v", "r", "r", "r", "r", "r", "r"}
}, new LevelData {
	   level_number= 10,
	   grid_width= 10,
		grid_height=  8,
	   move_count= 10,
	   grid= new List<string>
	{"b", "b", "b", "b", "b", "b", "b", "b", "b", "b", "bo", "bo", "bo", "b", "b", "b", "b", "bo", "bo", "b", "bo", "b", "b", "bo", "b", "b", "bo", "b", "b", "bo", "bo", "b", "b", "bo", "b", "b", "bo", "b", "bo", "bo", "bo", "b", "b", "bo", "b", "b", "bo", "b", "b", "b", "bo", "b", "b", "bo", "b", "b", "bo", "b", "b", "bo", "bo", "bo", "bo", "b", "b", "b", "b", "bo", "bo", "b", "b", "b", "b", "b", "b", "b", "b", "b", "b", "b"}
}

};



		//current level -1 
		int current_level_number = PlayerPrefs.GetInt("CurrentLevel", 1);
		Debug.Log("current_level_number : " + current_level_number);
		current_level = levelDataArray[current_level_number - 1];



		// Update grid dimensions
		xDim = current_level.grid_width;
		yDim = current_level.grid_height;

		// Resize and initialize the grid array
		pieces = new GamePiece[xDim, yDim];

		// Update level-specific settings
		// This assumes you have some mechanism in your "Level" class to handle these

		// Initialize the grid with pieces based on JSON 'grid' list
		InitializeGridFromJson(current_level.grid);

		piecePrefabDict = new Dictionary<PieceType, GameObject>();

		for (int i = 0; i < piecePrefabs.Length; i++)
		{
			if (!piecePrefabDict.ContainsKey(piecePrefabs[i].type))
			{
				piecePrefabDict.Add(piecePrefabs[i].type, piecePrefabs[i].prefab);
			}
		}

		for (int x = 0; x < xDim; x++)
		{
			for (int y = 0; y < yDim; y++)
			{
				GameObject background = (GameObject)Instantiate(backgroundPrefab, GetWorldPosition(x, y), Quaternion.identity);
				background.transform.parent = transform;
			}
		}

		pieces = new GamePiece[xDim, yDim];

		for (int i = 0; i < initialPieces.Length; i++)
		{
			if (initialPieces[i].x >= 0 && initialPieces[i].x < xDim && initialPieces[i].y >= 0 && initialPieces[i].y < yDim)
			{
				Debug.Log("initialPieces[i].color : " + initialPieces[i].color + " " + initialPieces[i].color.GetType());
				AddNewPiece(initialPieces[i].x, initialPieces[i].y, initialPieces[i].type, initialPieces[i].color);
			}

		}
		//If it is not already a piece there ( initialized by level) , spawn empty
		for (int x = 0; x < xDim; x++)
		{
			for (int y = 0; y < yDim; y++)
			{
				if (pieces[x, y] == null)
				{
					SpawnNewPiece(x, y, PieceType.EMPTY);
				}


			}
		}


		StartCoroutine(Fill());



	}



	void InitializeGridFromJson(List<string> gridData)
	{
		List<PiecePosition> tempPiecePositions = new List<PiecePosition>();

		// Assuming gridData starts from the bottom-left going horizontally to the top-right
		for (int y = 0; y < yDim; y++)
		{
			for (int x = 0; x < xDim; x++)
			{
				// Calculate the index in the gridData list
				int index = (yDim - 1 - y) * xDim + x; // This reverses the y order
				string pieceTypeStr = gridData[index];
				PieceType pieceType = ConvertStringToPieceType(pieceTypeStr);
				ColorPiece.ColorType color = ConvertStringToPieceColor(pieceTypeStr);

				// Update or create the initial piece position
				PiecePosition newPiecePosition = new PiecePosition()
				{
					type = pieceType,
					x = x,
					y = y,
					color = color // Assuming your PiecePosition struct supports color
				};

				tempPiecePositions.Add(newPiecePosition);
			}
		}

		initialPieces = tempPiecePositions.ToArray(); // Convert list back to array if necessary
	}


	ColorPiece.ColorType ConvertStringToPieceColor(string colorStr)
	{
		switch (colorStr)
		{
			case "r": return ColorPiece.ColorType.RED;
			case "g": return ColorPiece.ColorType.GREEN;
			case "b": return ColorPiece.ColorType.BLUE;
			case "rand": return GetRandomColorPieceType();
			default: return ColorPiece.ColorType.COUNT; // Used for any other cases
		}
	}

	PieceType ConvertStringToPieceType(string pieceTypeStr)
	{
		switch (pieceTypeStr)
		{
			case "r": return PieceType.NORMAL;
			case "g": return PieceType.NORMAL;
			case "b": return PieceType.NORMAL;
			case "y": return PieceType.NORMAL;
			case "rand":
				return PieceType.NORMAL;
			case "t": return PieceType.TNT;
			case "bo": return PieceType.BOX;
			case "s": return PieceType.STONE;
			case "v": return PieceType.VASE;
			default: return PieceType.EMPTY;
		}
	}

	// Example implementation for GetRandomColorPieceType()
	ColorPiece.ColorType GetRandomColorPieceType()
	{
		ColorPiece.ColorType[] colorTypes = {
		ColorPiece.ColorType.RED,
		ColorPiece.ColorType.GREEN,
		ColorPiece.ColorType.BLUE,
		ColorPiece.ColorType.YELLOW
	};
		int index = Random.Range(0, colorTypes.Length);
		return colorTypes[index];
	}

	void SpawnBoxObstacles()
	{
		// Define your BOX piece spawn locations here
		var boxPositions = new List<Vector2Int>
	{
		new Vector2Int(3, 4),
		new Vector2Int(5, 3),
		new Vector2Int(6, 4),
        // Add more as needed
    };

		foreach (var pos in boxPositions)
		{
			Destroy(pieces[pos.x, pos.y].gameObject); // Clean up the initial EMPTY piece
			SpawnNewPiece(pos.x, pos.y, PieceType.BOX);

		}
	}

	// Update is called once per frame
	void Update()
	{

	}
	public IEnumerator Fill()
	{
		Debug.Log("Starting to refill the grid");
		bool needsRefill = true;
		while (needsRefill && !gameOver)
		{
			yield return new WaitForSeconds(fillTime);
			while (FillStep() && !gameOver)
			{
				inverse = !inverse;
				yield return new WaitForSeconds(fillTime);
				Debug.Log("filling");
			}
			for (int i = 0; i < initialPieces.Length; i++)
			{
				MovableNewPiece(initialPieces[i].x, initialPieces[i].y, initialPieces[i].type);

			}
			needsRefill = false;

		}


	}
	public IEnumerator FillFollowing()
	{
		Debug.Log("Starting to refill the grid");
		bool needsRefill = true;
		while (needsRefill && !gameOver)
		{
			yield return new WaitForSeconds(fillTime);
			while (FillStep() && !gameOver)
			{
				inverse = !inverse;
				yield return new WaitForSeconds(fillTime);
				Debug.Log("filling 2");
			}
			for (int i = 0; i < initialPieces.Length; i++)
			{
				MovableNewPiece(initialPieces[i].x, initialPieces[i].y, initialPieces[i].type);

			}
			needsRefill = false;

		}


	}

	public bool FillStep()
	{
		bool movedPiece = false;

		for (int y = yDim - 2; y >= 0; y--)
		{
			for (int loopX = 0; loopX < xDim; loopX++)
			{
				int x = loopX;
				if (inverse)
				{
					x = xDim - 1 - loopX;
				}
				GamePiece piece = pieces[x, y];

				if (piece.IsMovable())
				{
					GamePiece pieceBelow = pieces[x, y + 1];

					if (pieceBelow.Type == PieceType.EMPTY)
					{
						Destroy(pieceBelow.gameObject);
						piece.MovableComponent.Move(x, y + 1, fillTime);
						pieces[x, y + 1] = piece;
						SpawnNewPiece(x, y, PieceType.EMPTY);
						movedPiece = true;
					}
					else
					{
						for (int diag = -1; diag <= 1; diag++)
						{
							if (diag != 0)
							{
								int diagX = x + diag;

								if (inverse)
								{
									diagX = x - diag;
								}

								if (diagX >= 0 && diagX < xDim)
								{
									GamePiece diagonalPiece = pieces[diagX, y + 1];

									if (diagonalPiece.Type == PieceType.EMPTY)
									{
										bool hasPieceAbove = true;

										for (int aboveY = y; aboveY >= 0; aboveY--)
										{
											GamePiece pieceAbove = pieces[diagX, aboveY];

											if (pieceAbove.IsMovable())
											{
												break;
											}
											else if (!pieceAbove.IsMovable() && pieceAbove.Type != PieceType.EMPTY)
											{
												hasPieceAbove = false;
												break;
											}
										}

										if (!hasPieceAbove)
										{
											Destroy(diagonalPiece.gameObject);
											piece.MovableComponent.Move(diagX, y + 1, fillTime);
											pieces[diagX, y + 1] = piece;
											SpawnNewPiece(x, y, PieceType.EMPTY);
											movedPiece = true;
											break;
										}
									}
								}
							}
						}
					}


				}
			}
		}

		for (int x = 0; x < xDim; x++)
		{
			GamePiece pieceBelow = pieces[x, 0];

			if (pieceBelow.Type == PieceType.EMPTY)
			{
				Destroy(pieceBelow.gameObject);
				GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[PieceType.NORMAL], GetWorldPosition(x, -1), Quaternion.identity);
				newPiece.transform.parent = transform;

				pieces[x, 0] = newPiece.GetComponent<GamePiece>();
				pieces[x, 0].Init(x, -1, this, PieceType.NORMAL);
				pieces[x, 0].MovableComponent.Move(x, 0, fillTime);
				pieces[x, 0].ColorComponent.SetColor((ColorPiece.ColorType)Random.Range(0, pieces[x, 0].ColorComponent.NumColors));
				movedPiece = true;
			}
		}

		return movedPiece;
	}


	public Vector2 GetWorldPosition(int x, int y)
	{
		return new Vector2(transform.position.x - xDim / 2.0f + x,
			transform.position.y + yDim / 2.0f - y);
	}

	public GamePiece SpawnNewPiece(int x, int y, PieceType type)
	{
		GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[type], GetWorldPosition(x, y), Quaternion.identity);
		newPiece.transform.parent = transform;

		pieces[x, y] = newPiece.GetComponent<GamePiece>();
		pieces[x, y].Init(x, y, this, type);

		return pieces[x, y];
	}
	public GamePiece AddNewPiece(int x, int y, PieceType type, ColorPiece.ColorType color)
	{
		// Debug.Log(x + " " + y + " " + type + " " + color);
		GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[type], GetWorldPosition(x, y), Quaternion.identity);
		newPiece.transform.parent = transform;

		pieces[x, y] = newPiece.GetComponent<GamePiece>();
		pieces[x, y].Init(x, y, this, type);
		if (pieces[x, y].ColorComponent != null)
		{
			pieces[x, y].MovableComponent = null;
			// movableComponent = GetComponent<MovablePiece>();

			pieces[x, y].ColorComponent.SetColor(color);
		}

		return pieces[x, y];
	}


	public GamePiece MovableNewPiece(int x, int y, PieceType type)
	{


		pieces[x, y].SetMovable();


		return pieces[x, y];
	}
	public bool IsAdjacent(GamePiece piece1, GamePiece piece2)
	{
		return (piece1.X == piece2.X && (int)Mathf.Abs(piece1.Y - piece2.Y) == 1) || (piece1.Y == piece2.Y && (int)Mathf.Abs(piece1.X - piece2.X) == 1);

	}



	public void PressPiece(GamePiece piece)
	{
		pressedPiece = piece;
	}




	public List<GamePiece> GetMatch(GamePiece piece, int newX, int newY)
	{
		if (piece.IsColored())
		{
			ColorPiece.ColorType color = piece.ColorComponent.Color;
			List<GamePiece> horizontalPieces = new List<GamePiece>();
			List<GamePiece> verticalPieces = new List<GamePiece>();
			List<GamePiece> matchingPieces = new List<GamePiece>();

			// First check horizontal
			horizontalPieces.Add(piece);

			for (int dir = 0; dir <= 1; dir++)
			{
				for (int xOffset = 1; xOffset < xDim; xOffset++)
				{
					int x;

					if (dir == 0)
					{ // Left
						x = newX - xOffset;
					}
					else
					{ // Right
						x = newX + xOffset;
					}

					if (x < 0 || x >= xDim)
					{
						break;
					}

					if (pieces[x, newY].IsColored() && pieces[x, newY].ColorComponent.Color == color)
					{
						horizontalPieces.Add(pieces[x, newY]);
					}
					else
					{
						break;
					}
				}
			}

			if (horizontalPieces.Count >= 2)
			{
				for (int i = 0; i < horizontalPieces.Count; i++)
				{
					matchingPieces.Add(horizontalPieces[i]);
				}
			}

			// Traverse vertically if we found a match (for L and T shapes)
			if (horizontalPieces.Count >= 2)
			{
				for (int i = 0; i < horizontalPieces.Count; i++)
				{
					for (int dir = 0; dir <= 1; dir++)
					{
						for (int yOffset = 1; yOffset < yDim; yOffset++)
						{
							int y;

							if (dir == 0)
							{ // Up
								y = newY - yOffset;
							}
							else
							{ // Down
								y = newY + yOffset;
							}

							if (y < 0 || y >= yDim)
							{
								break;
							}

							if (pieces[horizontalPieces[i].X, y].IsColored() && pieces[horizontalPieces[i].X, y].ColorComponent.Color == color)
							{
								verticalPieces.Add(pieces[horizontalPieces[i].X, y]);
							}
							else
							{
								break;
							}
						}
					}

					if (verticalPieces.Count < 2)
					{
						verticalPieces.Clear();
					}
					else
					{
						for (int j = 0; j < verticalPieces.Count; j++)
						{
							matchingPieces.Add(verticalPieces[j]);
						}

						break;
					}
				}
			}

			if (matchingPieces.Count >= 2)
			{
				Debug.Log($"Found match of {matchingPieces.Count} pieces at position ({piece.X}, {piece.Y}) with color {piece.ColorComponent.Color}");
				foreach (var matchPiece in matchingPieces)
				{
					Debug.Log($"Matched piece is at ({matchPiece.X}, {matchPiece.Y}) of color {matchPiece.ColorComponent.Color}");
				}
				return matchingPieces;
			}

			// Didn't find anything going horizontally first,
			// so now check vertically
			horizontalPieces.Clear();
			verticalPieces.Clear();
			verticalPieces.Add(piece);

			for (int dir = 0; dir <= 1; dir++)
			{
				for (int yOffset = 1; yOffset < yDim; yOffset++)
				{
					int y;

					if (dir == 0)
					{ // Up
						y = newY - yOffset;
					}
					else
					{ // Down
						y = newY + yOffset;
					}

					if (y < 0 || y >= yDim)
					{
						break;
					}

					if (pieces[newX, y].IsColored() && pieces[newX, y].ColorComponent.Color == color)
					{
						verticalPieces.Add(pieces[newX, y]);
					}
					else
					{
						break;
					}
				}
			}

			if (verticalPieces.Count >= 2)
			{
				for (int i = 0; i < verticalPieces.Count; i++)
				{
					matchingPieces.Add(verticalPieces[i]);
				}
			}

			// Traverse horizontally if we found a match (for L and T shapes)
			if (verticalPieces.Count >= 2)
			{
				for (int i = 0; i < verticalPieces.Count; i++)
				{
					for (int dir = 0; dir <= 1; dir++)
					{
						for (int xOffset = 1; xOffset < xDim; xOffset++)
						{
							int x;

							if (dir == 0)
							{ // Left
								x = newX - xOffset;
							}
							else
							{ // Right
								x = newX + xOffset;
							}

							if (x < 0 || x >= xDim)
							{
								break;
							}

							if (pieces[x, verticalPieces[i].Y].IsColored() && pieces[x, verticalPieces[i].Y].ColorComponent.Color == color)
							{
								horizontalPieces.Add(pieces[x, verticalPieces[i].Y]);
							}
							else
							{
								break;
							}
						}
					}

					if (horizontalPieces.Count < 2)
					{
						horizontalPieces.Clear();
					}
					else
					{
						for (int j = 0; j < horizontalPieces.Count; j++)
						{
							matchingPieces.Add(horizontalPieces[j]);
						}

						break;
					}
				}
			}

			if (matchingPieces.Count >= 2)
			{
				if (matchingPieces.Count >= 5)
				{
					Debug.Log("Match count >= 5, creating TNT");
					return matchingPieces;
				}
				return matchingPieces;
			}

		}

		return null;
	}


	public bool ClearPiece(int x, int y)
	{
		if (pieces[x, y].IsClearable() && !pieces[x, y].ClearableComponent.IsBeingCleared)
		{
			pieces[x, y].ClearableComponent.Clear();
			SpawnNewPiece(x, y, PieceType.EMPTY);

			ClearObstacles(x, y);
			return true;
		}
		return false;
	}

	public void ClearObstacles(int x, int y)
	{
		for (int adjacentX = x - 1; adjacentX <= x + 1; adjacentX++)
		{
			if (adjacentX != x && adjacentX >= 0 && adjacentX < xDim)
			{
				if (pieces[adjacentX, y].Type == PieceType.BOX && pieces[adjacentX, y].IsClearable())
				{
					pieces[adjacentX, y].ClearableComponent.Clear();
					SpawnNewPiece(adjacentX, y, PieceType.EMPTY);
				}
				else if (pieces[adjacentX, y].Type == PieceType.VASE && pieces[adjacentX, y].IsClearable())
				{
					// Increment hit for the vase
					pieces[adjacentX, y].Hit++;
					//pieces[adjacentX, y].UpdateVaseVisual();
					//pieces[adjacentX, y].UpdateVaseVisual();
					Debug.Log($"Vase at [{adjacentX}, {y}] hit. Current hit count: {pieces[adjacentX, y].Hit}");
					// Check if hits are enough to clear
					if (pieces[adjacentX, y].Hit >= 2)
					{
						pieces[adjacentX, y].ClearableComponent.Clear();
						SpawnNewPiece(adjacentX, y, PieceType.EMPTY);
						Debug.Log($"Vase at [{adjacentX}, {y}] cleared.");
					}
				}
			}
		}

		for (int adjacentY = y - 1; adjacentY <= y + 1; adjacentY++)
		{
			if (adjacentY != y && adjacentY >= 0 && adjacentY < yDim)
			{
				if (pieces[x, adjacentY].Type == PieceType.BOX && pieces[x, adjacentY].IsClearable())
				{
					pieces[x, adjacentY].ClearableComponent.Clear();
					SpawnNewPiece(x, adjacentY, PieceType.EMPTY);

				}
				else if (pieces[x, adjacentY].Type == PieceType.VASE && pieces[x, adjacentY].IsClearable())
				{
					// Increment hit for the vase
					pieces[x, adjacentY].Hit++;
					//pieces[x, adjacentY].UpdateVaseVisual();
					Debug.Log($"Vase at [{x}, {adjacentY}] hit. Current hit count: {pieces[x, adjacentY].Hit}");
					// Check if hits are enough to clear
					if (pieces[x, adjacentY].Hit >= 2)
					{
						pieces[x, adjacentY].ClearableComponent.Clear();
						SpawnNewPiece(x, adjacentY, PieceType.EMPTY);
						Debug.Log($"Vase at [{x}, {adjacentY}] cleared.");
					}
				}
			}
		}
	}



	// Call this method when a TNT cube is tapped or explodes.
	public bool ExplodeTNT(int tntX, int tntY, bool isCombo = false)
	{
		int range = isCombo ? 3 : 2; // 3 for a 7x7 area, 2 for a 5x5 area
		int minX = Mathf.Max(tntX - range, 0);
		int maxX = Mathf.Min(tntX + range, xDim - 1);
		int minY = Mathf.Max(tntY - range, 0);
		int maxY = Mathf.Min(tntY + range, yDim - 1);

		for (int x = minX; x <= maxX; x++)
		{
			for (int y = minY; y <= maxY; y++)
			{
				if (pieces[x, y] != null)
				{

					ClearPiece(x, y);
				}
			}
		}
		StartCoroutine(Fill());
		return true;
	}


	public void HandlePieceTapped(GamePiece piece)
	{
		if (gameOver)
		{
			return;
		}

		if (piece.Type == PieceType.NORMAL)
		{
			Debug.Log($"Tapped piece is at ({piece.X}, {piece.Y}) of color: " + piece.ColorComponent.Color);
		}

		// Retrieve the matches for the tapped piece.
		List<GamePiece> matchedPieces = GetMatch(piece, piece.X, piece.Y);
		bool new_match = (matchedPieces != null && matchedPieces.Count >= 2) ? true : false;
		// Check if there are enough matched pieces.

		GamePiece newPiece = null;
		//Debug.Log("newPiece is null");

		if (piece.Type == PieceType.TNT)
		{
			bool isCombo = false; // Flag to detect TNT combo

			// Iterate through all pieces to find and check adjacent TNTs
			foreach (GamePiece piece_n in pieces)
			{
				// Check if the piece is a TNT and is adjacent to the tapped piece
				if (piece_n != null && piece_n.Type == PieceType.TNT && IsAdjacent(piece_n, piece))
				{
					isCombo = true; // Found at least one TNT adjacent to the tapped TNT
					break; // No need to check further
				}
			}

			// Explode with combo logic if another TNT is adjacent, otherwise explode normally
			ExplodeTNT(piece.X, piece.Y, isCombo);
			// Note: ClearPiece is called inside ExplodeTNT method if needed
		}
		else if (new_match)
		{
			StartCoroutine(FillFollowing());
			level.OnMove();

			// current_level.move_count = current_level.move_count - 1;
			// Debug.Log("current_level.move_count : " + current_level.move_count);
			// level.hud.SetRemaining(current_level.move_count);

			bool new_bomb = matchedPieces.Count >= 5 ? true : false;

			foreach (var matchedPiece in matchedPieces)
			{
				Debug.Log("matchedPiece.Type :  " + matchedPiece.Type + " " + matchedPiece.X + " " + matchedPiece.Y + " " + matchedPiece.Type);
				Debug.Log("colorComponent : " + matchedPiece.ColorComponent.Color);
				if (matchedPiece.X == piece.X && matchedPiece.Y == piece.Y && new_bomb)
				{
					Debug.Log("EZGİMO :  TRUE");
					Debug.Log("Match count >= 5, creating TNT");
					Destroy(pieces[piece.X, piece.Y].gameObject);
					newPiece = SpawnNewPiece(piece.X, piece.Y, PieceType.TNT);
					continue; // Skip this iteration, leaving TNT on the grid.
				}
				else
				{
					ClearPiece(matchedPiece.X, matchedPiece.Y);
				}
			}
		}

	}

	public void GameOver()
	{

		gameOver = true;
	}

	public List<GamePiece> getPiecesOfType(PieceType type)
	{
		List<GamePiece> piecesOfType = new List<GamePiece>();
		for (int x = 0; x < xDim; x++)
		{
			for (int y = 0; y < yDim; y++)
			{
				if (pieces[x, y].Type == type)
				{
					piecesOfType.Add(pieces[x, y]);
				}

			}
		}
		return piecesOfType;
	}

	// In Grid.cs
	public bool ContainsObstacleType(PieceType type)
	{
		for (int x = 0; x < xDim; x++)
		{
			for (int y = 0; y < yDim; y++)
			{
				if (pieces[x, y] != null && pieces[x, y].Type == type)
				{
					return true;
				}
			}
		}
		return false;
	}

	// In Grid.cs
	public int CountPiecesOfType(PieceType type)
	{
		int count = 0;
		for (int x = 0; x < xDim; x++)
		{
			for (int y = 0; y < yDim; y++)
			{
				if (pieces[x, y] != null && pieces[x, y].Type == type)
				{
					count++;
				}
			}
		}
		return count;
	}



}

