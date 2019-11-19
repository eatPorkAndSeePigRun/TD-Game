using UnityEngine;

public class TileObject : MonoBehaviour
{
    public static TileObject Instance = null;

    public LayerMask tileLayer;
    public float tileSize = 1;
    public int xTileCount = 2;
    public int zTileCount = 2;

    // 格子的数值，0表示锁定，无法摆放物体；1表示敌人通道，2表示可摆放防守单位
    public int[] data;
    [HideInInspector]
    public int dataID = 0;

    [HideInInspector]
    public bool debug = false;

    private void Awake()
    {
        Instance = this;
    }

    public void Reset()
    {
        data = new int[xTileCount * zTileCount];
    }

    public int getDataFromPosition(float pox, float poz)
    {
        int index = (int)((pox - transform.position.x) / tileSize) * zTileCount + 
            (int)((poz - transform.position.z) / tileSize);
        if (index < 0 || index >= data.Length)
            return 0;
        return data[index];
    }

    public void setDataFromPosition(float pox, float poz, int number)
    {
        int index = (int)((pox - transform.position.x) / tileSize) * zTileCount +
            (int)((poz - transform.position.z) / tileSize);
        if (index < 0 || index >= data.Length)
            return;
        data[index] = number;
    }

    private void OnDrawGizmos()
    {
        if (!debug)
            return;

        if (data == null)
        {
            Debug.Log("Please reset data first");
            return;
        }

        Vector3 pos = transform.position;
        for (int i = 0; i < xTileCount; i++)    // 画z方向轴辅助线
        {
            Gizmos.color = new Color(0, 0, 1, 1);
            Gizmos.DrawLine(pos + new Vector3(tileSize * i, pos.y, 0),
                transform.TransformPoint(tileSize * i, pos.y, tileSize * zTileCount));
            for (int k = 0; k < zTileCount; k++)    // 高亮当前数值格子
            {
                if ((i * zTileCount + k) < data.Length &&
                    data[i * zTileCount + k] == dataID)
                {
                    Gizmos.color = new Color(1, 0, 0, 0.3f);
                    Gizmos.DrawCube(new Vector3(pos.x + i * tileSize + tileSize * 0.5f, pos.y, 
                        pos.z + k * tileSize + tileSize * 0.5f), new Vector3(tileSize, 0.2f, tileSize));
                }
            }

            for(int k = 0; k < zTileCount; k++)     // 画x方向轴辅助线
            {
                Gizmos.color = new Color(0, 0, 1, 1);
                Gizmos.DrawLine(pos + new Vector3(0, pos.y, tileSize * k),
                    this.transform.TransformPoint(tileSize * xTileCount, pos.y, tileSize * k));
            }
        }
    }
}
