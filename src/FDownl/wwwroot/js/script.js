/* Global variables */
var selDiv = "";
var filesSize = 0;
var formPost = document.getElementById("form_post");
var formData = new FormData(formPost);


/* Drop file selection handling */
document.addEventListener("DOMContentLoaded", init, false);

function init() {
	document.querySelector('#files').addEventListener('change', handleFileSelect, false);
	selDiv = document.querySelector("#selectedFiles");
}

let dropArea = document.getElementById("drop-area");

// Prevent default drag behaviors
;['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
	dropArea.addEventListener(eventName, preventDefaults, false);
	document.body.addEventListener(eventName, preventDefaults, false);
})

// Highlight drop area when item is dragged over it
;['dragenter', 'dragover'].forEach(eventName => {
	dropArea.addEventListener(eventName, highlight, false);
})

;['dragleave', 'drop'].forEach(eventName => {
	dropArea.addEventListener(eventName, unhighlight, false);
})

// Handle dropped files
dropArea.addEventListener('drop', handleDrop, false);

function preventDefaults(e) {
	e.preventDefault();
	e.stopPropagation();
}

function highlight(e) {
	dropArea.classList.add('highlight');
	$("#page-mask").fadeIn(250);
}

function unhighlight(e) {
	dropArea.classList.remove('active');
	$("#page-mask").fadeOut(250);
}

function handleDrop(e) {
	var dt = e.dataTransfer;
	var files = dt.files;

	handleFiles(files);
}

function progressHandling (event) {
	var percent = 0;
	var position = event.loaded || event.position;
	var total = event.total;
	var progress_bar_id = "#progress-wrp";
	if (event.lengthComputable) {
		percent = Math.ceil(position / total * 100);
	}
	$(progress_bar_id + " .progress-bar").css("width", +percent + "%");
	$(progress_bar_id + " .status").text((percent < 100) ? (percent + "%") : ("Uploading..."));
};

function handleFileSelect(e) {
	if (!e.target.files || !window.FileReader) return;

	selDiv.innerHTML = "";
	formData = new FormData(formPost);
	$("#file-selection").fadeOut(100);

	var files = e.target.files;
	filesSize = 0;
	handleFiles(files);
}

function handleFiles(files) {
	var filesArr = Array.prototype.slice.call(files);

	Promise.all(filesArr.map(async (f) => { filesSize += f.size; })).then(() => {
		if (filesSize > 100000000) {
			alert("Uploaded files are bigger than 100MB!");
			return;
		}
		$("#file-selection").fadeIn(1000);
		filesArr.forEach(function (f, i) {
			var f = files[i];
			var button = "<button id=\"" + i + "\" onclick=\"removeElement(this.id)\" class=\"btn btn-outline-danger btn-sm float-end remove-file-btn\">X</button>";
			if (f.type.match("image.*")) {
				var reader = new FileReader();
				reader.onload = function (e) {
					var html = "<li class=\"list-group-item\"><div class=\"d-flex\"><img src=\"" + e.target.result + "\"><span class=\"my-auto file-text\">" + f.name + "</span>" + button + "</div></li>";
					selDiv.innerHTML += html;
				}
				reader.readAsDataURL(f);
			} else {
				var html = "<li class=\"list-group-item\"><span class=\"my-auto file-text\">" + f.name + "</span>" + button + "</li>";
				selDiv.innerHTML += html;
			}
			formData.append("files", f, f.name);
		});
	});
}

function removeElement(i) {
	selDiv.removeChild(selDiv.children[i]);
	var formVal = formData.getAll("files");
	formVal.splice(i, 1);
	formData.delete("files");
	for (var i = 0; i < formVal.length; i++) {
		formData.append("files", formVal[i]);
    }
	if (selDiv.children.length == 0) {
		$("#file-selection").fadeOut(1000);
	} else {
		for (var i = 0; i < selDiv.children.length; i++) {
			selDiv.children[i].lastChild.id = i;
		}
    }
}


/* URL GET parsing */
const queryString = window.location.search;
const urlParams = new URLSearchParams(queryString);
const code = urlParams.get('code');
document.getElementById("coupon_code").value = code;


/* POST handling */
var selector = document.getElementById("serverlocation");
var upload = document.getElementById("upload_btn");

// Retrieve server locations
selector.innerHTML = "<option value=\"\">Loading...</option>";
var xhttp = new XMLHttpRequest();
xhttp.onreadystatechange = function () {
	if (this.readyState == 4 && this.status == 200) {
		var res = JSON.parse(this.responseText);
		selector.innerHTML = "";
		for (var i = 0; i < res.length; i++) {
			selector.innerHTML += "<option value=\"" + res[i].hostname + "\">" + res[i].location + "</option>";
		}
		upload.disabled = false;
	}
};
xhttp.open("GET", "/api/storageservers/get", true);
xhttp.send();

// Password field handling
function togglePassword() {
	var isEnc = document.getElementById("isEncrypted");
	var passfield = document.getElementById("password");
	if (isEnc.checked) {
		passfield.style.display = "";
	} else {
		passfield.style.display = "None";
	}
}

// Upload handling
$("#upload_btn").click(function (e) {
	e.preventDefault();

	var url = "https://" + selector.value + "/upload/";
	$("#progress-wrp").fadeIn(1000);
	formData.set("lifetime", document.getElementById("lifetime").value);
	formData.set("code", document.getElementById("coupon_code").value);
	formData.set("public", document.getElementById("public").value);
	if (document.getElementById("isEncrypted").checked) {
		formData.set("password", document.getElementById("password").value);
	} else {
		formData.set("password", "");
	}

	$.ajax({
		type: "POST",
		url: url,
		xhr: function () {
			var myXhr = $.ajaxSettings.xhr();
			if (myXhr.upload) {
				myXhr.upload.addEventListener('progress', progressHandling, false);
			}
			return myXhr;
		},
		data: formData,
		success: function (data) {
			$("#progress-wrp").fadeOut(1000);
			if (data["code"] == 0) {
				window.location.href = "/" + data["id"];
			} else {
				alert(data["error"]);
			}
		},
		async: true,
		cache: false,
		contentType: false,
		processData: false,
		timeout: 120000
	});
}); 
