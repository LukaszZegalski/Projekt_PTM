using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class BoardMenager : MonoBehaviour
{

    public Text PlayerMove;
    public Text NameRound;
    //zmienna ikreślająca etap gry (wykładanie, ruchy)
    private int stageOne = 8;
    //kamera obserwująca plansze 
    private Camera CameraPlayer;


    //private float valueAudio;
    public AudioMixer audioMixer;
    public Slider audioSlider;

    public static BoardMenager Instance { get; set; }
    //przechowuje możliwe ruchy danego piona w celu ich wuświetlenia
    private bool[,] allowedMoves { get; set; }

    //tablica reprezentująca plansze 
    public Pawns[,] Pawns { get; set; }
    //Wybrany pion
    private Pawns selectedPawns;

    //ielkość pojedynczej płytki planszy
    private const float TILE_SIZE = 1.0f;
    //przesunięcie, wukorzystywane przy obliczaniu środków 
    private const float TILE_OFFSET = 0.5f;
    //Wyświetlane napisy
    private const string PhaseOne = "Phase One";
    private const string PhaseTwo = "Phase Two";
    private const string SetBlack = "Round black\nSet pawns";
    private const string SetWhite = "Round white\nSet pawns";
    private const string MoveBlack = "Round black\nmove pawns";
    private const string MoveWhite = "Round white\nmove pawns";
     
    //współrzędne kursora, wartość -1 oznacza że znajduje się poza planszą gry
    private int selectionX = -1;
    private int selectionY = -1;

    //lista z pionami w grze
    public List<GameObject> PawnsPrefabs;
    //lista z aktywnumi pionami (wybrnymi)
    private List<GameObject> PawnActive = new List<GameObject>();

    //Określa czy pion jest biały/czarny
    public bool isWhiteTure = true;

    //Wykonywane przy starsie gry
    private void Start()
    {
        float volume;
        audioMixer.GetFloat("volume", out volume);
        audioSlider.value = volume;
        //SetValueAudio(volume);
        PlayerMove.text = "";
        NameRound.text = "";
        CameraPlayer = Camera.main;
        CameraPlayer.enabled = true;
        Instance = this;
        SpwanAllPawns();
    }

    //Wykonywane na okrągło podczas rozgrywki (główna pętla gry)
    private void Update()
    {
        SetPlayerMove();
        UpdateSelection();
        //DrawTeekoBoard();
        //DrawSelection();
        if (stageOne == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (selectionX >= 0 && selectionY >= 0)
                {
                    if (selectedPawns == null)
                    {
                        SelectPawns(selectionX, selectionY);
                    }
                    else
                    {
                        MovePawns(selectionX, selectionY);
                    }
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (selectionX >= 0 && selectionY >= 0)
                {
                    if (Pawns[selectionX,selectionY] == null)
                    {
                        if(isWhiteTure)
                            SpawnPawns(1,selectionX,selectionY);
                        else
                            SpawnPawns(0, selectionX, selectionY);
                        stageOne -= 1;
                        isWhiteTure = !isWhiteTure;
                    }
                }
            }
        }
        CheckWin();
    }

   // public void SetValueAudio(float audio)
 //   {
  //      valueAudio = audio;
  //  }

 //   public float GetValeAudio()
 //   {
 //       return valueAudio;
 //   }


    //Wybranie danego piona
    private void SelectPawns(int x, int y)
    {
        if (Pawns[x, y] == null)
            return;
        if (Pawns[x, y].isWhite != isWhiteTure)
            return;
        allowedMoves = Pawns[x, y].PosibleMove();
        selectedPawns = Pawns[x, y];
        BoardSquareLights.Instance.SquareLightAllowedMoves(allowedMoves);
    }

    //Przemieszczenie piona
    private void MovePawns(int x, int y)
    {
        if (allowedMoves[x, y])
        {
            Pawns[selectedPawns.CurentX, selectedPawns.CurentY] = null;
            selectedPawns.transform.position = GetTitleCenter(x, y);
            selectedPawns.SetPosition(x, y);
            Pawns[x, y] = selectedPawns;
            isWhiteTure = !isWhiteTure;
        }
        BoardSquareLights.Instance.HidenSquareLights();
        selectedPawns = null;
    }

    //Załadowanie pojedynczego piona
    private void SpawnPawns(int index, int x, int y)
    {
        GameObject go = Instantiate(PawnsPrefabs[index], GetTitleCenter(x, y), Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        Pawns[x, y] = go.GetComponent<Pawns>();
        Pawns[x, y].SetPosition(x, y);
        PawnActive.Add(go);
    }

    //Stworzenie zmiennej do przechowywania pionów
    private void SpwanAllPawns()
    {
        PawnActive = new List<GameObject>();
        Pawns = new Pawns[5, 5];
    }

    //Zwraca współrzędne środków pól
    private Vector3 GetTitleCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        return origin;
    }

    //Aktualizowanie wybranego pola po zmianie położenia kursora
    private void UpdateSelection()
    {
        if (!CameraPlayer)
            return;
        RaycastHit hit;

        if (Physics.Raycast(CameraPlayer.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("TeekoPlane")))
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }

    //Rysowanie planszy
    private void DrawTeekoBoard()
    {
        Vector3 widthLine = Vector3.right * 5;
        Vector3 heightLine = Vector3.forward * 5;

        for (int i = 0; i <= 5; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine, Color.red);
            for (int j = 0; j <= 5; j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine, Color.red);
            }
        }
    }

    //Zaznaczenie wybranego pola po najechaniu kurorem
    private void DrawSelection()
    {
        if (selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(
                Vector3.forward * selectionY + Vector3.right * selectionX,
                Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1), Color.red);
            Debug.DrawLine(
               Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
               Vector3.forward * selectionY + Vector3.right * (selectionX + 1), Color.red);
        }
    }

    //Sprawdzanie czy po danym ruchu nastąpił wygrana
    private void CheckWin()
    {
        if (CheckDiagonal() || CheckHorizontal() || CheckSquare() || CheckVertical())
            EndGame();
    }

    //Sprawdzenie czy piony ułożone są w lini poziomej
    private bool CheckHorizontal()
    {
        for (int i = 0; i < 5; i++)
        {
            if ((Pawns[0, i] != null && Pawns[1, i] != null && Pawns[2, i] != null && Pawns[3, i] != null))
                if ((Pawns[0, i].isWhite && Pawns[1, i].isWhite && Pawns[2, i].isWhite && Pawns[3, i].isWhite))
                    return true;

            if ((Pawns[1, i] != null && Pawns[2, i] != null && Pawns[3, i] != null && Pawns[4, i] != null))
                if ((Pawns[1, i].isWhite && Pawns[2, i].isWhite && Pawns[3, i].isWhite && Pawns[4, i].isWhite))
                    return true;

            if ((Pawns[0, i] != null && Pawns[1, i] != null && Pawns[2, i] != null && Pawns[3, i] != null))
                if (!(Pawns[0, i].isWhite || Pawns[1, i].isWhite || Pawns[2, i].isWhite || Pawns[3, i].isWhite))
                    return true;

            if ((Pawns[1, i] != null && Pawns[2, i] != null && Pawns[3, i] != null && Pawns[4, i] != null))
                if (!(Pawns[1, i].isWhite || Pawns[2, i].isWhite || Pawns[3, i].isWhite || Pawns[4, i].isWhite))
                    return true;
        }
        return false;
    }

    //Sprawdzenie czy piony ułożone są w lini pionowej
    private bool CheckVertical()
    {
        for (int i = 0; i < 5; i++)
        {
            if ((Pawns[i, 0] != null && Pawns[i, 1] != null && Pawns[i, 2] != null && Pawns[i, 3] != null))
                if ((Pawns[i, 0].isWhite && Pawns[i, 1].isWhite && Pawns[i, 2].isWhite && Pawns[i, 3].isWhite))
                    return true;
            if ((Pawns[i, 1] != null && Pawns[i, 2] != null && Pawns[i, 3] != null && Pawns[i, 4] != null))
                if ((Pawns[i, 1].isWhite && Pawns[i, 2].isWhite && Pawns[i, 3].isWhite && Pawns[i, 4].isWhite))
                    return true;
            if ((Pawns[i, 0] != null && Pawns[i, 1] != null && Pawns[i, 2] != null && Pawns[i, 3] != null))
                if (!(Pawns[i, 0].isWhite || Pawns[i, 1].isWhite || Pawns[i, 2].isWhite || Pawns[i, 3].isWhite))
                    return true;
            if ((Pawns[i, 1] != null && Pawns[i, 2] != null && Pawns[i, 3] != null && Pawns[i, 4] != null))
                if (!(Pawns[i, 1].isWhite || Pawns[i, 2].isWhite || Pawns[i, 3].isWhite || Pawns[i, 4].isWhite))
                    return true;
        }
        return false;
    }

    //Sprawdzenie czy piony ułożone są w lini skośnej
    private bool CheckDiagonal()
    {
        if ((Pawns[0, 1] != null && Pawns[1, 2] != null && Pawns[2, 3] != null && Pawns[3, 4] != null))
            if ((Pawns[0, 1].isWhite && Pawns[1, 2].isWhite && Pawns[2, 3].isWhite && Pawns[3, 4].isWhite))
                return true;

        if ((Pawns[0, 0] != null && Pawns[1, 1] != null && Pawns[2, 2] != null && Pawns[3, 3] != null))
            if ((Pawns[0, 0].isWhite && Pawns[1, 1].isWhite && Pawns[2, 2].isWhite && Pawns[3, 3].isWhite))
                return true;

        if ((Pawns[1, 1] != null && Pawns[2, 2] != null && Pawns[3, 3] != null && Pawns[4, 4] != null))
            if ((Pawns[1, 1].isWhite && Pawns[2, 2].isWhite && Pawns[3, 3].isWhite && Pawns[4, 4].isWhite))
                return true;

        if ((Pawns[1, 0] != null && Pawns[2, 1] != null && Pawns[3, 2] != null && Pawns[4, 3] != null))
            if ((Pawns[1, 0].isWhite && Pawns[2, 1].isWhite && Pawns[3, 2].isWhite && Pawns[4, 3].isWhite))
                return true;

        if ((Pawns[0, 4] != null && Pawns[1, 3] != null && Pawns[2, 2] != null && Pawns[3, 1] != null))
            if ((Pawns[0, 4].isWhite && Pawns[1, 3].isWhite && Pawns[2, 2].isWhite && Pawns[3, 1].isWhite))
                return true;

        if ((Pawns[0, 3] != null && Pawns[1, 2] != null && Pawns[2, 1] != null && Pawns[3, 0] != null))
            if ((Pawns[0, 3].isWhite && Pawns[1, 2].isWhite && Pawns[2, 1].isWhite && Pawns[3, 0].isWhite))
                return true;

        if ((Pawns[1, 4] != null && Pawns[2, 3] != null && Pawns[3, 2] != null && Pawns[4, 1] != null))
            if ((Pawns[1, 4].isWhite && Pawns[2, 3].isWhite && Pawns[3, 2].isWhite && Pawns[4, 1].isWhite))
                return true;

        if ((Pawns[4, 0] != null && Pawns[3, 1] != null && Pawns[2, 2] != null && Pawns[1, 3] != null))
            if ((Pawns[4, 0].isWhite && Pawns[3, 1].isWhite && Pawns[2, 2].isWhite && Pawns[1, 3].isWhite))
                return true;


        if ((Pawns[0, 1] != null && Pawns[1, 2] != null && Pawns[2, 3] != null && Pawns[3, 4] != null))
            if (!(Pawns[0, 1].isWhite || Pawns[1, 2].isWhite || Pawns[2, 3].isWhite || Pawns[3, 4].isWhite))
                return true;

        if ((Pawns[0, 0] != null && Pawns[1, 1] != null && Pawns[2, 2] != null && Pawns[3, 3] != null))
            if (!(Pawns[0, 0].isWhite || Pawns[1, 1].isWhite || Pawns[2, 2].isWhite || Pawns[3, 3].isWhite))
                return true;

        if ((Pawns[1, 1] != null && Pawns[2, 2] != null && Pawns[3, 3] != null && Pawns[4, 4] != null))
            if (!(Pawns[1, 1].isWhite || Pawns[2, 2].isWhite || Pawns[3, 3].isWhite || Pawns[4, 4].isWhite))
                return true;

        if ((Pawns[1, 0] != null && Pawns[2, 1] != null && Pawns[3, 2] != null && Pawns[4, 3] != null))
            if (!(Pawns[1, 0].isWhite || Pawns[2, 1].isWhite || Pawns[3, 2].isWhite || Pawns[4, 3].isWhite))
                return true;

        if ((Pawns[0, 4] != null && Pawns[1, 3] != null && Pawns[2, 2] != null && Pawns[3, 1] != null))
            if (!(Pawns[0, 4].isWhite || Pawns[1, 3].isWhite || Pawns[2, 2].isWhite || Pawns[3, 1].isWhite))
                return true;

        if ((Pawns[0, 3] != null && Pawns[1, 2] != null && Pawns[2, 1] != null && Pawns[3, 0] != null))
            if (!(Pawns[0, 3].isWhite || Pawns[1, 2].isWhite || Pawns[2, 1].isWhite || Pawns[3, 0].isWhite))
                return true;

        if ((Pawns[1, 4] != null && Pawns[2, 3] != null && Pawns[3, 2] != null && Pawns[4, 1] != null))
            if (!(Pawns[1, 4].isWhite || Pawns[2, 3].isWhite || Pawns[3, 2].isWhite || Pawns[4, 1].isWhite))
                return true;

        if ((Pawns[4, 0] != null && Pawns[3, 1] != null && Pawns[2, 2] != null && Pawns[1, 3] != null))
            if (!(Pawns[4, 0].isWhite || Pawns[3, 1].isWhite || Pawns[2, 2].isWhite || Pawns[1, 3].isWhite))
                return true;

        return false;
    }

    //Sprawdzenie czy piony ułożone są w kwadracie
    private bool CheckSquare()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (Pawns[i, j] != null && Pawns[i, j + 1] != null && Pawns[i + 1, j] != null && Pawns[i + 1, j + 1] != null)
                    if (Pawns[i, j].isWhite && Pawns[i, j + 1].isWhite && Pawns[i + 1, j].isWhite && Pawns[i + 1, j + 1].isWhite)
                        return true;
                if ((Pawns[i, j] != null && Pawns[i, j + 1] != null && Pawns[i + 1, j] != null && Pawns[i + 1, j + 1] != null))
                    if (!(Pawns[i, j].isWhite || Pawns[i, j + 1].isWhite || Pawns[i + 1, j].isWhite || Pawns[i + 1, j + 1].isWhite))
                        return true;
            }
        }
        return false;
    }

    private void EndGame()
    {
        foreach (GameObject go in PawnActive)
            Destroy(go);
        if (isWhiteTure)
            SceneManager.LoadScene(3);
        else
            SceneManager.LoadScene(2);
    }

    private void ExitGame()
    {
        foreach (GameObject go in PawnActive)
            Destroy(go);
        SceneManager.LoadScene(0);
    }

    private void SetPlayerMove()
    {
        if (stageOne!=0) {
            NameRound.text = PhaseOne;
            if (isWhiteTure)
                PlayerMove.text = SetWhite;
            else
                PlayerMove.text = SetBlack;
        }
        else
        {
            NameRound.text = PhaseTwo;
            if (isWhiteTure)
                PlayerMove.text = MoveWhite;
            else
                PlayerMove.text = MoveBlack;
        }
    }

}
