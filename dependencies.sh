#!/usr/bin/env bash

# backend (c# in src) 
cd src
dotnet add package Microsoft.AspNetCore.Mvc
dotnet restore 

# frontend (node/electron in app)
cd ../app
npm install

