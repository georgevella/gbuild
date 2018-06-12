# High Level Component List

## Workspace management (projects, configuration, etc)
  - Each project has a location on disk and is used in various other components (ex: commit analysis) (TODO)
  - list of all projects in workspace (determine automatically + from configuration) (TODO)

## Versioning Strategy

## Version Number Generator
  - returns all projects and their respective version   (TODO)
  - use commit analysis and optionally project analysis to determine the version of each project (TODO)

## Commit Analyser
Compares current branch with follower branch / release tag and determines:
  - Number of commits introduced
  - Projects changed
  - list of new commits and which projects they target (TODO)
  - determine if a project has a breaking change (TODO)
  - determine if a project has a new feature (TODO)

## Project Change Analyzer

## Project Builder

## Documentation Builder