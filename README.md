# NetCoreSignalR

## Project Description
This project demonstrates the implementation of **SignalR** for real-time communication using multiple .NET Core components, including APIs, Web Clients, Worker Services, and Console Applications.

## Technologies and Tools Used
- **ASP.NET Core**
- **SignalR**
- **Worker Services**
- **Console Applications**

## How to Run the Project
Follow these steps to set up and run the project:

1. **Restore dependencies:**
   dotnet restore
2. **Build the solution:**
dotnet build
3. **Run the desired project (e.g., API, Web Client, or Worker Service):**
dotnet run --project <ProjectPath>

**Project Structure**

**NetCoreSignalR.API**: Backend API for managing SignalR communication.

**NetCoreSignalRClient.WorkerServiceApp**: Background worker demonstrating SignalR usage.

**NetSignalRClient.ConsoleApp**: Console application client for testing SignalR functionality.

**SignalR_Sample_Project.Web**:This sample web application integrates SignalR and Identity for user authentication. Users must log in via the Identity system, after which they can download an Excel file containing products associated with their account. Notifications about this process are sent in real time using SignalR, and the file download is triggered through AJAX to ensure a smooth user experience. .

**Contributing**
Feel free to fork the repository and contribute through pull requests.

**License**
This project is for educational purposes and does not have a specific license.

