## Rewrite C#

Implements OpenRewrite support for the C# language.
Most of OpenRewrite including the core framework is Java-based, so a remoting mechanism is used to communicate between the Java and C# runtimes.

## Project Folders Structure

* `Rewrite` - OpenRewrite port to C#
* `rewrite-csharp` - module with pure C# related LST classes written in Java.
* `rewrite-csharp-remoting` - module with the generated C# serializers/deserializers classes required for cross-process communication
* `rewrite-test-engine-remote` - test engine used to power the OpenRewrite LST test written in C#
* `rewrite-csharp-remote-server` - the embedded C# based tcp server zipped dlls required to run C# lang server during CLI executions (build/recipe run/ etc )
