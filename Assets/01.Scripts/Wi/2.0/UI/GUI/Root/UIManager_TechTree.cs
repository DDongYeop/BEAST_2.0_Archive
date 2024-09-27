
public class UIManager_TechTree : UI_Root
{
    public static UIManager_TechTree Instance = null;

    protected override void Awake()
    {
        base.Awake();

        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
}
