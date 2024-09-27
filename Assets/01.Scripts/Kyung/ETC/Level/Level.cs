using System;

[Serializable]
public class Level 
{
    public int Count = 0;
    public bool Clear;

    public Level(int cnt)
    {
        Count = cnt;
    }
}
