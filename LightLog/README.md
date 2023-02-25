# LightLog
A lightweight logger created with dependency injection in mind.
It is done for the most part.

# Loggers
There are different kinds of loggers that come packed. Here they are and their differences:

1. The basic logger `Logger`: This logger has the most flexibility as it is single-threaded. It is also the only one capable of having its output redirected to another TextWriter.
2. The async logger `AsyncLogger`: It has the same capabilities as the basic logger but is unable to have its output redirected. When a thread logs using this logger, it will begin a task that adds the message to a blocking collection backed by a concurrent queue that is constantly emptied by an independent thread. The thread that writes to the TextWriter is blocked while there are no queued logs.