using TMPro;
using UniRx;
using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UniRxTimers_3 : MonoBehaviour
{
    [Header("Example 1")]
    [SerializeField] private Button example1Button;
    
    [Header("Example 2")]
    [SerializeField] private TMP_Text example2Text;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        // Example 1: 0.5 seconds after clicking, changes randomly the x-position of the button
        // 2 Ways to do this:
        float y = example1Button.transform.localPosition.y;

        // Option 1: with Observable.Timer()
        example1Button.OnClickAsObservable()
            .Subscribe(x =>
            {
                // Timers can be linked to Unity's timeScale ( by default ) or be independent of it ( very important! )
                // to ignore time scale -> Observable.Timer(TimeSpan.FromSeconds(0.5f), Scheduler.MainThreadIgnoreTimeScale)
                Observable.Timer(TimeSpan.FromSeconds(0.5f))
                    .Subscribe(_ =>
                    {
                        example1Button.transform.localPosition = new Vector2(Random.Range(-300, 300), y);
                    })
                    .AddTo(this);
            })
            .AddTo(this);

        // Option 2: with Delay()
        /*
            example1Button.OnClickAsObservable()
                .Delay(TimeSpan.FromSeconds(0.5f))
                .Subscribe(x =>
                {
                    example1Button.transform.localPosition = new Vector2(Random.Range(-300, 300), y);
                })
            .AddTo(this);
        */

        // Example 2: Counter
        Observable.Interval(TimeSpan.FromSeconds(1f))
            .Subscribe(x =>
            {
                example2Text.text = x + " seconds passed";
            })
            .AddTo(this);
        
        // Example 3: Observe on EveryUpdate(), EveryLateUpdate(), EveryFixedUpdate()
        // This way you can have manager classes that are not MonoBehaviours ( no need to attach them in scenes )
        // And still can access Unity's Update calls
        Observable.EveryUpdate()
            .Subscribe(x =>
            {
                // Do something every update
            })
            .AddTo(this);
    }
}
