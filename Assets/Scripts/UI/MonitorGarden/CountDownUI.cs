using UnityEngine;

namespace VitsehLand.Scripts.UI.MonitorGarden
{
    public class CountDownUI : MonoBehaviour
    {
        public float start;
        public float end;
        public float timeElapsed, t;
        public float duration;
        public bool startCountdown = false;

        // Update is called once per frame
        void Update()
        {
            if (startCountdown)
            {
                t += Time.deltaTime;
                timeElapsed = Mathf.Lerp(start, end, t / duration);
                if (timeElapsed >= end) EndCountDown();
            }
        }

        public int GetTimeElapsed()
        {
            return (int)(end - timeElapsed);
        }

        public void StartCountDown(int _end)
        {
            //Debug.Log("Start");
            end = _end;
            duration = end - start;
            timeElapsed = 0;
            t = 0;
            startCountdown = true;
        }

        public void EndCountDown()
        {
            //Debug.Log("End");
            startCountdown = false;
        }

        public void ResetCountDown()
        {
            startCountdown = false;
            start = 0;
            end = 0;
            timeElapsed = 0;
            duration = 0;
            t = 0;
        }
    }
}