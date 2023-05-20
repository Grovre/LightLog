# LightLog
A lightweight logger created with dependency injection in mind.
It is done for the most part.

# Loggers
There are different kinds of loggers that come with this library. Here they are and what they do:

1. The basic logger `Logger`: This logger has the most flexibility as it is single-threaded. The AsyncLogger was recently merged with this Logger type so both synchronous and asynchronous methods are available depending on the required use case. An available option with this logger is the provided pre and post log events so users of the library can have their own things happen right before and right after a log happens.
2. The toggle logger `ToggleLogger`: Backed by any kind of other ILogger, comes with an internal boolean state that determines whether or not a log will happen. Has a backing ILogger that all methods are delegated to. To get this, use the "UseForDebugging" extension method available to all ILogger implementations. This one can be good for debugging. One example of using this logger for debugging is using the DEBUG preprocessor flag to enable logging, otherwise leave disabled. The only loss in performance at that point is a boolean check that is guaranteed to be false. If the boolean is guaranteed to be false, so is the return type. That means the JIT will most likely optimize it so it doesn't affect anything.

The package can be found on [NuGet here](https://www.nuget.org/packages/LighterLog/)