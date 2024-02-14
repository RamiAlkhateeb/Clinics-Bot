
# Teams Bot for Clinic Appointments

## Overview

This repository contains the code and documentation for a .NET Core API project that integrates with Microsoft Teams to enable users to schedule doctor appointments through a Teams bot.

## Project Description

The goal of this project is to provide a convenient way for users to schedule appointments with doctors at clinics using the Microsoft Teams platform. The Teams bot facilitates communication between users and the clinic's appointment scheduling system, allowing users to view available time slots, select a preferred time, and confirm their appointment.

## Features

- **Appointment Scheduling**: Users can request appointments with doctors based on their availability.
- **Real-time Notifications**: Users receive real-time notifications about appointment confirmations, cancellations, and reminders.
- **Integration with Clinic System**: The bot integrates seamlessly with the clinic's appointment scheduling system to manage appointments and availability.

## Repository Structure

- `src/`: Contains the source code for the .NET Core API project.
- `docs/`: Documentation directory containing project-related documents, including API documentation, architecture overview, and deployment instructions.
- `tests/`: Directory for unit tests and integration tests for the API project.
- `LICENSE`: License file for the repository.
- `README.md`: This README file providing an overview of the project.

## Dependencies

The project requires the following dependencies:

- .NET Core SDK
- Microsoft Teams SDK (for bot integration)
- Entity Framework Core (for database interaction)
- Other dependencies as specified in the `packages.config` file

## Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/RamiAlkhateeb/Clinics-Bot.git
   ```

2. Navigate to the `src/` directory:

   ```bash
   cd src
   ```

3. Build the solution:

   ```bash
   dotnet build
   ```

4. Run the application:

   ```bash
   dotnet run
   ```

## Usage

1. Add the bot to your Microsoft Teams workspace.
2. Initiate a conversation with the bot to start scheduling appointments.
3. Follow the prompts to select a doctor, choose a date and time, and confirm your appointment.

## Contributing

Contributions to the project are welcome. Feel free to open an issue to report bugs, suggest enhancements, or submit pull requests.

## License

This project is licensed under the [MIT License](LICENSE).

## Contact

For inquiries or feedback, please contact [your-name](mailto:rami13alkhateeb@gmail.com).
