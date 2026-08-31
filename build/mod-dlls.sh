#!/usr/bin/env bash

# copies build mod DLLs to be included in the release

# TODO waiting patiently for .NET 11 to dotnet sln *.slnf list to just work

DO_NOT_SHIP=("Experimental")
projects=$(dotnet sln Bnfour.MoeJigsawMods.slnx list | sed -nE 's/^(.*)\/.*\.csproj/\1/p')
for project in $projects
do
    if echo "${DO_NOT_SHIP[*]}" | grep -qw "$project";
    then
        echo "Not packing $project (configured skip)"
    else
        cp --verbose "$project/bin/Release/net35/$project.dll" release/
    fi
done
