public class TimerTMPText : TMPController
{
    private void Start()
    {
        GetComponent<TimerDisplay>().textUpdateEvent += SetText;
    }
}
