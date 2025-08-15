
window.windowInterop = {
    getWidth: function () {
        return window.innerWidth;
    },
    registerResizeHandler: function (dotnetHelper) {
        window.addEventListener("resize", () => {
            dotnetHelper.invokeMethodAsync("UpdateWidth", window.innerWidth);
        });
    }
};
