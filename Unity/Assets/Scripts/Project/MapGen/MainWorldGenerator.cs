using FFramework.MVVM;
using FFramework.RefCache;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using YooAsset;

/// <summary>
/// 主世界生成器
/// </summary>
public class MainWorldGenerator
{
    /// <summary>
    /// 地图尺寸
    /// </summary>
    public Vector3Int Size { get; set; }

    /// <summary>
    /// 大概空气高度占比
    /// </summary>
    public float AirRatio { get; set; }
    public MainWorldGenerator(int width = 1600, int height = 16, int length = 1600, float airRatio = 0.6f)
    {
        if (Size.x % MapModel.ChunkSize.x != 0 || Size.y % MapModel.ChunkSize.y != 0 || Size.z % MapModel.ChunkSize.z != 0)
        {
            throw new Exception("地图尺寸必须是区块尺寸的整数倍");
        }
        Size = new Vector3Int(length, height, width);
        AirRatio = airRatio;
    }


    public async Task Generate(Action<float> progress)
    {
        var map = MV.GetModel<MapModel>();
        await Task.Run( () =>
        {
            var model = MV.GetModel<BasicBlockPropertiesModel>();
           

            for (int x = 0; x < Size.x; x += MapModel.ChunkSize.x)
            {
                for (int z = 0; z < Size.z; z += MapModel.ChunkSize.z)
                {
                    for (int y = 0; y < Size.y; y += MapModel.ChunkSize.y)
                    {

                        Chunk chunk = new Chunk(MapModel.ChunkSize.x, MapModel.ChunkSize.z, MapModel.ChunkSize.y);
                        chunk.Position = new Vector3Int(x, y, z);
                        
                        for (int localY = 0; localY < MapModel.ChunkSize.y; localY++)
                        {
                            for (int localX = 0; localX < MapModel.ChunkSize.x; localX++)
                            {
                                for (int localZ = 0; localZ < MapModel.ChunkSize.z; localZ++)
                                {

                                    var item = model.Data[1];
                                    Block block = new Block();
                                    block.InChunk = chunk;
                                    block.BlockID = item.id;
                                    block.LocalPosition = new Vector3Int(localX, localY, localZ);


                                    chunk[localX, localY, localZ] = block;
                                    if (localY > MapModel.ChunkSize.y / 4) chunk[localX, localY, localZ] = null;
                                }
                            }
                        }
                        map.AddChunk(chunk);
                    }
                }
                progress?.Invoke((float)x / (float)Size.x);
               
            }
        });

        
    }
}
