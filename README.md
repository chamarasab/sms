# SMS Sending Project README

This README provides an overview of a C# based SMS sending project. The project consists of a C# application that uses a WebSocket server to receive JSON data containing mobile numbers and SMS messages and sends SMS messages through a GSM modem using GsmComm library.

## Prerequisites
Before you start working with this project, you need to ensure the following prerequisites are met:

1. **C# Development Environment**: You should have a C# development environment set up, including Visual Studio or Visual Studio Code.

2. **GsmComm Library**: This project utilizes the GsmComm library for communicating with the GSM modem. Make sure to install this library via NuGet.

3. **WebSocket and HTTP Listener**: The project uses WebSocket and HTTP listener to handle incoming requests. These are available in the .NET Framework.

4. **GSM Modem**: You need a GSM modem connected to your computer to send SMS messages.

## Project Structure
The project consists of a C# class `Program.cs` and two additional classes, `GSMsms.cs` and `GSMcom.cs`. Here's a brief overview of each class:

### Program.cs
- The `Program.cs` file is the entry point of the application and contains the code for handling WebSocket connections and processing JSON data for sending SMS messages.

### GSMsms.cs
- The `GSMsms` class handles the communication with the GSM modem.
- It provides methods for listing available COM ports, searching for a GSM device, connecting to it, reading SMS messages, and sending SMS messages.
- It also includes properties such as `IsDeviceFound` and `IsConnected` to track the status of the GSM modem.

### GSMcom.cs
- The `GSMcom` class represents COM ports for GSM modems.
- It includes properties for the name and description of the COM port and a `ToString` method for easy string representation.

## Usage

To use this project for sending SMS messages:

1. Create an instance of the `GSMsms` class.
2. Use the `Connect` method to establish a connection to the GSM modem.
3. Use the `Send` method to send SMS messages.

Example:

```csharp
GSMsms sms = new GSMsms();
if (sms.Connect())
{
    sms.Send("recipient_number", "Hello, this is a test message.");
}
