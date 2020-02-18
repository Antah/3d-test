using UnityEngine;

public class BlockColorChanger : MonoBehaviour
{
    public GameObject LeftRing;
    public Transform RightRing;
    public Renderer Table;
    public Material[] Materials;
    public bool Marked;

    enum BlockColors
    {
        Red = 0,
        Green = 1,
        Blue = 2,
    }

    void UpdateColor(BlockColors color)
    {
        if (!Marked)
        {
            switch (color)
            {
                case BlockColors.Red:
                    LeftRing.GetComponent<Renderer>().material = Materials[0];
                    break;
                case BlockColors.Green:
                    RightRing.GetComponent<Renderer>().material = Materials[1];
                    break;
                case BlockColors.Blue:
                    Table.material = Materials[2];
                    break;
            }
        }
    }

    public void upcol(int c)
    {
        UpdateColor((BlockColors)c);
    }
}