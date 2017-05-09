using UnityEngine;
using System.Collections;
using UnityEngine.AI;

/**
 * file: MazeSpawner.cs
 * editor: Wai Phyo
 * class: CS 470 - Game Development
 * Main Project
 * last modified: 05/04/2017
 * 
 * purpose: Generate maze in a 5x5 grid layout. 
 * Taken from https://www.assetstore.unity3d.com/en/#!/content/38689
 * Edits: 
 * 1. remove unnecessary objects. 
 * 2. update 5 walls to have unrepeated walls. 
 * 3. refactor. 
 */
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
	public GameObject Pillar = null;
    //put different walls with different textures. 
    public GameObject[] Walls = new GameObject[5];
    //put zombie object
    public GameObject ZombiePrefab = null;
    //put cat object
    public GameObject GoalPrefab = null;
    //put first person shooter here.
    public GameObject HeroPrefab = null;
    // delay period when a zombie is generated. 
    public float spawnPeriod = 3.0f;
    public static int Rows = 3;
    public static int Columns = 4;
	public float CellWidth = 5;
	public float CellHeight = 5;
	public bool AddGaps = true;
    
    private BasicMazeGenerator mMazeGenerator = null;

    /**
     * Spawn everything here. 
     * 1. get the correct maze generator. 
     * 2. generate maze. 
     * 3. Once you have maze, create walls. 
     * 4. Create pillars
     * 5. Create goal / cat.
     * 6. Start co routine to spawn enemies / zombies at a fixed time. 
     * 7. Generate First Person Shooter
     */
    void Start () {
        if (!FullRandom)
        {
            Random.seed = RandomSeed;
        }
        AssignMazeGenerator();
        mMazeGenerator.GenerateMaze();
        GenerateWalls();
        FillPillers();
        GenerateGoal();
        StartCoroutine(GenerateEnemy());
        GenerateHero();
    }

    /**
     * create maze generator objet based on user selected algorithm
     */
    private void AssignMazeGenerator() {
        switch (Algorithm) {
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
    }

    /**
     * Generate walls where cell walls flags from maze generator is true. 
     */
    private void GenerateWalls() {
        for (int row = 0; row < Rows; row++) {
            for (int column = 0; column < Columns; column++) {
                float x = GetXForCell(column);
                float z = GetZForCell(row);
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
            }
        }
    }

    /**
     * Generate Cat / Goal Object in a random cell
     */
    private void GenerateGoal() {
        int row = Random.Range(0, Rows);
        int column = Random.Range(0, Columns);
        MazeCell cell = mMazeGenerator.GetMazeCell(row, column);

        if (GoalPrefab != null) {
            GameObject tmp = Instantiate(GoalPrefab, new Vector3(GetXForCell(column), 0.01f, GetZForCell(row)), Quaternion.Euler(0, 0, 0)) as GameObject;
            tmp.transform.parent = transform;
        }
    }

    /**
     * Generating a single wall based on vector and quaternion
     */
    private void GenerateWall(float x, float y, float z, float eulerX, float eulerY, float eulerZ) {
        GameObject tmp = Instantiate(Walls[Random.Range(0, Walls.Length)], new Vector3(x, y, z), Quaternion.Euler(eulerX, eulerY, eulerZ)) as GameObject;
        tmp.transform.parent = transform;
    }

    /**
     * Fill pillers on maze
     */
    private void FillPillers() {
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

    /**
     * Calculate location by using cell width and gaps flag. 
     */
    private float GetXForCell(int column) {
        return column * (CellWidth + (AddGaps ? .2f : 0));
    }

    /**
     * Calculate location by using cell height and gaps flag. 
     */
    private float GetZForCell(int row) {
        return row * (CellHeight + (AddGaps ? .2f : 0));
    }

    /**
     * Generating enemies / zombies
     * Generating them at the end of the maze. 
     * "diff" is added temporary to see if it works. 
     * TODO remove "diff"
     */
    private IEnumerator GenerateEnemy() {
        int row = Rows - 1;
        int column = Columns - 1;
        float diff = 0f;
        while (true) {
            yield return new WaitForSeconds(spawnPeriod);
            MazeCell cell = mMazeGenerator.GetMazeCell(row, column);
            if (ZombiePrefab != null) {
                GameObject tmp = Instantiate(ZombiePrefab, new Vector3(GetXForCell(column) + diff, 0.01f, GetZForCell(row) + diff), Quaternion.Euler(0, 0, 0)) as GameObject;
                tmp.transform.parent = transform;
                diff += 0.01f;
            }
        }
    }

    /**
     * Generate Hero at the beginning of maze
     */
    private void GenerateHero() {
        MazeCell cell = mMazeGenerator.GetMazeCell(0, 0);
        if (HeroPrefab != null) {
            GameObject tmp = Instantiate(HeroPrefab, new Vector3(GetXForCell(0), 0.01f, GetZForCell(0)), Quaternion.Euler(0, 0, 0)) as GameObject;
            tmp.transform.parent = transform;
        }
    }
}
