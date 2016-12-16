(function () {
	function getSuggestions(fragment, callback) {
		fetch(`/api/Tags/Suggestions/${fragment}`)
			.then(r => r.json())
			.then(callback);
	}

	const suggesters = document.querySelectorAll("[data-tag-suggest]");

	suggesters.forEach(s => {
		let results = document.createElement("ul");
		results.className = "suggestions-list";
		s.insertAdjacentElement("afterend", results);

		s.addEventListener("keyup", e => {
			// Only autocomplete when the user is typing at the end of the textbox
			if (s.selectionStart !== s.value.length) return;

			// Get current tag fragment
			let fragment = s.value.substring(s.value.lastIndexOf(",") + 1).trim();

			if (fragment.length >= 1) {
				getSuggestions(fragment, suggestions => {
					results.innerHTML = "";
					suggestions.forEach(sg => {
						let li = document.createElement("li");
						li.innerHTML = sg.TagLabel;
						li.onclick = function () {
							s.value = s.value.substring(0, s.value.length - fragment.length);
							s.value += sg.TagLabel + ", ";
							s.focus();
							results.innerHTML = "";
						};
						results.appendChild(li);
					});
				});
			}
		}, 0);
	});
}());