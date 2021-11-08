/* Global variables */
displayDiv = document.getElementById("display-file");
downloadDiv = document.getElementById('download-link');

/* Retrieve file preview */
if (displayDiv != null) {
	const range = 5000;
    displayDiv.innerHTML = "<p>Loading...</p>";
	var xhttp = new XMLHttpRequest();
	xhttp.onreadystatechange = function () {
		if (this.readyState == 4 && this.status == 200) {
			var res = this.responseText;
			if (/^[ -~\t\n\r]+$/.test(resStart)) {
				displayDiv.innerHTML = "<pre class=\"prettyprint\">" + resStart + "</pre>";
				prettyPrint();
			} else {
				displayDiv.innerHTML = "<p>Looks like this file cannot be previewed.</p>";
            }
		}
	};
	xhttp.open("GET", downloadDiv.href, true);
        xhttp.setRequestHeader("Range", "bytes=-" + range);
	xhttp.send();
}
