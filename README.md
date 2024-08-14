# Eco Server Utility

A management tool for the ECO game server, designed to provide an intuitive user interface for controlling server operations, monitoring server status, and scheduling automatic reboots. Built with .NET and C#, this application allows users to start, stop, and restart the ECO server, as well as configure various settings related to server management.

## Description

`Eco Server Utility` offers several key features:
- **Server Status Monitoring:** Continuously checks if the ECO server is running and responsive, and provides visual feedback on the status.
- **Automatic Restart:** Configurable automatic server restarts based on user-defined schedules.
- **Startup Management:** Option to automatically start the application on system startup.
- **Server Directory Management:** Ability to set and verify the directory where the ECO server executable is located.
- **Tooltips Management:** Option to enable or disable tooltips for user assistance.
- **GUI Features:** Real-time updates, configurable settings, and user-friendly interface for ease of management.

## Getting Started

To get started with the Eco Server Utility application:

1. **Download and Run:**
   - Download the latest release.
   - Run the application.

2. **Configuration:**
   - **Server Directory:** Set the path to the ECO server executable in the `Directory` field.
   - **Automatic Start:** Check the `Start on Startup` checkbox to have the application start automatically when Windows boots.
   - **Automatic Reboot:** Configure the schedule for automatic server reboots by selecting the desired hour and minute.
   - **Tooltips:** Enable or disable tooltips based on your preference.

3. **Usage:**
   - **Start/Stop Server:** The application will automatically start the server if it is not running or responsive, based on your settings.
   - **Server Status:** Monitor the status of the server through the interface which displays if the server is running, not running, or not responding.
   - **Restart Server:** If configured, the application will restart the server at the scheduled time.

4. **Additional Information:**
   - Ensure that the `EcoServer.exe` file is located in the directory specified in the application settings.
   - If you encounter issues, verify the path and ensure the application has appropriate permissions to manage processes.
