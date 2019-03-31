using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSquareLights : MonoBehaviour
{
    public static BoardSquareLights Instance { get; set; }

    public GameObject squarelightPrefab;
    private List<GameObject> squarelights;

    private void Start()
    {
        Instance = this;
        squarelights = new List<GameObject>();
    }

    //przekazanie zmiennej z "podświetleniami"
    private GameObject GetSquareLightObject()
    {
        GameObject go = squarelights.Find(g => !g.activeSelf);
        if (go == null)
        {
            go = Instantiate(squarelightPrefab);
            squarelights.Add(go);
        }
        return go;
    }

    //Rysowanie zaznaczeń
    public void SquareLightAllowedMoves(bool[,] moves)
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (moves[i, j])
                {
                    GameObject go = GetSquareLightObject();
                    go.SetActive(true);
                    go.transform.position = new Vector3(i + 0.5f, 0, j + 0.5f);
                }
            }
        }
    }

    //wygaszenie możliwych do wykonania ruchów po jego wykonaniu
    public void HidenSquareLights()
    {
        foreach (GameObject go in squarelights)
        {
            go.SetActive(false);
        }
    }
}
