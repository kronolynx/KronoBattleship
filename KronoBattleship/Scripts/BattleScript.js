"use strict";

var generateBoard = function () {
    var board = "";
    for (var i = 0; i < 100; i++) {
        board += "<div class='seaBox' id='" + i + "'></div>";
    }
    return board;
};

var playerReady = function () {
    $("#placementBoard").remove();
    $("#btnGiveUp").show();
    $("#enemy-board").show();
    $(".ship").draggable("destroy");
    $(".seaBox").droppable("destroy");
    $(".ship").off("dblclick");

    $("#messages").text("Waiting for the enemy").removeClass().addClass("bg-info");
}

var ships = [["aircraft", "aircraftV", "destroyer1", "destroyer1V", "destroyer2", "destroyer2V", "cruiser1", "cruiser1V", "cruiser2", "cruiser2V"],
    ['a', 'c', 'e', 'g', 'i', 'k', 'm', 'o', 'q', 's']];

function displayPlayerShips(board) {
    var ignoreChar = "x"
    for (var i = 0; i < 100; i++) {
        var char = charWithoutAttack(board[i]);
        if (ignoreChar.indexOf(char) == -1) {
            setShip(i, char);
            ignoreChar += board[i];
            if (char != board[i]) {
                ignoreChar += char;
            }
        }
    }
}

function displayplayerAttacks(board) {
    var ignoreChar = "xacegikmoqs"
    for (var i = 0; i < board.length; i++) {
        var char = board[i];
        if (ignoreChar.indexOf(char) == -1) {
            var boardCell = $("#player-board #" + i);
            if (char == 'y') {
                boardCell.append("<span class='hole miss'></span>");
            } else {
                boardCell.append("<div class='explosion'></div>");
            }
        }
    }
}


function displayEnemyBoard(board) {
    var ignoreChar = "xacegikmoqs"
    for (var i = 0; i < board.length; i++) {
        var char = board[i];
        if (ignoreChar.indexOf(char) == -1) {
            var boardCell = $("#enemy-board #" + i);
            if (char == 'y') {
                boardCell.children().addClass("miss");
            } else {
                boardCell.children().addClass("hit");
            }
            boardCell.addClass("shot");
            boardCell.off("click mouseenter");

        }
    }
}


function charWithoutAttack(char) {
    return char.charCodeAt(0) % 2 == 0 ? String.fromCharCode(char.charCodeAt(0) - 1) : char;
}

function setShip(pos, char) {
    $("#player-board #" + pos).append("<div class='ship' id='" + shipIdFromChar(char) + "'></div>")
}

/**
 * set the initial board where the ships will be placed, TODO find a better name
 */
function setBoardPlacement() {
    // TODO ecmascript6 replace with for loop instead because old browsers may not work

    var boardArray = Array(100).fill('x');
    var boardString = "";
    $(".ship").each(function () {
        var ship = $(this);
        var head = getShipAppendedPosition(ship);
        if (head != "shipYard") {
            var shipId = getShipId(ship);
            var step = getStep(ship);
            var tail = getShipTail(ship);

            for (var i = parseInt(head) ; i <= tail; i += step) {
                boardArray[i] = charFromShipId(shipId);
            }
            boardString = boardArray.join("");
        }
        else {
            boardString = "";
            return false;
        }
    });
    if (boardString) {
        console.table(battleJson);
        $.ajax({
            url: "/Battle/Ready",
            type: "POST",
            data: { battleId: battleJson.BattleId, playerBoard: boardString },
            success: function (e) {
                //console.log('ready', e);
                playerReady();
                battleJson.PlayerBoard = boardString;
                battleJson.EnemyBoard = e.EnemyBoard;
                battleHub.server.callFunction(battleJson.EnemyName, battleJson.EnemyBoard === "" ? "enemyReady" : "readyToAttack");
            },
            error: function (e) {
                console.log('Error on ready ' + e);
            }
        });
    } else {
        $("#placeShipsModal").modal();
    }
}

function getShipSize(ship) {
    return getShipSizeById(getShipId(ship));
}
/**
 *
 * @param shipId  id of the ship
 * @returns {number}
 */
function getShipSizeById(shipId) {
    var size = 0;
    if (shipId.indexOf("aircraft") > -1) {
        size = 5;
    } else if (shipId.indexOf("cruiser") > -1) {
        size = 3;
    } else {
        size = 2;
    }
    return size;
}

/**
 *
 * @param ship
 * @returns {number} or NAN if the ship is in the shipyard
 */
function getStep(ship) {
    return getStepById(getShipId(ship))
}
function getStepById(shipId) {
    return shipId.indexOf('V') > -1 ? 10 : 1;
}
/**
 * Get the position of the tail
 * @param ship
 * @returns {number}
 */
function getShipTail(ship) {
    return getShipTailCalculated(getShipAppendedPosition(ship), getShipSize(ship), getStep(ship));
}
/**
 * Get the position of the tail based on the position , size and step
 * @param position
 * @param size
 * @param step
 * @returns {number}
 */
function getShipTailCalculated(position, size, step) {
    return (parseInt(position) + ((parseInt(size) - 1) * step));
}

/**
 * get the id of the div where the ships is appended
 * @param ship object
 * @returns {string}
 */
function getShipAppendedPosition(ship) {
    return ship.parent().attr('id');
}
/**
 * get the ship Id
 * @param ship
 * @returns string
 */
function getShipId(ship) {
    return ship.attr("id");
}

/**
 * from a char get the shipId
 * @param char
 * @returns {*}
 */
function shipIdFromChar(char) {
    return ships[0][ships[1].indexOf(char)];
}
/**
 *
 * @param shipId
 * @returns the char equivalent for this ship to use in the string board array
 */
function charFromShipId(shipId) {
    return ships[1][ships[0].indexOf(shipId)];
}

/**
 * activate shooting depending if is the players turn
 * @param isPlayerTurn
 */
function readyToAttack(isPlayerTurn) {
    isPlayerTurn ? activateClick() : deactivateClick();
}
/**
 *  check if the position of the ship is inside the board
 * @param ship
 * @returns {boolean}
 */
function isValidPosition(ship) {
    // when horizontal the step is 10 because we calculate for a vertical ship and the difference is 10 boxes
    // the tail must be in a position that is less than 100
    var head = getShipAppendedPosition(ship);
    var shipSize = getShipSize(ship);
    return (isHorizontal(ship) && getShipTailCalculated(head, shipSize, 10) < 100) ||
           (isVertical(ship) && ((head % 10) < (getShipTailCalculated(head, shipSize, 1) % 10)));
}

function isVertical(ship) {
    return getShipId(ship).indexOf('V') > -1;
}

function isHorizontal(ship) {
    return !isVertical(ship);
}


/**
 * Append ship to board
 */
function appendShip(ship, pos) {
    ship.detach().appendTo($("#" + calculateShipPosition(getShipId(ship), pos)));
}
/**
 * when the ship is dragged the position detected is in the middle of the ship so we need to recalculate it to get the top left box
 * @param shipId
 * @param pos
 * @returns {string}
 */
function calculateShipPosition(shipId, pos) {
    var headPos = "";
    if (shipId.indexOf("aircraft") > -1) {
        headPos = pos - (2 * getStepById(shipId));
    }
    else if (shipId.indexOf("cruiser") > -1) {
        headPos = pos - (1 * getStepById(shipId));
    } else {
        headPos = pos;
    }
    return headPos;
}

function checkCollision(ship) {
    return checkCollisionById(getShipId(ship), getShipAppendedPosition(ship));
}
/**
 * Check if there's collision
 * @param shipId id of dragged ship
 * @param shipPos position of the ship
 * return true if collision found
 */
function checkCollisionById(shipId, shipPos) {
    var collision = false;
    $(".ship").each(function () {
        var otherShip = $(this);
        var otherId = getShipId(otherShip);
        var otherPos = getShipAppendedPosition(otherShip);
        if (shipId != otherId && shipPos != "shipYard") {
            if (comparePosition(shipPos, otherPos, getShipSizeById(shipId), getShipSizeById(otherId), getStepById(shipId), getStepById(otherId))) {
                collision = true;
                // return false to break out of the loop
                return false;
            }
        }
    });
    return collision;
}

/**
 *
 * @param calculatedPosShipDragged position of the dragged ship
 * @param pos2 position of the other ship
 * @param size1 size of the ship at pos1
 * @param size2 size of the ship at pos2
 * @param step1 if is horizontal the step 1
 * @param step2 if is vertical the step is 10
 */
function comparePosition(calculatedPosShipDragged, pos2, size1, size2, step1, step2) {

    if (pos2 != "shipYard") {
        var tailShip1 = getShipTailCalculated(calculatedPosShipDragged, size1, step1);
        var tailShip2 = getShipTailCalculated(pos2, size2, step2);
        for (var i = parseInt(calculatedPosShipDragged) ; i <= tailShip1; i += step1) {
            for (var k = parseInt(pos2) ; k <= tailShip2; k += step2) {
                if (k == i) {
                    return true;
                }
            }
        }
    }
    return false;
}

// if the user board is null then we need to display the shipyard
var setBoard = function () {
    var placementBoard = function () {
        var placementBoard =
            '<div id="placementBoard">' +
            '<div id="shipSelection">' +
            '<div id="shipYard">' +
            '<div class="ship" id="aircraft"></div>' +
            '<div class="spacer"> </div>' +
            '<div class="ship" id="destroyer1"></div>' +
            '<div class="spacer"> </div>' +
            '<div class="ship" id="cruiser1"></div>' +
            '<div class="spacer"> </div>' +
            '<div class="ship" id="cruiser2"></div>' +
            '<div class="spacer"> </div>' +
            '<div class="ship" id="destroyer2"></div>' +
            '</div>' +
            '<button id="ready" class="btn btn-primary btn-block btn-36 alt">Ready</button>' +
            '</div>' +
            '</div>';
        return placementBoard;
    }

    var placementBoard = placementBoard();

    $('#enemy-board').hide();
    $('#battle-field').append(placementBoard);
    $('#player-board').addClass('placementBoard');
    $('#messages').text("Place your ships").removeClass().addClass("bg-info");

    $('.ship').draggable({
        containment: '.placementBoard',
        snap: ".seaBox",
        grid: [40, 40],
        revert: "invalid"
    });

    $('.seaBox').droppable({
        tolerance: "intersect",
        accept: ".ship",
        drop: function (event, ui) {
            if (!checkCollisionById(getShipId(ui.draggable), calculateShipPosition(getShipId(ui.draggable), getShipId($(this)))))
                appendShip(ui.draggable, getShipId($(this)));
            else {
                var position = $("#" + getShipAppendedPosition(ui.draggable)).position();
                ui.draggable.animate({
                    top: position.top,
                    left: position.left
                }, 500);
            }
        }
    });

    $(".ship").dblclick(function () {
        var ship = $(this);
        // check if the tail of the ship is inside the board
        if (isValidPosition(ship)) {
            var ship_id = getShipId(ship);
            var shipPostfix = ship_id.charAt(ship_id.length - 1)
            var new_ship_id = shipPostfix == 'V' ? ship_id.slice(0, -1) : ship_id + "V";

            // check if with the new id there's collisions if not then sets the dimensions of the ship else undo the change
            ship.attr("id", new_ship_id);
            if (!checkCollision(ship)) {

                if (document.getBoxObjectFor != null || window.mozInnerScreenX != null) {
                    var height = ship.css("height");
                    var width = ship.css("width");

                    ship.css("height", width);
                    ship.css("width", height);
                }
            } else {
                ship.attr("id", ship_id);
            }
        }
    });

    $("#ready").click(function () {
        setBoardPlacement();
    });
}

// after the player has set the ships we must display the enemy board
var displayBoard = function () {

    $("#enemy-board").show();
    $('#btnGiveUp').show();

    displayPlayerShips(battleJson.PlayerBoard);
    displayplayerAttacks(battleJson.PlayerBoard);
    displayEnemyBoard(battleJson.EnemyBoard);
    // empty enemyboard after displaying it
    //If problems remove this line
    //battleJson.EnemyBoard = "";
    if (battleJson.EnemyBoard === "") {
        deactivateClick()
    } else {
        readyToAttack(battleJson.PlayerName === battleJson.ActivePlayer);
    }
}


var displayAttack = function (hit, attack) {
    var shot = $("#enemy-board #" + attack);
    if (hit) {
        console.info("hit");
        shot.children().addClass("hit");
    }
    else {
        console.info("miss");
        shot.children().addClass("miss");
    }
    shot.addClass("shot").off("click mouseenter");
};


var gameOver = function (winner) {
    $('#btn-giveup').prop("disabled", true);

    function closeModal() {
        $("#game-over").modal('hide');
    }

    var result = "<div class='end-game ";
    if (winner) {
        $("#messages").text("You've won").removeClass().addClass("bg-success");
        result += "win'><a href='#' onclick='closeModal();' class='close'>X</a><div class='winner'>";
    }
    else {
        $("#messages").text("You've lost").removeClass().addClass("bg-danger");
        result += "lose'><a href='#' onclick='closeModal();' class='close'>X</a><div class='gameover'>";
    }
    result += "</div>" +
        "<a href='/' class='btn btn-default float-right'>Back Home</a></div>";

    var dialog = "<div class='modal fade' id='game-over'>" + result + "</div>"
    $("#battle-field").append(dialog);
    $("#game-over").modal('show');

    disableSeaBox($('#enemy-board .seaBox'));
    $('#ready').prop("disabled", true);
}
/**
 * function that allows to attack the other player
 * required here because must access the variables in the controller
 */
function activateClick() {
    var enemySeabox = $('#enemy-board .seaBox:not(.shot)');
    $("#messages").text("Ready to attack !!").removeClass().addClass("bg-success");
    enemySeabox.on("mouseenter", function () {
        $(this).addClass("target");
    });
    enemySeabox.on("mouseleave", function () {
        $(this).removeClass("target");
    });
    enemySeabox.on("click", function () {
        deactivateClick();
        var attack = $(this).attr("id");
        $.ajax({
            url: "/Battle/Attack",
            type: "POST",
            data: { battleId: battleJson.BattleId, attack: attack },
            success: function (result) {
                displayAttack(result.Hit, attack);
                //console.log('successful attack', e);
                battleHub.server.attack(battleJson.EnemyName, result.Hit ? "hit" : "miss", attack, result.GameOver);
                if (result.GameOver) {
                    gameOver(true);
                    battleHub.server.finishGame(battleJson.EnemyName, !result.GameOver);
                }
            },
            error: function () {
                alert('Error');
            }
        });
    });
}
/**
 * function that tells the user to wait while the other player attacks
 * required here because must access the variables in the controller
 */
function deactivateClick() {
    $("#messages").text("Waiting for the enemy").removeClass().addClass("bg-danger");
    var enemySeabox = $('#enemy-board .seaBox');
    disableSeaBox(enemySeabox);

}
function disableSeaBox(enemySeabox) {
    enemySeabox.off("click");
    enemySeabox.off("mouseenter");
    enemySeabox.off("mouseleave", function () {
        $(this).removeClass("target");
    });
}

$("#btn-giveup").click(function () {
    $(this).prop("disabled", true);
    var enemySeabox = $('#enemy-board .seaBox');
    disableSeaBox(enemySeabox);
    $.ajax({
        url: "/Battle/GameOver",
        type: "POST",
        data: { battleId: battleJson.BattleId},
        success: function (result) {
            gameOver(false);
            battleHub.server.finishGame(battleJson.EnemyName, true);
        },
        error: function () {
            alert('Error');
        }
    });

});

$(document).ready(function () {
    var board = generateBoard();
    $(".board").append(board);
    $("#enemy-board").children().append("<span class='hole'></span>");

    if (battleJson.PlayerBoard === "") {
        setBoard();
    } else {
        displayBoard();
    }

});