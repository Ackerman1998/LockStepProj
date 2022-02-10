using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleBase : MonoBehaviour
{
	public int moveSpeed;
	private Vector3 renderPosition;  // 渲染位置
	private Quaternion renderDir;
	private GameVector2 logicSpeed;
	private int roleDirection;//角色朝向
    private GameVector2 basePosition;
	private Transform modleParent;
	public void InitData(GameVector2 _logicPos) {
		logicSpeed = GameVector2.zero;
		SetPosition(_logicPos);
		modleParent = transform.Find("Modle");
		modleParent.localPosition = new Vector3(0, 0.5f, 0);
		renderPosition = GetPositionVec3();
		transform.position = renderPosition;
		roleDirection = 0;
		renderDir = Quaternion.LookRotation(new Vector3(1f, 0f, 0f));
		modleParent.rotation = renderDir;
	}

	void Update()
	{
		transform.position = Vector3.Lerp(transform.position, renderPosition, 0.4f);
		modleParent.rotation = Quaternion.Lerp(modleParent.rotation, renderDir, 0.2f);
	}
	/// <summary>
	/// Update Player Rotation
	/// </summary>
	/// <param name="_dir"></param>
	public virtual void Logic_UpdateMoveDir(int _dir)
	{
		
		if (_dir > 120)
		{
			logicSpeed = GameVector2.zero;
		}
		else
		{
			roleDirection = _dir * 3;
			logicSpeed = moveSpeed * BattleData.Instance.GetSpeed(roleDirection);
			Vector3 _renderDir = ToolGameVector.ChangeGameVectorToVector3(logicSpeed);
			renderDir = Quaternion.LookRotation(_renderDir);
		}

	}
	/// <summary>
	/// Update Player Position
	/// </summary>
	public virtual void Logic_Move()
	{
		//Debug.Log("logicSpeed:"+ logicSpeed);
		//  Debug.Log("Logic_Move  "  + Time.realtimeSinceStartup);
		if (logicSpeed != GameVector2.zero)
		{ // 如果逻辑速度不等于0
			GameVector2 _targetPos = GetPosition() + logicSpeed; // 计算目标位置
			UpdateLogicPosition(_targetPos); //更新逻辑位置， 使用算法平滑处理。
			renderPosition = GetPositionVec3(); // 获取渲染位置。
		}
	}

	public virtual void Logic_Move_Correction()
	{
		
	}

	void UpdateLogicPosition(GameVector2 _logicPos)
	{

		SetPosition(BattleData.Instance.GetMapLogicPosition(_logicPos));
	}

	//获取逻辑坐标
	public virtual GameVector2 GetPosition()
	{
		return basePosition;
	}
	//获取渲染坐标
	public Vector3 GetPositionVec3(float _y = 0f)
	{
		return ToolGameVector.ChangeGameVectorToVector3(basePosition, _y);
	}
	//设置逻辑坐标
	public void SetPosition(GameVector2 _pos)
	{
		basePosition = _pos;
	}
}
