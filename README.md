# API Testing Project with C#

## Overview
This project demonstrates API testing using C#, RestSharp, and xUnit. It covers various HTTP methods (GET, POST, PUT, DELETE) and includes positive and negative test cases.

## Tools and Frameworks
- C#
- xUnit (for testing)
- RestSharp (for making API calls)
- FluentAssertions (for assertions)

## API Used
- [JSONPlaceholder](https://jsonplaceholder.typicode.com/)

## How to Run the Tests
1. Clone the repository.
2. Open the solution in Visual Studio.
3. Restore NuGet packages.
4. Run the tests using the Test Explorer in Visual Studio.

## Test Cases
- `GetPosts`: Verifies fetching a list of posts.
- `CreatePost`: Verifies creating a new post.
- `UpdatePost`: Verifies updating an existing post.
- `DeletePost`: Verifies deleting a post.
- `GetNonExistingPost`: Verifies handling a request for a non-existing post.

## Project Structure
