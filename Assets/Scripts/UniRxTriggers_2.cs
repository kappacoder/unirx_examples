using TMPro;
using UniRx;
using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx.Triggers;

public class UniRxTriggers_2 : MonoBehaviour
{
    [Header("Example 1")]
    [SerializeField] private Button example1Button;
    [SerializeField] private TMP_Text example1Text;
    
    [Header("Example 2")]
    [SerializeField] private Button example2Button1;
    [SerializeField] private Button example2Button2;
    [SerializeField] private Button example2Button3;
    [SerializeField] private TMP_Text example2Text;
    
    [Header("Example 3")]
    [SerializeField] private Button example3Button1;
    [SerializeField] private Button example3Button2;
    [SerializeField] private Button example3Button3;
    [SerializeField] private TMP_Text example3Text;

    private IDisposable hideTextObservable1;
    private IDisposable hideTextObservable2;
    private IDisposable hideTextObservable3;
    
    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        // Example 1:
        
        // Buffer: https://docs.microsoft.com/en-us/previous-versions/dotnet/reactive-extensions/hh229813(v=vs.103)
        // Creates a buffer that will hold all events that occur during the duration of the timeSpan parameter.
        // This allows an application to buffer items to be delivered in batches ( in this case clicks ).
        
        // Throttle: https://docs.microsoft.com/en-us/previous-versions/dotnet/reactive-extensions/hh229400(v=vs.103)
        // Ignores the values from an observable sequence which are followed by another value
        
        var clickObservable = example1Button.OnPointerClickAsObservable();
        
        clickObservable
            .Buffer(clickObservable.Throttle(TimeSpan.FromMilliseconds(250f)))
            .Where(x => x.Count >= 2)
            .Subscribe(x =>
            {
                example1Text.text = "Double click detected!";
                
                // Reset text after 1 second
                hideTextObservable1?.Dispose();
                hideTextObservable1 = Observable.Timer(TimeSpan.FromSeconds(1f))
                    .Subscribe(xx => example1Text.text = "")
                    .AddTo(this);
            });
        
        // Example 2:
        // When all buttons are clicked

        Observable.WhenAll(
                example2Button1.OnClickAsObservable().First(), 
                example2Button2.OnClickAsObservable().First(),
                example2Button3.OnClickAsObservable().First())
            .First()
            .DelayFrame(1)
            .RepeatSafe()
            .Subscribe(x =>
            {
                example2Button1.interactable = true;
                example2Button2.interactable = true;
                example2Button3.interactable = true;
                
                example2Text.text = "All buttons clicked";
                
                // Reset text after 1 second
                hideTextObservable2?.Dispose();
                hideTextObservable2 = Observable.Timer(TimeSpan.FromSeconds(1f))
                    .Subscribe(xx => example2Text.text = "")
                    .AddTo(this);
            })
            .AddTo(this);
        
        Observable.Merge(
                example2Button1.OnClickAsObservable().Select(x => example2Button1), 
                example2Button2.OnClickAsObservable().Select(x => example2Button2),
                example2Button3.OnClickAsObservable().Select(x => example2Button3))
            .Subscribe(x => x.interactable = false)
            .AddTo(this);
        
        // Example 3:
        // Any of the buttons clicked will trigger the event

        Observable.Merge(
                example3Button1.OnClickAsObservable(), 
                example3Button2.OnClickAsObservable(),
                example3Button3.OnClickAsObservable())
            .Subscribe(x =>
            {
                example3Text.text = "Any of the buttons clicked";
                
                // Reset text after 1 second
                hideTextObservable3?.Dispose();
                hideTextObservable3 = Observable.Timer(TimeSpan.FromSeconds(1f))
                    .Subscribe(xx => example3Text.text = "")
                    .AddTo(this);
            })
            .AddTo(this);
    }
}
