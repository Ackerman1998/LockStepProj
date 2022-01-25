using PBBattle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleManager : MonoSingleton<RoleManager>
{
    private Dictionary<int, RoleBase> dic_role;
    public Transform mapRoot;

    private GameObject pre_roleBase;

    public void Initialized()
    {
        dic_role = new Dictionary<int, RoleBase>();
        pre_roleBase = Resources.Load<GameObject>("Role/RoleBase");
        print(" TcpManager.Instance.userData.battleUserInfoes.Count=" + TcpManager.Instance.userData.battleUserInfoes.Count);
        for (int i=0;i < TcpManager.Instance.userData.battleUserInfoes.Count;i++) {
            GameObject _base = Instantiate(pre_roleBase, mapRoot);
            RoleBase _roleCon = _base.GetComponent<RoleBase>();
            _roleCon.InitData(new GameVector2(0,0));
            dic_role[TcpManager.Instance.userData.battleUserInfoes[i].Uid] = _roleCon;
        }
    }
    public void AddAllRoleOperations(AllPlayerOperation _allOp) {
        foreach (var item in _allOp.Operations)
        {
            dic_role[item.Uid].Logic_UpdateMoveDir(item.Move);
        }
    }
    public void Logic_Move()
    { // 逻辑移动。遍历每一个角色完成移动
      // Debug.Log("dic_role.Count  "  + dic_role.Count);
        foreach (var item in dic_role)
        {
            item.Value.Logic_Move();
        }
    }
}
