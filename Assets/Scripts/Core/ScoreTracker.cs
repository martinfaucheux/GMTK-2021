using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScoreTracker: MonoBehaviour
{
    [SerializeField] int minimumMoves;

    public int moves;

    public int GetScore(){
        if(moves <= minimumMoves){
            return 3;
        }
        else if (moves <= 1.5f * minimumMoves){
            return 2;
        }
        else if (moves < 2 * minimumMoves){
            return 1;
        };
        return 0;
    }
}
