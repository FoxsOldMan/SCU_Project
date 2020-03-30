using UnityEngine;
using System.Collections;

public class ST_PuzzleTile : MonoBehaviour 
{

	public Vector3 TargetPosition;

	public bool Active = true;
    //是否在正确位置
	public bool CorrectLocation = false;

	//存储顺序
	public Vector2 ArrayLocation = new Vector2();
	public Vector2 GridLocation = new Vector2();

	void Awake()
	{
		TargetPosition = this.transform.localPosition;

		// 启动移动程序
		StartCoroutine(UpdatePosition());
	}

	public void LaunchPositionCoroutine(Vector3 newPosition)
	{
		//分配新的目标位置
		TargetPosition = newPosition;

		// 启动移动程序
		StartCoroutine(UpdatePosition());
	}

	public IEnumerator UpdatePosition()
	{
        // 不在目标位置
        while (TargetPosition != this.transform.localPosition)
        {
            // 向目标位置移动
            this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, TargetPosition, 10.0f * Time.deltaTime);
            yield return null;
        }

        //每次搬家后，检查一下我们现在是否在正确的位置。
        if (ArrayLocation == GridLocation){CorrectLocation = true;}else{CorrectLocation = false;}

		if(Active == false)
		{
			this.GetComponent<Renderer>().enabled = false;
		}
        else
        {
            this.GetComponent<Renderer>().enabled = true;

        }

        yield break; ;
	}

	public void ExecuteAdditionalMove()
	{
        // 获取拼图显示并从这个平铺中返回新的目标位置。 
        LaunchPositionCoroutine(this.transform.parent.parent.GetComponent<ST_PuzzleDisplay>().GetTargetLocation(this.GetComponent<ST_PuzzleTile>()));
	}

	void OnMouseDown()
	{
        //获取拼图显示并从这个平铺中返回新的目标位置。
        LaunchPositionCoroutine(this.transform.parent.parent.GetComponent<ST_PuzzleDisplay>().GetTargetLocation(this.GetComponent<ST_PuzzleTile>()));
	}
}
