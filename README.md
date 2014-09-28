MockableStatics
===============

A .NET library for converting static classes into interfaces to enable stubbing and mocking

.NET static classes are not unit test friendly. For example System.IO.File.

This tool enumerates all .NET static classes using reflection and generates interfaces and implementations that wrap the static classes.

This allows mocking and testing frameworks to replace the interfaces with mocks, stubs and spies.
