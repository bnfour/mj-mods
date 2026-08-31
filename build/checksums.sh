#!/usr/bin/env bash

# shows sha256 sums for dlls
# stores them into a file to be listed in the release notes draft

cd release || exit 2;

mods=$(sha256sum ./*.dll)

# don't forget to go back to the root
cd ..

# just into the root folder
cat > checksums <<EOF
$mods
EOF

# show in the log as well
cat checksums
