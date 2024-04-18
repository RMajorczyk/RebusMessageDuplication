# .NET 6 Solution with 2 Projects

This repository contains a .NET 6 solution with two projects

## RebusMessageDuplication

This console application contains the main handlers for the Rebus implementation

### TestHandler

The `TestHandler` class is responsible for handling the `TestEvent` message. It contains the necessary logic to process incoming `TestEvent` messages.

### FailedMessageHandler

The `FailedMessageHandler` handles the messages wrapped with the `IFailed` type. This contains logic to resend the message x amount of times before moving it to the error queue.

## RebusMessageDuplication.Sender

The console application serves as a client for sending messages to the Rebus project. Its primary purpose is to send `TestEvent` messages to the queue for processing by the `TestHandler` in the Rebus project.

### Usage

To use the console application, follow these steps:

1. Build the solution.
2. Run the console application.
3. Input the necessary data or parameters required for sending the `TestEvent` message.
4. The console application will then send the `TestEvent` message to the Rebus project for handling.

## Dependencies

This solution relies on the following dependencies:

- .NET 6 SDK