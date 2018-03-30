const electron = require('electron')
// Module to control application life.
const app = electron.app
// Module to create native browser window.
const BrowserWindow = electron.BrowserWindow

const path = require('path')
const url = require('url')

let mainWindow

function createWindow () {
	mainWindow = new BrowserWindow({width: 800, height: 600})

	mainWindow.loadURL(url.format({
		pathname: path.join(__dirname, 'index.html'),
		protocol: 'file:',
		slashes: true
	}))

	// TODO uncomment
	mainWindow.webContents.openDevTools()

	mainWindow.on('closed', function () {
		mainWindow = null
	})
}


// TODO: remember to uncomment!!!!
app.on('ready', startApi)

// Quit when all windows are closed.
app.on('window-all-closed', function () {
	// On OS X it is common for applications and their menu bar
	// to stay active until the user quits explicitly with Cmd + Q
	if (process.platform !== 'darwin') {
		app.quit()
	}
})

app.on('activate', function () {
	// On OS X it's common to re-create a window in the app when the
	// dock icon is clicked and there are no other windows open.
	if (mainWindow === null) {
		createWindow()
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
	var mm_bin_path = path.join(__dirname, '..\\src\\bin\\dist\\win\\MasterMind.exe')
	if (os.platform() === 'darwin') {
		mm_bin_path = path.join(__dirname, '..//src//bin//dist//osx//MasterMind')
	}
	apiProcess = proc(mm_bin_path)

	apiProcess.stdout.on('data', (data) => {
		writeLog(`stdout: ${data}`);
		if (mainWindow == null) {
			createWindow();
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
