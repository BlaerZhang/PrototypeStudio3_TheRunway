using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbeManager : MonoBehaviour
{
    public List<ReflectionProbe> airportProbeList;
    public List<ReflectionProbe> sushiProbeList;
    public List<ReflectionProbe> runwayProbeList;
    private GameManager gameManager;
    void Start()
    {
        gameManager = GetComponent<GameManager>();
    }
    
    void Update()
    {
        switch (gameManager.currentSceneIndex)
        {
            case 0:
                foreach (var probe in airportProbeList)
                {
                    probe.importance = 1;
                }

                foreach (var probe in sushiProbeList)
                {
                    probe.importance = 0;
                }

                foreach (var probe in runwayProbeList)
                {
                    probe.importance = 0;
                }
                break;
            case 1:
                foreach (var probe in airportProbeList)
                {
                    probe.importance = 0;
                }

                foreach (var probe in sushiProbeList)
                {
                    probe.importance = 1;
                }

                foreach (var probe in runwayProbeList)
                {
                    probe.importance = 0;
                }
                break;
            case 2:
                foreach (var probe in airportProbeList)
                {
                    probe.importance = 0;
                }

                foreach (var probe in sushiProbeList)
                {
                    probe.importance = 0;
                }

                foreach (var probe in runwayProbeList)
                {
                    probe.importance = 1;
                }
                break;
            case 3:
                foreach (var probe in airportProbeList)
                {
                    probe.importance = 1;
                }

                foreach (var probe in sushiProbeList)
                {
                    probe.importance = 0;
                }

                foreach (var probe in runwayProbeList)
                {
                    probe.importance = 0;
                }
                break;
        }
    }
}
