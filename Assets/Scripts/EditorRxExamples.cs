using System;
using UniRx;
using UnityEditor;
using UnityEngine;

public static class EditorRxExamples
{
    [MenuItem("RxExamples/CreateExample", priority = 1)]
    static void CreateExample()
    {
        var myObservable = Observable.Create<int>((obs) =>
        {
            obs.OnNext(12);
            obs.OnNext(23);
            obs.OnError(new Exception("SOMETHING HORRIBLE HAPPENED"));
            // No information will be emitted after the error
            obs.OnNext(36);
            obs.OnCompleted();

            return obs as IDisposable;
        });

        myObservable
            .Subscribe(value =>
            {
                Debug.Log(value);
            }, onError: (e) =>
            {
                Debug.Log(e.Message);
            }, onCompleted: () =>
            {
                Debug.Log("ON COMPLETED");
            });
    }

    [MenuItem("RxExamples/FilterExample", priority = 2)]
    static void FilterExample()
    {
        var myObservable = Observable.Interval(TimeSpan.FromSeconds(1));

        myObservable
            .Select(value => value * 10)
            .Where(value => value <= 50)
            // .Skip(2)
            // .Take(2)
            .Subscribe(value =>
            {
                Debug.Log(value);
            }, onCompleted: () =>
            {
                Debug.Log("ON COMPLETED");
            });
    }

    [MenuItem("RxExamples/CombineExample", priority = 3)]
    static void CombineExample()
    {
        var firstObservable = Observable.Interval(TimeSpan.FromMilliseconds(250)).Take(8);
        var secondObservable = Observable.Interval(TimeSpan.FromSeconds(1)).Take(4);

        Observable.Merge(firstObservable, secondObservable)
            .Subscribe(value =>
            {
                Debug.Log(value);
            });

        // Observable.Merge(firstObservable.Select(x => "first Observable: " + x), secondObservable.Select(x => "second Observable: " + x))
        //     .Subscribe(value =>
        //     {
        //         Debug.Log(value);
        //     });
        //
        // Observable.Concat(firstObservable.Select(x => "first Observable: " + x), secondObservable.Select(x => "second Observable: " + x))
        //     .Subscribe(value =>
        //     {
        //         Debug.Log(value);
        //     });
    }

    [MenuItem("RxExamples/ScanExample", priority = 4)]
    static void ScanExample()
    {
        var firstObservable = Observable.Interval(TimeSpan.FromMilliseconds(250)).Take(8);

        firstObservable.Scan((acc, curr) => acc + curr)
            .Subscribe(value =>
            {
                Debug.Log(value);
            });
    }
}
