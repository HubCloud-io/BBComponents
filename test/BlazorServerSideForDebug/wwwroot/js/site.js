
function getElementInfo(element) {

    var box = element.getBoundingClientRect();

    console.log("element info:", element);
    console.log("top", box.top);
    console.log("left", box.left);

    console.log("top scroll", box.top + pageYOffset);
    console.log("left scroll", box.left + pageXOffset);

    let info = { top: box.top + pageYOffset, left: box.left + pageXOffset};

    return info;
}

window.bbComponents = {

    windowHeight: function () {
        return window.innerHeight;
    },
    windowWidth: function () {
        return window.innerWidth;
    },
    setDocumentTitle: function (title) {

        document.title = title;

    },
    selectInputText: function (element) {

        if (element == null) {
            return;
        }

        element.select();
    }


};

window.outsideClickHandler = {
    addEvent: function (elementId, dotnetHelper) {
        window.addEventListener("click", function (e) {
            if (document.getElementById(elementId) != null) {
                if (!document.getElementById(elementId).contains(e.target)) {
                    dotnetHelper.invokeMethodAsync("InvokeClickOutside");
                }
            }
        });
    }
};