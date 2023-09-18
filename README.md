# SignalR using Observable Example

Basic example of using SignalR and Observable.

Example is using **timer** to update 2 Observables. Obsarvables store string format of current local and UTC time.

When **timer** ticks, value in **Observable** is updated which triggers **Subscribers** to push update using **SignalR**.

Example contains **standalone** and **combined** Subscribers.

## Important files

* [Data.TimeService.cs](SignalRTest.Blazor.Server/Data/TimeService.cs)
* [Hubs.ClockHub.cs](SignalRTest.Blazor.Server/Hubs/ClockHub.cs)
* [Pages.Index.razor](SignalRTest.Blazor.Server/Pages/Index.razor)
