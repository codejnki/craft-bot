#! /usr/bin/bash

if [[ "$1" != "" ]]; then
  VERSION="$1"
else
  echo "No version specified.  Please specify version"
  exit 1
fi

OWNER=codejnki
REPO=craft-bot

curl -LO https://github.com/"${OWNER}"/"${REPO}"/releases/download/"${VERSION}"/craft-bot-linux-x64.tar.gz
