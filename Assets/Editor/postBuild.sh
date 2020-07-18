#!/bin/sh

set -x

export buildfolder="$(find . -regex '.*\/Default WebGL\/Build' -type d -print -quit)"
if [ -z "$buildfolder" ]; then
  echo "Could not find build folder"
  exit 1
fi

if [ ! -d ./tmp ]; then
  git clone "https://github.com/AleksanderMisztal/WebGL.git" ./tmp
fi
cp -r "$buildfolder" ./tmp
cd ./tmp
git add Build
git config --global user.email "$githubemail"
git config --global user.name "$nickname"
git commit -m "unity cloud build"
git push --force