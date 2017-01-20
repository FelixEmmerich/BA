using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.UI;

public class ScanStateGUI : MonoBehaviour
{
    public Text ScanStateText;
    public Text StatsText;
    private bool _scanning;
    private int _frameCount;
    private IntPtr _statsPtr;

    // Use this for initialization
    void Start()
    {
        _scanning = true;
        SpatialUnderstanding.Instance.ScanStateChanged += OnStateChange;
    }

    void OnStateChange()
    {
        /*if (!_scanning && (int) SpatialUnderstanding.Instance.ScanState > 0)
        {
            _scanning = true;
        }*/
        ScanStateText.text = "Scan state: " + SpatialUnderstanding.Instance.ScanState;
    }

    // Update is called once per frame
    void Update()
    {
        if (_scanning)
        {
            CheckForStats();
        }
    }

    private void CheckForStats()
    {
        if (_statsPtr == IntPtr.Zero)
        {
            _statsPtr = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStatsPtr();
        }
        if (((++_frameCount % 10) == 0) && (SpatialUnderstandingDll.Imports.QueryPlayspaceStats(_statsPtr) != 0))
        {
            var stats = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStats();
            StatsText.text =
            (String.Format("total {0} wall {1} horizontal {2} ceiling {3}", stats.TotalSurfaceArea,
                stats.WallSurfaceArea, stats.HorizSurfaceArea, stats.VirtualCeilingSurfaceArea));
        }
    }
}
