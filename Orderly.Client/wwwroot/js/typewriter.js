
window.startTyped = function (elementId, phrases) {
    // If on mobile, use the first phrase (this is the primary
    // phrase in the list)

    if (window.innerWidth <= 768) {
        const el = document.querySelector(elementId);
        if (el && phrases.length > 0) {
            el.innerHTML = phrases[0];
        }
        return;
    }

    // Show the primary phrase (first in the list) for 1 second

    if (phrases.length > 0) {
        phrases[0] += "^1000";
    }

    new Typed(elementId, {
        strings: phrases,
        typeSpeed: 50,
        backSpeed: 25,
        backDelay: 500,
        loop: true,
        html: true
    });
}
