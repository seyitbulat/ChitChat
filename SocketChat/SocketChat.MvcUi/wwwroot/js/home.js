



var socket;
function sendMessage() {
	var message = $("#message").val();
	socket.send(message);
	appendMessage(username + ": " + message, true);
	$("#message").val('');

}
$(document).ready(function () {
	document.querySelector(".chat-container").style.display = "flex";

	$.each([1, 2, 3, 4, 5, 6], function (index, value) {
		var button = `<button class="hub-btn m-2" data-button-id="` + value + `" onclick="selectHub(` + value + `)">Hub ` + value + `</button>`;
		$("#hub-list .hub .hub-list").append(button);
	});

	if (selectedHub !== null || selectedHub !== 0) {
		$('.hub-btn[data-button-id="' + selectedHub + '"]').addClass('selected-hub');
		var url = 'ws://10.19.10.42:5555/chatHub?hub=Hub' + selectedHub + '&user=' + username;
		socket = new WebSocket(url);
		
	}

	socket.addEventListener("open", (event) => {
		socket.send("Hello Server!");
	});

	socket.addEventListener("message", (event) => {
		appendMessage(event.data, false);
	});

});



function selectHub(hubId) {

	if (hubId == selectedHub) {
		$('.hub-btn[data-button-id="' + selectedHub + '"]').removeClass("selected-hub")
		selectedHub = null;
		return;
	}
	if (hubId != selectedHub) {
		if (selectedHub !== null) {

			$('.hub-btn[data-button-id="' + selectedHub + '"]').removeClass("selected-hub")
		}

		selectedHub = hubId;
		var button = $('.hub-btn[data-button-id="' + hubId + '"]');
		button.addClass("selected-hub");
	}
}




function appendMessage(message, isSender) {
	var msgElement = $('<li>').text(message);
	if (isSender) {
		msgElement.addClass('sent');
	} else {
		msgElement.addClass('received');
	}
	$('#chat-box-list').append(msgElement);
	getCurrentUsers()
}

var getObject = {
	
}
function getCurrentUsers() {
	$.ajax({
		url: 'http://10.19.10.42:5555/Hub/ActiveUsers',
		type: 'GET',
		datatype: 'json',
		cache: false,
		success: function (response) {
			$("#user-list").empty();
			$.each(response, function (index, value) {

				var user = $('<li>').text(value.userName);
				$("#user-list").append(user);
			});		}
	});

	
}