using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UniRx.Triggers;

public class UniRxTriggers_1 : MonoBehaviour
{
    [Header("Example 1")]
    [SerializeField] private Button example1Button;
    [SerializeField] private TMP_Text example1Text;
    
    [Header("Example 2")]
    [SerializeField] private Button example2Button;
    [SerializeField] private TMP_Text example2Text;
    
    [Header("Example 3")]
    [SerializeField] private Image example3Image;
    [SerializeField] private TMP_Text example3Text;
    
    [Header("Example 4")]
    [SerializeField] private TMP_InputField example4InputField;
    [SerializeField] private TMP_Text example4Text;
    
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
        example1Button.OnPointerDownAsObservable()
            .Subscribe(x =>
            {
                example1Text.text = "OnPointerDown";
            })
            .AddTo(this);
        
        example1Button.OnPointerUpAsObservable()
            .Subscribe(x =>
            {
                example1Text.text = "OnPointerUp";
                
                // Reset text after 1 second
                hideTextObservable1?.Dispose();
                hideTextObservable1 = Observable.Timer(TimeSpan.FromSeconds(1))
                    .TakeUntil(example1Button.OnPointerDownAsObservable())
                    .Subscribe(xx => example1Text.text = "")
                    .AddTo(this);
            })
            .AddTo(this);
        
        // Example 2:
        example2Button.OnPointerClickAsObservable()
            .Subscribe(x =>
            {
                example2Text.text = "OnPointerClick";
                
                // Reset text after 1 second
                hideTextObservable2?.Dispose();
                hideTextObservable2 = Observable.Timer(TimeSpan.FromSeconds(1))
                    .Subscribe(xx => example2Text.text = "")
                    .AddTo(this);
            })
            .AddTo(this);
        
        // Example 3:
        example3Image.OnPointerEnterAsObservable()
            .Subscribe(x =>
            {
                example3Text.text = "OnPointerEnter";
                example3Image.color = Color.green;
            })
            .AddTo(this);
        
        example3Image.OnPointerExitAsObservable()
            .Subscribe(x =>
            {
                example3Text.text = "OnPointerExit";
                example3Image.color = Color.white;
                
                // Reset text after 1 second
                hideTextObservable3?.Dispose();
                hideTextObservable3 = Observable.Timer(TimeSpan.FromSeconds(1))
                    .TakeUntil(example3Image.OnPointerEnterAsObservable())
                    .Subscribe(xx => example3Text.text = "")
                    .AddTo(this);
            })
            .AddTo(this);
        
        // Example 4:
        example4InputField.onValueChanged.AsObservable()
            .Subscribe(x =>
            {
                example4Text.text = x == string.Empty ? 
                    "" : $"OnValueChanged(): {x}";
            })
            .AddTo(this);
    }
}
