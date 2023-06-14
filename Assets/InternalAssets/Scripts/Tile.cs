using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Tile : MonoBehaviour
{
    MeshRenderer meshRenderer;

    [SerializeField]
    TextMesh textMesh;
    public Material Material => meshRenderer.material;

    int tileScore;
    public int TileScore
    {
        get => tileScore;
        set
        {
            tileScore = value;
            textMesh.text = tileScore.ToString();

        }
    }
    TileState tileState = TileState.Free;
    public TileState GetTileState
    {
        get => tileState;
        set => tileState = value;
    }

    //class MyVar<T>
    //{
    //    private T value;
    //    System.Action<T> getter, setter;
    //    public T Value
    //    {
    //        get { getter?.Invoke(value);
    //            return value; }

    //        set { setter?.Invoke(value);
    //            this.value = value; }
    //    }
    //    private MyVar() { }
    //    public MyVar(System.Action<T> get, System.Action<T> set, T startValue)
    //    {
    //        getter = get; setter = set; value = startValue;
    //    }
    //}
    //MyVar<int> tileScores;
    //void GetterCallback(int val)
    //{ Debug.Log("GetterNotify"); }
    //void SetterCallback(int val)
    //{ textMesh.text = tileScore.ToString(); }

    private void Awake()
    {
        //tileScores = new MyVar<int>(GetterCallback, SetterCallback, 0);
        //tileScores.Value = 0;

        meshRenderer = GetComponent<MeshRenderer>();
        tileState = TileState.Free;
        TileScore = 0;
    }
    public enum TileState
    {
        Free,
        Occupied
    }

}
