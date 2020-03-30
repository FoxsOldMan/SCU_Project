using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ST_PuzzleDisplay : MonoBehaviour, Interactable
{
    public GameMode gameMode;
    public PlayerAndCameraController controller;
    public GameObject mainCamera;


    public int puzzleNum; 
    // 拼图纹路.
    public Texture puzzleImage;

	// 块的行与列.
	public int height = 3;
	public int width  = 3;

	// 扩展值.
	public Vector3 PuzzleScale = new Vector3(1.0f, 1.0f, 1.0f);

	// 附加位置偏移量.
	public Vector3 PuzzlePosition = new Vector3(0.0f, 0.0f, 0.0f);

	// 拼图之间的分隔值.
	public float SeperationBetweenTiles = 0.5f;

	public GameObject Tile;

	// 渲染器的shader.
	public Shader PuzzleShader;

	// 块数组.
	private GameObject[,] TileDisplayArray;
	private List<Vector3>  DisplayPositions = new List<Vector3>();

	// 完成度
	private bool Complete = false;

    private bool start = false;
    private bool isPlaying = false;


    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    Debug.LogFormat("Player X:{0}, Y:{1}, Z:{2}", Player.transform.position.x, Player.transform.position.y, Player.transform.position.z);
        //    Debug.LogFormat("Puzzle X:{0}, Y:{1}, Z:{2}", transform.position.x, transform.position.y, transform.position.z);
        //}

        if (start && isPlaying)
        {
            //transform.GetChild(0).Rotate(new Vector3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal")));

            // 将拼图移动到检查器中设置的位置。
            this.transform.GetChild(0).position = PuzzlePosition;

            // 在检查器中设置整个拼图对象的比例.
            this.transform.GetChild(0).localScale = PuzzleScale;

            //if(transform.GetChild(0).childCount >= Height * Width)
            //{
            //    Debug.Log("转换");
            //}
        }
    }

    public void Interaction()
    {
        start = !start;

        if (start)
        {
            StartPuzzle();
        }
        else
        {
            EndPuzzle();
        }
    }

    private void StartPuzzle()
    {
        controller.isActive = false;
        Vector3 difference = mainCamera.transform.position - transform.GetChild(0).position;

        PuzzlePosition = mainCamera.transform.position + mainCamera.transform.rotation * new Vector3(0, 0, 10f);

        //创建拼图
        CreatePuzzleTiles();

        // 拼图随机变化产生.
        StartCoroutine(JugglePuzzle());

        isPlaying = true;

    }

    private void EndPuzzle()
    {
        controller.isActive = true;
        DeletePuzzleTiles();
        isPlaying = false;

        if (Complete)
        {
            gameMode.puzzlesCompleteness[puzzleNum] = true;
        }
    }

    public Vector3 GetTargetLocation(ST_PuzzleTile thisTile)
	{
        // 检查我们是否可以移动这个块并得到我们可以移动到的位置.
        ST_PuzzleTile MoveTo = CheckIfWeCanMove((int)thisTile.GridLocation.x, (int)thisTile.GridLocation.y, thisTile);

		if(MoveTo != thisTile)
		{
			// 获得新块的目标位置.
			Vector3 TargetPos = MoveTo.TargetPosition;
			Vector2 GridLocation = thisTile.GridLocation;
			thisTile.GridLocation = MoveTo.GridLocation;

			// 空块移动到当前位置.
			MoveTo.LaunchPositionCoroutine(thisTile.TargetPosition);
			MoveTo.GridLocation = GridLocation;

			// 返回新的目标位置.
			return TargetPos;
		}

		// 不移动.
		return thisTile.TargetPosition;
	}

	private ST_PuzzleTile CheckMoveLeft(int Xpos, int Ypos, ST_PuzzleTile thisTile)
	{
		// 左移 
		if((Xpos - 1)  >= 0)
		{
            // 我们可以向左移动，当前的空间被使用了吗?
            return GetTileAtThisGridLocation(Xpos - 1, Ypos, thisTile);
		}
		
		return thisTile;
	}
	
	private ST_PuzzleTile CheckMoveRight(int Xpos, int Ypos, ST_PuzzleTile thisTile)
	{
		// 右移 
		if((Xpos + 1)  < width)
		{
            // 我们可以向右移动，当前的空间被使用了吗?
            return GetTileAtThisGridLocation(Xpos + 1, Ypos , thisTile);
		}
		
		return thisTile;
	}
	
	private ST_PuzzleTile CheckMoveDown(int Xpos, int Ypos, ST_PuzzleTile thisTile)
	{
		// 下移 
		if((Ypos - 1)  >= 0)
		{
            // 我们可以向下移动，当前的空间被使用了吗?
            return GetTileAtThisGridLocation(Xpos, Ypos  - 1, thisTile);
		}
		
		return thisTile;
	}
	
	private ST_PuzzleTile CheckMoveUp(int Xpos, int Ypos, ST_PuzzleTile thisTile)
	{
		// 上移 
		if((Ypos + 1)  < height)
		{
            // 我们可以向上移动，当前的空间被使用了吗?
            return GetTileAtThisGridLocation(Xpos, Ypos  + 1, thisTile);
		}
		
		return thisTile;
	}
	
	private ST_PuzzleTile CheckIfWeCanMove(int Xpos, int Ypos, ST_PuzzleTile thisTile)
	{
		// 检查可移动方向
		if(CheckMoveLeft(Xpos, Ypos, thisTile) != thisTile)
		{
			return CheckMoveLeft(Xpos, Ypos, thisTile);
		}
		
		if(CheckMoveRight(Xpos, Ypos, thisTile) != thisTile)
		{
			return CheckMoveRight(Xpos, Ypos, thisTile);
		}
		
		if(CheckMoveDown(Xpos, Ypos, thisTile) != thisTile)
		{
			return CheckMoveDown(Xpos, Ypos, thisTile);
		}
		
		if(CheckMoveUp(Xpos, Ypos, thisTile) != thisTile)
		{
			return CheckMoveUp(Xpos, Ypos, thisTile);
		}

		return thisTile;
	}

	private ST_PuzzleTile GetTileAtThisGridLocation(int x, int y, ST_PuzzleTile thisTile)
	{
		for(int j = height - 1; j >= 0; j--)
		{
			for(int i = 0; i < width; i++)
			{
                // 检查此块是否具有正确的网格显示位置.
                if ((TileDisplayArray[i,j].GetComponent<ST_PuzzleTile>().GridLocation.x == x)&&
				   (TileDisplayArray[i,j].GetComponent<ST_PuzzleTile>().GridLocation.y == y))
				{
					if(TileDisplayArray[i,j].GetComponent<ST_PuzzleTile>().Active == false)
					{
						// 返回块的活动属性. 
						return TileDisplayArray[i,j].GetComponent<ST_PuzzleTile>();
					}
				}
			}
		}

		return thisTile;
	}

	private IEnumerator JugglePuzzle()
	{
		transform.GetChild(0).Rotate(new Vector3(0, 0, mainCamera.transform.eulerAngles.y));
		transform.GetChild(0).Rotate(new Vector3(mainCamera.transform.eulerAngles.x, 0, 0));

		yield return new WaitForSeconds(0.5f);

        if (TileDisplayArray[0, 0] == null)
        {
            yield break;
        }
        // 隐藏一个拼图块保证可移动.
        TileDisplayArray[0,0].GetComponent<ST_PuzzleTile>().Active = false;

		yield return new WaitForSeconds(1.0f);

		for(int k = 0; k < 10; k++)
		{
            // 使用random来定位数组中的每个谜题部分，一旦空格被填满就删除数字.
            for (int j = 0; j < height; j++)
			{
				for(int i = 0; i < width; i++)
				{
                    if (TileDisplayArray[i, j] == null)
                    {
                        yield break;
                    }

                    // 尝试执行此块的移动.
                    TileDisplayArray[i,j].GetComponent<ST_PuzzleTile>().ExecuteAdditionalMove();

					yield return new WaitForSeconds(0.02f);
				}
			}
		}
        Complete = false;

        // 检查正确结果.
        StartCoroutine(CheckForComplete());

		yield return null;
	}

	public IEnumerator CheckForComplete()
	{
        while (Complete == false)
        {
            // 遍历所有块并检查它们的位置是否正确
            Complete = true;
            for (int j = height - 1; j >= 0; j--)
            {
                for (int i = 0; i < width; i++)
                {
                    //if (i == 0 && j == 2) Complete = true;
                    // 检查此块是否具有正确的网格显示位置。.
                    if(TileDisplayArray[i, j] == null)
                    {
                        yield break;
                    }
                    if (TileDisplayArray[i, j].GetComponent<ST_PuzzleTile>().CorrectLocation == false)
                    {
                        Complete = false;
                    }
                }
            }

            yield return null;
        }

        if (Complete)
        {
            TileDisplayArray[0, 0].GetComponent<ST_PuzzleTile>().Active = true;
            TileDisplayArray[0, 0].GetComponent<MeshRenderer>().enabled = true;
            TileDisplayArray[0, 0].GetComponent<MeshCollider>().enabled = true;
            Debug.Log("Puzzle Complete!");

            Invoke("EndPuzzle", 3);
        }

        yield return null;
	}

	private Vector2 ConvertIndexToGrid(int index)
	{
		int WidthIndex = index;
		int HeightIndex = 0;

        // 获取索引值并返回网格数组位置X,Y.
        for (int i = 0; i < height; i++)
		{
			if(WidthIndex < width)
			{
				return new Vector2(WidthIndex, HeightIndex);
			}
			else
			{
				WidthIndex -= width;
				HeightIndex++;
			}
		}

		return new Vector2(WidthIndex, HeightIndex);
	}

	private void CreatePuzzleTiles()
	{
        Vector3 Scale;
	    Vector3 Position;

        // 使用宽度和高度变量创建一个数组.
        TileDisplayArray = new GameObject[width,height];

        // 设置该拼图的比例和位置值.
        Scale = new Vector3(1.0f/width, 1.0f, 1.0f/height);
		Tile.transform.localScale = Scale;

        // 用于计算块的数量并为每个块分配正确的值.
        int TileValue = 0;

		for(int j = height - 1; j >= 0; j--)
		{
			for(int i = 0; i < width; i++)
			{
                // 计算所有以向量3(0.0f, 0.0f, 0.0f)为中心的瓦片的位置.
                Position = new Vector3(((Scale.x * (i + 0.5f))-(Scale.x * (width/2.0f))) * (10.0f + SeperationBetweenTiles), 
				                       0.0f, 
				                      ((Scale.z * (j + 0.5f))-(Scale.z * (height/2.0f))) * (10.0f + SeperationBetweenTiles));

                // 在显示网格上设置此位置.
                DisplayPositions.Add(Position);

				TileDisplayArray[i,j] = Instantiate(Tile, new Vector3(0.0f, 0.0f, 0.0f) , Quaternion.Euler(90.0f, 180.0f, 0.0f));
				TileDisplayArray[i,j].gameObject.transform.parent = this.transform.GetChild(0);

                // 设置并增加显示数字计数器.
                ST_PuzzleTile thisTile = TileDisplayArray[i,j].GetComponent<ST_PuzzleTile>();
				thisTile.ArrayLocation = new Vector2(i,j);
				thisTile.GridLocation = new Vector2(i,j);
				thisTile.LaunchPositionCoroutine(Position);
				TileValue++;

                // 使用定义的着色器创建一个新的材质.
                Material thisTileMaterial = new Material(PuzzleShader);

				thisTileMaterial.mainTexture = puzzleImage;

                // 设置此材质的偏移和块值.
                thisTileMaterial.mainTextureOffset = new Vector2(1.0f/width * i, 1.0f/height * j);
				thisTileMaterial.mainTextureScale  = new Vector2(1.0f/width, 1.0f/height);

                // 将图片分配到此块片上进行显示.
                TileDisplayArray[i,j].GetComponent<Renderer>().material = thisTileMaterial;
			}
		}

        /*
		// Enable an impossible puzzle for fun!
		// 切换第二和第三网格位置纹理.
		Material thisTileMaterial2 = TileDisplayArray[1,3].GetComponent<Renderer>().material;
		Material thisTileMaterial3 = TileDisplayArray[2,3].GetComponent<Renderer>().material;
		TileDisplayArray[1,3].GetComponent<Renderer>().material = thisTileMaterial3;
		TileDisplayArray[2,3].GetComponent<Renderer>().material = thisTileMaterial2;
		*/


    }

    private void DeletePuzzleTiles()
    {

        if (TileDisplayArray == null)
            return;

        for(int row = 0; row < TileDisplayArray.GetLength(0); row++)
        {
            for(int col = 0;col< TileDisplayArray.GetLength(1); col++)
            {
                if(TileDisplayArray[row,col] != null)
                    Destroy(TileDisplayArray[row, col]);
            }
        }

        //transform.GetChild(0).Rotate(new Vector3(-Camera.transform.eulerAngles.x, 0, -Camera.transform.eulerAngles.y));

        transform.GetChild(0).rotation = Quaternion.Euler(-90, 0, 0);
    }
}
