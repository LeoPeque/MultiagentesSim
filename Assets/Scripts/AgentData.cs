using System;
using System.Collections.Generic;

[Serializable]
public class AgentData
{
    public int numberOfAgents;
    public List<int[]> initialPositions;
    public List<int[]> objectives;
}