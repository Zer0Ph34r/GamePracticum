using UnityEngine;
using UnityEngine.Networking;

public struct GemEnumStruct
{

    #region Fields

    public Vector3 Position { get; set; }
    public GlobalVariables.GemTypes gemType { get; set; }

    #endregion

}

public class GemSyncList : SyncListStruct<GemEnumStruct>
{ }