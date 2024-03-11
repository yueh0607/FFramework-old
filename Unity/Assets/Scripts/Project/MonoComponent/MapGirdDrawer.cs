using System.Collections.Generic;
using UnityEngine;



//按此方式接入MONO脚本


public class MapGirdDrawer : MonoBehaviour
{
    //网格大小
    public float MeshSize = 20;
    //单个网格大小
    public float CellSize = 1;
    //网格材质
    public Material LineMat;
    //网格中心点
    public Vector3 Center = Vector3.zero;
    //网格颜色
    public Color MeshColor;
    //网格线坐标存储
    List<Vector3[]> m_linePoints = new List<Vector3[]>();

    // Start is called before the first frame update
    void Start()
    {
        Initialized();
    }

    //初始化网格坐标
    void Initialized()
    {
        //计算有多少横线
        int rowCount = (int)(MeshSize / CellSize) + 1;

        //得到横线的起始、终止坐标
        for (int i = 0; i < rowCount; i++)
        {
            Vector3[] points = new Vector3[2];
            points[0] = new Vector3(-MeshSize / 2, 0, MeshSize / 2 - CellSize * i) + Center;
            points[1] = new Vector3(MeshSize / 2, 0, MeshSize / 2 - CellSize * i) + Center;

            m_linePoints.Add(points);
        }

        //得到竖线的起始、终止坐标
        for (int i = 0; i < rowCount; i++)
        {
            Vector3[] points = new Vector3[2];
            points[0] = new Vector3(-MeshSize / 2 + CellSize * i, 0, MeshSize / 2) + Center;
            points[1] = new Vector3(-MeshSize / 2 + CellSize * i, 0, -MeshSize / 2) + Center;

            m_linePoints.Add(points);
        }

        //修改网格材质颜色
        LineMat.SetColor("_Color", MeshColor);
    }

    /// <summary>
    /// 照相机完成场景渲染后调用
    /// </summary>
    void OnPostRender()
    {
        //线条材质
        LineMat.SetPass(0);
        GL.PushMatrix();
        //线条颜色,当前材质下，该方式修改颜色无效，详情可以看官方文档
        //GL.Color(MeshColor);
        //绘制线条
        GL.Begin(GL.LINES);

        //所有线条 (两点一条线)
        for (int i = 0; i < m_linePoints.Count; i++)
        {
            GL.Vertex(m_linePoints[i][0]);
            GL.Vertex(m_linePoints[i][1]);
        }
        GL.End();
        GL.PopMatrix();
    }
}