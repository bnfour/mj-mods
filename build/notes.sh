#!/usr/bin/env bash

# creates a skeleton for release notes

cat > notes <<EOF
_funne subtitle_

general tl;dr

## Changelog
tl;dr mentioning changed mods

---
The rest of the mods are re-released.

## Checksums
Don't forget to verify your downloads! SHA256 checksums for DLLs:
\`\`\`
$(cat checksums)
\`\`\`
EOF
