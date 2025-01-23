# cs-http
Download http resource and measure speed with different .NET appoaches.  
All links in [url.txt](url.txt) file.  
For each approach try to download with arbitrary degree of parallelism.  
After download each resource is saved in it folder to check correctness.  
For example, `Task.Run` appoach with 10 tasks for `yandex.ru` url will be saved in `Task.Run\10\yandex.ru.html' file.  
Finally, result of measuaments will be plotted.

## HttpTask class
[HttpTask.cs](HttpTask.cs) class loads [url.txt](url.txt) file at startup.  
Have `synchronous` operation for download using `HttpRequest` .NET class.  
Have `asynchronous` analog for download using `HttpClient` .NET class.  
Both operation saves result in file, perform time measure and update download statistic.
[~task.png](task.png)
Operation have small cpu calculation at begining and ending and a lot of wait period for i/o task.
