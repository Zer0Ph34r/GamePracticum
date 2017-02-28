using UnityEngine;

public class GemEnumScript : MonoBehaviour {

    #region Gem Type Enum

    public enum GemTypes {White, Yellow, Blue, Green, Red, Purple }

    #endregion

    #region Fields

    public Vector3 Position { get; set; }
    public GemTypes gemType { get; set; }

    #endregion

    #region Constructor

    public GemEnumScript(Vector3 InitialPosition, GemTypes GemType)
    {
        Position = InitialPosition;
        gemType = GemType;
    }

    #endregion

    #region Methods

    //public GameObject CreateGem()
    //{
    //    GameObject gem;

    //    switch

    //    return gem;
    //}

    #endregion

}
