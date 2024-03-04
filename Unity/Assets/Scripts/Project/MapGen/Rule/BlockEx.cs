using FFramework;
using FFramework.MVVM;
using FFramework.RefCache;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YooAsset;
using static UnityEditor.PlayerSettings;
using static UnityEditor.Progress;

public static class BlockEx
{
    /// <summary>
    /// 检查是否有指定的Tag
    /// </summary>
    /// <param name="item"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool HasTag(this BasicBlockPropertiesItem item, int id)
    {
        for (int i = 0; i < item.categories.Length; i++)
        {
            if (item.categories[i] == id) return true;
        }
        return false;
    }

    /// <summary>
    /// 设置方块的渲染器和碰撞是否启用
    /// </summary>
    /// <param name="block"></param>
    /// <param name="value"></param>
    public static void SetMeshAndCollisionEnable(this Block block, bool value)
    {

        if (block == null || block.BlockObject == null) return;
        var collider = block.BlockObject.GetComponent<Collider>();
        var ren = block.BlockObject.GetComponent<MeshRenderer>();
        if (collider != null) collider.enabled = value;
        if (ren != null) ren.enabled = value;
    }

    /// <summary>
    /// 获取邻接方块
    /// </summary>
    /// <param name="block"></param>
    /// <param name="offset"></param>
    /// <returns>如果不存在返回空</returns>
    public static Block GetOffsetBlock(this Block block, Vector3Int offset)
    {
        Vector3Int left = block.Position + offset;
        return MV.GetModel<MapModel>()[left];
    }
    /// <summary>
    /// 获取邻接方块
    /// </summary>
    /// <param name="block"></param>
    /// <param name="offset"></param>
    /// <returns>如果不存在返回空</returns>
    public static Block GetOffsetBlock(this Vector3Int block, Vector3Int offset)
    {
        Vector3Int left = block + offset;
        return MV.GetModel<MapModel>()[left];
    }

    /// <summary>
    /// 判断是否六面有方块
    /// </summary>
    /// <param name="block"></param>
    /// <returns></returns>
    public static bool IsSixFaceExist(this Vector3Int block)
    {

        var model = MV.GetModel<BasicBlockPropertiesModel>();
        //如果六面都是非透明方块返回true
        for (int i = 0; i < 6; i++)
        {
            var offset = Vector3Int.zero;
            switch (i)
            {
                case 0:
                    offset = Vector3Int.up;
                    break;
                case 1:
                    offset = Vector3Int.down;
                    break;
                case 2:
                    offset = Vector3Int.left;
                    break;
                case 3:
                    offset = Vector3Int.right;
                    break;
                case 4:
                    offset = Vector3Int.forward;
                    break;
                case 5:
                    offset = Vector3Int.back;
                    break;
            }
            var neighbor = block.GetOffsetBlock(offset);
            if (neighbor == null || model.DataMap[neighbor.BlockID].isTransparent) return false;
        }
        //Debug.Log($"SixCheck:{block},{true}");
        return true;
    }
    /// <summary>
    /// 判断是否六面有方块
    /// </summary>
    /// <param name="block"></param>
    /// <returns></returns>
    public static bool IsSixFaceExist(this Block block)
    {
        return IsSixFaceExist(block.Position);
    }

    /// <summary>
    /// 加载方块
    /// </summary>
    /// <param name="block"></param>
    /// <returns></returns>
    public static async FTask LoadBlock(this Block block)
    {
        var item = MV.GetModel<BasicBlockPropertiesModel>().DataMap[block.BlockID];

        var prefabLoad = YooAssets.LoadAssetAsync(item.prefabPath);
        await prefabLoad;

        var ins = prefabLoad.InstantiateAsync();
        await ins;

        var mat = YooAssets.LoadAssetAsync(item.matPath);
        await mat;
        ins.Result.GetComponent<MeshRenderer>().material = (Material)mat.AssetObject;
        block.BlockObject = ins.Result;
        ins.Result.transform.position = block.Position;
        ins.Result.name = $"Chunk-{(int)block.InChunk.Position.x}-{(int)block.InChunk.Position.y}-{(int)block.InChunk.Position.z}";
    }

    /// <summary>
    /// 加载方块 
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static async FTask LoadBlock(this Vector3Int pos)
    {
        var block = MV.GetModel<MapModel>()[pos];
        await LoadBlock(block);
    }

    public static (Vector3Int, Vector3Int) CalChunkPos(this Vector3Int worldPos)
    {
        worldPos = worldPos - MapModel.ChunkOrigin;
        //区块坐标系位置计算
        int x, y, z;
        //增量XYZ，使得区块原始点（不是原点）偏移回(0,0,0)
        int xInc = 0, yInc = 0, zInc = 0;
        if (worldPos.x < 0) xInc = 1;
        if (worldPos.y < 0) yInc = 1;
        if (worldPos.z < 0) zInc = 1;
        worldPos += new Vector3Int(xInc, yInc, zInc);
        //记录坐标的符号
        int xSign = Math.Sign(worldPos.x);
        int ySign = Math.Sign(worldPos.y);
        int zSign = Math.Sign(worldPos.z);
        //正值化
        x = Math.Abs(worldPos.x);
        y = Math.Abs(worldPos.y);
        z = Math.Abs(worldPos.z);

        x = worldPos.x % MapModel.ChunkSize.x;
        y = worldPos.y % MapModel.ChunkSize.y;
        z = worldPos.z % MapModel.ChunkSize.z;
        x -= xInc;
        y -= yInc;
        z -= zInc;


        //方块相对于区块原点的坐标
        Vector3Int blockPosInChunk = new Vector3Int(x, y, z);
        //区块原点的坐标
        Vector3Int chunkPos = worldPos - blockPosInChunk;

        return (chunkPos, blockPosInChunk);
    }
}
