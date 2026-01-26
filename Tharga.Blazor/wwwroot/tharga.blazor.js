window.clipboard = {
    copyText: function (text) {
        return navigator.clipboard.writeText(text);
    }
};