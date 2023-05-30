window.outsideClickHandler = {
    addEvent: function (elementId, dotnetHelper) {

        console.log("JS. Add outside click listener", elementId);

        window.addEventListener("click", function (e) {
            if (document.getElementById(elementId) != null) {
                if (!document.getElementById(elementId).contains(e.target)) {

                    console.log("JS. InvokeClickOutside", elementId);
                    console.log("Target", e.target);

                    dotnetHelper.invokeMethodAsync("InvokeClickOutside");
                }
            }
        });
    }
};