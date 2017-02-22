
public class TimerScript {

    #region Fields

    // time limit for given timer
    int m_time;

    // updated time
    float currTime = 0;

    // bool for checking timer state (started/stopped)
    bool started = false;

    public delegate void FireMethod();
    public static event FireMethod timerEnded;

    #endregion

    #region Constructor

    /// <summary>
    /// Creates new timer with given time
    /// </summary>
    public TimerScript(int time)
    {
        m_time = time;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Start Timer
    /// </summary>
    public void StartTimer()
    {
        started = true;
    }

    /// <summary>
    /// Changes amount of time in given timer and resets timer
    /// WARNING: If you need different times, you should use different timers in most cases
    /// </summary>
    /// <param name="newTime"></param>
    public void ChangeTime(int newTime)
    {
        m_time = newTime;
        currTime = 0;
    }

    /// <summary>
    /// Pause a started timer
    /// </summary>
    public void PauseTimer()
    {
        started = false;
    }

    /// <summary>
    /// updates timer with given time and fires event when timer is up
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Update(float deltaTime)
    {
        // update only if timer has started
        if (started)
        {
            currTime += deltaTime;
        }
        // check if timer has reached it's max time
        if (currTime >= m_time)
        {
            timerEnded();
            started = false;
            currTime = 0;
        }
    }
    
    #endregion

}
