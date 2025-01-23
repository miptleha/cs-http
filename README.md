# cs-http
Download http resource and measure speed with different .NET approaches.  
All links in [url.txt](url.txt) file.  
For each approach try to download with arbitrary degree of parallelism.  
After download each resource is saved in it folder to check correctness.  
For example, `Task.Run` approach with 10 tasks for `yandex.ru` url  
will be saved in `Task.Run\10\yandex.html` file.  
Suffix will be added for same resources, bad requests will have empty files.  
Finally, result of measurements will be plotted.

## HttpTask class
[HttpTask.cs](HttpTask.cs) class loads [url.txt](url.txt) file at startup.  
Have `synchronous` operation for download using `HttpRequest` .NET class.  
Have `asynchronous` analog for download using `HttpClient` .NET class.  
Both operation saves result in file, perform time measure and update download statistic.
![task.png](task.png)  
Operation have small cpu calculation at starting and ending and a lot of wait period for i/o task.

## How to run

Some features are not implemented yet (plot not ready).  
Try current application:  
```
dotnet run
```
