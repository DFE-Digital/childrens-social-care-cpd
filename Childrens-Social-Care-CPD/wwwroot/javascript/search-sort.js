document.addEventListener("DOMContentLoaded", function () {

    // Get references to the source and target divs
    let sourceDiv = document.getElementById("filter-form");
    let targetDiv = document.getElementById("sort-order-placeholder");

    // Get the element to move by its ID
    let elementToMove = document.getElementById("js-sort-options");

    // Check if the source and target elements exist
    if (sourceDiv && targetDiv && elementToMove) {
        // Move the element from the source div to the target div
        targetDiv.appendChild(elementToMove);

        // Get references to the select and hidden input elements
        let selectElement = document.getElementById("order");
        let hiddenInput = document.createElement("input");
        hiddenInput.type = "hidden";
        hiddenInput.name = "sortOrder";

        let selectedOptionInit = selectElement.options[selectElement.selectedIndex];
        let selectedValueInit = selectedOptionInit.value;

        hiddenInput.value = selectedValueInit;
        hiddenInput.id = "HSortOrder";
        sourceDiv.appendChild(hiddenInput);
       
        // Add an event listener to the select element
        selectElement.addEventListener("change", function () {
            // Get the selected option's value and label
            let selectedOption = selectElement.options[selectElement.selectedIndex];
            let selectedValue = selectedOption.value;

            // Update the hidden input value with the selected label
            hiddenInput.value = selectedValue;

            sourceDiv.submit();
        });
    }
});