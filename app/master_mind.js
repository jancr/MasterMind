'use strict';

////////////////////////////////////////////////////////////////////////////////
// MasterMindBord
////////////////////////////////////////////////////////////////////////////////
// constructor
function MasterMindBord() {
    this.bord = null;
    this.pegs = null;
    this.game_status = null;
    this.guess_vector = [-1, -1, -1, -1];
    this.n_guesses = null;

    this.canvas = $("#bord")[0];
    this.debug_info = $("#debug_info");
    this.n_rows = 10;
    this.n_cols = 4;
    this.context = this.canvas.getContext("2d");
    this.n_colors = 6;
    this.padding = 10
}

    
// pseudo constructor
MasterMindBord.prototype.newGame = function(n_rows, n_cols) {
    if (typeof(n_rows) !== 'undefined') {
        this.n_rows = n_rows;
    } if (typeof(n_cols) !== 'undefined') {
        this.n_cols = n_cols;
    }
    // reset
    this.n_guesses = 0;
    this.guess_vector = [-1, -1, -1, -1];
    this.context.clearRect(0, 0, this.canvas.width, this.canvas.height);
    
    let diff_names = ["Easy", "Medium", "Hard"]
    let diff_index = $("#difficulty-select")[0].selectedIndex;
    this.n_colors = 4 + 2 * diff_index
    $.getJSON("http://localhost:5000/api/mm/new-game/" + diff_names[diff_index], 
              $.proxy(this.update, this));
}

MasterMindBord.prototype.update = function(response) {
    this.bord = response.bord;
    this.pegs = response.pegs;
    this.game_status = response.game_status;
    // this.debug_info = response.bord;
    this._drawBord();
}

MasterMindBord.prototype.guess = function() {
    if (!this.guess_vector.includes(-1)) {
        let url = "http://localhost:5000/api/mm/guess/";
        url += this.guess_vector.join() + '/' + $("#user-name")
        $.getJSON(url, $.proxy(this.update, this));
        this.guess_vector = [-1, -1, -1, -1];
        this.n_guesses++;
    } 
}

// private helper methods
MasterMindBord.prototype._drawBord = function drawBoard(){
    //grid width and height
    let bord_width = 400;
    let bord_height = 800;
    //padding around grid
    let padding = this.padding
    //size of canvas
    let canvas_width = bord_width + (padding * 2) + 1;
    let canvas_height = bord_height + (padding * 2) + 1;

    $("#bord").attr({width: canvas_width, height: canvas_height});

    this.x_step =  bord_width / (this.n_cols + 1);
    this.y_step = bord_height / this.n_rows;

    for (let x = 0; x <= bord_width; x += this.x_step) {
        this.context.moveTo(0.5 + x + padding, padding);
        this.context.lineTo(0.5 + x + padding, bord_height + padding);
    }

    for (let y = 0; y <= bord_height; y += this.y_step) {
        this.context.moveTo(padding, 0.5 + y + padding);
        this.context.lineTo(bord_width + padding, 0.5 + y + padding);
    }

    this.context.strokeStyle = "black";
    this.context.stroke();

    let self = this;

    for (let i_row = 0; i_row < this.n_rows; i_row++) {
        if (this.bord[i_row][0] != -1) {
            // guess
            for (let i_col = 0; i_col < this.n_cols; i_col++) {
                this.insertBall(i_col, i_row, this.bord[i_row][i_col]);
            }
            
            // pegs
            let black_pegs = this.pegs[i_row][0];
            let white_pegs = this.pegs[i_row][1];
            for (let i = 0; i < black_pegs; i++) {
                this.insertPeg(i_row, i, 'black');
            }
            for (let i = 0; i < white_pegs; i++) {
                this.insertPeg(i_row, i+black_pegs, 'white');
            }
            for (let i = 0; i < (4 - white_pegs - black_pegs); i++) {
                this.insertPeg(i_row, i + black_pegs + white_pegs, 'empty');
            }
        }
    }
}

MasterMindBord.prototype.insertPeg = function(i_row, n, img_str) {
    let base_image = new Image();
    base_image.src = 'static/img/' + img_str + '_peg.png';
    let x = this.padding + this.n_cols * this.x_step;
    let y = this.padding + (this.n_rows - i_row - 1) * this.y_step;
    if (2 <= n) {
        y += this.y_step * 0.5
    } if (n % 2 == 1) {
        x += 0.5 * this.x_step 
    }
    let self = this;
    base_image.onload = function(){
        // image is 30x50, so we need to crop 10px from top and buttom
        self.context.drawImage(base_image, 0, 10, 30, 30, x, y, 
                               0.5 * self.x_step, 0.5 * self.y_step);
    }
}

MasterMindBord.prototype.insertBall = function(i_col, i_row, n_image) {
    let base_image = new Image();
    base_image.src = 'static/img/shadow_ball_' + n_image + '.png';
    let x = this.padding + i_col * this.x_step;
    let y = this.padding + (this.n_rows - i_row - 1) * this.y_step;
    this.context.clearRect(x+1, y+1, this.x_step-1, this.y_step-1);
    self = this;
    base_image.onload = function(){
        self.context.drawImage(base_image, x, y, self.x_step, self.y_step) 
    }
}

//
////////////////////////////////////////////////////////////////////////////////
// events
////////////////////////////////////////////////////////////////////////////////
// create one instance of the bord class
var masterMindBord = new MasterMindBord();

let canvas = $("#bord")[0];
let context = masterMindBord.context;

////////////////////////////////////////
// helper functions
////////////////////////////////////////
let getMousePos = function(canvas, event) {
    var rect = canvas.getBoundingClientRect();
    return {
        x: event.clientX - rect.left,
        y: event.clientY - rect.top
    };
}

let clickCanvas = function(event) {
    var pos = getMousePos(canvas, event);
    let m = masterMindBord;
    let padding = m.padding;
    let i_col = Math.floor((pos.x - padding) / m.x_step);
    if (i_col < m.n_cols) {
        m.guess_vector[i_col] = (m.guess_vector[i_col] + 1) % m.n_colors
        m.insertBall(i_col, m.n_guesses, m.guess_vector[i_col]);
    }
}

var clickKey = function(event) {
    let m = masterMindBord;
    if (m.game_status == 'Ongoing') {
        let col_1 = '1'.charCodeAt(0);
        let col_4 = '4'.charCodeAt(0);
        let col_i = event.keyCode;
        if ((col_1 <= col_i) && (col_i <= col_4)) {
            let i_col = col_i - col_1;
            m.guess_vector[i_col] = (m.guess_vector[i_col] + 1) % m.n_colors
            m.insertBall(i_col, m.n_guesses, m.guess_vector[i_col]);
        } else if (event.keyCode == 13) { // return ley
            masterMindBord.guess();
        } 
    }
    $("#debug_info").text(event.keyCode);
    if ((event.keyCode === 'n'.charCodeAt(0)) || 
        (event.keyCode === 'N'.charCodeAt(0))) {
        masterMindBord.newGame();
    }
}
////////////////////////////////////////
// binding
////////////////////////////////////////
canvas.addEventListener('click', clickCanvas, false);
document.addEventListener('keydown', clickKey, false);
$("#guess-btn").click(function() { masterMindBord.guess(); });
$("#new-game-btn").click(function() { masterMindBord.newGame(); });


$(document).ready(function() {
    // I think this is inly called on load and refresh, not when events update
    masterMindBord.newGame();
    // masterMindBord.guess_vector = [0, 0, 1, 1];
    // masterMindBord.guess();
});        
