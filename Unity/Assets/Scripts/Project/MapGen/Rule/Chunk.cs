using FFramework;
using FFramework.MVVM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 区块
/// </summary>
public class Chunk
{

    Block[,,] blocks;

    /// <summary>
    /// 区块世界坐标
    /// </summary>
    public Vector3Int Position { get; set; }


    /// <summary>
    /// 区块默认大小为16*16
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public Chunk(int length = 16, int width = 16, int height = 16)
    {
        blocks = new Block[length, height, width];

    }


    public Block this[int x, int y, int z]
    {
        get
        {
            return blocks[x, y, z];
        }
        set
        {
            blocks[x, y, z] = value;
        }
    }
    public Block this[Vector3Int pos]
    {
        get
        {
            return blocks[pos.x, pos.y, pos.z];
        }
        set
        {
            blocks[pos.x, pos.y, pos.z] = value;
        }
    }
    public void UnloadChunk()
    {
        var map = MV.GetModel<MapModel>();
        for (int x = 0; x < blocks.Length; x++)
        {
            for (int y = 0; y < blocks.Length; y++)
            {
                for (int z = 0; z < blocks.Length; z++)
                {
                    map[x, y, z].SetMeshAndCollisionEnable(false);
                }
            }
        }
    }
    public async FTask DeleteChunk()
    {
        await FTask.CompletedTask;
    }


    public bool IsLoaded { get; private set; } = false;
    public bool IsLoading { get; private set; } = false;

    /// <summary>
    /// 显示可见方块/隐藏不可见方块
    /// </summary>
    /// <param name="reverse"></param>
    private void SetVisableBlockState(bool reverse = false)
    {
        var map = MV.GetModel<MapModel>();

        //显示
        if (!reverse)
        {
            for (int x = 0; x < MapModel.ChunkSize.x; x++)
            {
                for (int y = 0; y < MapModel.ChunkSize.y; y++)
                {
                    for (int z = 0; z < MapModel.ChunkSize.z; z++)
                    {
                        if (map[x, y, z] != null)
                            //如果六面都有不透明方块则不加载
                            if (!map[x, y, z].Position.IsSixFaceExist())
                            {
                                map[x, y, z].SetMeshAndCollisionEnable(true);
                            }
                    }
                }
            }
        }
        else
        {
            for (int x = 0; x < MapModel.ChunkSize.x; x++)
            {
                for (int y = 0; y < MapModel.ChunkSize.y; y++)
                {
                    for (int z = 0; z < MapModel.ChunkSize.z; z++)
                    {
                        if (map[x, y, z] != null)
                            //如果六面都有不透明方块则剔除
                            if (map[x, y, z].Position.IsSixFaceExist())
                            {
                                map[x, y, z].SetMeshAndCollisionEnable(false);
                            }
                    }
                }
            }
        }

    }
    /// <summary>
    /// 加载区块
    /// </summary>
    /// <returns></returns>
    public async FTask CreateChunk()
    {
        if (IsLoading|| IsLoaded) return;
        IsLoading = true;
   
        List<FTask> tasks = new List<FTask>();
        for (int x = 0; x < MapModel.ChunkSize.x; x++)
        {
            for (int y = 0; y < MapModel.ChunkSize.y; y++)
            {
                for (int z = 0; z < MapModel.ChunkSize.z; z++)
                {
                    if (blocks[x, y, z] == null)
                    {
                        continue;
                    }
                    var task = blocks[x, y, z].LoadBlock();
                    tasks.Add(task);

                }
            }
        }
        await FTask.WaitAll(tasks);
        //隐藏不可见方块
        SetVisableBlockState(true);
        IsLoaded = true;
        IsLoading = false;
    }
}
