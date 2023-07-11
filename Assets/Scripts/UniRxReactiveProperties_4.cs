using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UniRxReactiveProperties_4 : MonoBehaviour
{
    // Reactive properties can be int, float, bool, char, string and whatever else you imagine :) ( also custom classes )
    // Always add "RX" at the end of reactive properties so that we know they are reactive
    public IReactiveProperty<int> HealthRX;
    
    // In reality we would probably have a class UserData
    // And this will be IReactiveCollection<UserData> UsersRX;
    public IReactiveCollection<string> UsernamesRX;

    // Imagine we are receiving messages from our backend server
    // This subject gets fired every time a new event was received
    public ISubject<string> OnNewMessageReceivedRX;
    
    [Header("Example 1")] 
    [SerializeField] private Button example1Button;
    [SerializeField] private TMP_Text example1Text;
    
    [Header("Example 2")]
    [SerializeField] private Button example2Button;
    [SerializeField] private TMP_Text example2Text;
    
    [Header("Example 3")]
    [SerializeField] private Button example3Button;
    [SerializeField] private TMP_Text example3Text;

    private void Start()
    {
        HealthRX = new ReactiveProperty<int>();
        UsernamesRX = new ReactiveCollection<string>();
        OnNewMessageReceivedRX = new Subject<string>();
        
        Subscribe();
    }

    private void Subscribe()
    {
        // Example 1: Clicking the button sets HealthRX to random value

        example1Button.OnClickAsObservable()
            .Subscribe(x =>
            {
                HealthRX.Value = Random.Range(0, 100);
            })
            .AddTo(this);
        
        // Imagine this is in another script, we listen for changes to HealthRX
        
        // Note that when you launch the scene, the default health ( 0 ) gets displayed instantly
        // If we want to skip the first default value just add Skip(1) - HealthRX.Skip(1).Where...
        HealthRX.Where(health => health <= 50)
            .Subscribe(health =>
            {
                example1Text.text = health.ToString();
                
                example1Text.color = Color.red;
            })
            .AddTo(this);
        
        HealthRX.Where(health => health > 50)
            .Subscribe(health =>
            {
                example1Text.text = health.ToString();

                example1Text.color = Color.green;
            })
            .AddTo(this);
            
       // Example 2: Clicking the button adds a string to the reactive collection
       
       example2Button.OnClickAsObservable()
           .Subscribe(x =>
           {
               UsernamesRX.Add("user_" + Random.Range(99, 999));
           })
           .AddTo(this);
       
       // Imagine this is in another script, we listen for changes to UsernamesRX
       // You can ObserveAdd(), ObserveMove(), ObserveRemove(), ObserveCountChanged(), ObserveReplace(), ObserveEveryValueChanged()
       UsernamesRX.ObserveAdd()
           .Subscribe(newUsername =>
           {
               example2Text.text = example2Text.text + newUsername.Value + "\n";
           })
           .AddTo(this);
       
       // Example 3: Clicking the button fires a string event
       
       example3Button.OnClickAsObservable()
           .Subscribe(x =>
           {
               OnNewMessageReceivedRX.OnNext("MessageFromServer_" + Random.Range(0, 999));
           })
           .AddTo(this);
       
       // Imagine this is in another script, we listen for fired events from the server
       OnNewMessageReceivedRX
           .Subscribe(x =>
           {
               example3Text.text = x;
           })
           .AddTo(this);
       
       // Also, check ReplaySubject() - it will fire all previous events to subscribers
    }
}
