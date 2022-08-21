

/* When the user scrolls down, hide the navbar. When the user scrolls up, show the navbar */
var prevScrollPos = window.pageYOffset;
window.onscroll = function () {
    var currentScrollPos = window.pageYOffset;
    if (prevScrollPos <= currentScrollPos) {
        document.getElementById("header").style.top = "-100px";
        prevScrollPos = currentScrollPos;
        return;
    }
    document.getElementById("header").style.top = "0";
    prevScrollPos = currentScrollPos;
}

