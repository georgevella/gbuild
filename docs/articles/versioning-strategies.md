# Versioning Strategies

## Git Flow

### Develop branch
1. release tag present? source = tag
	1. get project version from tag data.  if no project version available, use starting version value.
	2. commit analysis between develop and tag
2. release tag not present? source = master
	1. use starting version value
	2. commit analysis between develop abd master
3. are there any feat commits? bump minor
4. are there any breaking commits? bump major
5. tag with 'dev-###' where ### is the number of commits between 