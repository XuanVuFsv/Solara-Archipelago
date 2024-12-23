using VitsehLand.Scripts.Pattern.Singleton;
using VitsehLand.Scripts.Ultilities;

public class UtilitiesManager: Singleton<UtilitiesManager>
{
    public bool ENABLE_DEBUG_LOG;

    // Start is called before the first frame update
    void Start()
    {
        MyDebug.enableDebugLog = ENABLE_DEBUG_LOG;
    }
}
