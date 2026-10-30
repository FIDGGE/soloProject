# Task Manager

## Problem statement
The application helps a user keep a simple list of tasks. The user can add tasks, view the list, mark tasks as completed, and remove tasks. It is a console application and does not require a database or external packages.

## Design pattern: Command
The `ICommand` interface describes an action with `Execute()` and `Undo()` methods. Adding, completing, and removing a task are implemented as separate commands. `TaskManager` stores executed commands in undo and redo stacks. This lets the user cancel the last change and repeat it.

## How to run
1. Install the .NET 10 SDK.
2. Open `TaskManagerLab.csproj` in Visual Studio.
3. Build the project and run with `Ctrl + F5`.

The source code is in one file: `Program.cs`.

## Example scenarios
1. Add a task named `Finish lab`.
2. Show tasks.
3. Mark the task as completed.
4. Choose Undo to restore its previous state.
5. Choose Redo to complete it again.
6. Remove the task and use Undo to restore it.

## GitHub Flow
The recommended workflow for developing the project:
1. Keep the stable version in `main`.
2. Create a branch for one change, for example `feature/task-undo`.
3. Make the change and commit it with a clear message.
4. Push the branch to GitHub and open a pull request.
5. Review and test the change, then merge it into `main`.
6. Repeat for the next feature.

Example commit messages:
- `Add task list`
- `Implement command pattern`
- `Add undo and redo`
- `Update README`

## Project structure
- `Program.cs` — all application code.
- `TaskManagerLab.csproj` — project settings.
- `README.md` — task description and instructions.
