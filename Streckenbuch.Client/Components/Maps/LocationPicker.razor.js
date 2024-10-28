export function addCenterMaker() {
	const element = document.getElementById('map');

	// Append CSS centered marker element
	let node = document.createElement("div")
	node.classList.add("centerMarker")

	if (element) {
		element.classList.add("location-picker")
		element.children[0].appendChild(node)
	}
}