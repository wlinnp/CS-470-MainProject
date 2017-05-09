using UnityEngine;
using System.Collections;

//<summary>
//Game object, that creates maze and instantiates it in scene
//</summary>
public class MazeSpawner : MonoBehaviour {
	public enum MazeGenerationAlgorithm{
		PureRecursive,
		RecursiveTree,
		RandomTree,
		OldestTree,
		RecursiveDivision,
	}

	public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
	public bool FullRandom = false;
	public int RandomSeed = 12345;
	public GameObject Floor = null;
	public GameObject Wall = null;
	public GameObject Pillar = null;
    public GameObject[] Walls = new GameObject[5];
	public int Rows = 5;
	public int Columns = 5;
	public float CellWidth = 5;
	public float CellHeight = 5;
	public bool AddGaps = true;
	public GameObject GoalPrefab = null;
    private System.Random random = new System.Random();

	private BasicMazeGenerator mMazeGenerator = null;

	void Start () {
        if (!FullRandom)
        {
            Random.seed = RandomSeed;
        }
        switch (Algorithm)
        {
            case MazeGenerationAlgorithm.PureRecursive:
                mMazeGenerator = new RecursiveMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RecursiveTree:
                mMazeGenerator = new RecursiveTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RandomTree:
                mMazeGenerator = new RandomTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.OldestTree:
                mMazeGenerator = new OldestTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RecursiveDivision:
                mMazeGenerator = new DivisionMazeGenerator(Rows, Columns);
                break;
        }
        mMazeGenerator.GenerateMaze();
        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                float x = column * (CellWidth + (AddGaps ? .2f : 0));
                float z = row * (CellHeight + (AddGaps ? .2f : 0));
                MazeCell cell = mMazeGenerator.GetMazeCell(row, column);
                GameObject tmp = Instantiate(Floor, new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0)) as GameObject;
                tmp.transform.parent = transform;
                if (cell.WallRight) {
                    GenerateWall(x + CellWidth / 2, 0, z, 0, 90, 0);
                }
                if (cell.WallFront) {
                    GenerateWall(x, 0, z + CellHeight / 2, 0, 0, 0);
                } 
                if (cell.WallLeft) {
                    GenerateWall(x - CellWidth / 2, 0, z, 0, 270, 0);
                } 
                if (cell.WallBack) {
                    GenerateWall(x, 0, z - CellHeight / 2, 0, 180, 0);
                }
                
                //coin generation
                if (cell.IsGoal && GoalPrefab != null)
                {
                    tmp = Instantiate(GoalPrefab, new Vector3(x, 0.01f, z), Quaternion.Euler(0, 0, 0)) as GameObject;
                    tmp.transform.parent = transform;
                }
            }
        }
        FillPillers(Rows, Columns);
    }

    private void GenerateWall(float x, float y, float z, float eulerX, float eulerY, float eulerZ) {
        GameObject tmp = Instantiate(Walls[random.Next(0, Walls.Length)], new Vector3(x, y, z), Quaternion.Euler(eulerX, eulerY, eulerZ)) as GameObject;
        tmp.transform.parent = transform;
    }

    /**
     * Fill pillers on maze
     */
    private void FillPillers(int Rows, int Columns) {
        if (Pillar != null) {
            for (int row = 0; row < Rows + 1; row++) {
                for (int column = 0; column < Columns + 1; column++) {
                    float x = column * (CellWidth + (AddGaps ? .2f : 0));
                    float z = row * (CellHeight + (AddGaps ? .2f : 0));
                    GameObject tmp = Instantiate(Pillar, new Vector3(x - CellWidth / 2, 0, z - CellHeight / 2), Quaternion.identity) as GameObject;
                    tmp.transform.parent = transform;
                }
            }
        }
    }
}
