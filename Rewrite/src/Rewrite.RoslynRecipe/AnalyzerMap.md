Types:

R - Review by hand for potential beahavior issues
F - automatic fixup

| Issue                                                        | Type | Notes                                                        |
| ------------------------------------------------------------ | ---- | ------------------------------------------------------------ |
| [ActivitySource.CreateActivity and ActivitySource.StartActivity behavior change](https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/activity-sampling) | R    | ActivityListener.Sample delegate returns or may return ActivitySamplingResult.PropagationData. In .NET 10, Activity.Recorded is now False (was True) when parent is Recorded. This analyzer will report if the return type is `PropagationData` or return value too complex to determine effectively |
|                                                              |      |                                                              |
|                                                              |      |                                                              |

