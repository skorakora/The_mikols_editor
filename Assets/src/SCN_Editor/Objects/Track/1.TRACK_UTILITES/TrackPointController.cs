using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPointController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ROOT;

    public void regenerateTrack()
    {

        Track_gen track_Gen = ROOT.GetComponent<Track_gen>();
        track_Gen.generateTrackProcedurally();
    }

}
