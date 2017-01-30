# Portable WCF Sandbox

This is intended as a minimal reproducible test case for an apparent bug when using a .Net Standard 1.3 library to access a WCF service and invoking that library from a Xamarin Android project.

I'm not entirely sure whether the bug is in the .Net WCF libraries ([System.ServiceModel.Http](https://www.nuget.org/packages/System.ServiceModel.Http/) or dependencies) or in the Xamarin Android implementation of the .Net Standard 1.3 API, although I incline towards the latter.

`DemoService.WCF` is hosted on Azure. `FrameworkClient` can successfully invoke it both synchronously and asynchronously. `AndroidClientIndirect` calling via `NetStandardClientLib` can successfully invoke it synchronously, but when it tries asynchronously an exception is thrown. The core of the exception is

> `Error in deserializing body of request message for operation 'Reverse'. OperationFormatter encountered an invalid Message body. Expected to find node type 'Element' with name 'Reverse' and namespace 'urn:fdc:cheddarmonk.org:2017:Demo'. Found node type 'Element' with name 'ReverseAsync' and namespace 'urn:fdc:cheddarmonk.org:2017:Demo'`

It seems that there's a difference in the way the underlying channel implementation converts the method name into a SOAP body: removing the `Async` in the .Net Framework implementation but not in the Xamarin Android implementation.
The reason I'm not sure whose fault it is is that `System.ServiceModel` doesn't appear to be mentioned at all in the [.Net Standard 1.3 API spec](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard1.3_ref.md). But it does look like a native method (`__icall_wrapper_mono_remoting_wrapper`) which does the work.

Full stack trace:

>     System.AggregateException: One or more errors occurred. ---> System.ServiceModel.FaultException`1[System.ServiceModel.ExceptionDetail]: Error in deserializing body of request message for operation 'Reverse'. OperationFormatter encountered an invalid Message body. Expected to find node type 'Element' with name 'Reverse' and namespace 'urn:fdc:cheddarmonk.org:2017:Demo'. Found node type 'Element' with name 'ReverseAsync' and namespace 'urn:fdc:cheddarmonk.org:2017:Demo'
      at (wrapper managed-to-native) System.Object:__icall_wrapper_mono_remoting_wrapper (intptr,intptr)
      at (wrapper remoting-invoke) FrameworkClient.Bar.IDemoService:ReverseAsync (string)
      at NetStandardClientLib.DemoServiceProxy+<ReverseAsync>d__3.MoveNext () [0x0002e] in C:\Users\pjt33\Documents\Visual Studio 2015\Projects\PortableWCFSandbox\NetStandardClientLib\DemoServiceProxy.cs:51 
       --- End of inner exception stack trace ---
      at System.Threading.Tasks.Task.ThrowIfExceptional (System.Boolean includeTaskCanceledExceptions) [0x00014] in /Users/builder/data/lanes/3511/501e63ce/source/mono/mcs/class/referencesource/mscorlib/system/threading/Tasks/Task.cs:2157 
      at System.Threading.Tasks.Task`1[TResult].GetResultCore (System.Boolean waitCompletionNotification) [0x00034] in /Users/builder/data/lanes/3511/501e63ce/source/mono/mcs/class/referencesource/mscorlib/system/threading/Tasks/Future.cs:562 
      at System.Threading.Tasks.Task`1[TResult].get_Result () [0x00000] in /Users/builder/data/lanes/3511/501e63ce/source/mono/mcs/class/referencesource/mscorlib/system/threading/Tasks/Future.cs:532 
      at AndroidClientIndirect.MainActivity.OnCreate (Android.OS.Bundle bundle) [0x0006e] in C:\Users\pjt33\Documents\Visual Studio 2015\Projects\PortableWCFSandbox\AndroidClientIndirect\MainActivity.cs:28 
    ---> (Inner Exception #0) System.ServiceModel.FaultException`1[System.ServiceModel.ExceptionDetail]: Error in deserializing body of request message for operation 'Reverse'. OperationFormatter encountered an invalid Message body. Expected to find node type 'Element' with name 'Reverse' and namespace 'urn:fdc:cheddarmonk.org:2017:Demo'. Found node type 'Element' with name 'ReverseAsync' and namespace 'urn:fdc:cheddarmonk.org:2017:Demo' (Fault Detail is equal to Error in deserializing body of request message for operation 'Reverse'. OperationFormatter encountered an invalid Message body. Expected to find node type 'Element' with name 'Reverse' and namespace 'urn:fdc:cheddarmonk.org:2017:Demo'. Found node type 'Element' with name 'ReverseAsync' and namespace 'urn:fdc:cheddarmonk.org:2017:Demo').<---