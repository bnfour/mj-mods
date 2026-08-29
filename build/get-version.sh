#!/usr/bin/env bash

# given the state of the repo (tags), figures out the version number for the new release,
# also defines the final asset archive name base on that

last_version=$(git tag --list 'v*' | colrm 1 1 | sort --numeric-sort --reverse | head --lines=1)

new_version=$(( "$last_version" + 1 ))
# to root of the repo you go
filename="mj-mods-$(printf "%02d" $new_version).zip"

echo "new_version=$new_version" >> "$GITHUB_ENV"
echo "filename=$filename" >> "$GITHUB_ENV"

echo "This is release version $new_version, apparently"
echo "Will pack into $filename"
