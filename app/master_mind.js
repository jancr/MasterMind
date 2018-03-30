'use strict';

const $ = require('jquery')

////////////////////////////////////////////////////////////////////////////////
// MasterMindBord
////////////////////////////////////////////////////////////////////////////////
// constructor
function MasterMindBord() {
	this.bord = null;
	this.pegs = null;
	this.game_status = null;
	this.n_guesses = null;

    this.canvas = $("#bord");
	this.debug_info = $("#debug_info");
	this.n_rows = 10;
	this.n_cols = 4;
}
	
// pseudo constructor
MasterMindBord.prototype.newGame = function(n_rows, n_cols) {
	if (typeof(n_rows) !== 'undefined') {
		this.n_rows = n_rows;
	} if (typeof(n_cols) !== 'undefined') {
		this.n_cols = n_cols;
	}
	this.n_guesses = 0;
	$.getJSON("http://localhost:5000/api/mm/new-game", 
			  $.proxy(this.update, this));
}

MasterMindBord.prototype.update = function(response) {
	this.bord = response.bord;
	this.pegs = response.pegs;
	this.game_status = response.game_status;
	this.debug_info = response.bord;
	this._drawBord();
}

MasterMindBord.prototype.guess =  function() {
	// TODO: get guess from canvas!!!
	let guess = "1,2,3,4";
	$.getJSON("http://localhost:5000/api/mm/guess/" + guess, 
			  $.proxy(this.update, this));
}

// private helper methods
MasterMindBord.prototype._drawBord = function drawBoard(){
	//grid width and height
	let bord_width = 500;
	let bord_height = 1000;
	//padding around grid
	let padding = 10;
	//size of canvas
	let canvas_width = bord_width + (padding * 2) + 1;
	let canvas_height = bord_height + (padding * 2) + 1;

	$('#bord').attr({width: canvas_width, height: canvas_height});

	let x_step = bord_width / this.n_cols;
	let y_step = bord_height / this.n_rows;

	let context = this.canvas.get(0).getContext("2d");
	for (let x = 0; x <= bord_width; x += x_step) {
		context.moveTo(0.5 + x + padding, padding);
		context.lineTo(0.5 + x + padding, bord_height + padding);
	}

	for (let y = 0; y <= bord_height; y += y_step) {
		context.moveTo(padding, 0.5 + y + padding);
		context.lineTo(bord_width + padding, 0.5 + y + padding);
	}

	context.strokeStyle = "black";
	context.stroke();

	function insertImage(i_col, i_row, n_image) {
		let base_image = new Image();
		base_image.src = 'static/img/shadow_ball_' + n_image + '.png';
		base_image.onload = function(){
			// TODO: is this this shadowed???
			// TODO: is this this shadowed???
			console.log(this);
			// calc x,y pos
			let x = padding + i_col * x_step;
			let y = padding + (this.n_rows - i_row) * y_step;

			context.drawImage(base_image, x, y, x_step, y_step) 
		}
	}
	for (let i_row = 0; i_row < this.n_rows; i_row++) {
		if (i_row < this.n_guesses) {

			for (let i_col = 0; i_col < this.n_cols; i_col++) {
				// TODO: do math
				// TODO, fix guess so you have stuff to draw
				// TODO: get n_image from guess or bord
				insertImage(i_col, i_row, this.bord[i_row][i_col]);
				// TODO: pegs??
			}
		}
	}
}


////////////////////////////////////////////////////////////////////////////////
// events
////////////////////////////////////////////////////////////////////////////////
// create one instance of the bord class
var masterMindBord = new MasterMindBord();

$(document).ready(function() {
	// I think this is inly called on load and refresh, not when events update
	masterMindBord.newGame();
	masterMindBord.guess();
});        
