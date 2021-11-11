/* Global variables */
displayDiv = document.getElementById("display-file");
downloadDiv = document.getElementById('download-link');

/* String sanitization */
function sanitize(string) {
	const map = {
		'&': '&amp;',
		'<': '&lt;',
		'>': '&gt;',
		"/": '&#x2F;',
		"`": '&grave;',
	};
	const reg = /[&<>`/]/ig;
	return string.replace(reg, (match)=>(map[match]))                            // mapped symbols
	.replace(/'''/g, '\u2034')                                                   // triple prime
	.replace(/(\W|^)"(\S)/g, '$1\u201c$2')                                       // beginning "
	.replace(/(\u201c[^"]*)"([^"]*$|[^\u201c"]*\u201c)/g, '$1\u201d$2')          // ending "
	.replace(/([^0-9])"/g,'$1\u201d')                                            // remaining " at end of word
	.replace(/''/g, '\u2033')                                                    // double prime
	.replace(/(\W|^)'(\S)/g, '$1\u2018$2')                                       // beginning '
	.replace(/([a-z])'([a-z])/ig, '$1\u2019$2')                                  // conjunction's possession
	.replace(/((\u2018[^']*)|[a-z])'([^0-9]|$)/ig, '$1\u2019$3')                 // ending '
	.replace(/(\u2018)([0-9]{2}[^\u2019]*)(\u2018([^0-9]|$)|$|\u2019[a-z])/ig, '\u2019$2$3')     // abbrev. years like '93
	.replace(/(\B|^)\u2018(?=([^\u2019]*\u2019\b)*([^\u2019\u2018]*\W[\u2019\u2018]\b|[^\u2019\u2018]*$))/ig, '$1\u2019') // backwards apostrophe
	.replace(/'/g, '\u2032');
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
        xhttp.setRequestHeader("Range", "bytes=-" + range);
	xhttp.send();
}
