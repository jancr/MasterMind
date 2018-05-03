
## Author note ##
This project is in a very immature development state, so this readme is more
notes to myself than an actual guide to how the software works

## Install ##

1. clone repository
    * `git clone https://github.com/jancr/MasterMind.git`

2. install dependencies
    * ```
cd MasterMind/MasterMind/src
bash dependencies.sh
```

3. Execute
    a. Run (for devel)
        * `dotnet run`

    b. Compile (for duistribution)
        * `dotnet publish -r osx.10.11-x64 --output bin/dist/osx`
        * `dotnet publish -r win10-x64 --output bin/dist/win`
        * `dotnet publish -r linux-x64 --output bin/dist/linux`


## Licence ##
All source code is licenced under the [Apache 2.0](http://www.apache.org/licenses/LICENSE-2.0)

Images shamelessly stolen from 
["https://github.com/robwil/Mastermind.Net"](https://github.com/robwil/Mastermind.Net), where I could not find a licence file.

## Inspiration ##
All c# was written with very little outside inspiration.
Front end inspiration:
* grid: 
	- http://jsfiddle.net/h2yJn/66/
* electron:
	- https://scotch.io/@rui/how-to-build-a-cross-platform-desktop-application-with-electron-and-net-core
	  thugh I dropped the react part, to not overwhelm myself
