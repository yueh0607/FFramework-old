using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    /// <summary>
    /// 方块ID
    /// </summary>
    public int BlockID { get; set; }
    
    /// <summary>
    /// 具体的方块对象
    /// </summary>
    public GameObject BlockObject { get; set; }

    /// <summary>
    /// 方块位置
    /// </summary>
    public Vector3Int Position
    {
        get=>LocalPosition+InChunk.Position;
        set
        {
            LocalPosition = value - InChunk.Position;
        }
    }

    /// <summary>
    /// 方块相对区块原点的位置
    /// </summary>
    public Vector3Int LocalPosition { get; set; }
    /// <summary>
    /// 所属的区块
    /// </summary>
    public Chunk InChunk { get; set; } = null;

}
