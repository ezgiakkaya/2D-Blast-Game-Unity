using UnityEngine;
using System.Collections;

public class ClearablePiece : MonoBehaviour
{

	public AnimationClip clearAnimation;

	private bool isBeingCleared = false;

	public bool IsBeingCleared
	{
		get { return isBeingCleared; }
	}

	protected GamePiece piece;

	void Awake()
	{
		piece = GetComponent<GamePiece>();
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}


	//virtual -> derived classes will overrride that with their own functionality
	public virtual void Clear()
	{

		isBeingCleared = true;
		StartCoroutine(ClearCoroutine());
		piece.GridRef.level.OnPieceCleared(piece);

	}

	private IEnumerator ClearCoroutine()
	{
		Animator animator = GetComponent<Animator>();

		if (animator)
		{
			animator.Play(clearAnimation.name);

			yield return new WaitForSeconds(clearAnimation.length);

			Destroy(gameObject);
		}
	}
}
