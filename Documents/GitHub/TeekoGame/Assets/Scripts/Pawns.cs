using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawns : MonoBehaviour
{
    public int CurentX { get; set; }
    public int CurentY { get; set; }
    public bool isWhite;
    //Ustawienie pozycji pionka 
    public void SetPosition(int x, int y)
    {
        CurentX = x;
        CurentY = y;
    }
    //Sprawdzenie czy dany ruch jest możliwy
    public bool[,] PosibleMove()
    {
        bool[,] r = new bool[5, 5];
        Pawns p;

        //lweo
        if (CurentX != 0)
        {
            p = BoardMenager.Instance.Pawns[CurentX-1, CurentY];
            if (p == null)
                r[CurentX-1, CurentY] = true;
        }
        //prawo
        if (CurentX != 4)
        {
            p = BoardMenager.Instance.Pawns[CurentX+1, CurentY];
            if (p == null)
                r[CurentX+1, CurentY] = true;
        }
        //dol
        if (CurentY != 0)
        {
            p = BoardMenager.Instance.Pawns[CurentX, CurentY - 1];
            if (p == null)
                r[CurentX, CurentY - 1] = true;
        }
        //gora
        if (CurentY != 4)
        {
            p = BoardMenager.Instance.Pawns[CurentX, CurentY + 1];
            if (p == null)
                r[CurentX, CurentY + 1] = true;
        }
        //lewo-gora
        if (CurentY != 4 && CurentX!=0)
        {
            p = BoardMenager.Instance.Pawns[CurentX-1, CurentY + 1];
            if (p == null)
                r[CurentX-1, CurentY + 1] = true;
        }
        //prawo-gora
        if (CurentY != 4 && CurentX!=4)
        {
            p = BoardMenager.Instance.Pawns[CurentX+1, CurentY + 1];
            if (p == null)
                r[CurentX+1, CurentY + 1] = true;
        }
        //lewo-dol
        if (CurentY != 0 && CurentX!=0)
        {
            p = BoardMenager.Instance.Pawns[CurentX-1, CurentY - 1];
            if (p == null)
                r[CurentX-1, CurentY - 1] = true;
        }
        //prawo-dol
        if (CurentY != 0 && CurentX!=4)
        {
            p = BoardMenager.Instance.Pawns[CurentX+1, CurentY - 1];
            if (p == null)
                r[CurentX+1, CurentY - 1] = true;
        }

        return r;
    }

}
