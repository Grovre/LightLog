# LightLog
A lightweight logger created with dependency injection in mind.
It is done for the most part.

# Loggers
There are different kinds of loggers that come packed. Here they are and their differences:

1. The basic logger `Logger`: This logger has the most flexibility as it is single-threaded. It is also the only one capable of having its output redirected to another TextWriter.
2. The async logger `AsyncLogger`: It has the same capabilities as the basic logger but is unable to have its output redirected. When a thread logs using this logger, it will begin a task that adds the message to a blocking collection backed by a concurrent queue that is constantly emptied by an independent thread. The thread that writes to the TextWriter is blocked while there are no queued logs.
3. The toggle logger `ToggleLogger`: Backed by any kind of other ILogger, comes with an internal boolean state that determines whether or not a log will happen. Has a backing ILogger that all methods are delegated to. To get this, use the "UseForDebugging" extension method available to all ILogger implementations. This one can be good for debugging. One example of using this logger for debugging is using the DEBUG preprocessor flag to enable logging, otherwise leave disabled. The only loss in performance at that point is a boolean check that is guaranteed to be false. If the boolean is guaranteed to be false, so is the return type. That means the JIT will most likely optimize it so it doesn't affect anything.