# **MainCatApiApp**

This repository contains the **MainCatApiApp** solution, which includes the following projects:

- **CatApiApp.Tests**: Unit tests for the CatApiApp project.
- **CatApiApp**: The main project is located in a separate repository. Check out the repository for **CatApiApp** [here](https://github.com/manosfragk/CatApiAppSolution).

## **About**

- **CatApiApp**: This is an ASP.NET Core Web API that fetches 25 cat images from the "TheCat API", stores them in a SQL Server database, and provides endpoints for retrieving this data with paging and filtering support.
- **CatApiApp.Tests**: Contains unit tests written in **xUnit** to ensure the correctness of the services, controllers, and other components of the **CatApiApp**.

## **Project Structure**

- **CatApiApp.Tests**: Unit testing project, using the following libraries:
  - **xUnit**: For unit testing.
  - **Moq**: For mocking dependencies.
  - **Entity Framework Core InMemory**: For testing database interactions in memory.

- **CatApiApp**: You can find the full source code for **CatApiApp** in its dedicated repository: [CatApiApp Solution](https://github.com/manosfragk/CatApiAppSolution).


Clone the **MainCatApiApp** repository:
   ```bash
   git clone https://github.com/manosfragk/MainCatApiApp.git
   cd MainCatApiApp
