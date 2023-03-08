# Orders TODO

[![Hack Together: Microsoft Graph and .NET](https://img.shields.io/badge/Microsoft%20-Hack--Together-orange?style=for-the-badge&logo=microsoft)](https://github.com/microsoft/hack-together)

This code is a sample implementation of using Azure Graph API to manage tasks in a Todo List. It uses the Microsoft Graph .NET SDK to interact with the Graph API.

The main purpose of this code is to create and manage tasks in a Todo List. In this example, the code creates a Todo List named "Orders" if it doesn't exist and then adds a new task with random order details to the "Orders" list.

## Prerequisites
- Azure subscription with access to Graph API
- Microsoft Graph .NET SDK (version 1.0)
- Azure.Identity package (version 1.8)

## Usage
- Clone the repository to your local machine.
- Open the solution in Visual Studio and update the `tenantId` and `clientId` variables with your Azure AD tenant and client IDs respectively.
- Run the code.

The code will open a browser window prompting the user to sign in with their Azure AD credentials. Once authenticated, the code will access the Graph API and create a new Todo List named "Orders" (if it doesn't exist), and add a new task to the list with random order details.

## Note
This code is a sample implementation and should not be used in production environments without appropriate modifications and testing. Additionally, it assumes that the user has appropriate permissions to create and manage Todo Lists using the Graph API.

[![Hack Together: Microsoft Graph and .NET](https://img.shields.io/badge/Microsoft%20-Hack--Together-orange?style=for-the-badge&logo=microsoft)](https://github.com/microsoft/hack-together)