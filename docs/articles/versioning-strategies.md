# Versioning Strategies

## concepts

- parent commit: the branch against which the current branch will be compared to (to retrieve commit history analysis)
- version source: branch / tag from where to obtain version information

## Git Flow

### Develop branch
1. release branch present?
	- get all release branches, sorted by latest update
	- walk master branch until last release / beginning of history
	- if release branch is merged, ignore, otherwise use as version source
	- build project versions from release branch and use as source versions
	- parent commit = parent of release branch (on develop)
2. if no release branches
	1. if release tag present? 
		- version source = tag
			- get project version from tag data.  if no project version available, use starting version value.
		- parent commit = tag commit
	2. release tag not present? 
		- version source = starting version value
		- parent commit = master HEAD
3. commit analysis between develop HEAD and parent commit
3. are there any feat commits? bump minor
4. are there any breaking commits? bump major
5. tag with 'dev-###' where ### is the number of commits between

### Release branch
