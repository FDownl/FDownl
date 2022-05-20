/* Global variables */
displayDiv = document.getElementById("display-file");
downloadDiv = document.getElementById('download-link');

/* String sanitization */
function sanitize(string) {
	const map = {
		'&': '&amp;',
		'<': '&lt;',
		'>': '&gt;',
		'"': '&quot;',
		"'": '&#x27;',
		"/": '&#x2F;',
		"`": '&grave;',
	};
	const reg = /[&<>"'`/]/ig;
	return string.replace(reg, (match)=>(map[match]));
}

/* Retrieve file preview */
if (displayDiv != null) {
	// Blob request size
	const range = 5000;
	displayDiv.innerHTML = "<p>Loading...</p>";
	var xhttp = new XMLHttpRequest();
	xhttp.onreadystatechange = function () {
		// Partial content request
		if (this.readyState == 4 && this.status == 206) {
			var res = this.responseText;
			if (/^[ -~\t\n\r]+$/.test(res)) {
				displayDiv.innerHTML = "<pre class=\"prettyprint\">" + sanitize(res) + "</pre>";
				prettyPrint();
			} else {
				displayDiv.innerHTML = "<p>Looks like this file cannot be previewed.</p>";
			}
		}
	};
	xhttp.open("GET", downloadDiv.href, true);
        xhttp.setRequestHeader("Range", "bytes=0-" + range);
	xhttp.send();
}
