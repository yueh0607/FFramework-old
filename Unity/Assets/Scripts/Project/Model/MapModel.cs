using FFramework;
using FFramework.MVVM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapModel : IModel
{



    /// <summary>
    /// 固定的区块尺寸
    /// </summary>
    public static readonly Vector3Int ChunkSize = new Vector3Int(16, 16, 16);
    /// <summary>
    /// 任意区块原点的位置（用于确定整个世界区块偏移体系）
    /// </summary>
    public static readonly Vector3Int ChunkOrigin = new Vector3Int(0, 0, 0);

    /// <summary>
    /// 加载的区块数量
    /// </summary>
    public static readonly int ChunkLoadCount = 3;

    //区块映射（key为区块xyz之和最小的立方体角处的方块）
    private Dictionary<Vector3Int, Chunk> chunks = new Dictionary<Vector3Int, Chunk>();

    public MapModel()
    {

    }

    /// <summary>
    /// 获取位置对应的区块
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public Block this[Vector3Int worldPos]
    {

        get
        {
            var tuple = worldPos.CalChunkPos();
            //方块相对于区块原点的坐标
            Vector3Int blockPosInChunk = tuple.Item2;
            //区块原点的坐标
            Vector3Int chunkPos = tuple.Item1;
            if (!chunks.ContainsKey(chunkPos)) return null;
            if (blockPosInChunk.x > MapModel.ChunkSize.x || blockPosInChunk.y > MapModel.ChunkSize.y || blockPosInChunk.z > MapModel.ChunkSize.z)
            {
                return null;
            }
            return chunks[chunkPos][blockPosInChunk];

        }
    }
    /// <summary>
    /// 获取对应位置的方块
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public Block this[int x, int y, int z]
    {
        get
        {
            return this[new Vector3Int(x, y, z)];
        }
    }

    public void AddChunk(Chunk chunk)
    {
        chunks.Add(chunk.Position, chunk);
    }

    public Chunk GetChunk(Vector3Int pos)
    {
        if (!chunks.ContainsKey(pos)) return null;
        return chunks[pos];
    }

    /// <summary>
    /// 通知某个方块发生更新
    /// </summary>
    /// <param name="pos"></param>
    public void NotifyUpdate(Vector3Int pos)
    {
        //启用六面临接方块的MeshRenderer和Collider
        pos.GetOffsetBlock(Vector3Int.left)?.SetMeshAndCollisionEnable(true);
        pos.GetOffsetBlock(Vector3Int.right)?.SetMeshAndCollisionEnable(true);
        pos.GetOffsetBlock(Vector3Int.up)?.SetMeshAndCollisionEnable(true);
        pos.GetOffsetBlock(Vector3Int.down)?.SetMeshAndCollisionEnable(true);
        pos.GetOffsetBlock(Vector3Int.forward)?.SetMeshAndCollisionEnable(true);
        pos.GetOffsetBlock(Vector3Int.back)?.SetMeshAndCollisionEnable(true);
    }
}
