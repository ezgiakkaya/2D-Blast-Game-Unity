using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTExplodePiece : ClearablePiece
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Clear()
    {
        base.Clear();

        //clear 5 cubes

        piece.GridRef.ExplodeTNT(piece.X, piece.Y);
    }
}
