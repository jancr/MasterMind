const electron = require('electron')
// Module to control application life.
const app = electron.app
// Module to create native browser window.
const BrowserWindow = electron.BrowserWindow

const path = require('path')
const url = require('url')

let mainWindow;
let backendStarted = false;

const isDev = require('electron-is-dev');

function createWindow () {
	mainWindow = new BrowserWindow({width: 1100, height: 550});

	mainWindow.loadURL(url.format({
		pathname: path.join(__dirname, 'index.html'),
		protocol: 'file:',
		slashes: true
	}));

    if (isDev) {
        mainWindow.webContents.openDevTools();
    }

	mainWindow.on('closed', function () {
		mainWindow = null
	});
}


app.on('ready', startApi);

// Quit when all windows are closed.
app.on('window-all-closed', function () {
	// On OS X it is common for applications and their menu bar
	// to stay active until the user quits explicitly with Cmd + Q
	if (process.platform !== 'darwin') {
		app.quit();
	}
})

app.on('activate', function () {
	// On OS X it's common to re-create a window in the app when the
	// dock icon is clicked and there are no other windows open.
	if (mainWindow === null) {
		createWindow();
	}
})

////////////////////////////////////////////////////////////////////////////////
// general stuff
////////////////////////////////////////////////////////////////////////////////
// In this file you can include the rest of your app's specific main process
// code. You can also put them in separate files and require them here.

// requre
const os = require('os');
var apiProcess = null;

function startApi() {
	var proc = require('child_process').spawn;
	//  run server
    var platform = os.platform()
    if (platform == 'win32') {
        var mm_bin_path = path.join(__dirname, 'backebnd\\windows\\MasterMind.exe');
    } else if (platform == 'darwin') {
		var mm_bin_path = path.join(__dirname, 'backend/macos/MasterMind');
	} else if (platform == 'linux') { 
		var mm_bin_path = path.join(__dirname, 'backend/linux/MasterMind');
    }
	apiProcess = proc(mm_bin_path);

	apiProcess.stdout.on('data', (data) => {
		writeLog(`stdout: ${data}`);
		if (mainWindow == null && !backendStarted) {
            if (/Application started/.exec(data)) {
                backendStarted = true;
            }
            if (backendStarted) {
                createWindow();
            }
		}
	});
}

//Kill process when electron exits
process.on('exit', function () {
	writeLog('exit');
	apiProcess.kill();
});

function writeLog(msg){
	console.log(msg);
}
